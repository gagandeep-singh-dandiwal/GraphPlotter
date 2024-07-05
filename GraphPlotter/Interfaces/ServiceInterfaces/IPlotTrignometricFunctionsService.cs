using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace GraphPlotter.ServicInterfaces.Interfaces
{
    public interface IPlotTrignometricFunctionsService
    {
        StrokeCollection PlotSine(double GraphWidth, double graphHeight,
            double CenterX, double CenterY, double XOffSet, double YOffSet, double XAxisZoomFactor, double YAxisZoomFactor,
            double InternalXAxisEnlargingFactor, double AmplitudeEnlargingFactorInternal,
            double AmplitudeEnlargingFactorExternal, string TimePeriod, string PhaseShift, 
            string VerticalShift, StrokeCollection Strokes);

        StrokeCollection PlotCos(double GraphWidth,
            double CenterX, double CenterY,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double InternalXAxisEnlargingFactor, double AmplitudeEnlargingFactorInternal,
            double AmplitudeEnlargingFactorExternal, string TimePeriod, string PhaseShift, 
            string VerticalShift, StrokeCollection Strokes);

        StrokeCollection PlotTan(double GraphWidth,
            double CenterX, double CenterY,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double InternalXAxisEnlargingFactor, double AmplitudeEnlargingFactorInternal,
            double AmplitudeEnlargingFactorExternal, string TimePeriod, string PhaseShift, 
            string VerticalShift,StrokeCollection Strokes);

        StrokeCollection PlotCosec(double GraphWidth,
            double CenterX, double CenterY,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double InternalXAxisEnlargingFactor, double AmplitudeEnlargingFactorInternal,
            double AmplitudeEnlargingFactorExternal, string TimePeriod, string PhaseShift, 
            string VerticalShift, StrokeCollection Strokes);

        StrokeCollection PlotSec(double GraphWidth,
            double CenterX, double CenterY,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double InternalXAxisEnlargingFactor, double AmplitudeEnlargingFactorInternal,
            double AmplitudeEnlargingFactorExternal, string TimePeriod, string PhaseShift, 
            string VerticalShift, StrokeCollection Strokes);

        StrokeCollection PlotCot(double GraphWidth,
            double CenterX, double CenterY,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double InternalXAxisEnlargingFactor, double AmplitudeEnlargingFactorInternal,
            double AmplitudeEnlargingFactorExternal, string TimePeriod, string PhaseShift, 
            string VerticalShift, StrokeCollection Strokes);

        StrokeCollection PlotSinC(double GraphWidth,
            double CenterX, double CenterY,
            double XAxisZoomFactor, double YAxisZoomFactor,
            double InternalXAxisEnlargingFactor, double AmplitudeEnlargingFactorInternal,
            double AmplitudeEnlargingFactorExternal, string TimePeriod, string PhaseShift, 
            string VerticalShift, StrokeCollection Strokes);
    }
}
