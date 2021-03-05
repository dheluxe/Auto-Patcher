
using System.Globalization;

namespace TYYongAutoPatcher.src.Models
{
    public class LocalConfigModel
    {
        public LauncherModel Launcher { get; set; } = new LauncherModel();

        public string Language { get; set; }

        public LocalConfigModel()
        {
            Language = CultureInfo.InstalledUICulture.Name;
        }
    }
}
