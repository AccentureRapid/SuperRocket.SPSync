using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using SuperRocket.Orchard.Core.SharePoint.Enums;
using SuperRocket.Orchard.Core.SharePoint.Common;

namespace SuperRocket.Orchard.Core.SharePoint
{
    public class SyncConfiguration
    {
        public SyncConfiguration() { }

        public static Dictionary<string, SyncConfiguration> AllConfigurations { get; private set; }

        static SyncConfiguration()
        {
            RevertConfigurationChanges();
        }

        public static void RevertConfigurationChanges()
        {
            AllConfigurations = new Dictionary<string, SyncConfiguration>();

            string configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            string configFile = Path.Combine(configFolder, "Config.xml");

            try
            {
                using (FileStream fs = new FileStream(configFile, FileMode.Open))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(SyncConfiguration[]));
                    var list = (SyncConfiguration[])ser.Deserialize(fs);

                    foreach (var item in list)
                    {
                        AllConfigurations.Add(item.LocalFolder, item);
                    }
                }
            }
            catch(Exception ex) {
                Logger.Log("RevertConfigurationChanges failed because of {0}",ex.StackTrace);
            }
        }

        public static void SaveAllConfigurations()
        {
            string configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            string configFile = Path.Combine(configFolder, "Config.xml");

            try
            {
                using (FileStream fs = new FileStream(configFile, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(SyncConfiguration[]));
                    ser.Serialize(fs, AllConfigurations.Values.ToArray());
                }
            }
            catch { }
        }

        public static SyncConfiguration FindConfiguration(string localFolder)
        {
            // used to detect only exact path and not only a part of the path
            localFolder = localFolder.EndsWith("\\") ? localFolder : localFolder + "\\";
            var conf = AllConfigurations.FirstOrDefault(p => localFolder.ToUpper().StartsWith(p.Key.ToUpper() + "\\"));
            return conf.Value;
        }

        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public string DocumentLibrary { get; set; }
        public string LocalFolder { get; set; }
        public string[] SelectedFolders { get; set; }
        public bool DownloadHeadersOnly { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public string AdfsRealm { get; set; }
        public string AdfsSTSUrl { get; set; }
        public ConflictHandling ConflictHandling { get; set; }
        public SyncDirection Direction { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }

        [XmlIgnore]
        public string Password
        {
            get { return Decrypt(PasswordEncrypted); }
            set { PasswordEncrypted = Encrypt(value); }
        }

        private string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var data = Encoding.Default.GetBytes(value);
            //var result = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(data);
        }

        private string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var data = Convert.FromBase64String(value);
            // result = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Default.GetString(data);
        }

        public SharePointManager GetSharePointManager() => new SharePointManager(this);

        public override string ToString() => LocalFolder;

        internal bool ShouldFileSync(string localFile)
        {
            if (SelectedFolders == null)
                return true;

            var dir = Path.GetDirectoryName(localFile).ToLowerInvariant();
            dir = dir.Replace(LocalFolder.ToLowerInvariant(), string.Empty).TrimStart('\\') + "\\";

            if (SelectedFolders.Count(p => !string.IsNullOrEmpty(p) && dir.StartsWith(p, StringComparison.OrdinalIgnoreCase)) > 0)
                return true;

            return false;
        }
    }
}
