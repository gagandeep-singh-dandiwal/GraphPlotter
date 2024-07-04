using GraphPlotter.ServicInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.Storage.RegistrationSet;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using GraphPlotter.Common;
using System.Collections.ObjectModel;

namespace GraphPlotter.Services
{
    /// <summary>
    /// This class provides the service of plotting trigonometric functions.
    /// </summary>
    public class PlotTrignometricFunctionsService : IPlotTrignometricFunctionsService
    {
        #region Methods
        /// <summary>
        /// The method plots the sine function
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="actualCenterXWPF">The x coordinate of center</param>
        /// <param name="actualCenterYWPF">The y coordinate of center</param>
        /// <param name="internalXAxisScalingFactor">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the sine wave</returns>
        public StrokeCollection PlotSine(double graphWidth,
            double actualCenterXWPF, double actualCenterYWPF, double XOffSet, double YOffSet, double XAxisZoomFactor, double YAxisZoomFactor,
            double internalXAxisScalingFactor, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift,
            string verticalShift, StrokeCollection strokes)
        {
            StylusPointCollection stylusPointsCollection = new StylusPointCollection();
            double graphheight = 600;
            for (double i = 0; i < graphWidth; i = i + 0.1)
            {
                double omega = 2 / Convert.ToDouble(timePeriod);
                double X = (i - actualCenterXWPF) / (internalXAxisScalingFactor*XAxisZoomFactor);
                double Phi = Convert.ToDouble(phaseShift) * (Math.PI);
                double xOffSetForSine = XOffSet*Math.PI;
                //double xOffSetForSine = (graphWidth/2-actualCenterXWPF)/ Math.PI * internalXAxisScalingFactor* XAxisZoomFactor;
                //double yOffSet = (graphheight/2-actualCenterYWPF)/amplitudeEnlargingFactorInternal*YAxisZoomFactor;
                double yOffSetForSine = YOffSet * amplitudeEnlargingFactorInternal * YAxisZoomFactor;
                double sineValue = (amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal *YAxisZoomFactor*
                    Math.Sin((omega * X) - Phi- xOffSetForSine));
                double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal*YAxisZoomFactor;
                StylusPoint pointSin = new StylusPoint
                    (i, actualCenterYWPF - sineValue - D - yOffSetForSine);
                stylusPointsCollection.Add(pointSin);
            }
            Stroke SingleStroke = new Stroke(stylusPointsCollection);
            SingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };
            strokes.Add(SingleStroke);
            return strokes;
        }

