using GraphPlotter.Common;
using GraphPlotter.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace GraphPlotter.Services
{
    public class ConvertStrokeCollectionToSVGService : IConvertStrokeCollectionToSVGService
    {
        public string ConvertStrokeToSvgPath(StrokeCollection strokeCollection)
        {
            string paths = string.Empty;
            var singlePathData = string.Empty;

            foreach (var stroke in strokeCollection)
            {
                singlePathData = string.Empty;
                // Get the stylus points from the stroke
                var points = stroke.StylusPoints;

                if (points.Count > 0)
                {
                    // Move to the first point
                    singlePathData += $"M {points[0].X} {points[0].Y} ";

                    // Draw lines to subsequent points
                    for (int i = 1; i < points.Count; i++)
                    {
                        singlePathData += $"L {points[i].X} {points[i].Y} ";
                    }
                }
                singlePathData = singlePathData.Replace(",", ".");
                string path = $@"<path d='{singlePathData}' stroke='Red' stroke-width='3' fill='none'/>";
                paths += path;
            }
            return paths.Trim();
        }
        public void SaveSvgToFile(string filePath,StrokeCollection Strokes,
            ObservableCollection<GridLine> XAxisGridLines, ObservableCollection<GridLine> YAxisGridLines,
            ObservableCollection<Number> XAxisNumbers, ObservableCollection<Number> YAxisNumbers,
            double GraphWidth,double GraphHeight)
        {
            string sGraphWidth = GraphWidth.ToString().Replace(",", ".");
            string sGraphHeight = GraphHeight.ToString().Replace(",", ".");
            string graphData = ConvertStrokeToSvgPath(Strokes);
            string gridLineData = ConvertGridLinesToSVG(XAxisGridLines, YAxisGridLines);
            string numbers = ConvertCoordinatesToSVG(XAxisNumbers, YAxisNumbers);
            string svgContent = $@"
                                <svg xmlns='http://www.w3.org/2000/svg' version='1.1' width='{sGraphWidth}' height='{sGraphHeight}'>
                                    <rect width='100%' height='100%' fill='lightgray'/>
                                    {gridLineData}
                                    {numbers}
                                    {graphData}
                                </svg>";

            System.IO.File.WriteAllText(filePath, svgContent);
        }
        public string ConvertCoordinatesToSVG(ObservableCollection<Number> XAxisNumbers, 
            ObservableCollection<Number> YAxisNumbers)
        {
            string textString = string.Empty;
            foreach (var xCoordinate in XAxisNumbers)
            {
                textString += $@"<text x='{xCoordinate.X}' y='{xCoordinate.Y}' fill='black' font-size='15'>{xCoordinate.Value}</text>"; ;
            }
            foreach (var yCoordinate in YAxisNumbers)
            {
                textString += $@"<text x='{yCoordinate.X}' y='{yCoordinate.Y}' fill='black' font-size='15'>{yCoordinate.Value}</text>";
            }
            textString = textString.Replace(",", ".");
            return textString;
        }
        public string ConvertGridLinesToSVG(ObservableCollection<GridLine> xAxisGridLineCollection,
            ObservableCollection<GridLine> yAxisGridLineCollection)
        {
            var singlePathData = string.Empty;
            string paths = string.Empty;

            foreach (GridLine xAxisGridLine in xAxisGridLineCollection)
            {
                singlePathData = string.Empty;
                // Move to the first point
                singlePathData += $"M {xAxisGridLine.StartXPoint} {xAxisGridLine.StartYPoint} ";
                singlePathData += $"L {xAxisGridLine.EndXPoint} {xAxisGridLine.EndYPoint} ";
                singlePathData = singlePathData.Replace(",", ".");
                string path = $@"<path d='{singlePathData}' stroke='Gray' stroke-width='{xAxisGridLine.Thickness}' fill='none'/>";
                paths += path;
            }

            foreach (GridLine yAxisGridLine in yAxisGridLineCollection)
            {
                singlePathData = string.Empty;
                // Move to the first point
                singlePathData += $"M {yAxisGridLine.StartXPoint} {yAxisGridLine.StartYPoint} ";
                singlePathData += $"L {yAxisGridLine.EndXPoint} {yAxisGridLine.EndYPoint} ";
                singlePathData = singlePathData.Replace(",", ".");
                string path = $@"<path d='{singlePathData}' stroke='Gray' stroke-width='{yAxisGridLine.Thickness}' fill='none'/>";
                paths += path;
            }

            return paths.Trim();
        }
    }
}
