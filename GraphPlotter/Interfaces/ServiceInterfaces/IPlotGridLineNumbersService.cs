using GraphPlotter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotter.Interfaces.ServiceInterfaces
{
    public interface IPlotGridLineNumbersService

    {
        void AddXAxisNumber(double _centerX, double _centerY,
            double xOffset, double yOffset,
            double GraphWidth, double GraphHeight, 
            double XAxisZoomFactor, double yaxiszoomfactor,
            double InternalXAxisScalingFactor,ref ObservableCollection<Number> XAxisNumbers);

        void AddYAxisNumber(double _centerX, double _centerY,
            double xOffset, double yOffset,
            double GraphWidth, double GraphHeight, double XAxisZoomFactor, double YAxisZoomFactor,
            double AmplitudeEnlargingFactorInternal, double InternalXAxisScalingFactor,ref ObservableCollection<Number> YAxisNumbers);
    }
}
