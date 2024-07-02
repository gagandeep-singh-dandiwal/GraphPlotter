using GraphPlotter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotter.ServicInterfaces.Interfaces
{
    public interface IPlotGridLinesService
    {
        void AddYaxisGridLines(double graphWidth, double graphHeight,
            double centerY, double amplitudeEnlargingFactorInternal,
            double internalXAxisEnlargingFactor,
            ObservableCollection<GridLine> yAxisNumberIndicatorLines);

        void AddXaxisGridLines(double graphWidth, double graphHeight,
            double centerX,
            double internalXAxisEnlargingFactor,
            ObservableCollection<GridLine> xAxisNumberIndicatorLines);
    }
}
