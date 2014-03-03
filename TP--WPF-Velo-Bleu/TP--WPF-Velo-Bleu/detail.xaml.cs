using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using System.Device.Location;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TP__WPF_Velo_Bleu
{
    public partial class detail : PhoneApplicationPage
    {
        public static string encode(string text)
        {


            byte[] mybyte = System.Text.Encoding.UTF8.GetBytes(text);
            string returntext = System.Convert.ToBase64String(mybyte);
            return returntext;
        }

        public detail()
        {
            InitializeComponent();
            List<Stand> tab = (List<Stand>)PhoneApplicationService.Current.State["stands"];
            int index = (int)PhoneApplicationService.Current.State["index"];

            //MessageBox.Show(index.ToString());
            Stand item = tab[index];

            station_id.Text = "Station n°" + item.Id;
            add.Text = item.Add.ToString();
            ab.Text = "Places : " + item.Ab.ToString();
            ap.Text = "Velos : " + item.Ap.ToString();
            ac.Text = "Capacité disponible : " + item.Ac.ToString();
            tc.Text = "Capacité totale: : " + item.Tc.ToString();

            GeoCoordinate loc = new GeoCoordinate();
            //Location loc = new Location();
            loc.Longitude = item.Lng;
            loc.Latitude = item.Lat;

            Map Carte = new Map();
            Carte.Center = loc;
            Carte.ZoomLevel = 17.0;

            Pushpin pin = new Pushpin();
            pin.GeoCoordinate = loc;
            //pin.Location = loc;
            ContentMap.Children.Add(Carte);

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.Stretch = System.Windows.Media.Stretch.UniformToFill;
            imgBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"velo_bleu.png", UriKind.Relative));

            Grid MyGrid = new Grid();
            MyGrid.RowDefinitions.Add(new RowDefinition());
            MyGrid.RowDefinitions.Add(new RowDefinition());
            MyGrid.Background = new SolidColorBrush(Colors.Transparent);

            Rectangle MyRectangle = new Rectangle();
            MyRectangle.Fill = imgBrush;
            MyRectangle.Height = 52;
            MyRectangle.Width = 85;
            MyRectangle.SetValue(Grid.RowProperty, 0);
            MyRectangle.SetValue(Grid.ColumnProperty, 0);

            MyGrid.Children.Add(MyRectangle);

            MapLayer layer = new MapLayer();
            MapOverlay overlay = new MapOverlay();
            overlay.GeoCoordinate = pin.GeoCoordinate;
            overlay.Content = MyGrid;
            layer.Add(overlay);



            //Carte.Children.Add(pin);
            Carte.Layers.Add(layer);
        }
    }
}