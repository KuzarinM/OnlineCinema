using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.SettingsModel
{
    public class FileSystemDiskModel
    {
        public string drivePath = string.Empty;

        public string posterDir = "E:/SERVICES/Картинки/Плитки";

        public string bacgroundDir = "E:/SERVICES/Картинки/Фоны";

        public string defaultImg = "E:/SERVICES/Картинки/default.png";

        public  List<string> blackFolderList = new();

        public List<string> whiteSeasonList = new();

        public List<string> blackExtensionList = new();

        public List<string> blackPackageList = new();
    }
}
