
using System.Collections.Generic;


namespace TYYongAutoPatcher.src.Models
{
    public class SettingModel
    {

        public ServerModel Server { get; set; } = new ServerModel();
        public GameModel Game { get; set; } = new GameModel();
        public List<PatchModel> PatchList { get; set; } = new List<PatchModel>();
        public LocalConfigModel LocalConfig { get; set; } = new LocalConfigModel();
    }

}
