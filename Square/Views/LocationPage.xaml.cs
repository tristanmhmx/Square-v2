using System;
using System.Collections.Generic;
using Square.ViewModels;
using Xamarin.Forms;

namespace Square.Views
{
    public partial class LocationPage : ContentPage
    {
		public LocationPage(string id)
		{
			BindingContext = new LocationViewModel(id);
			InitializeComponent();
		}
    }
}
