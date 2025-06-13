    .text
    .global WriteCr3
WriteCr3:
    mov %rdi, %cr3
    ret

    .global SetCr4Pae
SetCr4Pae:
    mov %cr4, %rax
    or  $0x20, %rax          
    mov %rax, %cr4
    ret

    .global SetCr0PgWpPe
SetCr0PgWpPe:
    mov %cr0, %rax
    or  $0x80000001, %rax    
    mov %rax, %cr0
    ret

    .global Sti
Sti:
    sti
    ret

    .global Lapic_Eoi
Lapic_Eoi:
    mov $0xFEE000B0, %rax
    movl $0, (%rax)
    ret
