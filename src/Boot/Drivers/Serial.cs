using System.Runtime.CompilerServices;

namespace AdrenalineOs.Boot.Drivers;

/// <summary>
/// Minimal 16550-compatible UART driver for legacy PC serial ports.
/// <para>
///     • <see cref="Init"/>   – programs the UART (default COM1 @ 0x3F8) to
///       38 400 N 8 1 with FIFO enabled, IRQs masked.<br/>
///     • <see cref="PutChar"/>/ <see cref="Write"/> – blocking transmit helpers.<br/>
///     • <see cref="GetChar"/> – blocking receive helper.
/// </para>
/// All I/O is performed with port-mapped <c>IN/OUT</c> instructions supplied
/// by unmanaged stubs (<see cref="Out8"/>, <see cref="In8"/>).  Caller must
/// ensure CPL 0 and that the chosen base address maps a present UART.
/// </summary>
internal static unsafe class Serial
{
    /// <summary>Base I/O address of the selected COM port (e.g. 0x3F8 for COM1).</summary>
    private static ushort _base;

    // ── raw port-I/O helpers implemented in assembly ─────────────────────────
    [MethodImpl(MethodImplOptions.ForwardRef)] private static extern void Out8(ushort port, byte value);
    [MethodImpl(MethodImplOptions.ForwardRef)] private static extern byte In8(ushort port);

    /// <summary>
    /// Initializes the UART.
    /// </summary>
    /// <param name="comBase">
    /// I/O base address of the UART (default 0x3F8 = COM1).
    /// </param>
    public static void Init(ushort comBase = 0x3F8)
    {
        _base = comBase;

        Out8((ushort)(_base + 1), 0x00); // Disable all IRQs
        Out8((ushort)(_base + 3), 0x80); // Set DLAB = 1
        Out8((ushort)(_base + 0), 0x03); // Divisor = 3 → 38 400 baud
        Out8((ushort)(_base + 1), 0x00);
        Out8((ushort)(_base + 3), 0x03); // 8 data bits, no parity, 1 stop bit
        Out8((ushort)(_base + 2), 0xC7); // Enable FIFO, clear, 14-byte threshold
        Out8((ushort)(_base + 4), 0x0B); // Assert RTS, DTR, OUT2
    }

    /// <summary>
    /// Blocking transmit of a single character.
    /// </summary>
    public static void PutChar(char c)
    {
        while ((In8((ushort)(_base + 5)) & 0x20) == 0) { } // Wait for THR empty
        Out8(_base, (byte)c);
    }

    /// <summary>
    /// Blocking transmit of an entire string.
    /// </summary>
    public static void Write(string s)
    {
        foreach (char ch in s)
            PutChar(ch);
    }

    /// <summary>
    /// Blocking receive of a single character.
    /// </summary>
    /// <returns>The next byte received from the UART.</returns>
    public static char GetChar()
    {
        while ((In8((ushort)(_base + 5)) & 1) == 0) { } // Wait for data ready
        return (char)In8(_base);
    }
}
