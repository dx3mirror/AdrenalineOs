namespace AdrenalineOs.Boot
{
    internal unsafe struct BootInfo
    {
        public ulong MemMap;      
        public ulong MemMapSize;
        public ulong MemDescSize;
        public ulong RtcYyyyMmDd;

        public ulong Bitmap;       
        public ulong BitmapSize;   
    }
}
