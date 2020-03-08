using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.Services
{
    public interface IDialogService
    {
        Task Show(string title, string msg, string closeText);
    }
}
