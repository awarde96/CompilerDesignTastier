; Procedure set
setBody
    LDR     R5, =22
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; i
    MOV     TOP, BP         ; reset top of stack
    LDR     BP, [TOP,#12]   ; and stack base pointers
    LDR     PC, [TOP]       ; return from set
set
    LDR     R0, =1          ; current lexic level
    LDR     R1, =0          ; number of local variables
    BL      enter           ; build new stack frame
    B       setBody
MainBody
    LDR     R5, =3
    LDR R2, =3
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; xy
    LDR     R5, =2
    LDR R2, =33
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; a
    LDR R2, =33
    ADD R2, R4, R2, LSL #2
    LDR R0, [R2] ; b
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R0, [R2] ; i
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L1
    DCB     "a.b := ", 0
    ALIGN
L1
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    MOV     R0, R5
    BL      TastierPrintIntLf
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L2
    DCB     "Constant xy = ", 0
    ALIGN
L2
    LDR R2, =3
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; xy
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    LDR     R6, =3
    CMP     R5, R6
    MOVGE   R5, #1
    MOVLT   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L3              ; jump on condition false
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L4
    DCB     "Error: Index out of bounds", 0
    ALIGN
L4
L3
    LDR     R7, =8
    LDR R2, =4
    ADD R2, R4, R2, LSL #2
    STR R7, [R2] ; zzz
    LDR     R5, =8
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; i
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    LDR     R6, =3
    CMP     R5, R6
    MOVGE   R5, #1
    MOVLT   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L5              ; jump on condition false
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L6
    DCB     "Error: Index out of bounds", 0
    ALIGN
L6
L5
    LDR     R7, =3
    LDR R2, =4
    ADD R2, R4, R2, LSL #2
    STR R7, [R2] ; zzz
    LDR R2, =4
    ADD R2, R4, R2, LSL #2
    LDR R0, [R2] ; zzz
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R0, [R2] ; i
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L7
    DCB     "[0]zzz = ", 0
    ALIGN
L7
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR     R5, =1
    LDR     R6, =3
    CMP     R5, R6
    MOVGE   R5, #1
    MOVLT   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L8              ; jump on condition false
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L9
    DCB     "Error: Index out of bounds", 0
    ALIGN
L9
L8
    LDR     R7, =9
    LDR R2, =4
    ADD R2, R4, R2, LSL #2
    STR R7, [R2] ; zzz
    LDR     R5, =0
    LDR     R6, =6
    CMP     R5, R6
    MOVGE   R5, #1
    MOVLT   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L10              ; jump on condition false
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L11
    DCB     "Error: Index out of bounds", 0
    ALIGN
L11
L10
    LDR     R7, =7
    LDR R2, =8
    ADD R2, R4, R2, LSL #2
    STR R7, [R2] ; xyz
    LDR     R5, =0
    LDR     R6, =4
    CMP     R5, R6
    MOVGE   R5, #1
    MOVLT   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L12              ; jump on condition false
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L13
    DCB     "Error: Index out of bounds", 0
    ALIGN
L13
L12
    LDR     R7, =6
    LDR R2, =15
    ADD R2, R4, R2, LSL #2
    STR R7, [R2] ; yyy
    LDR     R5, =2
    LDR     R6, =4
    CMP     R5, R6
    MOVGE   R5, #1
    MOVLT   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L14              ; jump on condition false
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L15
    DCB     "Error: Index out of bounds", 0
    ALIGN
L15
L14
    LDR     R7, =12
    LDR R2, =23
    ADD R2, R4, R2, LSL #2
    STR R7, [R2] ; yyy
    LDR R2, =25
    ADD R2, R4, R2, LSL #2
    LDR R0, [R2] ; yyy
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    STR R0, [R2] ; i
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L16
    DCB     "[2][2]yyy = ", 0
    ALIGN
L16
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
    BEQ     L18              ; jump on condition false
    LDR     R5, =1
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
L18
    LDR     R6, =2
    LDR     R5, =2
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
L19
    LDR     R6, =3
    LDR     R5, =3
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L17
L20
    LDR     R6, =4
    LDR     R5, =4
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L17
L21
    LDR     R5, =10
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
L17
    ADD     R0, PC, #4      ; string address
    BL      TastierPrintString
    B       L22
    DCB     "j = ", 0
    ALIGN
L22
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; j
    MOV     R0, R5
    BL      TastierPrintIntLf
    LDR     R5, =0
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; n
L23
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; n
    LDR     R6, =3
    CMP     R5, R6
    MOVLT   R5, #1
    MOVGE   R5, #0
    MOVS    R5, R5          ; reset Z flag in CPSR
    BEQ     L24              ; jump on condition false
    B       L25
L26
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; n
    LDR     R6, =1
    ADD     R5, R5, R6
    LDR R2, =2
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; n
    B       L23
L25
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; j
    LDR     R6, =3
    ADD     R5, R5, R6
    LDR R2, =1
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; j
    B       L26
L24
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
    ADD     R0, PC, #4      ; store return address
    STR     R0, [TOP]       ; in new stack frame
    B       set
    LDR R2, =0
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; i
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
   ;Name: check, Type: struct, Kind: var, Sort: scalar, address: 35, parent: null
   ;Name: set, Type: undef, Kind: proc, Sort: scalar, address: 0, parent: null
   ;Name: main, Type: undef, Kind: proc, Sort: scalar, address: 0, parent: null
