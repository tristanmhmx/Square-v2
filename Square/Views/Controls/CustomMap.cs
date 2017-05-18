using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Square.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Square
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(CustomMap));
        public ObservableCollection<CustomPin> ItemsSource
        {
            get { return (ObservableCollection<CustomPin>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public Action<string> Navigate = async (string obj) =>
        {
            //await Application.Current.MainPage.Navigation.PushAsync();
        };

    }
}
