using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Interrupts
{
    /// <summary>
    /// One 16-byte interrupt-descriptor-table entry (AMD64 long mode).
    /// <para>
    /// Layout:                                                <br/>
    /// • <c>OffsetLo</c>  – bits 15‥0   of handler address   <br/>
    /// • <c>Selector</c>  – code-segment selector            <br/>
    /// • <c>Ist</c>       – IST index (bits 2‥0), rest zero  <br/>
    /// • <c>TypeAttr</c>  – type/attributes (<c>0x8E</c> = present, DPL 0, interrupt gate) <br/>
    /// • <c>OffsetMid</c> – bits 31‥16 of handler address    <br/>
    /// • <c>OffsetHi</c>  – bits 63‥32 of handler address    <br/>
    /// • <c>Zero</c>      – reserved (must be zero)          <br/>
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct IdtEntry
    {
        public ushort OffsetLo;
        public ushort Selector;
        public byte Ist;        // 0 = use current RSP
        public byte TypeAttr;   // 0x8E = P|DPL0|InterruptGate
        public ushort OffsetMid;
        public uint OffsetHi;
        public uint Zero;

        /// <summary>
        /// Populates the descriptor with the given interrupt handler target.
        /// </summary>
        /// <param name="target">
        /// Pointer to an <c>extern "C"</c> interrupt stub (must preserve regs).
        /// </param>
        public void Set(delegate* unmanaged<void> target)
        {
            ulong addr = (ulong)target;
            OffsetLo = (ushort)addr;
            Selector = 0x08;          // Kernel CS (flat 64-bit code)
            Ist = 0;             // Same RSP
            TypeAttr = 0x8E;          // Present | DPL0 | 64-bit interrupt gate
            OffsetMid = (ushort)(addr >> 16);
            OffsetHi = (uint)(addr >> 32);
            Zero = 0;
        }
    }
}
