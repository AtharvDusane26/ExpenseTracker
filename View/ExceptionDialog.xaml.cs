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
using System.Windows.Shapes;

namespace ExpenseTracker.View
{
    /// <summary>
    /// Interaction logic for UnhandledExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionDialog : Window
    {
        private readonly Exception _exception;
        private bool _detailsVisible = false;

        public ExceptionDialog(Exception ex, string message)
        {
            InitializeComponent();
            _exception = ex;
            if (_exception != null)
            {
                BuildTreeFromException(null, _exception);
            }
            else
            {
                DetailButton.Visibility = Visibility.Collapsed;
            }
        }
     
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            _detailsVisible = !_detailsVisible;
            DetailTree.Visibility = _detailsVisible ? Visibility.Visible : Visibility.Collapsed;
            DetailButton.Content = _detailsVisible ? "Details <<" : "Details >>";
        }

        private void BuildTreeFromException(TreeViewItem parent, Exception ex)
        {
            var node = new TreeViewItem { Header = ex.Message };

            if (ex.StackTrace != null)
            {
                foreach (var line in ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    node.Items.Add(new TreeViewItem { Header = line });
                }
            }

            if (ex.InnerException != null)
            {
                var inner = new TreeViewItem { Header = "InnerException" };
                BuildTreeFromException(inner, ex.InnerException);
                node.Items.Add(inner);
            }

            if (parent == null)
                DetailTree.Items.Add(node);
            else
                parent.Items.Add(node);
        }

        private void DetailTree_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var menu = new ContextMenu();
            var copyItem = new MenuItem { Header = "Copy" };
            copyItem.Click += (s, args) =>
            {
                Clipboard.SetText(BuildMessageFromException(_exception));
            };
            menu.Items.Add(copyItem);
            DetailTree.ContextMenu = menu;
        }

        private static string BuildMessageFromException(Exception e)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{e.Source}: {e.Message}");
            sb.AppendLine(e.StackTrace);

            if (e.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendLine("Inner Exception:");
                sb.AppendLine(BuildMessageFromException(e.InnerException));
            }

            return sb.ToString();
        }
    }
}
