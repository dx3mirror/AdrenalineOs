.text
.global  Serial_Out8
Serial_Out8:            # RDI = port, RSI = value
    mov %di, %dx
    mov %sil, %al
    outb %al, %dx
    ret

.global  Serial_In8
Serial_In8:             # RDI = port
    mov %di, %dx
    inb  %dx, %al
    movzbq %al, %rax    # zero-extend to 64 bit
    ret
