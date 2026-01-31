using ApplicationBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ExpenseTracker.DataManagement.Database
{
    public class DatabaseConnectionComponent : ApplicationComponent
    {
        private string _configName = "DbConfig.xml";
        private string _userId;
        private bool _password = false;
        public DatabaseConnectionComponent()
        {
        }
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                NotifyPropertyChanged(nameof(UserId));
            }
        }
        public bool Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }
        public string ConfigFilePath
        {
            get => System.IO.Path.Combine(AppContext.BaseDirectory, _configName);
        }
        public void LoadConfig()
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(AppContext.BaseDirectory, _configName)))
                return;
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("Configuration");
            doc.AppendChild(root);

            XmlElement userElement = doc.CreateElement("DatabaseFilePath");
            userElement.InnerText = System.IO.Path.Combine(AppContext.BaseDirectory, "ExpenseTracker.db");
            root.AppendChild(userElement);

            XmlElement passwordElement = doc.CreateElement("UsePassword");
            passwordElement.InnerText = Password ? "true" : "false";
            root.AppendChild(passwordElement);

            doc.Save(System.IO.Path.Combine(AppContext.BaseDirectory, _configName));
        }

    }
}
