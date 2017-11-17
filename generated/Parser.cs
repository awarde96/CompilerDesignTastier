/*----------------------------------------------------------------------
Compiler Generator Coco/R,
Copyright (c) 1990, 2004 Hanspeter Moessenboeck, University of Linz
extended by M. Loeberbauer & A. Woess, Univ. of Linz
with improvements by Pat Terry, Rhodes University

This program is free software; you can redistribute it and/or modify it 
under the terms of the GNU General Public License as published by the 
Free Software Foundation; either version 2, or (at your option) any 
later version.

This program is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License 
for more details.

You should have received a copy of the GNU General Public License along 
with this program; if not, write to the Free Software Foundation, Inc., 
59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.

As an exception, it is allowed to write an extension of Coco/R that is
used as a plugin in non-free software.

If not otherwise stated, any source code generated by Coco/R (other than 
Coco/R itself) does not fall under the GNU General Public License.
-----------------------------------------------------------------------*/

using System;

namespace Tastier {



public class Parser {
	public const int _EOF = 0;
	public const int _number = 1;
	public const int _ident = 2;
	public const int _string = 3;
	public const int maxT = 44;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

const int // object kinds
      var = 0, proc = 1, constant = 3;

   const int // types
      undef = 0, integer = 1, boolean = 2;

  const int // sorts
      none = 0, scalar = 1, array = 2;

   public SymbolTable tab;
   public CodeGenerator gen;

/*-------------------------------------------------------------------------------------------*/



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void AddOp(out Op op) {
		op = Op.ADD; 
		if (la.kind == 4) {
			Get();
		} else if (la.kind == 5) {
			Get();
			op = Op.SUB; 
		} else SynErr(45);
	}

	void Expr(out int reg,        // load value of Expr into register
out int type) {
		int typeR, regR; Op op; 
		SimExpr(out reg,
out type);
		if (StartOf(1)) {
			RelOp(out op);
			SimExpr(out regR,
out typeR);
			if (type == typeR) {
			  type = boolean;
			  gen.RelOp(op, reg, regR);
			}
			else SemErr("incompatible types");
			
		}
		gen.ClearRegisters(); 
	}

	void SimExpr(out int reg,     //load value of SimExpr into register
out int type) {
		int typeR, regR; Op op; 
		Term(out reg,
out type);
		while (la.kind == 4 || la.kind == 5) {
			AddOp(out op);
			Term(out regR,
out typeR);
			if (type == integer && typeR == integer)
			  gen.AddOp(op, reg, regR);
			else SemErr("integer type expected");
			
		}
	}

	void RelOp(out Op op) {
		op = Op.EQU; 
		switch (la.kind) {
		case 18: {
			Get();
			break;
		}
		case 19: {
			Get();
			op = Op.LSS; 
			break;
		}
		case 20: {
			Get();
			op = Op.GTR; 
			break;
		}
		case 21: {
			Get();
			op = Op.NEQ; 
			break;
		}
		case 22: {
			Get();
			op = Op.LEQ; 
			break;
		}
		case 23: {
			Get();
			op = Op.GEQ; 
			break;
		}
		default: SynErr(46); break;
		}
	}

	void Primary(out int reg,     // load Primary into register
out int type) {
		int n; Obj obj; string name; 
		type = undef;
		reg = gen.GetRegister();
		
		switch (la.kind) {
		case 2: {
			Ident(out name);
			obj = tab.Find(name); type = obj.type;
			if (obj.kind == var || obj.kind == constant) {
			  if (obj.level == 0)
			     gen.LoadGlobal(reg, obj.adr, name);
			  else
			     gen.LoadLocal(reg, tab.curLevel-obj.level, obj.adr, name);
			  if (type == boolean)
			  // reset Z flag in CPSR
			     gen.ResetZ(reg);
			}
			else SemErr("variable expected");
			
			break;
		}
		case 1: {
			Get();
			type = integer;
			n = Convert.ToInt32(t.val);
			gen.LoadConstant(reg, n);
			
			break;
		}
		case 5: {
			Get();
			Primary(out reg,
out type);
			if (type == integer)
			  gen.NegateValue(reg);
			else SemErr("integer type expected");
			
			break;
		}
		case 6: {
			Get();
			type = boolean;
			gen.LoadTrue(reg);
			
			break;
		}
		case 7: {
			Get();
			type = boolean;
			gen.LoadFalse(reg);
			
			break;
		}
		case 8: {
			Get();
			Expr(out reg,
out type);
			Expect(9);
			break;
		}
		default: SynErr(47); break;
		}
	}

