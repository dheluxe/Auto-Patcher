using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TYYongAutoPatcher.src.Exceptions;
using TYYongAutoPatcher.src.Models;
using TYYongAutoPatcher.src.UI;

namespace TYYongAutoPatcher.src.Controllers
{
    class FileDownloadController
    {

        private MainController app;
        public WebClient client;

        public FileDownloadController(MainController app)
        {
            this.app = app;
        }

        public async Task DownloadFilesAysnc(Uri url, string savePath, PatchModel patch)
        {
            app.UpdateState(StateCode.Downloading);
            try
            {
                client = new WebClient();
                client.DownloadProgressChanged += app.ui.DownloadPatchProgressChangedHandler(patch);
                client.DownloadFileCompleted += app.ui.AsyncCompletedEventHandler(patch);
                // Add Multiple languages.
                for (var i = 0; i < app.ui.Messages.Count; i++)
                    app.ui.Messages[i].Add(new MessagesModel($"{app.Language.Get(i).UIComponent.Downloading} {patch.FileName}...", StateCode.Downloading));
                app.ui.UpdateMsg();

                Stopwatch timer = app.ui.GetDownloadTimer();
                timer.Restart();
                timer.Start();
                await client.DownloadFileTaskAsync(url, savePath);
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*************FileDownloadController.DownloadFilesAysnc(Uri url, string savePath, PatchModel patch): {ex.Message}");
                // Add Multiple languages.
                for (var i = 0; i < app.ui.Messages.Count; i++)
                    app.ui.Messages[i].Add(new MessagesModel($"{app.Language.Get(i).UIComponent.DownloadFailed} {patch.FileName}", StateCode.ErrorConnectingFail));
                app.ui.UpdateMsg();
                app.UpdateState(StateCode.FailedToDownload);
                throw new FileDownloadException();
            }
        }

        public async Task<string> DownloadStringAysnc(string url)
        {
            var client = new WebClient();
            var result = await client.DownloadStringTaskAsync(url);
            return result;
        }

        public async Task DownloadConfigAysnc(string url)
        {
            var client = new WebClient();
            client.DownloadStringCompleted += app.ui.DownloadStrinCompletedgAysncHandler;
            await client.DownloadStringTaskAsync(url);
        }
    }
}
