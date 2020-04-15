using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.Services
{
    public class DialogService : IDialogService
    {
        public async Task Show(string title, string msg, string closeText)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, msg, closeText);
        }

        public async Task<bool> Show(string title, string question, string okText, string cancelText)
        {
            return await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, question, okText, cancelText);
        }

        public async Task<string> Show(string title, string question)
        {
            return await Xamarin.Forms.Application.Current.MainPage.DisplayPromptAsync(title, question);
        }

        public async Task<string> ShowActionSheet(string title, string cancelText, string destroyText, string option1, string option2, string option3 )
        {
            if (option3 != "")
               return await Xamarin.Forms.Application.Current.MainPage.DisplayActionSheet(title, cancelText, destroyText != "" ? destroyText : null, option1, option2, option3);
            else if (option2 != "")
                return await Xamarin.Forms.Application.Current.MainPage.DisplayActionSheet(title, cancelText, destroyText != "" ? destroyText : null, option1, option2);
            else
                return await Xamarin.Forms.Application.Current.MainPage.DisplayActionSheet(title, cancelText, destroyText != "" ? destroyText : null, option1);
        }

    }
}