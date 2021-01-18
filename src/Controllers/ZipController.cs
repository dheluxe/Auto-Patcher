using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TYYongAutoPatcher.src.Models;
using TYYongAutoPatcher.src.UI;

namespace TYYongAutoPatcher.src.Controllers
{
    class ZipController
    {
        private MainController app;
        private int noOfUnzipped;
        public int NoOfUnzipped { get { return noOfUnzipped; } }
        public ZipController(MainController app)
        {
            this.app = app;
        }

        public async Task Unzip(string fileName, string targetDir, PatchModel patch)
        {
            try
            {
                using (var zip = ZipFile.Read(fileName))
                {
                    zip.ExtractProgress += app.ui.ExtractProgress(patch);
                    app.UpdateState(StateCode.Extracting);
                    patch.NoOfZippedFiles = zip.Count;
                    foreach (var entry in zip)
                    {
                        //app.ui.Add($"正在安裝: {entry.FileName}");
                        if (app.cts.IsCancellationRequested) app.cts.Token.ThrowIfCancellationRequested();
                        try
                        {
                            patch.NoOfUnZippedFiles++;
                            await Task.Run(() => entry.Extract(targetDir, ExtractExistingFileAction.OverwriteSilently));
                            patch.SizeOfUnzippedFiles += entry.UncompressedSize;
                            patch.SizeOfZippedFiles += entry.CompressedSize;

                        }
                        catch (Exception ex)
                        {
                            //ui.AddMsg($"無法刪除 {tempDir}", StateCode.Error);
                            Console.WriteLine($"*************1 ZipController.Unzip(string fileName, string targetDir, PatchModel patch) {ex.Message}");
                            app.UpdateState(StateCode.ErrorExtractingFail);
                            app.ui.AddMsg($"安裝失敗: {entry.FileName}", StateCode.ErrorExtractingFail);
                        }
                    }
                    app.ui.AddMsg($"已安裝更新包 {patch.FileName} - 共 {app.SizeToString(patch.Size)}", StateCode.Extracted);
                    patch.IsUnzipSucceed = true;
                    await app.DeleteTempFile(patch.FileName);
                    if(++noOfUnzipped == app.Setting.PatchList.Count) app.ui.CompeteUpdating();
                    app.UpdateLocalPatchVersion(patch);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*************2 ZipController.Unzip(string fileName, string targetDir, PatchModel patch) {ex.Message}");
                app.UpdateState(StateCode.ErrorExtractingFail);
                //app.ui.AddMsg(ex.Message, StateCode.ErrorExtractingFail);
            }
        }
    }
}
