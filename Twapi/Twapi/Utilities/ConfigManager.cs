using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Utilities
{
    class ConfigManager
    {
        #region Configファイル用フォルダパス
        /// <summary>
        /// Configファイル用フォルダパス
        /// </summary>
        public static string ConfigDir
        {
            get
            {
                string path = PathManager.GetApplicationFolder();
                string config_dir = Path.Combine(path, "Config");
                return config_dir;
            }
        }
        #endregion

        #region キーファイルパス
        /// <summary>
        /// キーファイルパス
        /// </summary>
        public static string Keys
        {
            get
            {
                string path = PathManager.GetApplicationFolder();
                string config_dir = Path.Combine(path, "Config");
                PathManager.CreateDirectory(config_dir);
                string config_path = Path.Combine(path, "Keys");

                return config_path;
            }
        }
        #endregion
    }
}
