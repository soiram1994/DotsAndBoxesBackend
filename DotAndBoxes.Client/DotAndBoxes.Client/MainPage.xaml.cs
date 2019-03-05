using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;

namespace DotAndBoxes.Client
{
    public partial class MainPage : ContentPage
    {
        HubConnection connection;
        public MainPage()
        {
            InitializeComponent();
            connection = new HubConnection("http://192.168.200.45:51925/gamehub");
            
        }
    }
}
