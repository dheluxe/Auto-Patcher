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
                    noOfUnzipped++;
                    foreach (var entry in zip)
                    {
                        //app.ui.Add($"正在安裝: {entry.FileName}");
                        if (app.cts.IsCancellationRequested) app.cts.Token.ThrowIfCancellationRequested();
                        try
                        {
                            patch.NoOfUnZippedFiles++;
                            await Task.Run(() => entry.Extract(targetDir, ExtractExistingFileAction.OverwriteSilently));
                            patch.SizeOfUnZippedFiles += entry.UncompressedSize;

                        }
                        catch (Exception ex)
                        {
                            app.UpdateState(StateCode.ErrorExtractingFail);
                            app.ui.AddMsg($"安裝失敗: {entry.FileName}", StateCode.ErrorExtractingFail);
                            noOfUnzipped--;
                        }
                    }
                    app.ui.AddMsg($"己安裝更新包 {patch.FileName}", StateCode.Success);
                    patch.IsUnzipSucceed = true;
                    await app.DeleteTempFile(patch.FileName);
                    if(NoOfUnzipped == app.Setting.PatchList.Count) app.ui.UpdateProgress();
                    app.UpdateLocalPatchVersion(patch);

                }
            }
            catch (Exception e)
            {
                app.UpdateState(StateCode.ErrorExtractingFail);
                app.ui.AddMsg(e.Message, StateCode.ErrorExtractingFail);
            }
        }
    }
}