        /// <summary>
        /// The method plots the cosine function
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="centerX">The x coordinate of center</param>
        /// <param name="centerY">The y coordinate of center</param>
        /// <param name="internalXAxisEnlargingFactorterY">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the cosine wave</returns>
        public StrokeCollection PlotCos(double graphWidth,
            double centerX, double centerY,
            double internalXAxisEnlargingFactorterY, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift, 
            string verticalShift, StrokeCollection strokes)
        {
            StylusPointCollection stylusPointsCollection = new StylusPointCollection();

            for (double i = 0; i < graphWidth; i = i + 0.1)
            {
                double omega = 2 / Convert.ToDouble(timePeriod);
                double X = (i - centerX) / internalXAxisEnlargingFactorterY;
                double Phi = Convert.ToDouble(phaseShift) * (Math.PI);
                double sineValue = (amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal *
                    Math.Cos((omega * X) - Phi));
                double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal;
                StylusPoint pointSin = new StylusPoint
                    (i, centerY - sineValue - D);
                stylusPointsCollection.Add(pointSin);
            }
            Stroke SingleStroke = new Stroke(stylusPointsCollection);
            SingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };
            strokes.Add(SingleStroke);
            return strokes;
        }

        /// <summary>
        /// The method plots the tangent function
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="centerX">The x coordinate of center</param>
        /// <param name="centerY">The y coordinate of center</param>
        /// <param name="internalXAxisEnlargingFactorterY">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the tangent wave</returns>
        public StrokeCollection PlotTan(double graphWidth,
            double centerX, double centerY,
            double internalXAxisEnlargingFactorterY, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift, 
            string verticalShift,StrokeCollection strokes)
        {
            double leftMostYCoordinates = -Math.Round((graphWidth / 2) / Math.PI, 0);
            double rightMostYCoordinates = Math.Round((graphWidth / 2) / Math.PI, 0);
            double omega = 2 / Convert.ToDouble(timePeriod);

            double Phi = Convert.ToDouble(phaseShift) * (Math.PI);
            //Plotting in +ve axis
            //Here I am drawing strokes from [0, π/2) in 1st loop,(π/2,π] in 2nd loop.. and so on,
            //because plotting on continous one stroke gives a line from +∞ to -∞ which is not required.
            for (int count = Convert.ToInt16(leftMostYCoordinates); count <= rightMostYCoordinates; count++)
            {
                int startingPiCount = count;
                int endPiCount = count + 1;

                StylusPointCollection stylusPointsCollection = new StylusPointCollection();
                int loopCount = 0;
                for (double i = ((startingPiCount * Math.PI / 2) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i < ((endPiCount * Math.PI / 2) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i = i + 0.1)
                {
                    //Infinity values should not be plotted, so when x=pi/2, plotting should be skipped
                    //and it needs to be skipped only the once at the beggining.
                    if (loopCount == 0)
                    {
                        loopCount++;
                        continue;
                    }
                    double X = i / internalXAxisEnlargingFactorterY;
                    double tanValue = amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal * Math.Tan(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal;
                    StylusPoint pointSin = new StylusPoint
                        (centerX + i,
                        centerY - tanValue - D);
                    stylusPointsCollection.Add(pointSin);
                }
                Stroke SingleStroke = new Stroke(stylusPointsCollection);
                SingleStroke.DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Red
                };
                strokes.Add(SingleStroke);
            }
            return strokes;
        }

        /// <summary>
        /// The method plots the Cosec function
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="centerX">The x coordinate of center</param>
        /// <param name="centerY">The y coordinate of center</param>
        /// <param name="internalXAxisEnlargingFactorterY">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the Cosec wave</returns>
        public StrokeCollection PlotCosec(double graphWidth,
            double centerX, double centerY,
            double internalXAxisEnlargingFactorterY, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift, 
            string verticalShift,StrokeCollection strokes)
        {
            double leftMostYCoordinates = -Math.Round((graphWidth / 2) / Math.PI, 0);
            double rightMostYCoordinates = Math.Round((graphWidth / 2) / Math.PI, 0);
            double omega = 2 / Convert.ToDouble(timePeriod);

            double Phi = Convert.ToDouble(phaseShift) * (Math.PI);
            //Plotting in +ve axis
            //Here I am drawing strokes from [0, π/2) in 1st loop,(π/2,π] in 2nd loop.. and so on,
            //because plotting on continous one stroke gives a line from +∞ to -∞ which is not required.
            for (int count = Convert.ToInt16(leftMostYCoordinates); count <= rightMostYCoordinates; count++)
            {
                int startingPiCount = count;
                int endPiCount = count + 1;

                StylusPointCollection stylusPointsCollection = new StylusPointCollection();
                int loopCount = 0;
                for (double i = ((startingPiCount * Math.PI) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i < ((endPiCount * Math.PI) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i = i + 0.1)
                {
                    //Infinity values should not be plotted, so when x=pi/2, plotting should be skipped
                    //and it needs to be skipped only the once at the beggining.
                    if (loopCount == 0)
                    {
                        loopCount++;
                        continue;
                    }
                    double X = i / internalXAxisEnlargingFactorterY;
                    double cosecValue = amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal / Math.Sin(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal;
                    StylusPoint point = new StylusPoint
                        (centerX + i,
                        centerY - cosecValue - D);
                    stylusPointsCollection.Add(point);
                }
                Stroke SingleStroke = new Stroke(stylusPointsCollection);
                SingleStroke.DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Red
                };
                strokes.Add(SingleStroke);
            }
            return strokes;
        }

        /// <summary>
        /// The method plots the Sec function
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="centerX">The x coordinate of center</param>
        /// <param name="centerY">The y coordinate of center</param>
        /// <param name="internalXAxisEnlargingFactorterY">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the Sec wave</returns>
        public StrokeCollection PlotSec(double graphWidth,
            double centerX, double centerY,
            double internalXAxisEnlargingFactorterY, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift, 
            string verticalShift,StrokeCollection strokes)
        {
            double leftMostYCoordinates = -Math.Round((graphWidth / 2) / Math.PI, 0);
            double rightMostYCoordinates = Math.Round((graphWidth / 2) / Math.PI, 0);
            double omega = 2 / Convert.ToDouble(timePeriod);

            double Phi = Convert.ToDouble(phaseShift) * (Math.PI);
            //Plotting in +ve axis
            //Here I am drawing strokes from [0, π/2) in 1st loop,(π/2,π] in 2nd loop.. and so on,
            //because plotting on continous one stroke gives a line from +∞ to -∞ which is not required.
            for (int count = Convert.ToInt16(leftMostYCoordinates); count <= rightMostYCoordinates; count++)
            {
                int startingPiCount = count;
                int endPiCount = count + 1;

                StylusPointCollection stylusPointsCollection = new StylusPointCollection();
                int loopCount = 0;

                for (double i = ((startingPiCount * Math.PI / 2) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i < ((endPiCount * Math.PI / 2) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i = i + 0.1)
                {
                    //Infinity values should not be plotted, so when x=pi/2, plotting should be skipped
                    //and it needs to be skipped only the once at the beggining.
                    if (loopCount == 0)
                    {
                        loopCount++;
                        continue;
                    }
                    double X = i / internalXAxisEnlargingFactorterY;
                    double tanValue = amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal / Math.Cos(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal;
                    StylusPoint pointSin = new StylusPoint
                        (centerX + i,
                        centerY - tanValue - D);
                    stylusPointsCollection.Add(pointSin);
                }
                Stroke SingleStroke = new Stroke(stylusPointsCollection);
                SingleStroke.DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Red
                };
                strokes.Add(SingleStroke);
            }
            return strokes;
        }

        /// <summary>
        /// The method plots the Cot function
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="centerX">The x coordinate of center</param>
        /// <param name="centerY">The y coordinate of center</param>
        /// <param name="internalXAxisEnlargingFactorterY">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the Cot wave</returns>
        public StrokeCollection PlotCot(double graphWidth,
            double centerX, double centerY,
            double internalXAxisEnlargingFactorterY, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift, 
            string verticalShift,StrokeCollection strokes)
        {
            double leftMostYCoordinates = -Math.Round((graphWidth / 2) / Math.PI, 0);
            double rightMostYCoordinates = Math.Round((graphWidth / 2) / Math.PI, 0);
            double omega = 2 / Convert.ToDouble(timePeriod);

            double Phi = Convert.ToDouble(phaseShift) * (Math.PI);
            //Plotting in +ve axis
            //Here I am drawing strokes from [0, π/2) in 1st loop,(π/2,π] in 2nd loop.. and so on,
            //because plotting on continous one stroke gives a line from +∞ to -∞ which is not required.
            for (int count = Convert.ToInt16(leftMostYCoordinates); count <= rightMostYCoordinates; count++)
            {
                int startingPiCount = count;
                int endPiCount = count + 1;

                StylusPointCollection stylusPointsCollection = new StylusPointCollection();
                int loopCount = 0;
                for (double i = ((startingPiCount * Math.PI / 2) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i < ((endPiCount * Math.PI / 2) + Phi) * internalXAxisEnlargingFactorterY / omega;
                    i = i + 0.1)
                {
                    //Infinity values should not be plotted, so when x=pi/2, plotting should be skipped
                    //and it needs to be skipped only the once at the beggining.
                    if (loopCount == 0)
                    {
                        loopCount++;
                        continue;
                    }
                    double X = i / internalXAxisEnlargingFactorterY;
                    double tanValue = amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal / Math.Tan(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal;
                    StylusPoint pointSin = new StylusPoint
                        (centerX + i,
                        centerY - tanValue - D);
                    stylusPointsCollection.Add(pointSin);
                }
                Stroke SingleStroke = new Stroke(stylusPointsCollection);
                SingleStroke.DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Red
                };
                strokes.Add(SingleStroke);
            }
            return strokes;
        }

        /// <summary>
        /// The method plots the SinC function
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="centerX">The x coordinate of center</param>
        /// <param name="centerY">The y coordinate of center</param>
        /// <param name="internalXAxisEnlargingFactorterY">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the SinC wave</returns>
        public StrokeCollection PlotSinC(double graphWidth,
            double centerX, double centerY,
            double internalXAxisEnlargingFactorterY, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift, 
            string verticalShift,StrokeCollection strokes)
        {
            StylusPointCollection stylusPointsCollection = new StylusPointCollection();

            for (double i = 0; i < graphWidth; i = i + 0.1)
            {
                double X = (i - centerX) / internalXAxisEnlargingFactorterY;
                double Phi = Convert.ToDouble(phaseShift) * (Math.PI);
                double numenator = (amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal *
                    Math.Sin(Math.PI * (X  - Phi) / Convert.ToDouble(timePeriod)));
                double denominator = Math.PI * (X  - Phi) / Convert.ToDouble(timePeriod);
                double sinCValue = numenator / denominator;
                double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal;
                StylusPoint pointSin = new StylusPoint
                    (i, centerY - sinCValue - D);
                stylusPointsCollection.Add(pointSin);
            }
            Stroke SingleStroke = new Stroke(stylusPointsCollection);
            SingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };
            strokes.Add(SingleStroke);
            return strokes;
        }
        #endregion
    }
}
