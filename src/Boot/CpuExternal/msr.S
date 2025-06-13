.text
.global  Msr_Read
Msr_Read:               
    mov %edi, %ecx
    rdmsr               
    shl $32, %rdx
    or  %rdx, %rax
    ret

.global  Msr_Write
Msr_Write:             
    mov %edi, %ecx
    mov %rsi, %rax
    shr $32, %rax
    mov %eax, %edx
    wrmsr
    ret