	void Ident(out string name) {
		Expect(2);
		name = t.val; 
	}

	void String(out string text) {
		Expect(3);
		text = t.val; 
	}

	void MulOp(out Op op) {
		op = Op.MUL; 
		if (la.kind == 10) {
			Get();
		} else if (la.kind == 11 || la.kind == 12) {
			if (la.kind == 11) {
				Get();
			} else {
				Get();
			}
			op = Op.DIV; 
		} else if (la.kind == 13 || la.kind == 14) {
			if (la.kind == 13) {
				Get();
			} else {
				Get();
			}
			op = Op.MOD; 
		} else SynErr(48);
	}

	void ProcDecl(string progName) {
		Obj obj; string procName; 
		Expect(15);
		Ident(out procName);
		obj = tab.NewObj(procName, proc, undef,1,0,0, false);
		if (procName == "main")
		  if (tab.curLevel == 0)
		     tab.mainPresent = true;
		  else SemErr("main not at lexic level 0");
		tab.OpenScope();
		
		Expect(8);
		Expect(9);
		Expect(16);
		while (StartOf(2)) {
			if (la.kind == 42) {
				ConstantDecl();
			} else if (la.kind == 39 || la.kind == 40) {
				VarDecl();
			} else {
				ArrayDecl();
			}
		}
		while (la.kind == 15) {
			ProcDecl(progName);
		}
		if (procName == "main")
		  gen.Label("Main", "Body");
		else {
		  gen.ProcNameComment(procName);
		  gen.Label(procName, "Body");
		}
		
		Stat();
		while (StartOf(3)) {
			Stat();
		}
		Expect(17);
		if (procName == "main") {
		  gen.StopProgram(progName);
		  gen.Enter("Main", tab.curLevel, tab.topScope.nextAdr);
		} else {
		  gen.Return(procName);
		  gen.Enter(procName, tab.curLevel, tab.topScope.nextAdr);
		}
		tab.CloseScope();
		
	}

	void ConstantDecl() {
		string name; int type;
		Expect(42);
		Type(out type);
		Ident(out name);
		tab.NewObj(name, constant, type,scalar,0,0,false); 
		Expect(27);
	}

	void VarDecl() {
		string name; int type; int sort = 1;
		Type(out type);
		Ident(out name);
		tab.NewObj(name, var, type, sort,0,0,false); 
		while (la.kind == 41) {
			Get();
			Ident(out name);
			tab.NewObj(name, var, type, sort,0,0,false); 
		}
		Expect(27);
	}

	void ArrayDecl() {
		int r; int c = 0; string name; int type;
		Expect(43);
		Type(out type);
		Ident(out name);
		Expect(24);
		Expect(1);
		type = integer;
		r = Convert.ToInt32(t.val);
		c = 1;
		
		Expect(25);
		if (la.kind == 24) {
			Get();
			Expect(1);
			type = integer;
			c = Convert.ToInt32(t.val);
			
			Expect(25);
		}
		tab.NewObj(name, var, type,array,r,c,false); 
		Expect(27);
	}

