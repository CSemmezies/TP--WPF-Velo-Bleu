using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ProjetVeloBleu.Resources;
using System.Diagnostics;


namespace ProjetVeloBleu
{
    public partial class MainPage : PhoneApplicationPage
    {
        WebClient _webClient;
        public MainPage()
        {
            InitializeComponent();

            getData();
        }

        public void getData()
        {

            _webClient = new WebClient();
            _webClient.DownloadStringCompleted += _webClient_DownloadStringCompleted;
            _webClient.DownloadStringAsync(new Uri("http://www.velo-vision.com/nice/oybike/stands.nsf/getsite?site=nice&format=xml&key=veolia"));
        }


        void _webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
             Console.WriteLine(e.Result.ToString());
             PhoneApplicationService.Current.State["stations"] = e.Result;
            NavigationService.Navigate(new Uri("/list.xaml", UriKind.Relative));
        }
    }
}