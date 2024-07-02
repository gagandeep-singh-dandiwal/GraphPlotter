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

            // Initialize the Unity container
            _container = new UnityContainer();
            ConfigureContainer(_container);

            // Resolve and show the main window
            var mainWindow = _container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureContainer(IUnityContainer container)
        {
            // Register types with the container
            container.RegisterType<IPlotTrignometricFunctionsService, PlotTrignometricFunctionsService>();
            container.RegisterType<IPlotGridLinesService, PlotGridLinesService>();
            container.RegisterType<MainWindow>();
        }
    }
}
