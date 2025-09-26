using ExpenseTracker.DataManagement.Serialization;
using ExpenseTracker.Model;
using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.Services;
using ExpenseTracker.View;
using ExpenseTracker.ViewModel;
using System.Configuration;
using System.Data;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using MessageBoxImage = ExpenseTracker.Model.Services.MessageBoxImage;
using MessageBoxResult = ExpenseTracker.Model.Services.MessageBoxResult;

namespace ExpenseTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private  Mutex _myMutex = new Mutex(true, "FF1BA877-5CF0-4FC3-8E6S-664E93206D4A");
        [STAThread]
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!IsAlreadyStarted())
            {
                base.OnStartup(e);
                RegisterUnhandledExceptionEvent();
                RegisterClosingEvent();
                RegisterServices();
                StartApp();
            }
            else
            {
                System.Windows.MessageBox.Show("Application is already running.", "Error", MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }
        private void StartApp()
        {
            var services = ServiceProvider.Instance;
            var loginViewModel = new LoginViewModel();
            var viewService = services.Resolve<IViewService>();
            var window = new MainWindow();
            Application.Current.MainWindow = window;
            bool? result = viewService.ShowDialog(loginViewModel);
            if (result == true)
            {

                window.Show();
            }
            else
            {
                Shutdown();
            }
        }
        private void RegisterServices()
        {
            var services = ServiceProvider.Instance;
            services.AddSingleton(new NotificationManager());
            services.AddSingleton(new UserManager());
            services.AddSingleton(new DataManager());
            services.AddTransient(f => new SerializableBase());
            services.AddSingleton<IViewService>(new WpfViewService());
            services.AddTransient<IMessageBoxService>(sp => new MessageBox());
            services.BuildServiceProvider();
        }
        private void RegisterUnhandledExceptionEvent()
        {
            DispatcherUnhandledException += (s, exArgs) =>
            {
                exArgs.Handled = true;
                var dialog = new ExceptionDialog(exArgs.Exception, "Unhandled Exception");
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.ShowInTaskbar = false;
                dialog.ShowDialog();
            };
        }
        private void RegisterClosingEvent()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                var services = ServiceProvider.Instance;
                services.Dispose();
            };
        }
        private bool IsAlreadyStarted()
        {
            return !_myMutex.WaitOne(0, false);
        }
    }
    public class MessageBox : IMessageBoxService
    {
        public MessageBoxResult Show(string message, MessageBoxArgs args, string messageBoxTitle = "")
        {
            System.Windows.MessageBoxResult result = System.Windows.MessageBoxResult.None;
            var window = GetActiveWindow();
            if (window == null)
                result = System.Windows.MessageBox.Show(message, messageBoxTitle, GetBoxButton(args.MessageBoxButton), GetMessageBoxImage(args.MessageBoxIcon));
            else
                result = System.Windows.MessageBox.Show(window, message, messageBoxTitle, GetBoxButton(args.MessageBoxButton), GetMessageBoxImage(args.MessageBoxIcon));

            return GetResult(result);
        }
        public MessageBoxResult Show(string message, string messageBoxTitle = "")
        {
            System.Windows.MessageBoxResult result = System.Windows.MessageBoxResult.None;
            var window = GetActiveWindow();
            if (window == null)
                result = System.Windows.MessageBox.Show(message, messageBoxTitle);
            else
                result = System.Windows.MessageBox.Show(window, message, messageBoxTitle);
            return GetResult(result);
        }
        private Window GetActiveWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.IsActive)
                {
                    return window;
                }
            }
            return null;
        }
        private System.Windows.MessageBoxButton GetBoxButton(MessageBoxButtons messageBoxButton)
        {
            System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK;
            switch (messageBoxButton)
            {
                case MessageBoxButtons.YesNo:
                    button = System.Windows.MessageBoxButton.YesNo;
                    break;
                case MessageBoxButtons.OK:
                    button = System.Windows.MessageBoxButton.OK;
                    break;
                case MessageBoxButtons.YesNoCancle:
                    button = System.Windows.MessageBoxButton.YesNoCancel;
                    break;
                case MessageBoxButtons.OKCancle:
                    button = System.Windows.MessageBoxButton.OKCancel;

                    break;
            }
            return button;
        }
        private System.Windows.MessageBoxImage GetMessageBoxImage(MessageBoxImage messageBoxImage)
        {
            System.Windows.MessageBoxImage image = System.Windows.MessageBoxImage.None;
            switch (messageBoxImage)
            {
                case MessageBoxImage.Information:
                    image = System.Windows.MessageBoxImage.Information;
                    break;
                case MessageBoxImage.Error:
                    image = System.Windows.MessageBoxImage.Error;
                    break;
                case MessageBoxImage.Question:
                    image = System.Windows.MessageBoxImage.Question;
                    break;
                case MessageBoxImage.Exclamation:
                    image = System.Windows.MessageBoxImage.Exclamation;
                    break;
                case MessageBoxImage.Asterisk:
                    image = System.Windows.MessageBoxImage.Asterisk;
                    break;
                case MessageBoxImage.Warning:
                    image = System.Windows.MessageBoxImage.Warning;
                    break;
                case MessageBoxImage.Stop:
                    image = System.Windows.MessageBoxImage.Stop;
                    break;
                case MessageBoxImage.Hand:
                    image = System.Windows.MessageBoxImage.Hand;
                    break;
                case MessageBoxImage.None:
                    image = System.Windows.MessageBoxImage.None;
                    break;
            }
            return image;
        }
        private MessageBoxResult GetResult(System.Windows.MessageBoxResult messageBoxResult)
        {
            var result = MessageBoxResult.None;
            switch (messageBoxResult)
            {
                case System.Windows.MessageBoxResult.Yes:
                    result = MessageBoxResult.Yes;
                    break;
                case System.Windows.MessageBoxResult.No:
                    result = MessageBoxResult.No;
                    break;
                case System.Windows.MessageBoxResult.OK:
                    result = MessageBoxResult.OK;
                    break;
                case System.Windows.MessageBoxResult.Cancel:
                    result = MessageBoxResult.Cancel;
                    break;
            }
            return result;
        }

    }
    public class WpfViewService : IViewService
    {
        private readonly Dictionary<object, Window> _openWindows = new();

        public event Action<object>? RequestClose;

        public void Show<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var window = CreateWindow(viewModel);
            window.Show();
        }
        private Window GetActiveWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.IsActive)
                {
                    return window;
                }
            }
            return null;
        }
        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var window = CreateWindow(viewModel);
            return window.ShowDialog();
        }

        private Window CreateWindow<TViewModel>(TViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            var window = new Window();
            var owner = GetActiveWindow();
            if (owner != null)
                window.Owner = owner;
            var view = CreateUserControl(viewModel);

            if (view is IDynamicView dView && dView.ViewCreatingArgs != null)
                ApplyWindowArgs(window, dView.ViewCreatingArgs, owner);
            else
                ApplyDefaultWindowSettings(window, owner);

            window.Content = view;
            _openWindows[viewModel] = window;

            void CloseHandler(object vm)
            {
                if (_openWindows.TryGetValue(vm, out var w))
                    w.Close();
            }

            RequestClose += CloseHandler;
            window.Closed += (s, e) =>
            {
                RequestClose -= CloseHandler;
                _openWindows.Remove(viewModel);
            };

            return window;
        }

        private void ApplyWindowArgs(Window window, ViewCreatingArgs args, Window? owner)
        {
            window.Title = args.Title ?? string.Empty;
            window.ResizeMode = args.ResizeMode switch
            {
                ViewResizeMode.NoResize => ResizeMode.NoResize,
                ViewResizeMode.CanResize => ResizeMode.CanResize,
                ViewResizeMode.CanMinimize => ResizeMode.CanMinimize,
                ViewResizeMode.CanResizeWithGrip => ResizeMode.CanResizeWithGrip,
                _ => ResizeMode.CanResize
            };

            window.WindowStartupLocation = args.StartupLocation switch
            {
                ViewStartupLocation.Manual => WindowStartupLocation.Manual,
                ViewStartupLocation.CenterScreen => WindowStartupLocation.CenterScreen,
                ViewStartupLocation.CenterOwner => WindowStartupLocation.CenterOwner,
                _ => WindowStartupLocation.CenterOwner
            };

            window.WindowState = args.WindowState switch
            {
                ViewState.Normal => WindowState.Normal,
                ViewState.Maximized => WindowState.Maximized,
                ViewState.Minimized => WindowState.Minimized,
                _ => WindowState.Normal
            };

            window.ShowInTaskbar = args.ShowInTaskbar;

            window.SizeToContent = window.WindowState == WindowState.Normal
                ? SizeToContent.WidthAndHeight
                : SizeToContent.Manual;
        }

        private void ApplyDefaultWindowSettings(Window window, Window? owner)
        {
            window.Title = owner?.Title ?? "New Window";
            window.ResizeMode = ResizeMode.CanResizeWithGrip;
            window.WindowStartupLocation = owner == null
                ? WindowStartupLocation.CenterScreen
                : WindowStartupLocation.CenterOwner;
            window.ShowInTaskbar = true;
            window.WindowState = WindowState.Maximized;
            window.SizeToContent = SizeToContent.Manual;
        }
        private UserControl CreateUserControl<TViewModel>(TViewModel viewModel)
        {
            var attr = typeof(TViewModel).GetCustomAttribute<AssociateViewTypeAttribute>()
                       ?? throw new InvalidOperationException($"No AssociateViewTypeAttribute on {typeof(TViewModel).Name}");
            var viewType = attr.ViewType ?? throw new InvalidOperationException($"Cannot resolve view type '{attr.ViewTypeFullName}'");
            var userControl = (UserControl)Activator.CreateInstance(viewType)!;

            if (userControl is IDynamicView dynamicView)
                dynamicView.SetComponent(viewModel);
            userControl.DataContext = viewModel;
            return userControl;
        }

        public void Close(object viewModel)
        {
            RequestClose?.Invoke(viewModel);
        }
    }

}
