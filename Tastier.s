MainBody
    LDR     R5, =3
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; xy
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; xy
    MOV     R0, R5
    BL      TastierPrintInt
    LDR     R5, =8
    LDR R2, =3
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; zzz
    LDR R2, =3
    ADD R2, R4, R2, LSL #2
    LDR R0, [R2] ; zzz
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R0, [R2] ; i
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    MOV     R0, R5
    BL      TastierPrintInt
    LDR     R5, =9
    LDR R2, =4
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; zzz
    LDR     R5, =7
    LDR R2, =7
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; xyz
    LDR     R5, =6
    LDR R2, =14
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; yyy
    LDR     R5, =12
    LDR R2, =24
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; yyy
    LDR R2, =24
    ADD R2, R4, R2, LSL #2
    LDR R0, [R2] ; yyy
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R0, [R2] ; i
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    MOV     R0, R5
    BL      TastierPrintInt
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
    BEQ     L2              ; jump on condition false
    LDR     R5, =1
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L1
L2
    LDR     R6, =2
    CMP     R6, R5
    MOVEQ   R6, #1
    MOVNE   R6, #0
    MOVS    R6, R6          ; reset Z flag in CPSR
    BEQ     L3              ; jump on condition false
    LDR     R5, =2
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L1
L3
    LDR     R5, =10
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
L1
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; j
    MOV     R0, R5
    BL      TastierPrintInt
StopTest
    B       StopTest
Main
    LDR     R0, =1          ; current lexic level
    LDR     R1, =0          ; number of local variables
    BL      enter           ; build new stack frame
    B       MainBody
   ;Name: i, Type: integer, Kind: var, Sort: scalar, address: 0
   ;Name: j, Type: integer, Kind: var, Sort: scalar, address: 1
   ;Name: xy, Type: integer, Kind: const, Sort: scalar, address: 2
   ;Name: zzz, Type: integer, Kind: var, Sort: array, address: 3
   ;Rows: 3, Columns: 1
   ;Name: xyz, Type: integer, Kind: var, Sort: array, address: 7
   ;Rows: 6, Columns: 1
   ;Name: yyy, Type: integer, Kind: var, Sort: array, address: 14
   ;Rows: 4, Columns: 4
   ;Name: main, Type: undef, Kind: proc, Sort: scalar, address: 0
