    LDR     R5, =0
    LDR     R6, =2
MainBody
    LDR     R7, =8
 LDR R2, =3
 ADD R2, R4, R2, LSL #2
 STR R7, [R2] ; zzz
    LDR     R5, =8
 LDR R2, =4
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; zzz
StopTest
    B       StopTest
Main
    LDR     R0, =1          ; current lexic level
    LDR     R1, =0          ; number of local variables
    BL      enter           ; build new stack frame
    B       MainBody
   ;Name: i, Type: integer, Kind: var, Sort: scalar
   ;Name: xy, Type: integer, Kind: const, Sort: scalar
   ;Name: cc, Type: integer, Kind: const, Sort: scalar
   ;Name: zzz, Type: integer, Kind: var, Sort: array
   ;Rows: 3, Columns: 1
   ;Name: main, Type: undef, Kind: proc, Sort: scalar
