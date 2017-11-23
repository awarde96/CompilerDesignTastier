MainBody
    LDR     R5, =3
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; xy
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L1
    DCB     "Constant xy = ", 0
    ALIGN
L1
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; xy
    MOV     R0, R5
    BL      TastierPrintIntLf
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
    CMP     R6, R5
    MOVEQ   R6, #1
    MOVNE   R6, #0
    MOVS    R6, R6          ; reset Z flag in CPSR
    BEQ     L6              ; jump on condition false
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
