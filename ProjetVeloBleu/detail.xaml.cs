using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using Microsoft.Phone.Shell;
using System.Collections.Generic;

namespace ProjetVeloBleu
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
            List<Stand> tab =  (List<Stand>) PhoneApplicationService.Current.State["stands"];
            int index = (int)PhoneApplicationService.Current.State["index"];

            //MessageBox.Show(index.ToString());
            Stand item =  tab[index];

            station_id.Text = "station n°" + item.Id;
            add.Text = item.Add.ToString();
            ab.Text = "Place : "+ item.Ab.ToString();
            ap.Text = "Velos : "+item.Ap.ToString();
            ac.Text = "Capacité disponible : " + item.Ac.ToString();
            tc.Text = "Capacité totale: : " + item.Tc.ToString();

            Location loc = new Location();
            loc.Longitude = item.Lng;
            loc.Latitude = item.Lat;
            
            Map Carte = new Map();
            Carte.Center = loc;
            Carte.ZoomLevel = 17.0;

            Pushpin pin = new Pushpin();
            pin.Location = loc;
            ContentMap.Children.Add(Carte);
            Carte.Children.Add(pin);

        }
    }
}