using AdrenalineOs.Boot.Memory;

namespace AdrenalineOs.Boot.Paging
{
    /// <summary>
    /// Constructs the minimal 4-level page table hierarchy required to enter
    /// long mode:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Allocates a fresh PML4 and PDPT (one page each).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Installs the PDPT at PML4 slotn0 <b>and</b> slot;511 so
    ///       that both the low canonical half (**0x0000‥7FFF_FFFF_FFFF**) and
    ///       the high “kernel” half (**FFFF_8000_0000_0000‥FFFF_FFFF_FFFF_FFFF**)
    ///       share the same identity mapping.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Marks PDPT entry;0 as a 1;GiB “huge” page covering the
    ///       physical range 0;–;0x3FFF_FFFF (identity-mapped, RxW).
    ///     </description>
    ///   </item>
    /// </list>
    /// The method returns the physical address of the PML4; pass this to
    /// </summary>
    internal static unsafe class PageTables
    {
        private const ulong PresentRW = 0b11;   // P | RW
        private const ulong Huge1GiB = 1ul << 7;

        private static void* _pml4;

        /// <summary>
        /// Builds the PML4/PDPT and returns the PML4 pointer (physical).
        /// </summary>
        public static void* Setup()
        {
            _pml4 = PageAllocator.AllocPage(); // level-4
            void* pdpt = PageAllocator.AllocPage(); // level-3

            // Map both canonical halves via the same PDPT.
            ((ulong*)_pml4)[0] = ((ulong)pdpt) | PresentRW; // low half
            ((ulong*)_pml4)[511] = ((ulong)pdpt) | PresentRW; // high half

            // 1 GiB identity-mapped region starting at 0.
            ((ulong*)pdpt)[0] = 0x00000000ul | PresentRW | Huge1GiB;

            return _pml4;
        }
    }
}
