using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.Services
{
    public interface IDialogService
    {
        Task Show(string title, string message, string closeText);

        Task<string> Show(string title, string question);

        Task<bool> Show(string title, string question, string okText, string cancelText);

        Task<string> ShowActionSheet(string title, string cancelText, string destroyText, string option1, string option2, string option3);
    }
}
