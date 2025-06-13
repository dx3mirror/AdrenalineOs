namespace AdrenalineOs.Boot.Uefi
{
    internal struct EFI_TIME
    {
        public ushort Year;      
        public byte Month;     
        public byte Day;       
        public byte Hour;      
        public byte Minute;    
        public byte Second;    
        public byte Pad1;
        public uint Nanosecond;
        public short TimeZone;
        public byte Daylight;
        public byte Pad2;
    }
}
