using GraphPlotter.Common;
using GraphPlotter.ServicInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.Storage.RegistrationSet;

namespace GraphPlotter.Services
{
    /// <summary>
    /// The class provides the service of plotting the x and y axis grid lines.
    /// </summary>
    public class PlotGridLinesService : IPlotGridLinesService
    {
        #region Methods
        /// <summary>
        /// This method draws the y axis grid lines.
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="graphHeight">The height of the graph</param>
        /// <param name="centerY">The center y coordinate.</param>
        /// <param name="amplitudeEnlargingFactorInternal">The y axis enrlagin factor</param>
        /// <param name="internalXAxisEnlargingFactor">The x axis enlargin factor</param>
        /// <param name="yAxisNumberIndicatorLines">The collection of y axis grid lines</param>
        public void AddYaxisGridLines(double graphWidth, double graphHeight,
            double centerY, double amplitudeEnlargingFactorInternal,
            double internalXAxisEnlargingFactor,
            ObservableCollection<GridLine> yAxisNumberIndicatorLines)
        {
            for (double i = 0; i < graphHeight; i = i + amplitudeEnlargingFactorInternal / 4)
            {
                GridLine numberIndicatorLine = new GridLine();
                numberIndicatorLine.StartXPoint = 0;
                numberIndicatorLine.StartYPoint = i;
                numberIndicatorLine.EndXPoint = graphWidth;
                numberIndicatorLine.EndYPoint = i;
                if (i % amplitudeEnlargingFactorInternal == 0)
                {
                    if (numberIndicatorLine.StartYPoint == centerY)
                    {
                        numberIndicatorLine.Thickness = 3;
                    }
                    else
                        numberIndicatorLine.Thickness = 2;
                }
                else
                {
                    numberIndicatorLine.Thickness = 0.5;
                }
                yAxisNumberIndicatorLines.Add(numberIndicatorLine);
            }
        }

        /// <summary>
        /// This method draws the x axis grid lines.
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="graphHeight">The height of the graph</param>
        /// <param name="centerX">The center x coordinate</param>
        /// <param name="internalXAxisEnlargingFactor">The x axis enlarging factor</param>
        /// <param name="xAxisNumberIndicatorLines">The collection of x axis grid lines</param>
        public void AddXaxisGridLines(double graphWidth, double graphHeight,
            double centerX,
            double internalXAxisEnlargingFactor,
            ObservableCollection<GridLine> xAxisNumberIndicatorLines)
        {
            //+ve X axis lines
            //increments in multiples of pi/4
            int lineCount = 0;
            for (double i = 0; i < graphWidth / 2; i = i + Math.PI / 4 * internalXAxisEnlargingFactor)
            {
                GridLine numberIndicatorLine = new GridLine();
                numberIndicatorLine.StartXPoint = i + centerX;
                numberIndicatorLine.StartYPoint = 0;
                numberIndicatorLine.EndXPoint = i + centerX;
                numberIndicatorLine.EndYPoint = graphHeight;

                //the Y axis has to be thickest
                if (i == 0)
                {
                    numberIndicatorLine.Thickness = 3;
                    xAxisNumberIndicatorLines.Add(numberIndicatorLine);
                }
                // all the Pi's have to be thick
                else if (lineCount % 4 == 0)
                {
                    numberIndicatorLine.Thickness = 2;
                    xAxisNumberIndicatorLines.Add(numberIndicatorLine);
                }
                else
                {
                    numberIndicatorLine.Thickness = 1;
                    xAxisNumberIndicatorLines.Add(numberIndicatorLine);
                }
                lineCount++;
            }

            // -ve x axis lines
            lineCount = 0;
            for (double i = 0; i < graphWidth / 2; i = i + Math.PI / 4 * internalXAxisEnlargingFactor)
            {
                GridLine numberIndicatorLine = new GridLine();
                //i in the below line has been multiplied by 10 , because the enlarging factor is 10
                //That means the actual 1 on x-axis is not 1 but 10
                numberIndicatorLine.StartXPoint = centerX -  i;
                numberIndicatorLine.StartYPoint = 0;
                numberIndicatorLine.EndXPoint = centerX - i;
                numberIndicatorLine.EndYPoint = graphHeight;

                //the Y axis has to be thickest
                if (i == 0)
                {
                    numberIndicatorLine.Thickness = 3;
                    xAxisNumberIndicatorLines.Add(numberIndicatorLine);
                }
                // all the Pi's have to be thick
                else if (lineCount % 4 == 0)
                {
                    numberIndicatorLine.Thickness = 2;
                    xAxisNumberIndicatorLines.Add(numberIndicatorLine);
                }
                else
                {
                    numberIndicatorLine.Thickness = 1;
                    xAxisNumberIndicatorLines.Add(numberIndicatorLine);
                }
                lineCount++;
            }
        }
        #endregion

    }
}
