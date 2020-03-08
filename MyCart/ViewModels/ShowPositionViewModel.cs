using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.ViewModels
{
    public class ShowPositionViewModel : ViewModelBase
    {
        FrameNumberStatus _status;
        public FrameNumberStatus Status
        {
            get => _status;
            set
            {
                SetProperty(ref _status, value);
            }
        }


        public ShowPositionViewModel(FrameNumberStatus status, INavigationService navigationService, IAnalyticsService analyticsService) : base(navigationService, analyticsService)
        {
            Status = status;
        }


    }
}
