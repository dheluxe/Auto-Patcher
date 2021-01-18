using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TYYongAutoPatcher.src.UI;

namespace TYYongAutoPatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadLibs);
            Application.Run(new MainUI());
        }

        static Assembly LoadLibs(object sender, ResolveEventArgs args)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var requredDllName = $"{(new AssemblyName(args.Name)).Name}.dll";
            var resource = currentAssembly.GetManifestResourceNames().Where(s => s.EndsWith(requredDllName)).FirstOrDefault();

            if(resource != null)
            {
                using( var stream = currentAssembly.GetManifestResourceStream(resource)) 
                {
                    if (stream == null) return null;

                    var block = new byte[stream.Length];
                    stream.Read(block, 0, block.Length);
                    return Assembly.Load(block);
                }
            }

            return null;
        }
    }
}
