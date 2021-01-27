

using System;
using System.Collections;

namespace TYYongAutoPatcher.src.Models
{
    public class PatchModel : IComparable<PatchModel>
    {
        public double DownloadedSize { get; set; } = 0.0;
        public double ExtractedSize { get; set; } = 0.0;
        public double Size { get; set; } = 0.0;
        public double DownloadedPercentage { get; set; } = 0;
        public int ExtractedPercentage { get; set; } = 0;
        public double Version { get; set; }
        public string FileName { get; set; }
        public string[] DownloadLinks { get; set; }

        public int NoOfZippedFiles { get; set; } = 0;
        public int NoOfUnZippedFiles { get; set; } = 0;
        public double SizeOfZippedFiles { get; set; } = 0;
        public double SizeOfUnzippedFiles { get; set; } = 0;
        public bool IsUnzipSucceed { get; set; } = false;
        public double DownloadSpeedPerSecond { get; set; } = 0;
        public double TotalDownloadTime { get; set; } = 0;
        public int CompareTo(PatchModel other)
        {
            if (Version > other.Version) return 1;
            else if (Version < other.Version) return -1;
            else return 0;
        }
    }
}
