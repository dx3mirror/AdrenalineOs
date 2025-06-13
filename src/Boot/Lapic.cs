using AdrenalineOs.Boot.CpuExternal;

namespace AdrenalineOs.Boot
{
    internal unsafe static class Lapic
    {
        private const uint IA32_APIC_BASE = 0x1B;
        private const ulong APIC_ENABLE = 1ul << 11;
        private const ulong APIC_DEFAULT_PHY = 0xFEE00000;

        private static uint* _base;

        private static uint* Reg(uint offset) => (uint*)((byte*)_base + offset);

        public static void Init()
        {
            ulong msr = Msr.Read(IA32_APIC_BASE);
            msr |= APIC_ENABLE;
            msr &= ~0xFFF_00000ul;
            msr |= APIC_DEFAULT_PHY;
            Msr.Write(IA32_APIC_BASE, msr);

            _base = (uint*)APIC_DEFAULT_PHY;

            *Reg(0x320) = 0x100 | 0xFF;   // LVT Timer masked

            *Reg(0x3E0) = 0b1011;         // Divide 1
            *Reg(0x320) = 0x20;           // LVT Timer vector 0x20, unmasked
            *Reg(0x380) = 10_000;         // Initial Count

            *Reg(0xB0) = 0;
        }

        public static void Eoi() => *Reg(0xB0) = 0;
    }
}
