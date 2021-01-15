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
        private int NoOfUnzipped { get; set; } = 0;
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
                    app.State = StateCode.Extracting;
                    patch.NoOfZippedFiles = zip.Count;
                    NoOfUnzipped++;
                    foreach (var entry in zip)
                    {
                        //app.ui.Add($"正在解壓: {entry.FileName}");
                        if (app.cts.IsCancellationRequested) app.cts.Token.ThrowIfCancellationRequested();
                        try
                        {
                            patch.NoOfUnZippedFiles++;
                            await Task.Run(() => entry.Extract(targetDir, ExtractExistingFileAction.OverwriteSilently));

                        }
                        catch (Exception ex)
                        {
                            app.UpdateState(StateCode.ErrorExtractingFail);
                            app.ui.AddMsg($"解壓失敗: {entry.FileName}", StateCode.ErrorExtractingFail);
                            NoOfUnzipped--;
                        }
                    }
                    app.ui.AddMsg($"己安裝更新包 {patch.FileName}", StateCode.Success);
                    await app.DeleteTempFile(patch.FileName);
                    if(NoOfUnzipped == zip.Count) app.ui.UpdateProgress();
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
