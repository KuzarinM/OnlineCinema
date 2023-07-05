using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Storage
{
    public interface ITagStorage
    {
        List<TagViewModel>? GetFullList();
    }
}
