using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace TP__WPF_Velo_Bleu
{

    public class Stand
    {
        string nom;
        int id;
        string add;
        double lng;
        double lat;
        int tc, ap, ab, ac;
        public Stand() { }
        public string Nom { get; set; }
        public int Id { get; set; }
        public string Add { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public int Tc { get; set; }
        public int Ap { get; set; }
        public int Ab { get; set; }
        public int Ac { get; set; }

        public override string ToString()
        {
            return "station n°" + this.Id;
        }
    }

    public partial class list : PhoneApplicationPage
    {
        List<Stand> stands = new List<Stand>();
        Map Carte;
        GeoCoordinate loc;
        //Location loc;
        public list()
        {
            InitializeComponent();
            String xmlStr = (String)PhoneApplicationService.Current.State["stations"];
            XDocument xmlDoc = XDocument.Parse(xmlStr);

            Carte = new Map();
            Carte.ZoomLevel = 10.0;
            ContentMap.Children.Add(Carte);
            int inc = 0;
            List<XElement> listOfStands = xmlDoc.Descendants("stand").ToList();
            foreach (XElement item in listOfStands)
            {

                if (int.Parse(item.Element("disp").Value) == 1)
                {
                    Stand std = new Stand();

                    if (item.HasAttributes)
                    {

                        std.Nom = item.Attribute("name").Value;
                        std.Id = int.Parse(item.Attribute("id").Value);
                    }

                    //Descendants
                    if (item.HasElements)
                    {
                        //MessageBox.Show(item.Element("lng").Value);
                        //Double test = Double.Parse(item.Element("lng").Value);
                        if (item.Element("wcom") != null) std.Add = item.Element("wcom").Value;
                        if (item.Element("lng") != null) std.Lng = Convert.ToDouble(item.Element("lng").Value.Replace(".",","));
                        if (item.Element("lat") != null) std.Lat = Convert.ToDouble(item.Element("lat").Value.Replace(".",","));
                        if (item.Element("ab") != null) std.Ab = int.Parse(item.Element("ab").Value);
                        if (item.Element("ac") != null) std.Ac = int.Parse(item.Element("ac").Value);
                        if (item.Element("ap") != null) std.Ap = int.Parse(item.Element("ap").Value);
                        if (item.Element("tc") != null) std.Tc = int.Parse(item.Element("tc").Value);

                        if (std.Add != null)
                        {
                            std.Add = std.Add.Replace("+", " ");
                            std.Add = std.Add.Replace("%c2", "°");
                        }
                        loc = new GeoCoordinate();
                        loc.Longitude = std.Lng;
                        loc.Latitude = std.Lat;
                        Pushpin pin = new Pushpin();

                        ToolTipService.SetToolTip(pin, "station n°" + std.Id + "\n" + std.Ap + " places disponible" + "\n" + std.Ab + " vélos disponible");

                        pin.TabIndex = inc;

                        pin.GeoCoordinate = loc;
                        pin.Name = inc.ToString();

                        Carte.Center = loc;
                        pin.Tap += pin_event;

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
                        MyRectangle.Name = inc.ToString();
                        MyRectangle.SetValue(Grid.RowProperty, 0);
                        MyRectangle.SetValue(Grid.ColumnProperty, 0);
                        MyRectangle.Tap += pin_event;
                        MyGrid.Children.Add(MyRectangle);

                        MapLayer layer = new MapLayer();
                        MapOverlay overlay = new MapOverlay();
                        overlay.Content = MyGrid;
                        overlay.GeoCoordinate = pin.GeoCoordinate;
                        layer.Add(overlay);

                        Carte.Layers.Add(layer);
                        inc++;
                    }
                    stands.Add(std);
                }

            }
            // standList.DataContext = stands;

        }
        void pin_event(object sender, RoutedEventArgs e)
        {
            Rectangle pin = (Rectangle)sender;
            detail(Convert.ToInt32(pin.Name));

        }
        private void stand_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //detail(standList.SelectedIndex);

        }

        void detail(int index)
        {
            PhoneApplicationService.Current.State["stands"] = stands;
            PhoneApplicationService.Current.State["index"] = index;
            
            //on lance la mapView
            NavigationService.Navigate(new Uri("/detail.xaml", UriKind.Relative));
        }

        private void list_event(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["stands"] = stands;
            NavigationService.Navigate(new Uri("/standlist.xaml", UriKind.Relative));
        }
        private void ButtonPlus(object sender, RoutedEventArgs e)
        {
            double offset = Carte.ZoomLevel;

            Carte.ZoomLevel = offset + 1;
        }

        private void ButtonMoins(object sender, RoutedEventArgs e)
        {
            double offset = Carte.ZoomLevel;
            if (offset > 1)
                Carte.ZoomLevel = offset - 1;
        }
    }
}