using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AdrenalineOs.Boot.Memory;

namespace AdrenalineOs.Boot.Interrupts;

/// <summary>
/// Builds and installs a 256-entry Interrupt Descriptor Table for long-mode
/// x86-64.
/// <para>
/// • Allocates a fresh 4 KiB page via <see cref="PageAllocator.AllocPage"/>. <br/>
/// • Initialises every vector to <see cref="DefaultIsr"/>.                <br/>
/// • Overrides vector 14 with <see cref="PageFaultIsr"/> and vector 0x20
///   with <see cref="TimerIsr"/>.                                         <br/>
/// • Loads the descriptor with <c>lidt</c> using <see cref="LoadIdt"/>.   <br/>
/// </para>
/// Call once during early bootstrap before enabling interrupts.
/// </summary>
internal static unsafe class IdtManager
{
    private static IdtEntry* _idt;

    /// <summary>
    /// Allocates and populates the IDT, then activates it with <c>lidt</c>.
    /// </summary>
    public static void Setup()
    {
        _idt = (IdtEntry*)PageAllocator.AllocPage();

        for (int i = 0; i < 256; i++)
            _idt[i].Set(&DefaultIsr);

        _idt[14].Set(&PageFaultIsr); // #PF
        _idt[0x20].Set(&TimerIsr);   // PIT

        var idtPtr = new IdtPtr
        {
            Size = (ushort)(256 * sizeof(IdtEntry) - 1),
            Base = (ulong)_idt
        };
        LoadIdt(&idtPtr);
    }

    /// <summary>Executes the <c>lidt</c> instruction with the supplied pointer.</summary>
    [MethodImpl(MethodImplOptions.ForwardRef)]
    private static extern void LoadIdt(IdtPtr* ptr);

    // ── Unmanaged interrupt stubs 
    [UnmanagedCallersOnly] private static extern void DefaultIsr();
    [UnmanagedCallersOnly] private static extern void PageFaultIsr();
    [UnmanagedCallersOnly] private static extern void TimerIsr();
}
