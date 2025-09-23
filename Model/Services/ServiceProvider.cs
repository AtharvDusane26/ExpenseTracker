using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExpenseTracker.Model.Services
{
    public sealed class ServiceProvider : IDisposable
    {
        private static readonly Lazy<ServiceProvider> _instance = new Lazy<ServiceProvider>(() => new ServiceProvider());
        private bool _disposed = false;
        private readonly IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        private ServiceProvider()
        {
            _services = new ServiceCollection();
        }

        public static ServiceProvider Instance => _instance.Value;

        public void AddSingleton<TService>(TService implementationInstance) where TService : class
        {
            _services.AddSingleton(implementationInstance);
        }
     
        public void AddTransient<TService>(Func<IServiceProvider, TService> factory) where TService : class
        {
            _services.AddTransient(factory);
        }

        /// <summary>
        /// Build the service provider (call after registering all services)
        /// </summary>
        public void BuildServiceProvider()
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = _services.BuildServiceProvider();
            }
        }

        /// <summary>
        /// Resolve a service (throws if not registered)
        /// </summary>
        public T Resolve<T>() where T : class
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("ServiceProvider not built. Call BuildServiceProvider() first.");

            return _serviceProvider.GetRequiredService<T>();
        }
        public void Dispose()
        {
            if (_disposed) return;

            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _services.Clear();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
