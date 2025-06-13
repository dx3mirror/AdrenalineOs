using AdrenalineOs.Boot.MemMap;
using AdrenalineOs.Boot.Uefi;

namespace AdrenalineOs.Boot.Memory
{
    /// <summary>
    /// Simple physical-page allocator used during early boot.
    /// <para>
    /// Internally delegates to <see cref="BootMem"/> for individual
    /// successive single frames in a loop; the pages returned are not
    /// guaranteed to be physically contiguous—only the first frame of the
    /// sequence is returned to the caller.  <see cref="FreeMany"/> uses
    /// the firmware’s <c>FreePages</c> service, so the caller must supply
    /// the original first frame and the exact count previously allocated.
    /// </para>
    /// </summary>
    internal static unsafe class PageAllocator
    {
        private static EFI_BOOT_SERVICES* _bs;

        /// <summary>Stores the pointer to <c>EFI_BOOT_SERVICES</c>.</summary>
        public static void Init(EFI_BOOT_SERVICES* bs) => _bs = bs;

        public static void* AllocPage() => AllocMany(1);

        public static void FreePage(void* addr) => FreeMany(addr, 1);

        /// <summary>
        /// Allocates <paramref name="pages"/> KiB frames.  
        /// For <c>pages == 1</c> the call is forwarded directly to
        /// <see cref="BootMem.AllocFrame"/>; otherwise consecutive single-frame
        /// allocations are performed and the first frame’s address is
        /// returned.
        /// </summary>
        public static void* AllocMany(ulong pages)
        {
            if (pages is 1)
                return BootMem.AllocFrame();

            void* first = BootMem.AllocFrame();
            for (ulong i = 1; i < pages; i++)
                BootMem.AllocFrame();

            return first;
        }

        /// <summary>
        /// Releases <paramref name="pages"/> previously obtained via
        /// <see cref="AllocMany"/> back to the firmware.
        /// </summary>
        public static void FreeMany(void* addr, ulong pages) =>
            _bs->FreePages(addr, (nuint)pages);
    }
}
