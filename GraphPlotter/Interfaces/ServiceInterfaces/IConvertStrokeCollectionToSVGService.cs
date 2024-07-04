using GraphPlotter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace GraphPlotter.Interfaces.ServiceInterfaces
{
    public interface IConvertStrokeCollectionToSVGService
    {
        string ConvertStrokeToSvgPath(StrokeCollection strokeCollection);
        void SaveSvgToFile(string filePath, StrokeCollection Strokes,
            ObservableCollection<GridLine> XAxisGridLines, ObservableCollection<GridLine> YAxisGridLines,
            ObservableCollection<Number> XAxisNumbers, ObservableCollection<Number> YAxisNumbers,
            double GraphWidth, double GraphHeight);
        string ConvertGridLinesToSVG(ObservableCollection<GridLine> xAxisGridLineCollection,
            ObservableCollection<GridLine> yAxisGridLineCollection);
    }
}
