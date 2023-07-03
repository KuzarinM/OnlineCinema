using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaStorageDatabase.DiskFileSystem;
using OnlineCinemaStorageDatabase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaConsileView
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileConverter Loader = new();

            Loader.LoadData(new FileSystemDiskModel
            {
                drivePath = "E:\\",
                blackFolderList = new()
                {
                    "EKT_PVR",
                    "EKT_PVR_TIMESHIFT",
                    "значки",
                    "System Volume Information"
                },
                whiteSeasonList = new()
                {
                    "сезон"
                },
                blackExtensionList = new()
                {
                    ".7z",
                    ".ini",
                    ".db",
                    ".srt"
                },
                blackPackageList = new()
                {
                    "фильмы"
                }
            });
        }
    }
}
