using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace NetworkScanner.ViewModel
{
    public class NetListViewModel
    {
        private List<string> theList = new List<string>();

        public NetListViewModel()
        {
            Info = "This is Net List";
            PingAll();
            List<Link> tempList = new List<Link>();
            foreach (string item in theList)
            {
                tempList.Add(new Link { DisplayName = item, Source = new Uri("http://google.co.uk") });
            }

            IEnumerable<Link> newList = tempList;

            IPList = new LinkCollection(newList);
        }

        public string Info { get; set; }

        public LinkCollection IPList { get; private set; }

        public static string NetworkGateway()
        {
            string ip = null;

            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (f.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                    {
                        ip = d.Address.ToString();
                    }
                }
            }

            Console.WriteLine(string.Format("Network Gateway: {0}", ip));
            return ip;
        }

        public void Ping(string host, int attempts, int timeout)
        {
            for (int i = 0; i < attempts; i++)
            {
                new Thread(delegate()
                {
                    try
                    {
                        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                        ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                        ping.SendAsync(host, timeout, host);
                    }
                    catch
                    {
                        // Do nothing and let it try again until the attempts are exausted.
                        // Exceptions are thrown for normal ping failurs like address lookup
                        // failed.  For this reason we are supressing errors.
                    }
                }).Start();
            }
        }

        public string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException)
            {
                // MessageBox.Show(e.Message.ToString());
            }

            return null;
        }

        // Get MAC address
        public string GetMacAddress(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "arp";
            process.StartInfo.Arguments = "-a " + ipAddress;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string strOutput = process.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "OWN Machine";
            }
        }

        public void PingAll()
        {
            string gate_ip = NetworkGateway();

            // Extracting and pinging all other ip's.
            string[] array = gate_ip.Split('.');

            for (int i = 2; i <= 255; i++)
            {
                string ping_var = array[0] + "." + array[1] + "." + array[2] + "." + i;

                // time in milliseconds
                Ping(ping_var, 4, 4000);
            }
        }

        private void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                string hostname = GetHostName(ip);
                theList.Add(hostname);
                string macaddres = GetMacAddress(ip);
                string[] arr = new string[3];

                // Logic for Ping Reply Success
                // if (!Application.Current.Dispatcher.CheckAccess())
                // {
                //    Application.Current.Dispatcher.Invoke(new Action(() =>
                //    {
                //        theList.Add(hostname);
                //    }));
                // }
                // else
                // {
                //    theList.Add(hostname);
                // }

                // Logic for Ping Reply Success
                // Console.WriteLine(String.Format("Host: {0} ping successful", ip));
            }
            else
            {
                // Logic for Ping Reply other than Success
            }
        }
    }
}
