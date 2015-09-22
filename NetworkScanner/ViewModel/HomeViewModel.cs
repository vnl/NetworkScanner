using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NetworkScanner.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace NetworkScanner.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private Network _NetWork = null;
        private NetworkInterface[] _Inf = null;
        private string _selectedInterface;
        private readonly ObservableCollection<string> lstInterfaceDetails = new ObservableCollection<string>();
        private List<string> _Interfaces = new List<string>();

        public HomeViewModel()
        {
            Info = "Your Home Network";
            // Instantiate the RelayCommand.  This is much less verbose
            // than the default WPF Command declaration, and why
            // RelayCommands are nice to use.
            NavigateToNetListCommand = new RelayCommand(NavigateToNetList);
            _NetWork = new Network();
            FillInterfaces();
            LoadSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> Interfaces
        {
            get { return _Interfaces; }

            set
            {
                if (_Interfaces != null)
                {
                    Interfaces = _Interfaces;
                }
            }
        }

        public ObservableCollection<string> InterfaceDetails
        {
            get { return lstInterfaceDetails; }

            // set
            // {
            //    if (lstInterfaceDetails != null)
            //    {
            //        InterfaceDetails = lstInterfaceDetails;
            //        //RaisePropertyChanged("InterfaceDetails");
            //    }
            // }
        }

        public string SelectedInterface
        {
            get
            {
                return _selectedInterface;
            }

            set
            {
                if (_selectedInterface != value)
                {
                    _selectedInterface = value;
                    RaisePropertyChanged("SelectedInterface");
                    SetInterfaceDetails(_selectedInterface);
                }
            }
        }

        // Info property, for a label to be shown on the view
        public string Info { get; set; }

        // Declare the RelayCommand
        public RelayCommand NavigateToNetListCommand { get; private set; }

        // Method to run when the command is executed
        public void NavigateToNetList()
        {
            // IMPLEMENT NAVIGATION HERE
            // Get a reference to NetList's ViewModel from the IoC container
            NetListViewModel vm = SimpleIoc.Default.GetInstance<NetListViewModel>();

            // Set the Info string value to something different
            vm.Info = "Initializing This Text from HomeViewModel";

            // Send the navigation message
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage() { TargetPage = "Views/NetList.xaml" });
        }

        public void LoadSettings()
        {
            try
            {
                // _selectedInterface = Properties.Settings.Default.network_interface;
                // numTimeOut.Value = Properties.Settings.Default.ping_timeout > 0 ? Properties.Settings.Default.ping_timeout : 15;
                this.SetInterfaceDetails(_selectedInterface);
            }
            catch (Exception ex)
            {
                // MessageHandler.ShowCustomErrorDialog(this.name + MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void FillInterfaces()
        {
            _Inf = _NetWork.GetInterfaces();
            foreach (NetworkInterface curInf in _Inf)
            {
                string item = curInf.Name + "|" + curInf.Id;
                _Interfaces.Add(item);
                _NetWork.AddInterface(item, curInf);
            }
            if (_Interfaces.Count > 0)
            {
                SelectedInterface = _Interfaces[0];
            }
        }

        private void SetInterfaceDetails(string pIf)
        {
            List<IPAddressInformation> ipAddressInformation = new List<IPAddressInformation>();

            string[] details = _NetWork.GetInterfaceDetails((NetworkInterface)_NetWork.Interfaces[pIf], ref ipAddressInformation);

            lstInterfaceDetails.Clear();

            if (details == null) return;

            for (int z = 0; z < details.Length; z++)
            {
                lstInterfaceDetails.Add(details[z]);
            }
        }
    }
}
