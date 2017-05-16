using System;
using System.Linq;
using MapKit;
using Square.Models;
using UIKit;

namespace Square.iOS.Protocols
{
    public class MapDelegate : MKMapViewDelegate
    {
        CustomMap formsMap;
        MKMapView nativeMap;
        public MapDelegate(CustomMap formsMap, MKMapView nativeMap)
        {
            this.formsMap = formsMap;
            this.nativeMap = nativeMap;
        }

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            var customPin = GetCustomPin(annotation);
            if(customPin == null)
            {
                return new CustomMkAnnotationView(annotation, "")
                {
                    Id = "me",
                    Image = UIImage.FromFile("dot")
                };
            }
            var annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
            if(annotationView != null)
            {
                annotationView = new CustomMkAnnotationView(annotation, customPin.Id)
                {
                    Image = UIImage.FromFile("pin")
                };
                ((CustomMkAnnotationView)annotationView).Id = customPin.Id;
            }
            annotationView.CanShowCallout = false;
            return annotationView;
        }

        CustomPin GetCustomPin(IMKAnnotation annotation)
        {
            try
            {
                var position = new Xamarin.Forms.Maps.Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
                return formsMap?.ItemsSource?.FirstOrDefault(pin => pin.Pin.Position == position);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }

    public class CustomMkAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }
        public CustomMkAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
        {
            
        }
    }
}
