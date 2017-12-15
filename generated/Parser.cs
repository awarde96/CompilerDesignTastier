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
	public const int maxT = 50;

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
      undef = 0, integer = 1, boolean = 2, structure = 3;

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
		} else SynErr(51);
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
		case 20: {
			Get();
			break;
		}
		case 21: {
			Get();
			op = Op.LSS; 
			break;
		}
		case 22: {
			Get();
			op = Op.GTR; 
			break;
		}
		case 23: {
			Get();
			op = Op.NEQ; 
			break;
		}
		case 24: {
			Get();
			op = Op.LEQ; 
			break;
		}
		case 25: {
			Get();
			op = Op.GEQ; 
			break;
		}
		default: SynErr(52); break;
		}
	}

	void Primary(out int reg,     // load Primary into register
out int type) {
		int n; Obj obj; string name; int index=0; int index2=0; int l1 = 0; int reg1 = 0;
		type = undef;
		reg = gen.GetRegister();
		
		switch (la.kind) {
		case 2: {
			Ident(out name);
			obj = tab.Find(name); 
			if (la.kind == 6) {
				Get();
				Expr(out reg,
out type);
				l1 = gen.NewLabel();
				index = reg;
				reg1 = gen.GetRegister();
				gen.LoadConstant(reg1, obj.rows);
				gen.RelOp(Op.LEQ,reg1, index);
				gen.BranchFalse(l1);
				gen.WriteString("\"Index rows out of bounds\"");
				gen.Label(l1);
				
				Expect(7);
				if (la.kind == 6) {
					Get();
					Expect(1);
					gen.GetRegister();
					gen.GetRegister();
					index2 = gen.GetRegister();
					gen.LoadConstant(index2, Int32.Parse(t.val));
					reg = gen.GetRegister();
					gen.LoadConstant(reg, obj.rows);
					gen.MulOp(Op.MUL, index2,reg);
					gen.AddOp(Op.ADD, index, index2);
					
					Expect(7);
				}
			}
			obj = tab.Find(name); type = obj.type;
			if (obj.sort == scalar){
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
			}
			else{
			if (obj.kind == var || obj.kind == constant) {
			 if (obj.level == 0)
			     gen.LoadIndexedGlobal(reg, obj.adr,index, name);
			 else
			     gen.LoadIndexedLocal(reg, tab.curLevel-obj.level, obj.adr,index, name);
			 if (type == boolean)
			 // reset Z flag in CPSR
			   gen.ResetZ(reg);
			}
			else SemErr("variable expected");
			}
			
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
		case 8: {
			Get();
			type = boolean;
			gen.LoadTrue(reg);
			
			break;
		}
		case 9: {
			Get();
			type = boolean;
			gen.LoadFalse(reg);
			
			break;
		}
		case 10: {
			Get();
			Expr(out reg,
out type);
			Expect(11);
			break;
		}
		default: SynErr(53); break;
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
		if (la.kind == 12) {
			Get();
		} else if (la.kind == 13 || la.kind == 14) {
			if (la.kind == 13) {
				Get();
			} else {
				Get();
			}
			op = Op.DIV; 
		} else if (la.kind == 15 || la.kind == 16) {
			if (la.kind == 15) {
				Get();
			} else {
				Get();
			}
			op = Op.MOD; 
		} else SynErr(54);
	}

	void ProcDecl(string progName) {
		Obj obj; string procName; 
		Expect(17);
		Ident(out procName);
		obj = tab.NewObj(procName, proc, undef,1,0,0, false,"null");
		if (procName == "main")
		  if (tab.curLevel == 0)
		     tab.mainPresent = true;
		  else SemErr("main not at lexic level 0");
		tab.OpenScope();
		
		Expect(10);
		Expect(11);
		Expect(18);
		while (StartOf(2)) {
			if (la.kind == 46) {
				ConstantDecl();
			} else if (la.kind == 43 || la.kind == 44) {
				VarDecl();
			} else if (la.kind == 49) {
				ArrayDecl();
			} else {
				StructAssign();
			}
		}
		while (la.kind == 17) {
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
		Expect(19);
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
		Expect(46);
		Type(out type);
		Ident(out name);
		tab.NewObj(name, constant, type,scalar,0,0,false,"null"); 
		Expect(28);
	}

	void VarDecl() {
		string name; int type; int sort = 1;
		Type(out type);
		Ident(out name);
		tab.NewObj(name, var, type, sort,0,0,false,"null"); 
		while (la.kind == 45) {
			Get();
			Ident(out name);
			tab.NewObj(name, var, type, sort,0,0,false,"null"); 
		}
		Expect(28);
	}

	void ArrayDecl() {
		int r; int c = 0; string name; int type;
		Expect(49);
		Type(out type);
		Ident(out name);
		Expect(6);
		Expect(1);
		type = integer;
		r = Convert.ToInt32(t.val);
		c = 1;
		
		Expect(7);
		if (la.kind == 6) {
			Get();
			Expect(1);
			type = integer;
			c = Convert.ToInt32(t.val);
			
			Expect(7);
		}
		tab.NewObj(name, var, type,array,r,c,false,"null"); 
		Expect(28);
	}

	void StructAssign() {
		string name; string structName; Obj obj; 
		Expect(48);
		Ident(out structName);
		obj = tab.Find(structName);
		 if (obj.type != structure){
		     SemErr("Must be of type stucture");
		 }
		
		Ident(out name);
		tab.NewObj(name, var, 3, scalar, 0,0,false,structName);  
		Expect(28);
	}

	void Stat() {
		int type; string name; Obj obj; Obj temp;int reg = 0; int regI; int index = 0; int index2 = 0; String arrayName; string structName; string sName;
		switch (la.kind) {
		case 2: {
			Ident(out name);
			obj = tab.Find(name); 
			if (la.kind == 6) {
				Get();
				Expr(out reg,
out type);
				gen.LoadConstant(index, Int32.Parse(t.val));
				//gen.RelOp(Op.EQU,reg2,reg1);
				//gen.BranchFalse(l2);
				
				Expect(7);
				if (la.kind == 6) {
					Get();
					Expect(1);
					index2 = gen.GetRegister();
					gen.LoadConstant(index2, Int32.Parse(t.val));
					reg = gen.GetRegister();
					gen.LoadConstant(reg, obj.rows);
					gen.MulOp(Op.MUL, index2, reg);
					gen.AddOp(Op.ADD, index, index2);
					
					Expect(7);
				}
			}
			if (la.kind == 26) {
				Get();
				Ident(out structName);
				temp = tab.Find(structName);
				if(temp.parent == obj.parent){
				  obj = temp;
				}
				else{
				 SemErr("Variable does not belong to this struct");
				}
				
			}
			if (la.kind == 27) {
				Get();
				if (obj.kind == constant)
				  SemErr("cannot assign to constant");
				
				if (obj.type == structure){
				  SemErr("cannot assign to structure variable");
				}
				
				if (obj.kind != var)
				  SemErr("cannot assign to procedure");
				
				if (StartOf(4)) {
					Expr(out reg,
out type);
					if (type == obj.type)
					  if (obj.level == 0)
					     if(obj.sort == scalar){
					       gen.StoreGlobal(reg, obj.adr, name);
					     }
					     else{
					       gen.StoreIndexedGlobal(reg,obj.adr,index,name);
					     }
					  else {
					     if(obj.sort == scalar){
					       gen.StoreLocal(reg, tab.curLevel-obj.level, obj.adr, name);
					     }
					     else{
					       gen.StoreIndexedLocal(reg, tab.curLevel-obj.level, obj.adr, index,name);
					     }
					  }
					else SemErr("incompatible types");
					
				} else if (la.kind == 26) {
					Get();
					Ident(out sName);
					Expect(26);
					Ident(out structName);
					temp = tab.Find(structName);
					Obj temp1 = tab.Find(sName);
					
					if(temp1.type != structure){
					  SemErr("Must be of type structure");
					}
					if(temp1.parent != temp.parent){
					  SemErr("Variable does not belong to struct");
					}
					
					if(temp.type != obj.type){
					 SemErr("Incompatable types");
					}
					
					if (temp.level == 0)
					  gen.LoadGlobal(reg, temp.adr, structName);
					else gen.LoadLocal(reg, tab.curLevel-obj.level, temp.adr, structName);
					
					if (obj.level == 0)
					  gen.StoreGlobal(reg, obj.adr, name);
					else gen.StoreLocal(reg, tab.curLevel-obj.level, obj.adr, name);
					
				} else SynErr(55);
				Expect(28);
			} else if (la.kind == 10) {
				Get();
				Expect(11);
				Expect(28);
				if (obj.kind == proc)
				  gen.Call(name);
				else SemErr("object is not a procedure");
				
			} else if (la.kind == 20) {
				Get();
				Expr(out reg,
out type);
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
				else
				  gen.StoreLocal(reg, tab.curLevel-obj.level, obj.adr, name);
				
				Expect(28);
			} else SynErr(56);
			break;
		}
		case 29: {
			Get();
			int l1, l2, l3; int reg1, reg2; int type1, type2; int check = 0; int errorCheck; 
			Expect(10);
			Expr(out reg1,
out type1);
			l3 = 0;
			l3 = gen.NewLabel();
			
			Expect(11);
			Expect(30);
			while (la.kind == 31) {
				Get();
				l2 = gen.NewLabel();
				gen.GetRegister();
				errorCheck = 0;
				
				Expect(10);
				Expr(out reg2,
out type2);
				if (type1 != type2){
				 SemErr("Types in case must match switch statement");
				}
				if(check == 0){
				 gen.RelOp(Op.EQU,reg2,reg1);
				 gen.BranchFalse(l2);
				}
				
				Expect(11);
				Stat();
				
				if (la.kind == 32) {
					Get();
					check = 1;
					errorCheck += 1;
					
				}
				if (la.kind == 33) {
					Get();
					gen.Branch(l3);
					errorCheck += 1;
					
				}
				if(errorCheck != 1){
				 SemErr("Must use either a continue or break in switch statement");
				}
				gen.Label(l2);
				
			}
			Expect(34);
			Stat();
			
			gen.Label(l3);
			
			break;
		}
		case 35: {
			Get();
			int l1, l2, l3, l4;
			l1 = 0;
			l2 = 0;
			l3 = 0;
			l4 = 0;
			
			Expect(10);
			Stat();
			l1 = gen.NewLabel();
			gen.Label(l1);
			
			Expr(out reg,
out type);
			if (type == boolean) {
			  l2 = gen.NewLabel();
			  gen.BranchFalse(l2);
			  l3 = gen.NewLabel();
			  gen.Branch(l3);
			}
			else SemErr("boolean type expected");
			
			Expect(28);
			l4 = gen.NewLabel();
			gen.Label(l4);
			
			Stat();
			gen.Branch(l1);
			gen.Label(l3);
			
			Expect(11);
			Expect(18);
			Stat();
			gen.Branch(l4);
			gen.Label(l2);
			
			Expect(19);
			break;
		}
		case 36: {
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
			
			if (la.kind == 37) {
				Get();
				Stat();
			}
			gen.Label(l2); 
			break;
		}
		case 38: {
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
		case 39: {
			Get();
			Ident(out name);
			Expect(28);
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
		case 40: {
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
			} else SynErr(57);
			Expect(28);
			break;
		}
		case 41: {
			Get();
			Expr(out reg,
out type);
			switch (type) {
			  case integer: gen.WriteInteger(reg, true);
			                break;
			  case boolean: gen.WriteBoolean(true);
			                break;
			}
			
			Expect(28);
			break;
		}
		case 18: {
			Get();
			tab.OpenSubScope(); 
			while (la.kind == 43 || la.kind == 44 || la.kind == 46) {
				if (la.kind == 43 || la.kind == 44) {
					VarDecl();
				} else {
					ConstantDecl();
				}
			}
			Stat();
			while (StartOf(3)) {
				Stat();
			}
			Expect(19);
			tab.CloseSubScope(); 
			break;
		}
		default: SynErr(58); break;
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
		Expect(42);
		Ident(out progName);
		tab.OpenScope(); 
		Expect(18);
		while (StartOf(6)) {
			if (la.kind == 43 || la.kind == 44) {
				VarDecl();
			} else if (la.kind == 46) {
				ConstantDecl();
			} else if (la.kind == 49) {
				ArrayDecl();
			} else {
				StructDecl();
			}
		}
		while (la.kind == 17) {
			ProcDecl(progName);
		}
		tab.CloseScope(); 
		Expect(19);
	}

	void StructDecl() {
		string name; string structName; int type;
		Expect(47);
		Ident(out structName);
		tab.NewObj(structName, var, 3, scalar, 0,0,false,"null");  
		Expect(18);
		while (la.kind == 43 || la.kind == 44) {
			Type(out type);
			Ident(out name);
			tab.NewObj(name, var, type, 1,0,0,false,structName); 
			Expect(28);
		}
		Expect(19);
	}

	void Type(out int type) {
		type = undef; 
		if (la.kind == 43) {
			Get();
			type = integer; 
		} else if (la.kind == 44) {
			Get();
			type = boolean; 
		} else SynErr(59);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Tastier();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,T,x, T,T,x,x},
		{x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,T, T,x,T,T, T,T,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,x, x,T,x,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,T,T, x,T,x,x}

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
			case 6: s = "\"[\" expected"; break;
			case 7: s = "\"]\" expected"; break;
			case 8: s = "\"true\" expected"; break;
			case 9: s = "\"false\" expected"; break;
			case 10: s = "\"(\" expected"; break;
			case 11: s = "\")\" expected"; break;
			case 12: s = "\"*\" expected"; break;
			case 13: s = "\"div\" expected"; break;
			case 14: s = "\"DIV\" expected"; break;
			case 15: s = "\"mod\" expected"; break;
			case 16: s = "\"MOD\" expected"; break;
			case 17: s = "\"void\" expected"; break;
			case 18: s = "\"{\" expected"; break;
			case 19: s = "\"}\" expected"; break;
			case 20: s = "\"=\" expected"; break;
			case 21: s = "\"<\" expected"; break;
			case 22: s = "\">\" expected"; break;
			case 23: s = "\"!=\" expected"; break;
			case 24: s = "\"<=\" expected"; break;
			case 25: s = "\">=\" expected"; break;
			case 26: s = "\".\" expected"; break;
			case 27: s = "\":=\" expected"; break;
			case 28: s = "\";\" expected"; break;
			case 29: s = "\"switch\" expected"; break;
			case 30: s = "\":\" expected"; break;
			case 31: s = "\"case\" expected"; break;
			case 32: s = "\"continue\" expected"; break;
			case 33: s = "\"break\" expected"; break;
			case 34: s = "\"default\" expected"; break;
			case 35: s = "\"for\" expected"; break;
			case 36: s = "\"if\" expected"; break;
			case 37: s = "\"else\" expected"; break;
			case 38: s = "\"while\" expected"; break;
			case 39: s = "\"read\" expected"; break;
			case 40: s = "\"write\" expected"; break;
			case 41: s = "\"writeln\" expected"; break;
			case 42: s = "\"program\" expected"; break;
			case 43: s = "\"int\" expected"; break;
			case 44: s = "\"bool\" expected"; break;
			case 45: s = "\",\" expected"; break;
			case 46: s = "\"const\" expected"; break;
			case 47: s = "\"struct\" expected"; break;
			case 48: s = "\"new\" expected"; break;
			case 49: s = "\"array\" expected"; break;
			case 50: s = "??? expected"; break;
			case 51: s = "invalid AddOp"; break;
			case 52: s = "invalid RelOp"; break;
			case 53: s = "invalid Primary"; break;
			case 54: s = "invalid MulOp"; break;
			case 55: s = "invalid Stat"; break;
			case 56: s = "invalid Stat"; break;
			case 57: s = "invalid Stat"; break;
			case 58: s = "invalid Stat"; break;
			case 59: s = "invalid Type"; break;

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