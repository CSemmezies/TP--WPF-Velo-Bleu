using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TP__WPF_Velo_Bleu
{
    public partial class standlist : PhoneApplicationPage
    {
        public standlist()
        {
            InitializeComponent();
            List<Stand> stands = new List<Stand>();
            stands = (List<Stand>)PhoneApplicationService.Current.State["stands"];
            maListe.ItemsSource = stands;

        }

        private void ListViewItem_OnTap(object sender,SelectionChangedEventArgs e)
        {
            int item = maListe.SelectedIndex;

            PhoneApplicationService.Current.State["index"] = item;
            //on lance la mapView
            NavigationService.Navigate(new Uri("/detail.xaml", UriKind.Relative));
        }
    }

   
    
}