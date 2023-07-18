using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.SettingsModel
{
    public class FileSystemSingletoneModel
    {
        #region Singletone
        private static FileSystemSingletoneModel? _instance;
        public static FileSystemSingletoneModel Instance()
        {
            if (_instance == null)
            {
                _instance = new FileSystemSingletoneModel();
            }
            return _instance;
        }
        #endregion Singletone

        public string drivePath = "E:\\";

        public string tmpDirPath = "E:/SERVICES/TMP";

        public string posterDir = "E:/SERVICES/Картинки/Плитки";

        public string bacgroundDir = "E:/SERVICES/Картинки/Фоны";

        public string defaultImg = "E:/SERVICES/Картинки/default.png";

        public List<string> blackFolderList = new()
            {
                "EKT_PVR",
                "EKT_PVR_TIMESHIFT",
                "значки",
                "System Volume Information",
                "SERVICES"
            };

        public List<string> whiteSeasonList = new()
            {
                "сезон",
                "диск"
            };

        public List<string> blackExtensionList = new()
            {
                ".7z",
                ".ini",
                ".db",
                ".srt",
                ".WMA"
            };

        public List<string> blackPackageList = new()
            {
                "фильмы"
            };

        public static void update(FileSystemSingletoneModel model)
        {
            Instance();
            if (!model.drivePath.IsNullOrEmpty()) _instance.drivePath = model.drivePath;
            if (!model.posterDir.IsNullOrEmpty()) _instance.posterDir = model.posterDir;
            if (!model.bacgroundDir.IsNullOrEmpty()) _instance.bacgroundDir = model.bacgroundDir;
            if (!model.defaultImg.IsNullOrEmpty()) _instance.defaultImg = model.defaultImg;
            if (model.blackFolderList != null && model.blackFolderList.Count > 0) _instance.blackFolderList = model.blackFolderList;
            if (model.whiteSeasonList != null && model.whiteSeasonList.Count > 0) _instance.whiteSeasonList = model.whiteSeasonList;
            if (model.blackExtensionList != null && model.blackExtensionList.Count > 0) _instance.blackExtensionList = model.blackExtensionList;
            if (model.blackPackageList != null && model.blackPackageList.Count > 0) _instance.blackPackageList = model.blackPackageList;
        }
    }
}
