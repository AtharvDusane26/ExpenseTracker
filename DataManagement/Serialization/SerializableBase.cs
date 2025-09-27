using ExpenseTracker.DataManagement.Entities;
using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExpenseTracker.DataManagement.Serialization
{
    public class SerializableBase
    {
        private readonly string _filePath;
        private readonly IMessageBoxService _messageBox;
        public SerializableBase()
        {
            var directory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Expense Tracker");
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            _filePath = System.IO.Path.Combine(directory, "Users.xml");
            _messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
        }
        public List<User> Get()
        {
            try
            {
                var users = new List<User>();
                var services = ServiceProvider.Instance;
                var dataManager = services.Resolve<DataManager>();
                var pocos = dataManager.Read<List<EntityUser>>(_filePath);
                if (pocos != null)
                {
                    pocos.ForEach(poco =>
                    {
                        users.Add(poco.Get());
                    });
                }
                return users;
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while receiving user data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
                return new List<User>();
            }

        }
        public void Set(List<User> value)
        {
            try
            {
                var pocos = new List<EntityUser>();
                var services = ServiceProvider.Instance;
                var dataManager = services.Resolve<DataManager>();
                foreach (var user in value)
                {
                    var entityUser = new EntityUser(user.UserId);
                    entityUser.Set(user);
                    pocos.Add(entityUser);
                }
                dataManager.Save(pocos, _filePath);
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving users data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
    }
}
