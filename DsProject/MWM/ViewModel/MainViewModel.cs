using DsProject.Core;
using DsProject.MWM.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DsProject.MWM.ViewModel
{



    class MainViewModel : ObservableObject
    {
        public Dictionary<string, RelayCommand> DriveCommands { get; set; }

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand DiscoveryViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }

        public DiscoveryViewModel DiscoveryVM { get; set; }

        


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        


        public MainViewModel() { 
            HomeVM = new HomeViewModel();
            DiscoveryVM = new DiscoveryViewModel() ;
                    DiscoveryView discoveryView = new DiscoveryView();
            
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            DiscoveryViewCommand = new RelayCommand(o =>
            {
                CurrentView = discoveryView;
            });

            DriveCommands = new Dictionary<string, RelayCommand>();

            foreach (string s in Directory.GetLogicalDrives())
            {
                DriveCommands[s] = new RelayCommand(o =>
                {
                    // Replace this with the code that should be executed when the RadioButton is clicked
                    //DiscoveryVM = new DiscoveryViewModel(s);
                    //DiscoveryView discoveryView = new DiscoveryView();
                    //discoveryView.Path = s;
                    //CurrentView = discoveryView;
                    CurrentView = new DiscoveryView();
                    MessageBox.Show($"You clicked {discoveryView.Path}");
                });
            }

        }


    }
}
