.text
.global LoadIdt
LoadIdt:
    lidt (%rdi)
    ret

.macro  ISR name, with_err
.global \name
\name:
    cli
    .if \with_err==0
        pushq $0        
    .endif
    mov $msg, %rdi      
    call puts           
    hlt                 
msg: .asciz "INTERRUPT!\n"
.endm

ISR DefaultIsr, 0      
ISR PageFaultIsr, 1    

ISR TimerIsr, 0

isr_common:
    mov $isr_msg, %rdi
    call puts
    cmp $0x20, (%rsp)        
    jne 1f
    call Lapic_Eoi
1:  iretq
