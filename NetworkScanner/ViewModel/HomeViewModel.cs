using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NetworkScanner.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkScanner.ViewModel
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Info = "Your Home Network";
            // Instantiate the RelayCommand.  This is much less verbose
            // than the default WPF Command declaration, and why
            // RelayCommands are nice to use.
            NavigateToNetListCommand = new RelayCommand(NavigateToNetList);
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
    }
}
