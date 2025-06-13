using AdrenalineOs.Boot.Memory;
using AdrenalineOs.Boot.Uefi;
using System.Runtime.CompilerServices;

namespace AdrenalineOs.Boot.MemMap
{
    /// <summary>
    /// **Boot-time physical frame allocator** backed by a bit-map extracted
    /// from the UEFI memory map.
    ///
    /// *Workflow*  
    /// 1. <see cref="Init"/> scans the firmware-supplied memory map for
    ///    <c>EfiConventionalMemory</c> regions (type 7) and determines the
    ///    highest frame number.  
    /// 2. A bit-map large enough to tag every 4 KiB frame is allocated via
    ///    <see cref="PageAllocator.AllocMany"/> and initialised to “all
    ///    used”.  
    /// 3. Bits corresponding to conventional RAM are cleared (free); the
    ///    memory map itself and the bit-map’s own frames are then marked
    ///    back as used so the allocator will never return them.  
    ///
    /// *Allocation strategy*  
    /// A naive first-fit linear scan is used by <see cref="AllocFrame"/>; the
    /// first zero bit found is set and its physical address is returned.
    /// <see cref="FreeFrame"/> simply clears the corresponding bit.
    /// </summary>
    internal static unsafe class BootMem
    {
        private static byte* _bitmap;       // 1 bit = 1 page
        private static ulong _pages;        // total number of pages tracked

        private const int PageShift = 12;   // 4 KiB

        /// <summary>
        /// Builds the bit-map from <paramref name="map"/> (UEFI memory map).
        /// Should be called once, very early, before any page allocations.
        /// </summary>
        public static void Init(
            EFI_MEMORY_DESCRIPTOR* map,
            ulong mapSz,
            ulong descSz)
        {
            ulong maxPage = 0;
            for (EFI_MEMORY_DESCRIPTOR* m = map;
                 (byte*)m < (byte*)map + mapSz;
                 m = (EFI_MEMORY_DESCRIPTOR*)((byte*)m + descSz))
            {
                if (m->Type != 7) continue;                       // not Conventional
                ulong top = (m->PhysicalStart >> PageShift) + m->NumberOfPages;
                if (top > maxPage) maxPage = top;
            }

            _pages = maxPage;
            ulong bmpBytes = (_pages + 7) / 8;                    // round-up

            _bitmap = (byte*)PageAllocator.AllocMany((bmpBytes + 0xFFF) >> PageShift);
            for (ulong i = 0; i < bmpBytes; i++) _bitmap[i] = 0xFF;

            for (EFI_MEMORY_DESCRIPTOR* m = map;
                 (byte*)m < (byte*)map + mapSz;
                 m = (EFI_MEMORY_DESCRIPTOR*)((byte*)m + descSz))
            {
                if (m->Type != 7) continue;
                ulong first = m->PhysicalStart >> PageShift;
                for (ulong p = 0; p < m->NumberOfPages; p++)
                    MarkFree(first + p);
            }

            for (ulong p = (ulong)map >> PageShift;
                 p < ((ulong)map + mapSz + 0xFFF) >> PageShift;
                 p++)
                MarkUsed(p);

            for (ulong p = (ulong)_bitmap >> PageShift;
                 p < ((ulong)_bitmap + bmpBytes + 0xFFF) >> PageShift;
                 p++)
                MarkUsed(p);
        }

        /// <summary>
        /// Allocates one 4 KiB frame; returns <c>null</c> on OOM.
        /// </summary>
        public static void* AllocFrame()
        {
            for (ulong idx = 0; idx < _pages; idx++)
                if (TestAndSet(idx))
                    return (void*)(idx << PageShift);
            return null;                                          // out of memory
        }

        /// <summary>
        /// Frees a frame previously obtained from <see cref="AllocFrame"/>.
        /// </summary>
        public static void FreeFrame(void* phys)
        {
            ulong idx = (ulong)phys >> PageShift;
            _bitmap[idx >> 3] &= (byte)~(1 << (int)(idx & 7));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TestAndSet(ulong idx)
        {
            byte mask = (byte)(1 << (int)(idx & 7));
            ref byte b = ref _bitmap[idx >> 3];
            if ((b & mask) != 0) return false;                     
            b |= mask;
            return true;
        }

        public static void* GetBitmapPtr() => _bitmap;
        public static ulong GetBitmapSize() => (_pages + 7) / 8;

        private static void MarkFree(ulong idx) =>
            _bitmap[idx >> 3] &= (byte)~(1 << (int)(idx & 7));

        private static void MarkUsed(ulong idx) =>
            _bitmap[idx >> 3] |= (byte)(1 << (int)(idx & 7));
    }
}
