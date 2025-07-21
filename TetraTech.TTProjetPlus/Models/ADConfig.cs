using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class ADConfig
    {
        public string KeyName { get; set; } =string.Empty;
        public string URL { get; set; } =string.Empty;
        public string UserId { get; set; } =string.Empty;
        public string Password { get; set; } =string.Empty;
        public string Directory { get; set; } =string.Empty;
        public string FriendlyName { get; set; } =string.Empty;
        public string DomainName { get; set; } =string.Empty;
         
        public static ADConfig ParseADConfigString(string config, string keyname)
        {
            ADConfig returnValue = new ADConfig();

            if (config != string.Empty)
            {
                string[] items = config.Split('~');

                if (items.Length > 0)
                {
                    returnValue = new ADConfig();

                    string[] splitDirGroup = items[1].Split(':');

                    returnValue.KeyName = keyname;
                    returnValue.URL = items[0];
                    returnValue.Directory = items[1];

                    string[] splitCred = items[2].Split(':');

                    returnValue.UserId = splitCred[0];
                    returnValue.Password = splitCred[1];

                    returnValue.FriendlyName = items[3];

                    if (items.Length > 4)
                    {
                        returnValue.DomainName = items[4];
                    }
                }
            }

            return (returnValue);
        }
    }
}