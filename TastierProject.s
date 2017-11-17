	AREA	TastierProject, CODE, READONLY

    IMPORT  TastierDiv
	IMPORT	TastierMod
	IMPORT	TastierReadInt
	IMPORT	TastierPrintInt
	IMPORT	TastierPrintIntLf
	IMPORT	TastierPrintTrue
	IMPORT	TastierPrintTrueLf
	IMPORT	TastierPrintFalse
    IMPORT	TastierPrintFalseLf
    IMPORT  TastierPrintString
    
; Entry point called from C runtime __main
	EXPORT	main

; Preserve 8-byte stack alignment for external routines
	PRESERVE8

; Register names
BP  RN 10	; pointer to stack base
TOP RN 11	; pointer to top of stack

main
; Initialization
	LDR		R4, =globals
	LDR 	BP, =stack		; address of stack base
	LDR 	TOP, =stack+16	; address of top of stack frame
	B		Main
MainBody
 LDR R2, =1
 ADD R2, R4, R2, LSL #2
 STR R0, [R2] ; xy
    LDR     R5, =8
 LDR R2, =2
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; zzz
    LDR     R5, =9
 LDR R2, =3
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; zzz
    LDR     R5, =7
 LDR R2, =6
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; xyz
    LDR     R5, =6
 LDR R2, =13
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; yyy
    LDR     R5, =12
 LDR R2, =23
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; yyy
 LDR R2, =2
 ADD R2, R4, R2, LSL #2
 LDR R0, [R2] ; zzz
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 STR R0, [R2] ; i
 LDR R2, =1
 ADD R2, R4, R2, LSL #2
 LDR R5, [R2] ; xy
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 LDR R5, [R2] ; i
 LDR R2, =1
 ADD R2, R4, R2, LSL #2
 LDR R5, [R2] ; xy
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; i
    LDR     R5, =2
    LDR     R5, =2
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; i
    LDR     R5, =10
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; i
StopTest
    B       StopTest
Main
    LDR     R0, =1          ; current lexic level
    LDR     R1, =0          ; number of local variables
    BL      enter           ; build new stack frame
    B       MainBody
   ;Name: i, Type: integer, Kind: var, Sort: scalar, address: 0
   ;Name: xy, Type: integer, Kind: const, Sort: scalar, address: 1
   ;Name: zzz, Type: integer, Kind: var, Sort: array, address: 2
   ;Rows: 3, Columns: 1
   ;Name: xyz, Type: integer, Kind: var, Sort: array, address: 6
   ;Rows: 6, Columns: 1
   ;Name: yyy, Type: integer, Kind: var, Sort: array, address: 13
   ;Rows: 4, Columns: 4
   ;Name: main, Type: undef, Kind: proc, Sort: scalar, address: 0

; Subroutine enter
; Construct stack frame for procedure
; Input: R0 - lexic level (LL)
;		 R1 - number of local variables
; Output: new stack frame

enter
	STR		R0, [TOP,#4]			; set lexic level
	STR		BP, [TOP,#12]			; and dynamic link
	; if called procedure is at the same lexic level as
	; calling procedure then its static link is a copy of
	; the calling procedure's static link, otherwise called
 	; procedure's static link is a copy of the static link 
	; found LL delta levels down the static link chain
    LDR		R2, [BP,#4]				; check if called LL (R0) and
	SUBS	R0, R2					; calling LL (R2) are the same
	BGT		enter1
	LDR		R0, [BP,#8]				; store calling procedure's static
	STR		R0, [TOP,#8]			; link in called procedure's frame
	B		enter2
enter1
	MOV		R3, BP					; load current base pointer
	SUBS	R0, R0, #1				; and step down static link chain
    BEQ     enter2-4                ; until LL delta has been reduced
	LDR		R3, [R3,#8]				; to zero
	B		enter1+4				;
	STR		R3, [TOP,#8]			; store computed static link
enter2
	MOV		BP, TOP					; reset base and top registers to
	ADD		TOP, TOP, #16			; point to new stack frame adding
	ADD		TOP, TOP, R1, LSL #2	; four bytes per local variable
	BX		LR						; return
	
	AREA	Memory, DATA, READWRITE
globals     SPACE 4096
stack      	SPACE 16384

	END