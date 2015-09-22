using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetworkScanner.ViewModel
{
    public class Network
    {
        private Ping _Ping = null;
        private Hashtable _Interfaces = null;

        public Hashtable Interfaces { get { return _Interfaces; } }

        public IPAddress Ip { get; set; }

        private const string NOT_AVAILABLE = "n/a";

        public Network()
        {
            try
            {
                _Interfaces = new Hashtable(16);
            }
            catch (Exception ex)
            {

                throw new Exception(this.ToString() + "->Network", ex);
            }
        }

        public PingReply SendPing(IPAddress pAddress, int pTimeout)
        {
            if (pAddress == null)
            {
                throw new Exception("Invalid IP");
            }

            _Ping = new Ping();

            PingReply reply;
            reply = _Ping.Send(pAddress, pTimeout);

            return reply;

        }

        public NetworkInterface[] GetInterfaces()
        {
            NetworkInterface[] nIf = NetworkInterface.GetAllNetworkInterfaces();
            return nIf;
        }

        public void AddInterface(string pInfId, NetworkInterface pInf)
        {
            if (pInfId == null)
            {
                return;
            }

            if (_Interfaces.ContainsKey(pInfId))
            {
                return;
            }

            _Interfaces.Add(pInfId, pInf);
        }

        public string[] GetInterfaceDetails(NetworkInterface pIf, ref List<IPAddressInformation> pAddressInformation)
        {
            if (pIf == null)
            {
                return null;
            }

            string[] details = new string[9];

            string addresses = null, masks = null;

            if (pIf.GetIPProperties().UnicastAddresses == null || pIf.GetIPProperties().UnicastAddresses.Count <= 0) return null;


            foreach (UnicastIPAddressInformation curIp in pIf.GetIPProperties().UnicastAddresses)
            {
                addresses += (curIp.Address != null) ? curIp.Address.ToString() + "," : NOT_AVAILABLE + ",";
                masks += (curIp.IPv4Mask != null) ? curIp.IPv4Mask.ToString() + "," : NOT_AVAILABLE + ",";
                pAddressInformation.Add(curIp);
            }

            // Details of Interface
            details[0] = "Id: " + pIf.Id;
            details[1] = "Name: " + pIf.Name;
            details[2] = "Type: " + pIf.NetworkInterfaceType.ToString();
            details[3] = "Speed: " + (pIf.Speed / 1000000).ToString();
            details[4] = "MAC: " + pIf.GetPhysicalAddress().ToString();
            details[5] = "Status: " + pIf.OperationalStatus.ToString();
            details[6] = "Desc.: " + pIf.Description;
            details[7] = "Address: " + addresses.TrimEnd(',');
            details[8] = "Mask: " + masks.TrimEnd(',');

            // Get IpV4 Address
            var ipV4 = from a in pAddressInformation
                       where a.Address.AddressFamily == AddressFamily.InterNetwork
                       select a;

            if (ipV4.Count() == 1)
            {
                Ip = ipV4.ToList<IPAddressInformation>()[0].Address;
            }

            return details;
        }

        public string GetHostName(IPAddress pAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(pAddress);
                return entry.HostName;
            }
            catch (Exception)
            {
                return pAddress.ToString();
            }

        }

    }
}
