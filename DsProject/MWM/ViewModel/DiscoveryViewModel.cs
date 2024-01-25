using FileExplorer.Core;
using FileExplorer.MWM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.MWM.ViewModel
{
    class DiscoveryViewModel : ObservableObject
    {

        private string _sPath;

        public string SPath
        {
            get { return _sPath; }
            set
            {
                _sPath = value;
                OnPropertyChanged();
            }
        }

        public DiscoveryViewModel() { 
        }
    }
}
