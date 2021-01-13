using System;
using System.Collections.Generic;
using System.Text;

namespace TYYongAutoPatcher.src
{
    public class ServerConnection
    {
        public string OfficialWeb { get; set; }
        public string LeftWeb { get; set; }
        public string RightWeb { get; set; }
        public string Shop { get; set; }
        public string Event { get; set; }
        public string Register { get; set; }
        public string PatcherList { get; set; }
        public string PatchDataDir { get; set; }
    }

    public class Game
    {
        public string Exe { get; set; }
        public string Paramter { get; set; }
    }

    public class Patch
    {
        public string Version { get; set; }
    }


    public class Launcher
    {
        // get config.json url
        public string Server { get; set; } = "http://192.168.0.139/patch/config.json";

        // local patch version
        public int PatchVersion { get; set; } = 0;
    }

    public class LocalSetting
    {
        public Launcher Launcher { get; set; } = new Launcher();
        public string test = "456";
    }

    public class Setting
    {

        public ServerConnection ServerConnection { get; set; } = new ServerConnection();
        public Game Game { get; set; } = new Game();
        public Patch Patch { get; set; } = new Patch();
        public LocalSetting LocalSetting { get; set; } = new LocalSetting();
    }

}
