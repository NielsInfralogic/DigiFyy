﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigiFyy.Models;
using DigiFyy.Helpers;
using DigiFyy.Services;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    /// <summary>
    /// ViewModel of AboutUs templates.
    /// </summary>
    public class AboutUsViewModel : ViewModelBase
    {
        #region Fields

        private string productDescription;

        private string productVersion;

        private string productIcon;

        private string cardsTopImage;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="T:DigiFyy.ViewModels.About.AboutUsViewModel"/> class.
        /// </summary>
        public AboutUsViewModel()
        {
            ProductDescription =
                "Situated in the heard of Smith-town, Acme Products, Inc., has a long-standing tradition of selling the best products while providing the fastest service on the market. Since 1952, we’ve helped our customers identify their needs, understand their wants, and capture their dreams.";
            ProductIcon = "xamarin.jpg";
            ProductVersion = "1.0";
            CardsTopImage = "xamarin.jpg";

            this.ItemSelectedCommand = new Helpers.Command(this.ItemSelected);
        }

        #endregion



        #region Properties

        /// <summary>
        /// Gets or sets the top image source of the About us with cards view.
        /// </summary>
        /// <value>Image source location.</value>
        public string CardsTopImage
        {
            get { return this.cardsTopImage; }

            set
            {
                SetProperty(ref cardsTopImage, value);
            }
        }

        /// <summary>
        /// Gets or sets the description of a product or a company.
        /// </summary>
        /// <value>The product description.</value>
        public string ProductDescription
        {
            get { return this.productDescription; }

            set
            {
                SetProperty(ref productDescription, value);
            }
        }

        /// <summary>
        /// Gets or sets the product icon.
        /// </summary>
        /// <value>The product icon.</value>
        public string ProductIcon
        {
            get { return this.productIcon; }

            set
            {
                SetProperty(ref productIcon, value);
            }
        }

        /// <summary>
        /// Gets or sets the product version.
        /// </summary>
        /// <value>The product version.</value>
        public string ProductVersion
        {
            get { return this.productVersion; }

            set
            {
                SetProperty(ref productVersion, value);
            }
        }

        /// <summary>
        /// Gets or sets the employee details.
        /// </summary>
        /// <value>The employee details.</value>
        public ObservableCollection<AboutUsModel> EmployeeDetails { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Helpers.Command ItemSelectedCommand { get; set; }

        #endregion

        #region Methods

       

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            // Do something
        }

        #endregion
    }
}