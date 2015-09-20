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

namespace NetworkScanner.Views.Setngs
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            abt.Text = "Network Scanner is a small utility that scans your wireless network and " +
                       "displays the list of all computers and devices that are currently connected to your network.\n" +
                       "For every computer or device that is connected to your network, the following information is displayed: " + 
                       "IP address, MAC address, the company that manufactured the network card, and optionally the computer name.\n" +
                       "You can also export the connected devices list into html/ xml / csv / text file, or copy the list to the clipboard " +
                       "and then paste into Excel or other spreadsheet application.";
        }
    }
}
