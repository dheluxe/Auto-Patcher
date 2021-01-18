using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TYYongAutoPatcher.src.Models
{
    class PatchReportModel
    {
        public int NoOfDownloadedPatches { get; set; } = 0;
        public int NoOfUnzipped { get; set; } = 0;

        public double DownloadSpeedPerSecond { get; set; } = 0;
        public string DownloadSpeedPerSecondToString { get; set; } = "";

        public double AvegragedownloadSpeedPerSecond { get; set; } = 0;
        public string AvegragedownloadSpeedPerSecondToString { get; set; } = "";

        public double TotalDownloadTime { get; set; } = 0;

        public double SizeOfDownloadPatches { get; set; } = 0;

        public double SizeOfDownloadedPatches { get; set; } = 0;
        public string SizeOfDownloadedPatchesToString { get; set; } = "";

        public double SizeOfExtractedZippedFiles { get; set; } = 0;
        public string SizeOfExtractedZippedFilesToString { get; set; } = "";

        public double SizeOfExtractedUnzippedFiles { get; set; } = 0;
        public string SizeOfExtractedUnzippedFilesToString { get; set; } = "";

        public double TotalPercentageOfDownloaded { get; set; } = 0;
        public string TotalPercentageOfDownloadedToString { get; set; } = "";

        public double TotalPercentageOfExtracted { get; set; } = 0;
        public string TotalPercentageOfExtractedToString { get; set; } = "";


    }
}
