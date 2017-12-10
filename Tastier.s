MainBody
    LDR     R5, =3
    LDR R2, =3
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; xy
    LDR     R5, =2
    LDR R2, =33
    ADD R2, R4, R2, LSL #2
    STR R5, [R2] ; a
    LDR R2, =32
    ADD R2, R4, R2, LSL #2
    LDR R5, [R2] ; a
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
1 compilation error(s)
