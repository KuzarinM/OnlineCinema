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

        public  List<string> blackFolderList = new();

        public List<string> whiteSeasonList = new();

        public List<string> blackExtensionList = new();

        public List<string> blackPackageList = new();
    }
}
