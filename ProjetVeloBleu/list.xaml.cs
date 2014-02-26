using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml.Linq;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Windows.Media;
using System.Windows.Shapes;



namespace ProjetVeloBleu
{

    public class Stand
    {
        string nom;
        int id;
        string add;
        double lng;
        double lat;
        int tc, ap, ab, ac;
        public Stand(){}
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
            return "station n°"+this.Id;
        }
    }
    public partial class list : PhoneApplicationPage
    {
        List<Stand> stands = new List<Stand>();
        Map Carte;
        Location loc;
        public list()
        {
            InitializeComponent();
            String xmlStr = (String) PhoneApplicationService.Current.State["stations"];
            XDocument xmlDoc = XDocument.Parse(xmlStr);

            Carte = new Map();
            Carte.ZoomLevel = 10.0;
            ContentMap.Children.Add(Carte);
            int inc=0;
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
                        if (item.Element("wcom") != null) std.Add = item.Element("wcom").Value;
                        if (item.Element("lng") != null) std.Lng = double.Parse(item.Element("lng").Value);
                        if (item.Element("lat") != null) std.Lat = double.Parse(item.Element("lat").Value);
                        if (item.Element("ab") != null) std.Ab = int.Parse(item.Element("ab").Value);
                        if (item.Element("ac") != null) std.Ac = int.Parse(item.Element("ac").Value);
                        if (item.Element("ap") != null) std.Ap = int.Parse(item.Element("ap").Value);
                        if (item.Element("tc") != null) std.Tc = int.Parse(item.Element("tc").Value);

                        if (std.Add != null)
                        {
                            std.Add = std.Add.Replace("+", " ");
                            std.Add = std.Add.Replace("%c2", "°");
                        } 
                       loc = new Location();
                        loc.Longitude = std.Lng;
                        loc.Latitude = std.Lat;
                        Pushpin pin = new Pushpin();
                        ToolTipService.SetToolTip(pin, "station n°" + std.Id + "\n" + std.Ap + " places disponible" + "\n" + std.Ab + " vélos disponible");

                        pin.TabIndex = inc;
                        
                        pin.Location = loc;
                        pin.Name = inc.ToString();
                        
                        Carte.Center = loc;
                        pin.DoubleTap += pin_event;
                        //pin.Template = this.Resources["PinTemplate"] as ControlTemplate;
                        ImageBrush imgBrush = new ImageBrush();
                        
                        imgBrush.Stretch = System.Windows.Media.Stretch.UniformToFill;
                        imgBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"velo_bleu.png", UriKind.Relative));

                        //pin.Background = imgBrush;



                        Carte.Children.Add(pin);
                        inc++;



                    }
                    
                     
                    stands.Add(std);


                   
                }
                 
            }
           // standList.DataContext = stands;
 
        }
        void pin_event(object sender, RoutedEventArgs e)
        {
            Pushpin pin = (Pushpin)sender;
            //MessageBox.Show("pin: "+pin.Name);
            detail(pin.TabIndex);

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

        private void ButtonPlus(object sender, RoutedEventArgs e)
        {
            double offset = Carte.ZoomLevel;

            Carte.ZoomLevel = offset + 1;
        }

        private void ButtonMoins(object sender, RoutedEventArgs e)
        {
            double offset = Carte.ZoomLevel;
            if(offset>1)
            Carte.ZoomLevel = offset-1;
        }
    }
}