using OnlineCinemaContracts.Models.SettingsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Logic
{
    public interface IFileConverterLogic
    {
        void LoadData(FileSystemDiskModel model);
    }
}
