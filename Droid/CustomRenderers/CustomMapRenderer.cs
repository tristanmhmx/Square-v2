using System;
using System.Collections.Specialized;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Square;
using Square.Droid.CustomRenderers;
using Square.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Maps;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Square.Droid.CustomRenderers
{
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        private GoogleMap nativeMap;
        private CustomMap formsMap;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);
            formsMap = (CustomMap)Element;
            if(e.OldElement != null && nativeMap != null)
            {
                nativeMap.MarkerClick -= NativeMap_MarkerClick;
                formsMap.ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            if(e.NewElement != null)
            {
                var mapView = Control as MapView;
                mapView.GetMapAsync(this);
                formsMap.ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
			}
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            nativeMap = googleMap;
            nativeMap.MarkerClick += NativeMap_MarkerClick;
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
				nativeMap.Clear();
				nativeMap.MyLocationEnabled = true;
                foreach(CustomPin item in e.NewItems)
                {
                    var markerWithIcon = new MarkerOptions();
                    markerWithIcon.SetPosition(new LatLng(item.Pin.Position.Latitude, item.Pin.Position.Longitude));
                    markerWithIcon.SetTitle(item.Pin.Label);
                    markerWithIcon.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
                    nativeMap.AddMarker(markerWithIcon);
                }

            }
        }

        private void NativeMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            
        }
    }
}
