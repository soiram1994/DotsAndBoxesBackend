using DotsAndBoxes.Common.CommonModels;

using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DotAndBoxes.Client
{

    public class MainPageViewModel:INotifyPropertyChanged
    {
        HubConnection _connection;
        private IHubProxy _hubProxy;

        public MainPageViewModel()
        {
            _connection = new HubConnection(Settings.ConnectionString);
            
            _hubProxy = _connection.CreateHubProxy("gamehub");
            _hubProxy.On("StartGame", (int dimension) => {
                GenerateBoard(dimension);
            });
            
            
            
        }
        private int _dimension;
        public int Dimension
        {
            get
            {
                return _dimension;
            }
            set
            {
                _dimension = value;
                OnpropertyChanged("Dimension");
            }
        }
        private void GenerateBoard(int dimension)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    Dots.Add(new Dot { X = i, Y = j });
                }
            }
        }

        private ObservableCollection<Dot> _dots;
        public ObservableCollection<Dot> Dots
        {
            get
            {
                return _dots;
            }
            set
            {
                _dots = value;
                OnpropertyChanged("Dots");
            }
        }
        private string _connectionStatus;
        public string ConnectionStatus
        {
            get
            {
                return _connectionStatus;
            }
            set
            {
                _connectionStatus = value;
                OnpropertyChanged("ConnectionStatus");
            }
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

        public async Task ConnectToHub()
        {
            try
            {
                await _connection.Start();
                
            }
            catch(Exception e)
            {
                var mes = e.Message;
            }
            ConnectionStatus = _connection.State.ToString();
        }
        public  ICommand Connect => new Command(_connect);

        private async void _connect(object obj)
        {
            await ConnectToHub();
        }
        public ICommand NewGame => new Command(_newGame);
        public ICommand JoinGame => new Command(_joinGame);

        private void _joinGame(object obj)
        {
            _hubProxy.Invoke("JoinGame");
        }

        private void _newGame(object obj)
        {
            if (Dimension > 2)
                _hubProxy.Invoke("NewGame", Dimension);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("propertyName"));
        }
    }
}
