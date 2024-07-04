using GraphPlotter.Common;
using GraphPlotter.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotter.Services
{
    internal class PlotGridLineNumbersService : IPlotGridLineNumbersService
    {
        /// <summary>
        /// The method is used to add numbers on the X-Axis.
        /// </summary>
        public void AddXAxisNumber(double _centerX, double _centerY,
            double GraphWidth, double GraphHeight, double XAxisZoomFactor,
            double InternalXAxisScalingFactor,ref ObservableCollection<Number> XAxisNumbers)
        {
            XAxisNumbers = new ObservableCollection<Number>();
            int numberOfLines = Convert.ToInt32(GraphWidth / (Math.PI * InternalXAxisScalingFactor) - 1);
            double start = -GraphWidth / (2 * Math.PI * InternalXAxisScalingFactor * XAxisZoomFactor);
            double end = -start;
            //+ve x axis numbers
            for (int i = 0; i < numberOfLines / 2 + 1; i++)
            {
                Number number = new Number();

                if (i == 0)
                {
                    number.X = _centerX;
                    number.Y = _centerY;
                    number.Value = "0";
                }
                else
                {
                    number.X = _centerX + (i * Math.PI * InternalXAxisScalingFactor);
                    number.Y = _centerY;
                    if (i / XAxisZoomFactor == 1)
                    {
                        number.Value = "π";
                    }
                    else
                    {
                        number.Value = Math.Round((i / XAxisZoomFactor), 4).ToString() + "π";
                    }
                }
                XAxisNumbers.Add(number);
            }
            //-ve x axis numbers
            for (int i = -1; i > -numberOfLines / 2 - 1; i--)
            {
                Number number = new Number();

                number.X = _centerX + (i * Math.PI * InternalXAxisScalingFactor);
                number.Y = _centerY;
                if (i / XAxisZoomFactor == -1)
                {
                    number.Value = "-π";
                }
                else
                {
                    number.Value = Math.Round((i / XAxisZoomFactor), 4).ToString() + "π";
                }

                XAxisNumbers.Add(number);
            }
        }

        /// <summary>
        /// This method is used to add numbers on the Y-Axis.
        /// </summary>
        public void AddYAxisNumber(double _centerX, double _centerY,
            double GraphWidth, double GraphHeight, double XAxisZoomFactor, double YAxisZoomFactor,
            double AmplitudeEnlargingFactorInternal, double InternalXAxisScalingFactor,ref ObservableCollection<Number> YAxisNumbers)
        {
            YAxisNumbers = new ObservableCollection<Number>();
            double start = -GraphHeight / (2 * AmplitudeEnlargingFactorInternal * YAxisZoomFactor);
            double end = -start;
            for (double i = start; i < end; i = i + 1 / YAxisZoomFactor)
            {
                Number number = new Number();

                if (i == 0)
                {
                    continue;
                }
                else
                {
                    number.X = _centerX + 4;
                    number.Y = _centerY - (i * AmplitudeEnlargingFactorInternal * YAxisZoomFactor);
                    number.Value = i.ToString();
                }
                YAxisNumbers.Add(number);
            }
        }
    }
}
