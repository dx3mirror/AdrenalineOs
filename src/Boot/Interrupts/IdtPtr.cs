using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Interrupts
{
    /// <summary>
    /// Descriptor expected by the <c>lidt</c> instruction in 64-bit mode.
    /// <para>
    /// • <see cref="Size"/> – size of the IDT in bytes minus 1 (i.e. the
    ///   maximum valid byte offset).<br/>
    /// • <see cref="Base"/> – linear (or physical in identity-mapped early
    ///   boot) address of the first <c>IdtEntry</c>.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct IdtPtr
    {
        /// <summary>
        /// Limit field loaded into IDTR.limit (IDT size – 1).
        /// </summary>
        public ushort Size;

        /// <summary>
        /// Address loaded into IDTR.base (pointer to the IDT).
        /// </summary>
        public ulong Base;
    }
}
