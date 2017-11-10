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
 LDR R2, =2
 ADD R2, R4, R2, LSL #2
 LDR R0, [R2] ; zzz
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 STR R0, [R2] ; i
StopTest
    B       StopTest
Main
    LDR     R0, =1          ; current lexic level
    LDR     R1, =0          ; number of local variables
    BL      enter           ; build new stack frame
    B       MainBody
   ;Name: i, Type: integer, Kind: var, Sort: scalar, address: 1
   ;Name: xy, Type: integer, Kind: const, Sort: scalar, address: 1
   ;Name: zzz, Type: integer, Kind: var, Sort: array, address: 3
   ;Rows: 3, Columns: 1
   ;Name: xyz, Type: integer, Kind: var, Sort: array, address: 6
   ;Rows: 6, Columns: 1
   ;Name: main, Type: undef, Kind: proc, Sort: scalar, address: 1
