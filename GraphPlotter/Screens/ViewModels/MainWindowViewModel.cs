using GraphPlotter.Common;
using GraphPlotter.Interfaces.ServiceInterfaces;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace GraphPlotter.Screens.ViewModels
{
    /// <summary>
    /// This class is the view model for the MainWindow.
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        #region Constructor
        public MainWindowViewModel(IPlotGridLinesService plotGridLinesService,
            IPlotTrignometricFunctionsService plotTrignometricFunctionsService,
            IPlotGridLineNumbersService plotGridLineNumbersService,
            IConvertStrokeCollectionToSVGService convertStrokeCollectionToSVGService)
        {
            _plotGridLinesService = plotGridLinesService;
            _plotTrignometricFunctionsService = plotTrignometricFunctionsService;
            _plotGridLineNumbersService = plotGridLineNumbersService;
            _convertStrokeCollectionToSVGService = convertStrokeCollectionToSVGService;
            HorizontalZoomInCommand = new RelayCommand(ExecuteHorizontalZoomIn, CanExecuteHorizontalZoomIn);
            HorizontalZoomOutCommand = new RelayCommand(ExecuteHorizontalZoomOut, CanExecuteHorizontalZoomOut);
            VerticalZoomInCommand = new RelayCommand(ExecuteVerticalZoomIn, CanExecuteVerticalZoomIn);
            VerticalZoomOutCommand = new RelayCommand(ExecuteVerticalZoomOut, CanExecuteVerticalZoomOut);
            SaveToSVGFormatCommand = new RelayCommand(ExecuteSaveToSVGFormat, CanExecuteSaveToSVGFormat);
            ResetZoomCommand = new RelayCommand(ExecuteResetZoom, CanExecuteResetZoom);
            ResetGraphPostionCommand = new RelayCommand(ExecuteResetGraphPosition, CanExecuteResetGraphPosition);
            RestoreDefaultCommand = new RelayCommand(ExecuteRestoreDefaultCommand, CanExecuteRestoreDefaultCommand);
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

        /// <summary>
        /// The number zero, to signify if something is zero
        /// </summary>
        private const int Zero = 0;

        /// <summary>
        /// The number one , 
        /// </summary>
        private const int DefaultZoomingFactor = 1;

        /// <summary>
        /// Zoom in factor when zoom in is clicked
        /// </summary>
        private const int ZoomInMultiple = 2;

        #endregion

        #region private variables

        /// <summary>
        /// The X=0 coordinate
        /// </summary>
        private double _defaultCenterXWPF;

        /// <summary>
        /// The Y=0 coordinate
        /// </summary>
        private double _defaultCenterYWPF;

        /// <summary>
        /// The actual x center of the graph according to wpf
        /// </summary>
        private double _actualCenterXWPF;

        /// <summary>
        /// The actual y center of the graph according to wpf
        /// </summary>
        private double _actualCenterYWPF;

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

        /// <summary>
        /// The field is for accessing the PlotGridLineNumbersService.
        /// It is received from the unity container in the constructor.
        /// </summary>
        private readonly IPlotGridLineNumbersService _plotGridLineNumbersService;

        /// <summary>
        /// The field is for accessing the ConvertStrokeCollectionToSVGService.
        /// It is received from the unity container in the constructor.
        /// </summary>
        private readonly IConvertStrokeCollectionToSVGService _convertStrokeCollectionToSVGService;

        
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
                if (value.ToLower().Equals(Sin.ToLower()) || value.ToLower().Equals(Cos.ToLower())
                    || value.ToLower().Equals(SinC.ToLower()))
                {
                    
                    IsOffsetTextboxEnabled = true;
                }
                else
                {
                    XOffset = 0.ToString();
                    YOffset = 0.ToString();
                    IsOffsetTextboxEnabled = false;
                }
                PlotTrignometricFunctions(value);
                OnPropertyChanged();
            }
        }

        private double yAxisZoomFactor = Properties.Settings.Default.VerticalZoom;
        /// <summary>
        /// The zoom factor along the y axis
        /// </summary>
        public double YAxisZoomFactor
        {
            get
            {
                return yAxisZoomFactor;
            }
            set
            {
                yAxisZoomFactor = value;
                Properties.Settings.Default.VerticalZoom = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The private field for the XAxisZoomFactor
        /// </summary>
        private double xAxisZoomFactor = Properties.Settings.Default.HorizontalZoom;

        /// <summary>
        /// The zoom factor along the x axis.
        /// </summary>
        public double XAxisZoomFactor
        {
            get
            {
                return xAxisZoomFactor;
            }
            set
            {
                xAxisZoomFactor = value;
                Properties.Settings.Default.HorizontalZoom = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The command which is called when Reset Zoom button is pressed.
        /// </summary>
        public RelayCommand ResetZoomCommand { get; set; }

        /// <summary>
        /// The command which is called when + for horizontal zoom in is pressed.
        /// </summary>
        public RelayCommand HorizontalZoomInCommand { get;set; }

        /// <summary>
        /// The command which is called when - for horizontal zoom out is pressed.
        /// </summary>
        public RelayCommand HorizontalZoomOutCommand { get; set; }

        /// <summary>
        /// The command which is called when + for vertical zoom in is pressed.
        /// </summary>
        public RelayCommand VerticalZoomInCommand { get; set; }

        /// <summary>
        /// The command which is called when - for vertical zoom out is pressed.
        /// </summary>
        public RelayCommand VerticalZoomOutCommand { get; set; }

        /// <summary>
        /// The command which is called when Save to SVG format is pressed.
        /// </summary>
        public RelayCommand SaveToSVGFormatCommand { get; set; }

        /// <summary>
        /// The command which is called when Reset Graph Position is pressed.
        /// </summary>
        public RelayCommand ResetGraphPostionCommand { get; set; }

        /// <summary>
        /// The command which is called when Restore Default is pressed.
        /// </summary>
        public RelayCommand RestoreDefaultCommand { get; set; }
        /// <summary>
        /// private field for the Property XOffset
        /// </summary>
        private string _xOffset = Properties.Settings.Default.CenterX.ToString();
        /// <summary>
        /// The offset of the graph along the x axis.
        /// This is also the point where the user wants to zoom in.
        /// </summary>
        public string XOffset
        {
            get
            {
                return _xOffset;
            }
            set
            {
                _xOffset = value;
                Properties.Settings.Default.CenterX = Convert.ToDouble(value);
                if (IsOffsetTextboxEnabled)
                {
                    _actualCenterXWPF = _defaultCenterXWPF - Convert.ToDouble(value) * Math.PI * InternalXAxisScalingFactor * XAxisZoomFactor;
                    PlotTrignometricFunctions(SelectedTrigoFunction);
                    //Plot the numbers on the X Axis
                    _plotGridLineNumbersService.AddXAxisNumber(_actualCenterXWPF, _actualCenterYWPF,
                        Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor,YAxisZoomFactor,
                    InternalXAxisScalingFactor, ref xAxisNumbers);
                    OnPropertyChanged(nameof(XAxisNumbers));
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// private field for the Property YOffset
        /// </summary>
        private string _yOffset = Properties.Settings.Default.CenterY.ToString();

        /// <summary>
        /// the offset of the graph along the y axis.
        /// This is also the point where the user wants to zoom in.
        /// </summary>
        public string YOffset
        {
            get { return _yOffset; }
            set 
            {
                if (IsOffsetTextboxEnabled)
                {
                    _yOffset = value;
                    Properties.Settings.Default.CenterY = Convert.ToDouble(value);
                    _actualCenterYWPF = _defaultCenterYWPF + Convert.ToDouble(value) * AmplitudeEnlargingFactorInternal * YAxisZoomFactor;

                    _plotGridLineNumbersService.AddXAxisNumber(_actualCenterXWPF, _actualCenterYWPF,
                        Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    InternalXAxisScalingFactor, ref xAxisNumbers);

                    //Plot the numbers on the Y Axis
                    _plotGridLineNumbersService.AddYAxisNumber(_actualCenterXWPF, _actualCenterYWPF,
                    Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    AmplitudeEnlargingFactorInternal, InternalXAxisScalingFactor, ref yAxisNumbers);

                    

                    OnPropertyChanged(nameof(YAxisNumbers));
                    OnPropertyChanged(nameof(XAxisNumbers));
                    PlotTrignometricFunctions(SelectedTrigoFunction);
                    OnPropertyChanged(nameof(Strokes));
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The private property for IsOffsetTextboxEnabled
        /// </summary>
        private bool isOffsetTextboxEnabled = true;
        /// <summary>
        /// This property tells if the offset textboxes are enabled or not.
        /// </summary>
        public bool IsOffsetTextboxEnabled
        {
            get { return isOffsetTextboxEnabled; }
            set 
            {
                isOffsetTextboxEnabled = value; 
                OnPropertyChanged(); 
            }
        }

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
                _defaultCenterXWPF = GraphWidth / NumberTwo;
                _defaultCenterYWPF = GraphHeight / NumberTwo;
                _actualCenterXWPF = Convert.ToDouble(XOffset) + _defaultCenterXWPF;
                _actualCenterYWPF = Convert.ToDouble(YOffset) + _defaultCenterYWPF;
                TrignometricFunctions = new ObservableCollection<string>()
                { Sin, Cos, SinC, Tan, Cosec, Sec, Cot  };

                //plot Y Axis Grid Lines
                _plotGridLinesService.AddYaxisGridLines(GraphWidth, GraphHeight,
                _defaultCenterYWPF, AmplitudeEnlargingFactorInternal,
                InternalXAxisScalingFactor,
                YAxisGridLines);

                //plot X Axis Grid lines
                _plotGridLinesService.AddXaxisGridLines(GraphWidth, GraphHeight,
                _defaultCenterXWPF,
                InternalXAxisScalingFactor,
                XAxisGridLines);

                SelectedTrigoFunction = Properties.Settings.Default.FunctionType;

                //Plot the numbers on the X Axis
                _plotGridLineNumbersService.AddXAxisNumber(_defaultCenterXWPF,_defaultCenterYWPF,
                    Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                GraphWidth, GraphHeight,  XAxisZoomFactor, YAxisZoomFactor,
                InternalXAxisScalingFactor,ref xAxisNumbers);
                OnPropertyChanged(nameof(XAxisNumbers));

                //Plot the numbers on the Y Axis
                _plotGridLineNumbersService.AddYAxisNumber(_actualCenterXWPF,_actualCenterYWPF,
                    Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                AmplitudeEnlargingFactorInternal, InternalXAxisScalingFactor,ref yAxisNumbers);
                OnPropertyChanged(nameof(YAxisNumbers));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    !string.IsNullOrEmpty(AmplitudeEnlargingFactorExternal) && !AmplitudeEnlargingFactorExternal.Equals("-") &&
                    !string.IsNullOrEmpty(TimePeriod) && !TimePeriod.Equals("-") && Convert.ToDouble(TimePeriod) != Zero &&
                    !string.IsNullOrEmpty(PhaseShift) && !PhaseShift.Equals("-") &&
                    !string.IsNullOrEmpty(VerticalShift) && !VerticalShift.Equals("-") &&
                    !XOffset.Equals("-") && !YOffset.Equals("-"))
                {
                    //if sine is selected call the PlotSine service 
                    if (function.ToString().ToLower().Equals(Sin))
                    {
                        
                        _plotTrignometricFunctionsService.PlotSine(GraphWidth,GraphHeight, _actualCenterXWPF, _actualCenterYWPF,
                            Convert.ToDouble(XOffset),Convert.ToDouble(YOffset),
                            XAxisZoomFactor,YAxisZoomFactor, 
                            InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal,Convert.ToDouble(AmplitudeEnlargingFactorExternal),
                            TimePeriod, PhaseShift, VerticalShift, Strokes);

                    }
                    else if (function.ToString().ToLower().Equals(Cos))
                    {
                        _plotTrignometricFunctionsService.PlotCos(GraphWidth,GraphHeight,_actualCenterXWPF, _actualCenterYWPF,
                            Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                            XAxisZoomFactor, YAxisZoomFactor, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Tan))
                    {
                        _plotTrignometricFunctionsService.PlotTan(GraphWidth, _defaultCenterXWPF, _defaultCenterYWPF,
                            XAxisZoomFactor, YAxisZoomFactor, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Cosec))
                    {
                        _plotTrignometricFunctionsService.PlotCosec(GraphWidth, _defaultCenterXWPF, _defaultCenterYWPF,
                            XAxisZoomFactor, YAxisZoomFactor, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Sec))
                    {
                        _plotTrignometricFunctionsService.PlotSec(GraphWidth, _defaultCenterXWPF, _defaultCenterYWPF,
                            XAxisZoomFactor, YAxisZoomFactor, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(Cot))
                    {
                        _plotTrignometricFunctionsService.PlotCot(GraphWidth, _defaultCenterXWPF, _defaultCenterYWPF,
                            XAxisZoomFactor, YAxisZoomFactor, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                    else if (function.ToString().ToLower().Equals(SinC.ToLower()))
                    {
                        _plotTrignometricFunctionsService.PlotSinC(GraphWidth,GraphHeight,
                            _actualCenterXWPF, _actualCenterYWPF,
                            Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                            XAxisZoomFactor, YAxisZoomFactor, InternalXAxisScalingFactor,
                            AmplitudeEnlargingFactorInternal, Convert.ToDouble(AmplitudeEnlargingFactorExternal), TimePeriod, PhaseShift, VerticalShift, Strokes);
                    }
                }
                OnPropertyChanged(nameof(Strokes));
            }
            catch (Exception ex)
            {
                Strokes = new StrokeCollection();
                OnPropertyChanged(nameof(Strokes));
                MessageBox.Show("Failed to draw the graph.\n" + ex.Message);
            }
        }

        /// <summary>
        /// This method tells if the horizontal zoom in can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteHorizontalZoomIn(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Horizontal Zoom In button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteHorizontalZoomIn(object obj)
        {
            try
            {
                XAxisZoomFactor = XAxisZoomFactor * ZoomInMultiple;
                _plotGridLineNumbersService.AddXAxisNumber(_actualCenterXWPF, _actualCenterYWPF,
                        Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    InternalXAxisScalingFactor, ref xAxisNumbers);
                OnPropertyChanged(nameof(XAxisNumbers));
                PlotTrignometricFunctions(SelectedTrigoFunction);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to zoom in horizontally.\n" +
                    exception.Message);
            }
            
        }

        /// <summary>
        /// This method tells if the horizontal zoom out can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteHorizontalZoomOut(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Horizontal Zoom Out button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteHorizontalZoomOut(object obj)
        {
            try
            {
                XAxisZoomFactor = XAxisZoomFactor / ZoomInMultiple;
                _plotGridLineNumbersService.AddXAxisNumber(_actualCenterXWPF, _actualCenterYWPF,
                        Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    InternalXAxisScalingFactor, ref xAxisNumbers);
                OnPropertyChanged(nameof(XAxisNumbers));
                PlotTrignometricFunctions(SelectedTrigoFunction);
            }
            
            catch (Exception exception)
            {
                MessageBox.Show("Failed to zoom out horizontally.\n" +
                    exception.Message);
            }
        }

        /// <summary>
        /// This method tells if the vertical zoom in can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteVerticalZoomIn(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Vertical Zoom In button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteVerticalZoomIn(object obj)
        {
            try
            {
                YAxisZoomFactor = YAxisZoomFactor * ZoomInMultiple;

                _plotGridLineNumbersService.AddYAxisNumber(_actualCenterXWPF, _actualCenterYWPF,
                    Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    AmplitudeEnlargingFactorInternal, InternalXAxisScalingFactor, ref yAxisNumbers);
                OnPropertyChanged(nameof(YAxisNumbers));
                PlotTrignometricFunctions(SelectedTrigoFunction);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to zoom in vertically.\n" +
                    exception.Message);
            }
        }

        /// <summary>
        /// This method tells if the vertical zoom out can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteVerticalZoomOut(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Vertical Zoom Out button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteVerticalZoomOut(object obj)
        {
            try
            {
                YAxisZoomFactor = YAxisZoomFactor / ZoomInMultiple;
                _plotGridLineNumbersService.AddYAxisNumber(_actualCenterXWPF, _actualCenterYWPF,
                    Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    AmplitudeEnlargingFactorInternal, InternalXAxisScalingFactor, ref yAxisNumbers);
                OnPropertyChanged(nameof(YAxisNumbers));

                PlotTrignometricFunctions(SelectedTrigoFunction);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to zoom out vertically.\n" +
                    exception.Message);
            }
        }

        /// <summary>
        /// This method tells if the save to SVG format can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteSaveToSVGFormat(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Save to SVG format button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteSaveToSVGFormat(object obj)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "SVG Files (*.svg)|*.svg";
                if (saveFileDialog.ShowDialog() == true)
                {
                    _convertStrokeCollectionToSVGService.SaveSvgToFile(saveFileDialog.FileName, Strokes,
                        XAxisGridLines, YAxisGridLines,
                        XAxisNumbers, YAxisNumbers,
                        GraphWidth, GraphHeight);

                    MessageBox.Show("File saved successfully at " +
                    saveFileDialog.FileName);
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save the file." + ex.Message);
            }
        }

        /// <summary>
        /// This method tells if the reset zoom can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteResetZoom(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Reset Zoom button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteResetZoom(object obj)
        {
            try
            {
                XAxisZoomFactor = DefaultZoomingFactor;
                YAxisZoomFactor = DefaultZoomingFactor;
                PlotTrignometricFunctions(SelectedTrigoFunction);
                _plotGridLineNumbersService.AddXAxisNumber(_defaultCenterXWPF, _defaultCenterYWPF,
                    Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    InternalXAxisScalingFactor, ref xAxisNumbers);
                OnPropertyChanged(nameof(XAxisNumbers));
                _plotGridLineNumbersService.AddYAxisNumber(_defaultCenterXWPF, _defaultCenterYWPF,
                    Convert.ToDouble(XOffset), Convert.ToDouble(YOffset),
                    GraphWidth, GraphHeight, XAxisZoomFactor, YAxisZoomFactor,
                    AmplitudeEnlargingFactorInternal, InternalXAxisScalingFactor, ref yAxisNumbers);
                OnPropertyChanged(nameof(YAxisNumbers));
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to reset zoom\n" +
                    exception.Message);
            }
        }

        /// <summary>
        /// This method tells if the reset graph position can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteResetGraphPosition(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Reset Graph Position button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteResetGraphPosition(object obj)
        {
            try 
            {
                XOffset = Zero.ToString();
                YOffset = Zero.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to reset graph position\n" +
                    exception.Message);
            }
        }

        /// <summary>
        /// This method tells if the restore default can be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteRestoreDefaultCommand(object obj)
        {
            return true;
        }

        /// <summary>
        /// This methods is called when Restore Default button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteRestoreDefaultCommand(object obj)
        {
            try
            {
                SelectedTrigoFunction = Sin;
                XAxisZoomFactor = DefaultZoomingFactor;
                YAxisZoomFactor = DefaultZoomingFactor;
                AmplitudeEnlargingFactorExternal = OnLoadAmplitudeEnlargingFactorExternal.ToString();
                TimePeriod = OnLoadTimePeriodValue.ToString();
                PhaseShift = OnLoadPhaseShift.ToString();
                VerticalShift = OnLoadVerticalShift.ToString();
                _actualCenterXWPF = _defaultCenterXWPF;
                _actualCenterYWPF = _defaultCenterYWPF;
                XOffset = Zero.ToString();
                YOffset = Zero.ToString();
                OnPropertyChanged(nameof(XAxisNumbers));
                OnPropertyChanged(nameof(YAxisNumbers));
            }
            
            catch (Exception exception)
            {
                MessageBox.Show("Failed to restore defaults.\n" +
                    exception.Message);
            }
        }
        #endregion

    }
}
