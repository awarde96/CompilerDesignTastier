MainBody
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 LDR R5, [R2] ; i
    LDR     R5, =1
    BEQ     L1              ; jump on condition false
    LDR     R5, =1
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; i
    B       L2
L1
    LDR     R5, =2
    BEQ     L3              ; jump on condition false
    LDR     R5, =2
 LDR R2, =0
 ADD R2, R4, R2, LSL #2
 STR R5, [R2] ; i
    B       L4
L3
L4
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
   ;Name: main, Type: undef, Kind: proc, Sort: scalar, address: 0