	void Stat() {
		int type; string name; Obj obj; int reg = 0; int index = 0; int index2 = 0; String arrayName; 
		switch (la.kind) {
		case 2: {
			Ident(out name);
			obj = tab.Find(name); 
			if (la.kind == 24) {
				Get();
				Expect(1);
				index = Convert.ToInt32(t.val);
				if(index > obj.rows | obj.rows < 0)
				 SemErr("array index rows out of bounds");
				
				Expect(25);
				if (la.kind == 24) {
					Get();
					Expect(1);
					index2 = Convert.ToInt32(t.val);
					if(index2 > obj.columns | obj.columns < 0)
					 SemErr("array index columns out of bounds");
					
					Expect(25);
				}
			}
			if (la.kind == 26) {
				Get();
				if (obj.kind == constant)
				  SemErr("cannot assign to constant");
				
				if (obj.kind != var)
				  SemErr("cannot assign to procedure");
				
				if (StartOf(4)) {
					Expr(out reg,
out type);
					if (type == obj.type)
					  if (obj.level == 0)
					     gen.StoreGlobal(reg, obj.adr + index +(index2*obj.rows), name);
					  else gen.StoreLocal(reg, tab.curLevel-obj.level, obj.adr + index +(index2*obj.rows), name);
					else SemErr("incompatible types");
					
				} else if (la.kind == 24) {
					Get();
					int rowIndex = 0; int columnIndex = 0; 
					Expect(1);
					rowIndex = Convert.ToInt32(t.val);
					Expect(25);
					if (la.kind == 24) {
						Get();
						Expect(1);
						columnIndex = Convert.ToInt32(t.val);
						Expect(25);
					}
					Ident(out arrayName);
					Obj arrayObj = tab.Find(arrayName);
					
					if(rowIndex > arrayObj.rows | rowIndex < 0){
					 SemErr("index rows out of bounds");
					}
					
					if(columnIndex > arrayObj.rows | columnIndex < 0){
					 SemErr("index columns out of bounds");
					}
					
					if(arrayObj.type != obj.type){
					 SemErr("Incompatable types");
					}
					
					if (arrayObj.level == 0)
					  gen.LoadGlobal(reg, arrayObj.adr + rowIndex+(columnIndex*arrayObj.rows), arrayName);
					else gen.LoadLocal(reg, tab.curLevel-arrayObj.level, arrayObj.adr + rowIndex+(columnIndex*arrayObj.rows), arrayName);
					
					if (obj.level == 0)
					  gen.StoreGlobal(reg, obj.adr + index +(index2*obj.rows), name);
					else
					  gen.LoadLocal(reg, tab.curLevel-obj.level, obj.adr + index +(index2*obj.rows), name);
					
				} else SynErr(49);
				Expect(27);
			} else if (la.kind == 8) {
				Get();
				Expect(9);
				Expect(27);
				if (obj.kind == proc)
				  gen.Call(name);
				else SemErr("object is not a procedure");
				
			} else if (la.kind == 18) {
				Get();
				Expect(1);
				if (obj.kind != constant)
				  SemErr("Must be a constant to use '='");
				
				if(obj.hasValue == false){
				  obj.hasValue = true;
				}
				else{
				  SemErr("Constant already assigned");
				}
				
				if (obj.level == 0)
				  gen.StoreGlobal(reg, obj.adr, name);
				else gen.StoreLocal(reg, tab.curLevel-obj.level, obj.adr, name);
				
				Expect(27);
			} else SynErr(50);
			break;
		}
		case 28: {
			Get();
			
			Expect(8);
			Expr(out reg,
out type);
			
			Expect(9);
			Expect(29);
			while (la.kind == 30) {
				Get();
				Expect(8);
				Expr(out reg,
out type);
				
				Expect(9);
				Stat();
				
			}
			Expect(31);
			Stat();
			
			break;
		}
		case 32: {
			Get();
			int l1, l2; l1 = 0; 
			Expr(out reg,
out type);
			if (type == boolean) {
			  l1 = gen.NewLabel();
			  gen.BranchFalse(l1);
			}
			else SemErr("boolean type expected");
			
			Stat();
			l2 = gen.NewLabel();
			gen.Branch(l2);
			gen.Label(l1);
			
			if (la.kind == 33) {
				Get();
				Stat();
			}
			gen.Label(l2); 
			break;
		}
		case 34: {
			Get();
			int l1, l2;
			l1 = gen.NewLabel();
			gen.Label(l1); l2=0;
			
			Expr(out reg,
out type);
			if (type == boolean) {
			  l2 = gen.NewLabel();
			  gen.BranchFalse(l2);
			}
			else SemErr("boolean type expected");
			
			Stat();
			gen.Branch(l1);
			gen.Label(l2);
			
			break;
		}
		case 35: {
			Get();
			Ident(out name);
			Expect(27);
			obj = tab.Find(name);
			if (obj.type == integer) {
			  gen.ReadInteger();
			  if (obj.level == 0)
			     gen.StoreGlobal(0, obj.adr, name);
			  else gen.StoreLocal(0, tab.curLevel-obj.level, obj.adr, name);
			}
			else SemErr("integer type expected");
			
			break;
		}
		case 36: {
			Get();
			string text; 
			if (StartOf(4)) {
				Expr(out reg,
out type);
				switch (type) {
				  case integer: gen.WriteInteger(reg, false);
				                break;
				  case boolean: gen.WriteBoolean(false);
				                break;
				}
				
			} else if (la.kind == 3) {
				String(out text);
				gen.WriteString(text); 
			} else SynErr(51);
			Expect(27);
			break;
		}
		case 37: {
			Get();
			Expr(out reg,
out type);
			switch (type) {
			  case integer: gen.WriteInteger(reg, true);
			                break;
			  case boolean: gen.WriteBoolean(true);
			                break;
			}
			
			Expect(27);
			break;
		}
		case 16: {
			Get();
			tab.OpenSubScope(); 
			while (la.kind == 39 || la.kind == 40 || la.kind == 42) {
				if (la.kind == 39 || la.kind == 40) {
					VarDecl();
				} else {
					ConstantDecl();
				}
			}
			Stat();
			while (StartOf(3)) {
				Stat();
			}
			Expect(17);
			tab.CloseSubScope(); 
			break;
		}
		default: SynErr(52); break;
		}
	}

