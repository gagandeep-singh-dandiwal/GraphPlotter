using System;
using System.Collections.ObjectModel;
using Xunit;
using GraphPlotter.Common;
using GraphPlotter.Services;
public class PlotGridLineNumbersService_Test
{
    [Fact]
    public void AddXAxisNumber_ShouldAddCorrectNumberOfXAxisNumbers()
    {
        // Arrange
        var service = new PlotGridLineNumbersService();
        var xAxisNumbers = new ObservableCollection<Number>();
        double graphWidth = 350*Math.PI;
        double graphHeight = 600;
        double actualCenterX = graphWidth / 2;
        double actualCenterY = graphHeight / 2;
        double internalXAxisScalingFactor = 20;
        double xAxisZoomFactor = 1;
        double yAxisZoomFactor = 1;
        double xOffset = 0;
        double yOffset = 0;

        // Act
        service.AddXAxisNumber(actualCenterX, actualCenterY, 
            xOffset, yOffset, 
            graphWidth, graphHeight, 
            xAxisZoomFactor, yAxisZoomFactor, 
            internalXAxisScalingFactor, ref xAxisNumbers);

        // Assert
        int expectedCount = Convert.ToInt32(graphWidth / (Math.PI * internalXAxisScalingFactor) - 1) + 1;
        Assert.Equal(expectedCount, xAxisNumbers.Count);
    }

    [Fact]
    public void AddYAxisNumber_ShouldAddCorrectNumberOfYAxisNumbers()
    {
        // Arrange
        var service = new PlotGridLineNumbersService();
        var yAxisNumbers = new ObservableCollection<Number>();
        double graphWidth = 350 * Math.PI;
        double graphHeight = 600;
        double actualCenterX = graphWidth / 2;
        double actualCenterY = graphHeight / 2;
        double amplitudeEnlargingFactorInternal = 10;
        double internalXAxisScalingFactor = 20;
        double xAxisZoomFactor = 1;
        double yAxisZoomFactor = 1;
        double xOffset = 0;
        double yOffset = 0;

        // Act
        service.AddYAxisNumber(actualCenterX, actualCenterY,
            xOffset, yOffset, 
            graphWidth, graphHeight, 
            xAxisZoomFactor, yAxisZoomFactor, 
            amplitudeEnlargingFactorInternal, internalXAxisScalingFactor, ref yAxisNumbers);

        // Assert
        int expectedCount = Convert.ToInt32(graphHeight / amplitudeEnlargingFactorInternal)-1;
        Assert.Equal(expectedCount, yAxisNumbers.Count);
    }

    [Fact]
    public void AddXAxisNumber_ShouldAddPositiveAndNegativeXAxisNumbers()
    {
        // Arrange
        var service = new PlotGridLineNumbersService();
        var xAxisNumbers = new ObservableCollection<Number>();
        double graphWidth = 350 * Math.PI;
        double graphHeight = 600;
        double actualCenterX = graphWidth / 2;
        double actualCenterY = graphHeight / 2;
        double amplitudeEnlargingFactorInternal = 10;
        double internalXAxisScalingFactor = 20;
        double xAxisZoomFactor = 1;
        double yAxisZoomFactor = 1;
        double xOffset = 0;
        double yOffset = 0;

        // Act
        service.AddYAxisNumber(actualCenterX, actualCenterY,
            xOffset, yOffset,
            graphWidth, graphHeight,
            xAxisZoomFactor, yAxisZoomFactor,
            amplitudeEnlargingFactorInternal, internalXAxisScalingFactor, ref xAxisNumbers);

        // Assert
        Assert.Contains(xAxisNumbers, n => n.Y > graphHeight/ 2); // Positive X axis numbers
        Assert.Contains(xAxisNumbers, n => n.Y < graphHeight / 2); // Negative X axis numbers
        Assert.Contains(xAxisNumbers, n => n.Y < graphHeight);
        Assert.Contains(xAxisNumbers, n => n.Y > 0);
    }

    [Fact]
    public void AddYAxisNumber_ShouldAddPositiveAndNegativeYAxisNumbers()
    {
        // Arrange
        var service = new PlotGridLineNumbersService();
        var yAxisNumbers = new ObservableCollection<Number>();
        double graphWidth = 350 * Math.PI;
        double graphHeight = 600;
        double actualCenterX = graphWidth / 2;
        double actualCenterY = graphHeight / 2;
        double amplitudeEnlargingFactorInternal = 10;
        double internalXAxisScalingFactor = 20;
        double xAxisZoomFactor = 1;
        double yAxisZoomFactor = 1;
        double xOffset = 0;
        double yOffset = 0;

        // Act
        service.AddYAxisNumber(actualCenterX, actualCenterY,
            xOffset, yOffset,
            graphWidth, graphHeight,
            xAxisZoomFactor, yAxisZoomFactor,
            amplitudeEnlargingFactorInternal, internalXAxisScalingFactor, ref yAxisNumbers);

        // Assert
        Assert.Contains(yAxisNumbers, n => n.Y < graphHeight / 2); // Positive Y axis numbers
        Assert.Contains(yAxisNumbers, n => n.Y > graphHeight / 2); // Negative Y axis numbers
    }

    [Fact]
    public void AddYAxisNumber_ShouldFormatValuesCorrectly()
    {
        // Arrange
        var service = new PlotGridLineNumbersService();
        var yAxisNumbers = new ObservableCollection<Number>();
        double graphWidth = 350 * Math.PI;
        double graphHeight = 600;
        double actualCenterX = graphWidth / 2;
        double actualCenterY = graphHeight / 2;
        double amplitudeEnlargingFactorInternal = 10;
        double internalXAxisScalingFactor = 20;
        double xAxisZoomFactor = 1;
        double yAxisZoomFactor = 1;
        double xOffset = 0;
        double yOffset = 0;

        // Act
        service.AddYAxisNumber(actualCenterX, actualCenterY,
            xOffset, yOffset,
            graphWidth, graphHeight,
            xAxisZoomFactor, yAxisZoomFactor,
            amplitudeEnlargingFactorInternal, internalXAxisScalingFactor, ref yAxisNumbers);

        // Assert
        Assert.All(yAxisNumbers, n => Assert.False(string.IsNullOrWhiteSpace(n.Value)));
    }
}

