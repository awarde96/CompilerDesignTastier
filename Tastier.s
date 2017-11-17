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