	void Term(out int reg,        // load value of Term into register
out int type) {
		int typeR, regR; Op op; 
		Primary(out reg,
out type);
		while (StartOf(5)) {
			MulOp(out op);
			Primary(out regR,
out typeR);
			if (type == integer && typeR == integer)
			  gen.MulOp(op, reg, regR);
			else SemErr("integer type expected");
			
		}
	}

	void Tastier() {
		string progName; 
		Expect(38);
		Ident(out progName);
		tab.OpenScope(); 
		Expect(16);
		while (StartOf(2)) {
			if (la.kind == 39 || la.kind == 40) {
				VarDecl();
			} else if (la.kind == 42) {
				ConstantDecl();
			} else {
				ArrayDecl();
			}
		}
		while (la.kind == 15) {
			ProcDecl(progName);
		}
		tab.CloseScope(); 
		Expect(17);
	}

	void Type(out int type) {
		type = undef; 
		if (la.kind == 39) {
			Get();
			type = integer; 
		} else if (la.kind == 40) {
			Get();
			type = boolean; 
		} else SynErr(53);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Tastier();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,T,T, x,x},
		{x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,T,T, T,T,x,x, x,x,x,x, x,x},
		{x,T,T,x, x,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
    public System.IO.TextWriter errorStream = Console.Error; // error messages go to this stream - was Console.Out DMA
    public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "number expected"; break;
			case 2: s = "ident expected"; break;
			case 3: s = "string expected"; break;
			case 4: s = "\"+\" expected"; break;
			case 5: s = "\"-\" expected"; break;
			case 6: s = "\"true\" expected"; break;
			case 7: s = "\"false\" expected"; break;
			case 8: s = "\"(\" expected"; break;
			case 9: s = "\")\" expected"; break;
			case 10: s = "\"*\" expected"; break;
			case 11: s = "\"div\" expected"; break;
			case 12: s = "\"DIV\" expected"; break;
			case 13: s = "\"mod\" expected"; break;
			case 14: s = "\"MOD\" expected"; break;
			case 15: s = "\"void\" expected"; break;
			case 16: s = "\"{\" expected"; break;
			case 17: s = "\"}\" expected"; break;
			case 18: s = "\"=\" expected"; break;
			case 19: s = "\"<\" expected"; break;
			case 20: s = "\">\" expected"; break;
			case 21: s = "\"!=\" expected"; break;
			case 22: s = "\"<=\" expected"; break;
			case 23: s = "\">=\" expected"; break;
			case 24: s = "\"[\" expected"; break;
			case 25: s = "\"]\" expected"; break;
			case 26: s = "\":=\" expected"; break;
			case 27: s = "\";\" expected"; break;
			case 28: s = "\"switch\" expected"; break;
			case 29: s = "\":\" expected"; break;
			case 30: s = "\"case\" expected"; break;
			case 31: s = "\"default\" expected"; break;
			case 32: s = "\"if\" expected"; break;
			case 33: s = "\"else\" expected"; break;
			case 34: s = "\"while\" expected"; break;
			case 35: s = "\"read\" expected"; break;
			case 36: s = "\"write\" expected"; break;
			case 37: s = "\"writeln\" expected"; break;
			case 38: s = "\"program\" expected"; break;
			case 39: s = "\"int\" expected"; break;
			case 40: s = "\"bool\" expected"; break;
			case 41: s = "\",\" expected"; break;
			case 42: s = "\"const\" expected"; break;
			case 43: s = "\"array\" expected"; break;
			case 44: s = "??? expected"; break;
			case 45: s = "invalid AddOp"; break;
			case 46: s = "invalid RelOp"; break;
			case 47: s = "invalid Primary"; break;
			case 48: s = "invalid MulOp"; break;
			case 49: s = "invalid Stat"; break;
			case 50: s = "invalid Stat"; break;
			case 51: s = "invalid Stat"; break;
			case 52: s = "invalid Stat"; break;
			case 53: s = "invalid Type"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}