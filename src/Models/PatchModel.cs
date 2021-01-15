

using System;

namespace TYYongAutoPatcher.src.Models
{
    public class PatchModel
    {
        public double DownloadedSize { get; set; } = 0.0;
        public double ExtractedSize { get; set; } = 0.0;
        public double Size { get; set; } = 0.0;
        public int DownloadedPercentage { get; set; } = 0;
        public int ExtractedPercentage { get; set; } = 0;
        public int Verison { get; set; }
        public string FileName { get; set; }

        public int NoOfZippedFiles { get; set; } = 0;
        public int NoOfUnZippedFiles { get; set; } = 0;



    }
}
