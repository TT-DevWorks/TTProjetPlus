using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{

        public class NetworkDrive
        {
            public enum ResourceScope
            {
                RESOURCE_CONNECTED = 1,
                RESOURCE_GLOBALNET,
                RESOURCE_REMEMBERED,
                RESOURCE_RECENT,
                RESOURCE_CONTEXT
            }

            public enum ResourceType
            {
                RESOURCETYPE_ANY,
                RESOURCETYPE_DISK,
                RESOURCETYPE_PRINT,
                RESOURCETYPE_RESERVED
            }

            public enum ResourceUsage
            {
                RESOURCEUSAGE_CONNECTABLE = 0x00000001,
                RESOURCEUSAGE_CONTAINER = 0x00000002,
                RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
                RESOURCEUSAGE_SIBLING = 0x00000008,
                RESOURCEUSAGE_ATTACHED = 0x00000010,
                RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
            }

            public enum ResourceDisplayType
            {
                RESOURCEDISPLAYTYPE_GENERIC,
                RESOURCEDISPLAYTYPE_DOMAIN,
                RESOURCEDISPLAYTYPE_SERVER,
                RESOURCEDISPLAYTYPE_SHARE,
                RESOURCEDISPLAYTYPE_FILE,
                RESOURCEDISPLAYTYPE_GROUP,
                RESOURCEDISPLAYTYPE_NETWORK,
                RESOURCEDISPLAYTYPE_ROOT,
                RESOURCEDISPLAYTYPE_SHAREADMIN,
                RESOURCEDISPLAYTYPE_DIRECTORY,
                RESOURCEDISPLAYTYPE_TREE,
                RESOURCEDISPLAYTYPE_NDSCONTAINER
            }

            [StructLayout(LayoutKind.Sequential)]
            private class NETRESOURCE
            {
                public ResourceScope dwScope = 0;
                public ResourceType dwType = 0;
                public ResourceDisplayType dwDisplayType = 0;
                public ResourceUsage dwUsage = 0;
                public string lpLocalName = null;
                public string lpRemoteName = null;
                public string lpComment = null;
                public string lpProvider = null;
            }

            [DllImport("mpr.dll")]
            private static extern int WNetAddConnection2(NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

            public int MapNetworkDrive(string unc, string drive, string user, string password)
            {
                NETRESOURCE myNetResource = new NETRESOURCE();
                myNetResource.lpLocalName = drive+":";
                myNetResource.lpRemoteName = unc;
                myNetResource.lpProvider = null;
                //ajout1 de fred a cause de l'erreur "66 The network resource type is not correct." 
                myNetResource.dwType = ResourceType.RESOURCETYPE_DISK;

            //ajout2 de fred pour forcer la deconnection du drive 
                //If Drive is already mapped disconnect the current
                //mapping before adding the new mapping
                if (IsDriveMapped(drive))
            {
                DisconnectNetworkDrive(drive, true);
            }
            //fin ajouts de fred
            int result = WNetAddConnection2(myNetResource, password, user, 0);
                return result;
            }

        public static bool IsDriveMapped(string sDriveLetter)
    {
        string[] DriveList = Environment.GetLogicalDrives();
        for (int i = 0; i < DriveList.Length; i++)
        {
            if (sDriveLetter + ":\\" == DriveList[i].ToString())
            {
                return true;
            }
        }
        return false;
    }

        public static int DisconnectNetworkDrive(string sDriveLetter, bool bForceDisconnect)
        {
            if (bForceDisconnect)
            {
                return WNetCancelConnection2(sDriveLetter + ":", 0, 1);
            }
            else
            {
                return WNetCancelConnection2(sDriveLetter + ":", 0, 0);
            }
        }

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2
            (string sLocalName, uint iFlags, int iForce);


    }


}
