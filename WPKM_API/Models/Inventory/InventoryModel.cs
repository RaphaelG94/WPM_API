using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models.Inventory
{

    public class InventoryModel { 
        public string Name { get; set; }
        public string Domain { get; set; }
        public string OS { get; set; }
        public string Version { get; set; }
        public string Model { get; set; }
        public string RAM { get; set; }
        public string SerialNumber { get; set; }
        public string CPU { get; set; }
        public string LastBootUpTime { get; set; }
        public string Timestamp { get; set; }
    }

    public class InventoryModelHW
    {
        public Win32_ComputerSystem Win32_ComputerSystem { get; set; }
        public Win32_BaseBoard Win32_BaseBoard { get; set; }
        public Win32_Processor Win32_Processor { get; set; }
    }

    public class Win32_ComputerSystem
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string SystemFamily { get; set; }
        public string TotalPhysicalMemory { get; set; }
    }

    public class Win32_BaseBoard
    {
        public string SerialNumber { get; set; }
    }

    public class Win32_Processor
    {
        public string Name { get; set; }
    } 

    public class Win32_DiskPartitionEntry
    {
        public int Index { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string InstallDate { get; set; }
        public string Availability { get; set; }
        public string ConfigManagerErrorCode { get; set; }
        public string ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string DeviceID { get; set; }
        public string ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public string LastErrorCode { get; set; }
        public string PNPDeviceID { get; set; }
        public string PowerManagementCapabilities { get; set; }
        public string PowerManagementSupported { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public string Access { get; set; }
        public int BlockSize { get; set; }
        public string ErrorMethodology { get; set; } 
        public int NumberOfBlocks { get; set; }
        public string Purpose { get; set; }
        public bool Bootable { get; set; }
        public bool PrimaryPartition { get; set; }
        public bool BootPartition { get; set; }
        public int DiskIndex { get; set; }
        public string HiddenSectors { get; set; }
        public string RewritePartition { get; set; }
        public int Size { get; set; }
        public int StartingOffset { get; set; }
        public string Type { get; set; }
        public string PSComputerName { get; set; }
    }

    public class InventoryModelOS
    {
        public Win32_OperatingSystem Win32_OperatingSystem { get; set; }
    }

    public class Win32_OperatingSystem
    {
        public string LastBootUpTime { get; set; }
    }

}
