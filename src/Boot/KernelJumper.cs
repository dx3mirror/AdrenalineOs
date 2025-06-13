namespace AdrenalineOs.Boot
{
    internal unsafe static class KernelJumper
    {
        public static void Go(void* entry, BootInfo* info)
        {
            var jump = (delegate* unmanaged<BootInfo*, void>)entry;
            jump(info);           
            while (true) { }       
        }
    }
}
