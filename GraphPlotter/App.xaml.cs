using GraphPlotter.Services;
using GraphPlotter.ServicInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace GraphPlotter
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            try
            {
                // Initialize the Unity container
                _container = new UnityContainer();
                ConfigureContainer(_container);

                // Resolve and show the main window
                var mainWindow = _container.Resolve<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

        private void ConfigureContainer(IUnityContainer container)
        {
            // Register types with the container
            container.RegisterType<IPlotTrignometricFunctionsService, PlotTrignometricFunctionsService>();
            container.RegisterType<IPlotGridLinesService, PlotGridLinesService>();
            container.RegisterType<MainWindow>();
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log the exception, show a message box, etc.
            MessageBox.Show("An unexpected error occurred: " + e.Exception.Message);
            e.Handled = true; // Prevent the application from crashing
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception, show a message box, etc.
            MessageBox.Show("An unexpected error occurred: " + ((Exception)e.ExceptionObject).Message);
            // The application will crash after this point
        }
        protected override void OnExit(ExitEventArgs e)
        {
            GraphPlotter.Properties.Settings.Default.Save();
        }
    }
}
