﻿using Ionic.Zip;
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
                    foreach (var entry in zip)
                    {
                        //app.ui.Add($"正在解壓: {entry.FileName}");
                        try
                        {
                            patch.NoOfUnZippedFiles++;
                            await Task.Run(() => entry.Extract(targetDir, ExtractExistingFileAction.OverwriteSilently));

                        }
                        catch (Exception ex)
                        {
                            app.UpdateState(StateCode.ErrorExtractingFail);
                            app.ui.AddMsg($"解壓失敗: {entry.FileName}", StateCode.ErrorExtractingFail);
                        }
                    }
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