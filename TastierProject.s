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
    LDR     R5, =3
    LDR R2, =3
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; xy
    LDR     R5, =2
    LDR R2, =33
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; a
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L1
    DCB     "Constant xy = ", 0
    ALIGN
L1
    LDR R2, =3
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; xy
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR     R5, =8
    LDR R2, =4
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; zzz
    LDR R2, =4
    ADD R2, R4, R2, LSL #2
    LDR R0, [R2] ; zzz
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R0, [R2] ; i
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L2
    DCB     "[0]zzz = ", 0
    ALIGN
L2
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR     R5, =9
    LDR R2, =5
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; zzz
    LDR     R5, =7
    LDR R2, =8
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; xyz
    LDR     R5, =6
    LDR R2, =15
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; yyy
    LDR     R5, =12
    LDR R2, =25
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; yyy
    LDR R2, =25
    ADD R2, R4, R2, LSL #2
    LDR R0, [R2] ; yyy
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R0, [R2] ; i
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L3
    DCB     "[2][2]yyy = ", 0
    ALIGN
L3
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR     R5, =2
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; i
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    LDR     R6, =1
    CMP     R6, R5
    MOVEQ   R6, #1
    MOVNE   R6, #0
    MOVS    R6, R6          ; reset Z flag in CPSR
    BEQ     L5              ; jump on condition false
    LDR     R5, =1
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
L5
    LDR     R6, =2
    LDR     R5, =2
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
L6
    LDR     R6, =3
    LDR     R5, =3
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L4
L7
    LDR     R6, =4
    LDR     R5, =4
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L4
L8
    LDR     R5, =10
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
L4
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L9
    DCB     "j = ", 0
    ALIGN
L9
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; j
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR     R5, =0
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; n
L10
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; n
    LDR     R6, =3
    CMP     R5, R6
    MOVLT   R5, #1
    MOVGE   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L11              ; jump on condition false
    B       L12
L13
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; n
    LDR     R6, =1
    ADD     R5, R5, R6
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; n
    B       L10
L12
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; j
    LDR     R6, =3
    ADD     R5, R5, R6
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L13
L11
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; n
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; j
    MOV     R0, R5
    BL      TastierPrintIntLf
StopTest
    B       StopTest
Main
    LDR     R0, =1          ; current lexic level
    LDR     R1, =0          ; number of local variables
    BL      enter           ; build new stack frame
    B       MainBody
   ;Name: i, Type: integer, Kind: var, Sort: scalar, address: 0, parent: null
   ;Name: j, Type: integer, Kind: var, Sort: scalar, address: 1, parent: null
   ;Name: n, Type: integer, Kind: var, Sort: scalar, address: 2, parent: null
   ;Name: xy, Type: integer, Kind: const, Sort: scalar, address: 3, parent: null
   ;Name: zzz, Type: integer, Kind: var, Sort: array, address: 4, parent: null
   ;Rows: 3, Columns: 1
   ;Name: xyz, Type: integer, Kind: var, Sort: array, address: 8, parent: null
   ;Rows: 6, Columns: 1
   ;Name: yyy, Type: integer, Kind: var, Sort: array, address: 15, parent: null
   ;Rows: 4, Columns: 4
   ;Name: a, Type: struct, Kind: var, Sort: scalar, address: 32, parent: null
   ;Name: b, Type: integer, Kind: var, Sort: scalar, address: 33, parent: a
   ;Name: c, Type: boolean, Kind: var, Sort: scalar, address: 34, parent: a
   ;Name: main, Type: undef, Kind: proc, Sort: scalar, address: 0, parent: null

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