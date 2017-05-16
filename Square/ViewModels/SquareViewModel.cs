using System;
using System.Collections.ObjectModel;
using Square.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Square.ViewModels;

namespace Square
{
    public class SquareViewModel : BaseViewModel<SquareModel>
    {
        public ObservableCollection<CustomPin> Locations
        {
            get { return Model.Locations; }
            set
            {
                if(value != null)
                {
                    Model.Locations = value;
                    SetPropertyChanged();
                }    
            }
        }

        private string searchCriteria;
        public string SearchCriteria
        {
            get { return searchCriteria; }
            set
            {
                if(value != null)
                {
                    searchCriteria = value;
                    SetPropertyChanged();
                }   
            }
        }

        public ICommand SearchCommand { get; set; }

        public SquareViewModel()
        {
            Locations = new ObservableCollection<CustomPin>();
            SearchCommand = new Command(SearchPins);
            App.Current.DatabaseConnection.CreateTableAsync<Location>();
            App.Current.DatabaseConnection.InsertAsync(new Location
            {
                MapId = "1",
                Label = "Plaza de la constitución",
                Address = "20 de Noviembre S/N, Col. Centro, México, Ciudad de México.",
                Latitude = 19.432560,
                Longitude = -99.132746
            });
        }

        private async void SearchPins(object obj)
        {
            var pin = await App.Current.DatabaseConnection.Table<Location>().Where(p => p.Label.Contains(SearchCriteria)).FirstOrDefaultAsync();
            if(pin != null)
            {
                Locations.Add(new CustomPin
                {
                    Id = pin.MapId,
                    Pin = new Xamarin.Forms.Maps.Pin
                    {
                        Label = pin.Label,
                        Address = pin.Address,
                        Position = new Xamarin.Forms.Maps.Position(pin.Latitude, pin.Longitude)
                    }
                }); 
            }
        }
    }
}
