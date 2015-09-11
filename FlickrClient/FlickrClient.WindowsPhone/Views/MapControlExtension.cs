using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FlickrClient.Views
{

    public class MapControlExtension
    {

        public static string GetTitle(MapControl mapControl)
        {
            return (string)mapControl.GetValue(TitleProperty);
        }

        public static void SetTitle(MapControl mapControl, string value)
        {
            mapControl.SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached(
            "Title", typeof(string), typeof(MapControlExtension), null);

        public static BasicGeoposition GetGeoLocation(MapControl mapControl)
        {
            return (BasicGeoposition)mapControl.GetValue(GeoLocationProperty);
        }

        public static void SetGeoLocation(MapControl mapControl, BasicGeoposition value)
        {
            mapControl.SetValue(GeoLocationProperty, value);
        }

        public static readonly DependencyProperty GeoLocationProperty =
            DependencyProperty.RegisterAttached(
            "GeoLocation", typeof(BasicGeoposition), typeof(MapControlExtension),
            new PropertyMetadata(null, OnGeoLocationPropertyChanged));

        private static void OnGeoLocationPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var mapControl = sender as MapControl;
            if (mapControl == null)
                throw new NotSupportedException();

            var pin = new Grid()
            {
                Width = 100,
                Height = 24,
            };

            pin.Children.Add(new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.DodgerBlue),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 1,
                Width = 100,
                Height = 24
            });

            pin.Children.Add(new TextBlock()
            {
                Text = GetTitle(mapControl),
                FontSize = 12,
                Margin = new Thickness(3,3,3,3),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                TextTrimming = Windows.UI.Xaml.TextTrimming.CharacterEllipsis,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center
            });

            MapControl.SetLocation(pin, (Geopoint)e.NewValue);
            mapControl.Center = (Geopoint)e.NewValue;
            mapControl.Children.Add(pin);
        }
    }

}
