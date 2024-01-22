using DsProject.Core;
using DsProject.MWM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsProject.MWM.ViewModel
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
