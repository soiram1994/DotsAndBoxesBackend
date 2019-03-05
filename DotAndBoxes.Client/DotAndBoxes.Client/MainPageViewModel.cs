using DotsAndBoxes.Common.CommonModels;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DotAndBoxes.Client
{

    public class MainPageViewModel:INotifyPropertyChanged
    {
        HubConnection _connection;

        public MainPageViewModel()
        {
            _connection = new HubConnection(Settings.ConnectionString);
            
        }

        private ObservableCollection<Line> _drawnLines;
        public ObservableCollection<Line> DrawnLines
        {
            get
            {
                return _drawnLines;
            }
            set
            {
                _drawnLines = value;
                OnpropertyChanged("DrawnLines");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("propertyName"));
        }
        public async Task Connect()
        {
            try
            {
                await _connection.Start();
            }
            catch(Exception e)
            {
                var mes = e.Message;
            }
        }
    }
}
