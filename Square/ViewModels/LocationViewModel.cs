﻿using System;
using System.IO;
using System.Windows.Input;
using Square.Interfaces;
using Square.Models;
using Xamarin.Forms;

namespace Square.ViewModels
{
    public class LocationViewModel : BaseViewModel<LocationModel>
    {
		public string Id
		{
			get
			{
				return Model.id;
			}
			set
			{
				if (value != null)
				{
					Model.id = value;
					SetPropertyChanged();
				}
			}
		}
		public string Label
		{
			get
			{
				return Model.label;
			}
			set
			{
				if (value != null)
				{
					Model.label = value;
					SetPropertyChanged();
				}
			}
		}

		public string Address
		{
			get
			{
				return Model.address;
			}
			set
			{
				if (value != null)
				{
					Model.address = value;
					SetPropertyChanged();
				}
			}
		}

		public double Latitude
		{
			get
			{
				return Model.latitude;
			}
			set
			{
				if (value != 0)
				{
					Model.latitude = value;
					SetPropertyChanged();
				}
			}
		}

		public double Longitude
		{
			get
			{
				return Model.longitude;
			}
			set
			{
				if (value != 0)
				{
					Model.longitude = value;
					SetPropertyChanged();
				}
			}
		}

        public ImageSource PictureUrl
        {
            get
            {
                return Model.pictureUrl;
            }
            set
            {
                if (value != null)
                {
                    Model.pictureUrl = value;
                    SetPropertyChanged();
                }
            }
        }

		public ICommand TakePicture { get; set; }
		public ICommand Navigate { get; set; }

		public LocationViewModel(string id)
		{
			var pin = App.Current.DatabaseConnection.Table<Location>().Where(l => l.MapId == id).FirstOrDefaultAsync().Result;
			if (pin != null)
			{
				Id = pin.MapId;
				Label = pin.Label;
				Address = pin.Address;
				Latitude = pin.Latitude;
				Longitude = pin.Longitude;
			}
			TakePicture = new Command(ReadPicture);
			Navigate = new Command(NavigateTo);
            PictureUrl = ImageSource.FromFile("noimage");
		}

        private void NavigateTo(object obj)
        {
            
        }

        private async void ReadPicture(object obj)
        {
            var photoService = DependencyService.Get<IPhotoService>();
            if(photoService.IsCameraAvailable)
            {
                var photo = await photoService.TakePhotoAsync();
                PictureUrl = ImageSource.FromStream(() => new MemoryStream(photo));
            }
        }
    }
}
