using System.Runtime.InteropServices;
using AdrenalineOs.Boot.Uefi;
using AdrenalineOs.Boot.Memory;
using AdrenalineOs.Boot.Fs;
using AdrenalineOs.Boot.Elf;
using AdrenalineOs.Boot.CpuExternal;
using AdrenalineOs.Boot.Paging;
using AdrenalineOs.Boot.Interrupts;
using AdrenalineOs.Boot.MemMap;
using AdrenalineOs.Boot.Heap;
using AdrenalineOs.Boot.Drivers;

namespace AdrenalineOs.Boot;

internal unsafe static class BootLoader
{
    private static delegate* unmanaged<void*, char*, ulong> _out;
    private static void* _con;
    private static EFI_RUNTIME_SERVICES* _rt;

    private static void PrintLine(string s)
    {
        fixed (char* p = (s + "\r\n").ToCharArray())
            _out(_con, p);
    }

    [UnmanagedCallersOnly(EntryPoint = "efi_main")]
    private static ulong Main(void* image, EFI_SYSTEM_TABLE* st)
    {
        InitDelegates(st);
        PageAllocator.Init(st->BootServices);

        Serial.Init();                       
        Serial.Write("=== COM online ===\r\n");

        PrintLine("Boot OK");
        Serial.Write("Boot OK VGA+COM\r\n");

        PrintLine("Boot OK");
        PrintCurrentTime();

        EFI_FILE_PROTOCOL* kFile =
            FileSystem.OpenKernel(image, st, out ulong kSize);
        PrintLine($"Kernel {kSize} bytes");

        void* elfImg = PageAllocator.AllocMany((kSize + 0xFFF) >> 12);
        ulong toRead = kSize;
        kFile->Read(kFile, &toRead, elfImg);

        PhdrInfo ph = ElfLoader.Parse(elfImg);
        PageStager.Load(ph.Table, ph.Count, kFile);
        PrintLine("ELF staged.");

        var (mapPtr, mapSz, descSz) = BootExit.CollectMapAndExit(st, image);
        BootMem.Init((EFI_MEMORY_DESCRIPTOR*)mapPtr, mapSz, descSz);
        PrintLine("ExitBootServices OK");

        void* pml4 = PageTables.Setup();     //PML4 + PDPT
        Cpu.EnablePaging(pml4);              // CR3/CR4/CR0
        PrintLine("Paging ON");
        KHeap.Init();
        void* a = KHeap.Alloc(24);
        void* b = KHeap.Alloc(100);
        PrintLine($"heap {((ulong)a):X} {((ulong)b):X}");
        KHeap.Free(b, 100);
        KHeap.Free(a, 24);

        PrintLine("Heap OK");
        Lapic.Init();
        IdtManager.Setup();
        Cpu.Sti();
        PrintLine("IDT ready");

        BootInfo* bi = (BootInfo*)PageAllocator.AllocPage();
        *bi = new BootInfo
        {
            MemMap = (ulong)mapPtr,
            MemMapSize = mapSz,
            MemDescSize = descSz,
            Bitmap = (ulong)BootMem.GetBitmapPtr(),
            BitmapSize = BootMem.GetBitmapSize()
        };

        KernelJumper.Go((void*)ph.Entry, bi);
        return 0;               
    }

    private static void PrintCurrentTime()
    {
        EFI_TIME t = default;
        _rt->GetTime(&t, null);

        // "HH:MM:SS\r\n\0" = 11 + term = 12
        Span<char> buf = stackalloc char[12];

        Format2(t.Hour, buf, 0);
        buf[2] = ':';
        Format2(t.Minute, buf, 3);
        buf[5] = ':';
        Format2(t.Second, buf, 6);
        buf[8] = '\r';
        buf[9] = '\n';
        buf[10] = '\0';

        fixed (char* p = buf)
            _out(_con, p);
    }

    private static void Format2(byte v, Span<char> dst, int pos)
    { dst[pos] = (char)('0' + v / 10); dst[pos + 1] = (char)('0' + v % 10); }

    private static void InitDelegates(EFI_SYSTEM_TABLE* st)
    {
        _con = st->ConOut;
        _out = *(delegate* unmanaged<void*, char*, ulong>*)(*(void**)st->ConOut);
        _rt = st->RuntimeServices;
    }
}
