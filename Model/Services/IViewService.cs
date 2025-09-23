using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Services
{
    public interface IDynamicView
    {
        void SetComponent(object component);
        ViewCreatingArgs ViewCreatingArgs { get; }
    }
    public class ViewCreatingArgs
    {
        public string? Title { get; set; }
        public ViewResizeMode ResizeMode { get; set; } = ViewResizeMode.CanResize;
        public ViewStartupLocation StartupLocation { get; set; } = ViewStartupLocation.CenterOwner;
        public ViewState WindowState { get; set; } = ViewState.Normal;
        public bool ShowInTaskbar { get; set; } = true;
        public bool IsModal { get; set; } = false;
      
    }

    /// <summary>UI-independent resize modes</summary>
    public enum ViewResizeMode
    {
        NoResize,
        CanResize,
        CanMinimize,
        CanResizeWithGrip
    }

    /// <summary>UI-independent window startup locations</summary>
    public enum ViewStartupLocation
    {
        Manual,
        CenterScreen,
        CenterOwner
    }
    public enum ViewState
    {
        Normal,
        Minimized,
        Maximized
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class AssociateViewTypeAttribute : Attribute
    {
        public AssociateViewTypeAttribute(string viewTypeFullName)
        {
            ViewTypeFullName = viewTypeFullName;
        }

        public string ViewTypeFullName { get; }

        public Type ViewType
        {

            get
            {
                var type = Type.GetType(ViewTypeFullName);
                if (type != null)
                    return type;

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(ViewTypeFullName, throwOnError: false, ignoreCase: false);
                    if (type != null) return type;
                }
                return type;

            }
        }
    }
    public interface IViewService
    {
        void Show<TViewModel>(TViewModel viewModel) where TViewModel : class;
        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class;
        event Action<object>? RequestClose;
        void Close(object viewModel);
    }
}
