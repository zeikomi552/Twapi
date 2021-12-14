using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace InstallCustomAction
{
    [RunInstaller(true)]
    public partial class InstallerClass : System.Configuration.Install.Installer
    {
        public InstallerClass()
        {
            InitializeComponent();
        }
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            // 環境変数「path」を編集
            string currentPath;
            currentPath = System.Environment.GetEnvironmentVariable("path", System.EnvironmentVariableTarget.User);
            string installPath = this.Context.Parameters["InstallPath"];
            string path = installPath + @"\bin;";
            string raypath = ".;" + installPath + @"\lib;";
            if (currentPath == null)
            {
                currentPath = path;
            }
            else if (currentPath.EndsWith(";"))
            {
                currentPath += path;
            }
            else
            {
                currentPath += ";" + path;
            }
            // 環境変数を設定する
            System.Environment.SetEnvironmentVariable("path", currentPath, System.EnvironmentVariableTarget.User);
            System.Environment.SetEnvironmentVariable("raypath", raypath, System.EnvironmentVariableTarget.User);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            //System.Diagnostics.Debugger.Launch(); // .net 4.0
            ////System.Diagnostics.Debugger.Break();    // .net 3.5?
            // 環境変数「path」を編集
            string currentPath;
            currentPath = System.Environment.GetEnvironmentVariable("path", System.EnvironmentVariableTarget.User);
            string installPath = this.Context.Parameters["InstallPath"];
            installPath += @"\bin;";
            currentPath = currentPath.Replace(installPath, "");
            // 環境変数を削除する
            System.Environment.SetEnvironmentVariable("path", currentPath, System.EnvironmentVariableTarget.User);
            System.Environment.SetEnvironmentVariable("raypath", "", System.EnvironmentVariableTarget.User);
        }
    }
}
