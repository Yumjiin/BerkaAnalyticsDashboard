using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.DTOs;

namespace YJI.Berka.Dashboard.Renderers;

public class AnomalyComparisonRender : IDisposable
{
    private WpfPlot? _plot;

    public void Render(WpfPlot plot, AnomalyComparisonDto dto)
    {
        _plot = plot;
        _plot.Plot.Clear();

        double[] values = new double[]
        {
            dto.ZscoreCount,
            dto.IforestCount,
            dto.AeCount,
            dto.HighConfidenceCount
        };

        string[] labels = new string[]
        {
            "Z-Score",
            "IForest",
            "Autoencoder",
            "High Conf."
        };

        for (int i = 0; i < values.Length; i++)
        {
            var bar = _plot.Plot.Add.Bar(position: i, value: values[i]);
            bar.Color = i == 3
                ? ScottPlot.Color.FromHex("#E53935")
                : ScottPlot.Color.FromHex("#4A90D9");
        }

        _plot.Plot.Axes.Bottom.TickGenerator =
            new ScottPlot.TickGenerators.NumericManual(
                Enumerable.Range(0, labels.Length)
                    .Select(i => new ScottPlot.Tick(i, labels[i]))
                    .ToArray()
            );

        _plot.Plot.YLabel("Count");
        _plot.Plot.Title("Anomaly Detection Comparison");

        _plot.Plot.Axes.AutoScale();
        _plot.Refresh();
    }

    public void Dispose()
    {
        _plot?.Plot.Clear();
    }
}
