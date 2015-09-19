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
    public class View1ViewModel
    {
        public View1ViewModel()
        {
            Info = "This is View 1";
            // Instantiate the RelayCommand.  This is much less verbose
            // than the default WPF Command declaration, and why
            // RelayCommands are nice to use.
            NavigateToView2Command = new RelayCommand(NavigateToView2);
        }

        // Info property, for a label to be shown on the view
        public string Info { get; set; }

        // Declare the RelayCommand
        public RelayCommand NavigateToView2Command { get; private set; }

        // Method to run when the command is executed
        public void NavigateToView2()
        {
            // IMPLEMENT NAVIGATION HERE
            // Get a reference to View2's ViewModel from the IoC container
            View2ViewModel vm = SimpleIoc.Default.GetInstance<View2ViewModel>();

            // Set the Info string value to something different
            vm.Info = "Initializing This Text from View1ViewModel";

            // Send the navigation message
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage() { TargetPage = "Views/View2.xaml" });
        }
    }
}
