using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using WpfCalava.ViewModels;

namespace WpfCalava
{
    /// <summary>
    /// MEF based Caliburn Micro bootstrapper.
    /// </summary>
    public sealed class MefBootstrapper : BootstrapperBase
    {
        private ComposablePartCatalog _catalog;
        private CompositionContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="MefBootstrapper"/> class.
        /// </summary>
        public MefBootstrapper()
        {
            Initialize();
        }

        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            // MEF catalog
            _catalog = new AggregateCatalog(
                // this app
                new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            _container = new CompositionContainer(_catalog);
            CompositionBatch batch = new CompositionBatch();

            // singletons:
            // window manager and events aggregator
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            IEventAggregator events = new EventAggregator();
            batch.AddExportedValue(events);

            // application
            batch.AddExportedValue<IApplication>(new ApplicationViewModel(events));

            // compose
            batch.AddExportedValue(_container);
            _container.Compose(batch);

#if DEBUG
            LogManager.GetLog = type => new DebugLogger(type);
#endif
        }

        /// <summary>
        /// Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>A list of assemblies to inspect.</returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            // Add your own view assemblies to return a list of searchable assemblies.
            // If you are dynamically loading modules, make sure they get registered 
            // with your IoC container and the AssemblySource.Instance when they are loaded.

            return new[]
            {
                Assembly.GetExecutingAssembly()
                // example: , Assembly.GetAssembly(typeof(SomeRepository))
            };
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="key">The key.</param>
        /// <returns>instance</returns>
        /// <exception cref="System.Exception">unable to locate contract</exception>
        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ?
                AttributedModelServices.GetContractName(serviceType) :
                key;
            var exports = _container.GetExportedValues<object>(contract);

            var export = exports.FirstOrDefault();
            if (export != null) return export;

            throw new Exception(String.Format
                ("Could not locate any instances of contract \"{0}\"", contract));
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instances</returns>
        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>
                (AttributedModelServices.GetContractName(serviceType));
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }

        /// <summary>
        /// Override this to add custom behavior for unhandled exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            base.OnUnhandledException(sender, e);

            // TODO eventually log error, and show it as preferred
            Debug.WriteLine(e.Exception.ToString());
            MessageBox.Show(e.Exception.Message);
        }
    }
}
