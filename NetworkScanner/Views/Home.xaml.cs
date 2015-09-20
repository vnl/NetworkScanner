using FirstFloor.ModernUI.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using NetworkScanner.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkScanner.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            txtBlkDetails.Text = "Details - " + Environment.UserName;
            Messenger.Default.Register<NavigationMessage>(this, p =>
            {
                // Create a URI to the target page
                var uri = new Uri(p.TargetPage, UriKind.Relative);

                // Find the frame we are currently in using the ModernUI "NavigationHelper" - THIS WILL NOT WORK IN THE VIEWMODEL
                var frame = NavigationHelper.FindFrame(null, this);

                // Set the frame source, which initiates navigation
                frame.Source = uri;
            });
        }
    }
}
