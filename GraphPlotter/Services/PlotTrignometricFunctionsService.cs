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
        /// This method plots the sine function according to the formula y = A sin(wx - φ) + D,
        /// where A is amplitude,
        /// w is 2pi/T,
        /// φ is phase shift in radians,
        /// D is vertical shift
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="graphHeight">The height of the graph</param>
        /// <param name="actualCenterXWPF">The actual x coordinate of center, in terms of wpf width</param>
        /// <param name="actualCenterYWPF">The actual y coordinate of center, in terms of wpf height</param>
        /// <param name="XOffSet">the x coordinate of the point which is at the center</param>
        /// <param name="YOffSet">the y coordinate of the point which is at the center</param>
        /// <param name="XAxisZoomFactor">the factor by which the x axis has been zoomed in</param>
        /// <param name="YAxisZoomFactor">the factor by which the y axis has been zoomed in</param>
        /// <param name="internalXAxisScalingFactor">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the sine wave</returns>
        public StrokeCollection PlotSine(double graphWidth,double graphHeight,
            double actualCenterXWPF, double actualCenterYWPF, 
            double XOffSet, double YOffSet, 
            double XAxisZoomFactor, double YAxisZoomFactor,
            double internalXAxisScalingFactor, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift,
            string verticalShift, StrokeCollection strokes)
        {
            StylusPointCollection stylusPointsCollection = new StylusPointCollection();
            //plotting the positive x axis
            for (double j = graphWidth/2; j<graphWidth; j=j+0.1)
            {
                //calculating omega w, which is 2pi/T
                double omega = 2 / Convert.ToDouble(timePeriod);

                //calculating the x coordinate for the sine wave, which depends on several factor
                double X = ((j - graphWidth/2) / (internalXAxisScalingFactor * XAxisZoomFactor))+XOffSet*Math.PI;

                //calculating the phase shift i.e φ
                double Phi = Convert.ToDouble(phaseShift) * (Math.PI);

                //calculating the value of the sine wave, multiplying by the vertical zoom in, amplitude(A) and internal scaling factor
                //so that the graph can be enlarged.
                double sineValue = (amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal * YAxisZoomFactor *
                    Math.Sin((omega * X) - Phi));

                //Calculation vertial shift D
                double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Calculation offset according to center coordinates
                double yOffSetForSine = YOffSet * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Creating the point to be put on the canvas, the x coordinate is j and y coordinate is calculated
                StylusPoint point = new StylusPoint
                    (j, (graphHeight/2) - sineValue - D + yOffSetForSine);

                //Adding the point to the collection of points
                stylusPointsCollection.Add(point);
            }

            //Add the points collection to the stroke
            Stroke PositiveXAxisSingleStroke = new Stroke(stylusPointsCollection);

            //Assign the stroke color red.
            PositiveXAxisSingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };
            stylusPointsCollection = new StylusPointCollection();

            //plotting the negative x axis, notice that j value is reduced in this loop
            //So the graph is being plotted from center to the left side of the graph
            for (double j = graphWidth / 2; j > 0; j = j - 0.1)
            {
                //calculating omega w, which is 2pi/T
                double omega = 2 / Convert.ToDouble(timePeriod);

                //calculating the x coordinate for the sine wave, which depends on several factor
                double X = ((j - graphWidth / 2) / (internalXAxisScalingFactor * XAxisZoomFactor)) + XOffSet * Math.PI;

                //calculating the phase shift i.e φ
                double Phi = Convert.ToDouble(phaseShift) * (Math.PI);

                //calculating the value of the sine wave, multiplying by the vertical zoom in, amplitude(A) and internal scaling factor
                double sineValue = (amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal * YAxisZoomFactor *
                    Math.Sin((omega * X) - Phi));

                //Calculation vertial shift D
                double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Calculation offset according to center coordinates set
                double yOffSetForSine = YOffSet * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Creating the point to be put on the canvas, the x coordinate is j and y coordinate is calculated
                StylusPoint pointSin = new StylusPoint
                    (j, (graphHeight / 2) - sineValue - D + yOffSetForSine);

                //Add the point to the collection of stylus points
                stylusPointsCollection.Add(pointSin);
            }
            Stroke NegativeXAxisSingleStroke = new Stroke(stylusPointsCollection);
            NegativeXAxisSingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };
            strokes.Add(PositiveXAxisSingleStroke);
            strokes.Add(NegativeXAxisSingleStroke);
            return strokes;
        }

        /// <summary>
        /// This method plots the sine function according to the formula y = A cos(wx - φ) + D,
        /// where A is amplitude,
        /// w is 2pi/T,
        /// φ is phase shift in radians,
        /// D is vertical shift
        /// </summary>
        /// <param name="graphWidth">The width of the graph</param>
        /// <param name="actualCenterXWPF">The x coordinate of center</param>
        /// <param name="actualCenterYWPF">The y coordinate of center</param>
        /// <param name="internalXAxisEnlargingFactorterY">The internal x axis sclaing factor</param>
        /// <param name="amplitudeEnlargingFactorInternal">The internal amplitude enlarging factor</param>
        /// <param name="amplitudeEnlargingFactorExternal">The amplitude A of the wave</param>
        /// <param name="timePeriod">The time period of the wave</param>
        /// <param name="phaseShift">The value of phi i.e. the change in phase</param>
        /// <param name="verticalShift">The value of D i.e the shift along the y axis</param>
        /// <param name="strokes">The colleciton of strokes which make up the wave</param>
        /// <returns>The collection of strokes which make up the cosine wave</returns>
        public StrokeCollection PlotCos(double graphWidth,double graphHeight,
            double actualCenterXWPF, double actualCenterYWPF,
            double XOffSet, double YOffSet,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double internalXAxisScalingFactor, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift, 
            string verticalShift, StrokeCollection strokes)
        {
            StylusPointCollection stylusPointsCollection = new StylusPointCollection();
            //plotting the positive x axis
            for (double j = graphWidth / 2; j < graphWidth; j = j + 0.1)
            {
                //Calculate omega w, which is 2pi/T
                double omega = 2 / Convert.ToDouble(timePeriod);

                //Calculate the x coordinate for the sine wave, which depends on several factors
                double X = ((j - graphWidth / 2) / (internalXAxisScalingFactor * XAxisZoomFactor)) + XOffSet * Math.PI;

                //Calculate the phase shift i.e φ
                double Phi = Convert.ToDouble(phaseShift) * (Math.PI);

                //Calculate the value of the sine wave, multiplying by the vertical zoom in, amplitude(A) and internal scaling factor
                double cosValue = (amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal * YAxisZoomFactor *
                    Math.Cos((omega * X) - Phi));

                //Calculate vertial shift D
                double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Calculate offset according to center coordinates
                double yOffSetForSine = YOffSet * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Creating the point to be put on the canvas, the x coordinate is j and y coordinate is calculated
                StylusPoint pointSin = new StylusPoint
                    (j, (graphHeight / 2) - cosValue - D + yOffSetForSine);
                stylusPointsCollection.Add(pointSin);
            }
            Stroke PositiveXAxisSingleStroke = new Stroke(stylusPointsCollection);
            PositiveXAxisSingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };
            stylusPointsCollection = new StylusPointCollection();

            //plotting the negative x axis
            for (double j = graphWidth / 2; j > 0; j = j - 0.1)
            {
                //Calculate omega w, which is 2pi/T
                double omega = 2 / Convert.ToDouble(timePeriod);

                //Calculate the x coordinate for the sine wave, which depends on several factors
                double X = ((j - graphWidth / 2) / (internalXAxisScalingFactor * XAxisZoomFactor)) + XOffSet * Math.PI;

                //Calculate the phase shift i.e φ
                double Phi = Convert.ToDouble(phaseShift) * (Math.PI);

                //Calculate the value of the sine wave, multiplying by the vertical zoom in, amplitude(A) and internal scaling factor
                double cosValue = (amplitudeEnlargingFactorInternal *
                    amplitudeEnlargingFactorExternal * YAxisZoomFactor *
                    Math.Cos((omega * X) - Phi));

                //Calculate vertial shift D
                double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Calculate offset according to center coordinates
                double yOffSetForSine = YOffSet * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Creating the point to be put on the canvas, the x coordinate is j and y coordinate is calculated
                StylusPoint pointSin = new StylusPoint
                    (j, (graphHeight / 2) - cosValue - D + yOffSetForSine);
                stylusPointsCollection.Add(pointSin);
            }
            Stroke NegativeXAxisSingleStroke = new Stroke(stylusPointsCollection);
            NegativeXAxisSingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };

            //Add the positive and the negative axis stroke to the collection of strokes.
            strokes.Add(PositiveXAxisSingleStroke);
            strokes.Add(NegativeXAxisSingleStroke);
            return strokes;
        }

        /// <summary>
        /// The method plots the SinC function according to the formula y = A sin(pi * X)/ (pi*X) + D
        /// where X equals to (x - φ)/T
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
        public StrokeCollection PlotSinC(double graphWidth, double graphHeight,
            double centerX, double centerY,
            double XOffSet, double YOffSet,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double internalXAxisScalingFactor, double amplitudeEnlargingFactorInternal,
            double amplitudeEnlargingFactorExternal, string timePeriod, string phaseShift,
            string verticalShift, StrokeCollection strokes)
        {
            StylusPointCollection stylusPointsCollection = new StylusPointCollection();
            //plotting the positive x axis
            for (double j = graphWidth / 2 + 0.0001; j < graphWidth; j = j + 0.1)
            {
                //Calculate X
                double X2 = ((j - graphWidth / 2) / (internalXAxisScalingFactor * XAxisZoomFactor)) + XOffSet * Math.PI;

                //Calculate phi
                double Phi2 = Convert.ToDouble(phaseShift) * (Math.PI);

                //Calculate the numerator
                double numenator2 = (amplitudeEnlargingFactorInternal * YAxisZoomFactor *
                    amplitudeEnlargingFactorExternal *
                    Math.Sin(Math.PI * (X2 - Phi2) / Convert.ToDouble(timePeriod)));

                //Calculate the denomenator
                double denominator2 = Math.PI * (X2 - Phi2) / Convert.ToDouble(timePeriod);

                //Calculate the value of the SinC wave
                double sinCValue2 = numenator2 / denominator2;

                //Calculate the vertical shift D
                double D2 = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Calculate the offset according to the center coordinates
                double yOffSetForSine = YOffSet * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Create the point to be put on the canvas
                StylusPoint pointSin = new StylusPoint
                    (j, (graphHeight / 2) - sinCValue2 - D2 + yOffSetForSine);
                stylusPointsCollection.Add(pointSin);
            }
            Stroke PositiveXAxisSingleStroke = new Stroke(stylusPointsCollection);
            PositiveXAxisSingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };

            //plotting the negative x axis
            stylusPointsCollection = new StylusPointCollection();
            for (double j = graphWidth / 2 - 0.0001; j > 0; j = j - 0.1)
            {
                //Calculate X
                double X2 = ((j - graphWidth / 2) / (internalXAxisScalingFactor * XAxisZoomFactor)) + XOffSet * Math.PI;

                //Calculate phi
                double Phi2 = Convert.ToDouble(phaseShift) * (Math.PI);

                //Calculate the numerator
                double numenator2 = (amplitudeEnlargingFactorInternal * YAxisZoomFactor *
                    amplitudeEnlargingFactorExternal *
                    Math.Sin(Math.PI * (X2 - Phi2) / Convert.ToDouble(timePeriod)));

                //Calculate the denomenator
                double denominator2 = Math.PI * (X2 - Phi2) / Convert.ToDouble(timePeriod);

                //Calculate the value of the SinC wave
                double sinCValue2 = numenator2 / denominator2;

                //Calculate the vertical shift D
                double D2 = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Calculate the offset according to the center coordinates
                double yOffSetForSine = YOffSet * amplitudeEnlargingFactorInternal * YAxisZoomFactor;

                //Create the point to be put on the canvas
                StylusPoint pointSin = new StylusPoint
                    (j, (graphHeight / 2) - sinCValue2 - D2 + yOffSetForSine);
                stylusPointsCollection.Add(pointSin);
            }
            Stroke NegativeXAxisSingleStroke = new Stroke(stylusPointsCollection);
            NegativeXAxisSingleStroke.DrawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
            };

            //Add the positive and the negative axis stroke to the collection of strokes.
            strokes.Add(PositiveXAxisSingleStroke);
            strokes.Add(NegativeXAxisSingleStroke);
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
            double XAxisZoomFactor, double YAxisZoomFactor,
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
                    double X = i / (internalXAxisEnlargingFactorterY*XAxisZoomFactor);
                    double tanValue = amplitudeEnlargingFactorInternal * YAxisZoomFactor *
                    amplitudeEnlargingFactorExternal * Math.Tan(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal*YAxisZoomFactor;
                    StylusPoint point = new StylusPoint
                        (centerX + i,
                        centerY - tanValue - D);
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
            double XAxisZoomFactor, double YAxisZoomFactor,
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
                    double X = i / (internalXAxisEnlargingFactorterY*XAxisZoomFactor);
                    double cosecValue = amplitudeEnlargingFactorInternal * YAxisZoomFactor *
                    amplitudeEnlargingFactorExternal / Math.Sin(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;
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
            double XAxisZoomFactor, double YAxisZoomFactor,
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
                    double X = i / (internalXAxisEnlargingFactorterY*XAxisZoomFactor);
                    double tanValue = amplitudeEnlargingFactorInternal * YAxisZoomFactor *
                    amplitudeEnlargingFactorExternal / Math.Cos(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal *YAxisZoomFactor;
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
            double XAxisZoomFactor, double YAxisZoomFactor,
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
                    double X = i / (internalXAxisEnlargingFactorterY*XAxisZoomFactor);
                    double tanValue = amplitudeEnlargingFactorInternal * YAxisZoomFactor *
                    amplitudeEnlargingFactorExternal / Math.Tan(omega * X - Phi);
                    double D = Convert.ToDouble(verticalShift) * amplitudeEnlargingFactorInternal * YAxisZoomFactor;
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
        #endregion
    }
}
