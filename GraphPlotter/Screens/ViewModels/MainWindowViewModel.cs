using GraphPlotter.Common;
using GraphPlotter.ServicInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace GraphPlotter.Screens.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Constructor
        public MainWindowViewModel(IPlotGridLinesService plotGridLinesService,
            IPlotTrignometricFunctionsService plotTrignometricFunctionsService)
        {
            _plotGridLinesService = plotGridLinesService;
            _plotTrignometricFunctionsService = plotTrignometricFunctionsService;
            HorizontalZoomInCommand = new RelayCommand(ExecuteHorizontalZoomIn, CanExecuteHorizontalZoomIn);
            HorizontalZoomOutCommand = new RelayCommand(ExecuteHorizontalZoomOut, CanExecuteHorizontalZoomOut);
            VerticalZoomInCommand = new RelayCommand(ExecuteVerticalZoomIn, CanExecuteVerticalZoomIn);
            VerticalZoomOutCommand = new RelayCommand(ExecuteVerticalZoomOut, CanExecuteVerticalZoomOut);
            InitializeGraph();
        }

        #endregion

        #region Constant Variables
        /// <summary>
        /// The X axis needs to be scaled/zoomed in so the graph is visible more clearly.
        /// So the X axis has been enlarged by a factor of 20.
        /// </summary>
        private const int InternalXAxisScalingFactor = 20;

        /// <summary>
        /// The amplitude of the waves ploted also needs to be increased so that the graph is 
        /// clearly visible. So the plotted wave is multiplied by this factor internally.
        /// </summary>
        private const int AmplitudeEnlargingFactorInternal = 60;

        /// <summary>
        /// On Load the Amplitude value for a wave
        /// </summary>
        private const int OnLoadAmplitudeEnlargingFactorExternal = 1;

        /// <summary>
        /// The value of time period(seconds) when the tool loads.
        /// </summary>
        private const int OnLoadTimePeriodValue = 2;

        /// <summary>
        /// The value of phase shift(radians) when the tool loads.
        /// </summary>
        private const int OnLoadPhaseShift = 0;

        /// <summary>
        /// The value of vertical shift when the tool loads.
        /// </summary>
        private const int OnLoadVerticalShift = 0;

        /// <summary>
        /// The graph is width is a multiple of Pi.
        /// The graph width is calculated by multiplying pi by this variable.
        /// </summary>
        private const double GraphWidthMultiple = 350;

        /// <summary>
        /// The height of the graph.
        /// </summary>
        private const double GraphHeightConstant = 600;

        /// <summary>
        /// The number 2 to get the center point of the graph by dividing the width and height by it.
        /// </summary>
        private const int NumberTwo = 2;

        /// <summary>
        /// The sine string
        /// </summary>
        private const string Sin = "sin";

        /// <summary>
        /// The cos string
        /// </summary>
        private const string Cos = "cos";

        /// <summary>
        /// The tan string
        /// </summary>
        private const string Tan = "tan";

        /// <summary>
        /// The cosec string
        /// </summary>
        private const string Cosec = "cosec";

        /// <summary>
        /// The sec string
        /// </summary>
        private const string Sec = "sec";

        /// <summary>
        /// The cot string
        /// </summary>
        private const string Cot = "cot";

        /// <summary>
        /// The SinC string
        /// </summary>
        private const string SinC = "SinC";
        #endregion

        #region private variables

        /// <summary>
        /// The X=0 coordinate
        /// </summary>
        private double _centerX;

        /// <summary>
        /// The Y=0 coordinate
        /// </summary>
        private double _centerY;

        /// <summary>
        /// The field for accessing the PlotGridLineService. 
        /// It is received from the unity container in the constructor.
        /// </summary>
        private readonly IPlotGridLinesService _plotGridLinesService;

        /// <summary>
        /// The field for accessing the PlotTrigonometricService. 
        /// It is received from the unity container in the constructor.
        /// </summary>
        private readonly IPlotTrignometricFunctionsService _plotTrignometricFunctionsService;

        #endregion

        #region Properties
        /// <summary>
        /// private field for the Property AmplitudeEnlargingFactorExternal
        /// </summary>
        private string amplitudeEnlargingFactorExternal = Properties.Settings.Default.Amplitude;
        /// <summary>
        /// This signifies the value of A i.e. amplitude in the formula A*sin( (2*pi/T) x - phi)+D
        /// </summary>
        public string AmplitudeEnlargingFactorExternal
        {
            get
            {
                return amplitudeEnlargingFactorExternal;
            }
            set
            {
                amplitudeEnlargingFactorExternal = value;
                PlotTrignometricFunctions(SelectedTrigoFunction);
                Properties.Settings.Default.Amplitude = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// private field for the Property TimePeriod
        /// </summary>
        private string timePeriod = Properties.Settings.Default.TimePeriod.ToString();
        /// <summary>
        /// This signifies the value of T i.e. time period(seconds) in the formula A*sin( (2*pi/T) x - phi)+D
        /// </summary>
        public string TimePeriod
        {
            get
            {
                return timePeriod;
            }
            set
            {
                timePeriod = value;
                PlotTrignometricFunctions(SelectedTrigoFunction);
                Properties.Settings.Default.TimePeriod = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// private field for the Property PhaseShift
        /// </summary>
        private string phaseShift = Properties.Settings.Default.PhaseShift.ToString();
        /// <summary>
        /// This signifies the value of phi i.e. the phase shift in the formula A*sin( (2*pi/T) x - phi)+D
        /// </summary>
        public string PhaseShift
        {
            get
            {
                return phaseShift;
            }
            set
            {
                phaseShift = value;
                PlotTrignometricFunctions(SelectedTrigoFunction);
                Properties.Settings.Default.PhaseShift = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// private field for the Property VerticalShift
        /// </summary>
        private string verticalShift = Properties.Settings.Default.VerticalShift.ToString();
        /// <summary>
        /// This signifies the value of D i.e. vertical shift in the formula A*sin( (2*pi/T) x - phi)+D
        /// </summary>
        public string VerticalShift
        {
            get { return verticalShift; }
            set
            {
                verticalShift = value;
                PlotTrignometricFunctions(SelectedTrigoFunction);
                Properties.Settings.Default.VerticalShift = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The private field for YAxisGridLines
        /// </summary>
        private ObservableCollection<GridLine> yAxisGridLines
            = new ObservableCollection<GridLine>();
        /// <summary>
        /// The collection of Y-Axis grid lines which get plotted on InkCanvas
        /// </summary>
        public ObservableCollection<GridLine> YAxisGridLines
        {
            get
            {
                return yAxisGridLines;
            }
            set
            {
                yAxisGridLines = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The private field for XAxisGridLines
        /// </summary>
        private ObservableCollection<GridLine> xAxisGridLines
            = new ObservableCollection<GridLine>();
        /// <summary>
        /// The collection of X-Axis grid lines which get plotted on InkCanvas
        /// </summary>
        public ObservableCollection<GridLine> XAxisGridLines
        {
            get
            {
                return xAxisGridLines;
            }
            set
            {
                xAxisGridLines = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The private variable for XAxisNumbers
        /// </summary>
        private ObservableCollection<Number> xAxisNumbers = new ObservableCollection<Number>();
        /// <summary>
        /// The list of numbers on the X-Axis
        /// </summary>
        public ObservableCollection<Number> XAxisNumbers
        {
            get { return xAxisNumbers; }
            set
            {
                xAxisNumbers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The private variable for YAxisNumbers
        /// </summary>
        private ObservableCollection<Number> yAxisNumbers = new ObservableCollection<Number>();
        /// <summary>
        /// The list of numbers on the Y-Axis
        /// </summary>
        public ObservableCollection<Number> YAxisNumbers
        {
            get { return yAxisNumbers; }
            set
            {
                yAxisNumbers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The width of the graph i.e. is the InkCanvas.
        /// It is binded to the Widht of InkCanvas in the View.
        /// </summary>
        public double GraphWidth { get; set; }

        /// <summary>
        /// The height of the graph i.e is the InkCanvas.
        /// It is binded to the Height of InkCanvas in the view.
        /// </summary>
        public double GraphHeight { get; set; }

        /// <summary>
        /// The trignometric function are drawn as strokes on the InkCanvas.
        /// This properties is the collection of all those strokes.
        /// </summary>
        public StrokeCollection Strokes { get; set; } = new StrokeCollection();

        /// <summary>
        /// The list of trigonometric functions which is binded to the function dropdown.
        /// </summary>
        public ObservableCollection<string> TrignometricFunctions { get; set; }

        /// <summary>
        /// The private field for SelectedTrigoFunction property.
        /// </summary>
        private string selectedTrigoFunction;
        /// <summary>
        /// The currently selected trignometric function from the drop down.
        /// </summary>
        public string SelectedTrigoFunction
        {
            get
            { return selectedTrigoFunction; }
            set
            {
                selectedTrigoFunction = value;
                Properties.Settings.Default.FunctionType = value;
                PlotTrignometricFunctions(value);
            }
        }

        /// <summary>
        /// The zoom factor along the y axis
        /// </summary>
        public double YAxisZoomFactor { get; set; } = 1;

        /// <summary>
        /// The zoom factor along the x axis.
        /// </summary>
        public double XAxisZoomFactor { get; set; } = 1;

        /// <summary>
        /// The command which is called when + for horizontal zoom is pressed.
        /// </summary>
        public RelayCommand HorizontalZoomInCommand { get;set; }
        public RelayCommand HorizontalZoomOutCommand { get; set; }
        public RelayCommand VerticalZoomInCommand { get; set; }
        public RelayCommand VerticalZoomOutCommand { get; set; }



        #endregion

        #region Methods
        /// <summary>
        /// This method initialises the graph. It is called in constructor.
        /// In this function the height and width of the graph is set, grid lines are plotted and
        /// trigonometic function dropdown is populated.
        /// </summary>
        private void InitializeGraph()
        {
            try
            {
                GraphWidth = Math.PI * GraphWidthMultiple;
                GraphHeight = GraphHeightConstant;
                _centerX = GraphWidth / NumberTwo;
                _centerY = GraphHeight / NumberTwo;
                TrignometricFunctions = new ObservableCollection<string>()
                { Sin, Cos, Tan, Cosec, Sec, Cot, SinC };

                //plot Y Axis Grid Lines
                _plotGridLinesService.AddYaxisGridLines(GraphWidth, GraphHeight,
                _centerY, AmplitudeEnlargingFactorInternal,
                InternalXAxisScalingFactor,
                YAxisGridLines);

                //plot X Axis Grid lines
                _plotGridLinesService.AddXaxisGridLines(GraphWidth, GraphHeight,
                _centerX,
                InternalXAxisScalingFactor,
                XAxisGridLines);

                SelectedTrigoFunction = Properties.Settings.Default.FunctionType;

                AddXAxisNumber();
                AddYAxisNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void AddXAxisNumber()
        {
            XAxisNumbers = new ObservableCollection<Number>();
            int numberOfLines = Convert.ToInt32(GraphWidth / (Math.PI * InternalXAxisScalingFactor) - 1);
            double start = -GraphWidth / (2*Math.PI * InternalXAxisScalingFactor*XAxisZoomFactor);
            double end = -start;
            //+ve x axis numbers
            for (int i = 0; i < numberOfLines/2+1; i++)
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
            OnPropertyChanged(nameof(XAxisNumbers));
        }

        public void AddYAxisNumber()
        {
            YAxisNumbers = new ObservableCollection<Number>();
            double start = -GraphHeight / (2*AmplitudeEnlargingFactorInternal * YAxisZoomFactor);
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
                    number.Y = _centerY - (i  * AmplitudeEnlargingFactorInternal * YAxisZoomFactor);
                    number.Value = i.ToString();
                }
                YAxisNumbers.Add(number);
            }
            OnPropertyChanged(nameof(XAxisNumbers));
        }
        public bool CanExecutePlotCommand(object parameter)
        {
            return true;
        }
        private void SaveSvgToFile(string svgPathData, string filePath)
        {
            string svgContent = $@"
                                <svg xmlns='http://www.w3.org/2000/svg' version='1.1' width='800' height='800'>
                                    <path d='{svgPathData}' stroke='Red' stroke-width='2' fill='none'/>
                                </svg>";

            File.WriteAllText(filePath, svgContent);
        }
        public void ExecutePlotCommand(object parameter)
        {

        }

        /// <summary>
        /// The method is called when a trigonometric function is selected from the dropdown.
        /// It is called form the setter of SelectedTrignometricFunction property.
        /// </summary>
        /// <param name="function">The selected trigonometric function</param>
        public void PlotTrignometricFunctions(string function)
        {
            try
            {
                Strokes = new StrokeCollection();
                StylusPointCollection stylusPointsCollection = new StylusPointCollection();

                if (!string.IsNullOrEmpty(function) &&
                    Convert.ToDouble(AmplitudeEnlargingFactorExternal) != null &&
                    !string.IsNullOrEmpty(TimePeriod) && Convert.ToDouble(TimePeriod) != 0 &&
                    !string.IsNullOrEmpty(PhaseShift) &&
                    !string.IsNullOrEmpty(VerticalShift))
                {
                    //if sine is selected call the PlotSine service 
                    if (function.ToString().ToLower().Equals(Sin))
                    {
                        _plotTrignometricFunctionsService.PlotSine(GraphWidth, _centerX, _centerY,XAxisZoomFactor,YAxisZoomFactor, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);

                    }
                    else if (function.ToString().ToLower().Equals(Cos))
                    {
                        _plotTrignometricFunctionsService.PlotCos(GraphWidth, _centerX, _centerY, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Tan))
                    {
                        _plotTrignometricFunctionsService.PlotTan(GraphWidth, _centerX, _centerY, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Cosec))
                    {
                        _plotTrignometricFunctionsService.PlotCosec(GraphWidth, _centerX, _centerY, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Sec))
                    {
                        _plotTrignometricFunctionsService.PlotSec(GraphWidth, _centerX, _centerY, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Cot))
                    {
                        _plotTrignometricFunctionsService.PlotCot(GraphWidth, _centerX, _centerY, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(SinC.ToLower()))
                    {
                        _plotTrignometricFunctionsService.PlotSinC(GraphWidth, _centerX, _centerY, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                }
                OnPropertyChanged(nameof(Strokes));
            }
            catch (Exception ex)
            {
                Strokes = new StrokeCollection();
                OnPropertyChanged(nameof(Strokes));
                MessageBox.Show("Failed to draw the graph. " + ex.Message);
            }
        }

        private bool CanExecuteHorizontalZoomIn(object obj)
        {
            return true;
        }

        private void ExecuteHorizontalZoomIn(object obj)
        {
            XAxisZoomFactor = XAxisZoomFactor * 2;
            AddXAxisNumber();
            PlotTrignometricFunctions(SelectedTrigoFunction);
        }


        private bool CanExecuteHorizontalZoomOut(object obj)
        {
            return true;
        }

        private void ExecuteHorizontalZoomOut(object obj)
        {
            XAxisZoomFactor = XAxisZoomFactor / 2;
            AddXAxisNumber();
            PlotTrignometricFunctions(SelectedTrigoFunction);
        }

        private bool CanExecuteVerticalZoomIn(object obj)
        {
            return true;
        }

        private void ExecuteVerticalZoomIn(object obj)
        {
            YAxisZoomFactor = YAxisZoomFactor * 2;
            AddYAxisNumber();
            PlotTrignometricFunctions(SelectedTrigoFunction);
        }


        private bool CanExecuteVerticalZoomOut(object obj)
        {
            return true;
        }

        private void ExecuteVerticalZoomOut(object obj)
        {
            YAxisZoomFactor = YAxisZoomFactor / 2;
            AddYAxisNumber();
            PlotTrignometricFunctions(SelectedTrigoFunction);
        }
        #endregion

    }
}
