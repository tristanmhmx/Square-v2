using System;
using System.Collections.Specialized;
using MapKit;
using Square;
using Square.iOS.CustomRenderers;
using Square.iOS.Protocols;
using Square.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Square.iOS.CustomRenderers
{
    public class CustomMapRenderer : MapRenderer
    {
        MKMapView nativeMap;
        CustomMap formsMap;
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            nativeMap = Control as MKMapView;
            formsMap = Element as CustomMap;

            if(e.OldElement != null)
            {
                nativeMap.Delegate = null;
                formsMap.ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            if(e.NewElement != null)
            {
                nativeMap.Delegate = null;
                nativeMap.Delegate = new MapDelegate(formsMap, nativeMap);
                formsMap.ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            }
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(CustomPin customPin in e.NewItems)
                {
                    formsMap.Pins.Add(customPin.Pin);
                }
            }
        }
    }
}
