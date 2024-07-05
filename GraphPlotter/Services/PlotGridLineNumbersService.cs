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
        public void AddXAxisNumber(double _actualCenterX, double _actualCenterY,
            double xOffset, double yOffset,
            double GraphWidth, double GraphHeight, 
            double XAxisZoomFactor, double yaxiszoomfactor,
            double InternalXAxisScalingFactor,ref ObservableCollection<Number> XAxisNumbers)
        {
            XAxisNumbers = new ObservableCollection<Number>();
            int numberOfLines = Convert.ToInt32(GraphWidth / (Math.PI * InternalXAxisScalingFactor) - 1);
            //+ve x axis numbers
            for (int i = 0; i < numberOfLines / 2 + 1; i++)
            {
                Number number = new Number();
                number.X = GraphWidth / 2 + (i * Math.PI * InternalXAxisScalingFactor);
                number.Y = GraphHeight / 2;
                if (i == 0)
                    number.Value = "(" + (Math.Round(xOffset + i / XAxisZoomFactor, 4)).ToString() + "π" + "," + (yOffset + i / yaxiszoomfactor)+")";
                else
                    number.Value = (Math.Round(xOffset + i / XAxisZoomFactor, 4)).ToString() + "π";

                XAxisNumbers.Add(number);
            }

            //-ve x axis numbers
            for (int i = -1; i > -numberOfLines / 2 - 1; i--)
            {
                Number number = new Number();
                number.X = GraphWidth / 2 + (i * Math.PI * InternalXAxisScalingFactor);
                number.Y = GraphHeight / 2; 
                number.Value = (Math.Round(xOffset + i / XAxisZoomFactor, 4)).ToString() + "π";

                //number.Value = Math.Round(((GraphWidth / 2 - _actualCenterX) / (Math.PI * InternalXAxisScalingFactor * XAxisZoomFactor) + i / (XAxisZoomFactor)), 4).ToString() + "π";

                XAxisNumbers.Add(number);
            }
        }

        /// <summary>
        /// This method is used to add numbers on the Y-Axis.
        /// </summary>
        public void AddYAxisNumber(double _falseCenterX, double _falseCenterY,
            double xOffset, double yOffset,
            double GraphWidth, double GraphHeight, double XAxisZoomFactor, double YAxisZoomFactor,
            double AmplitudeEnlargingFactorInternal, double InternalXAxisScalingFactor,ref ObservableCollection<Number> YAxisNumbers)
        {
            int numberOfLines = Convert.ToInt32(GraphHeight /AmplitudeEnlargingFactorInternal);
            YAxisNumbers = new ObservableCollection<Number>();
            for (int i = 0; i < numberOfLines/2; i++)
            {
                Number number = new Number();
                if (i==0)
                {
                    continue;
                }
                number.X = GraphWidth/2;
                number.Y = GraphHeight / 2 - (i * AmplitudeEnlargingFactorInternal);
                number.Value = Convert.ToString(yOffset+i/YAxisZoomFactor);
                //number.Value = ((_falseCenterY - GraphHeight / 2) / (AmplitudeEnlargingFactorInternal * YAxisZoomFactor) + i / YAxisZoomFactor).ToString();

                YAxisNumbers.Add(number);
            }
            for (int i = -1; i > -numberOfLines / 2 - 1; i--)
            {
                Number number = new Number();
                number.X = GraphWidth / 2;
                number.Y = GraphHeight / 2 - (i * AmplitudeEnlargingFactorInternal);
                number.Value = Convert.ToString(yOffset + i / YAxisZoomFactor);
                //number.Value = ((_falseCenterY - GraphHeight / 2) / (AmplitudeEnlargingFactorInternal * YAxisZoomFactor) + i / YAxisZoomFactor).ToString();
                YAxisNumbers.Add(number);
            }
        }
    }
}
