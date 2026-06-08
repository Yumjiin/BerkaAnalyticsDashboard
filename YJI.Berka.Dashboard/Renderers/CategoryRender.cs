using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.DTOs;

namespace YJI.Berka.Dashboard.Renderers;

public class CategoryRender : IDisposable
{
    private WpfPlot? _plot;

    public void Render(WpfPlot plot, IEnumerable<CategoryDto> data)
    {
        _plot = plot;
        _plot.Plot.Clear();

        var list = data.OrderBy(c => c.TotalAmount).ToList();
        if (list.Count == 0)
        {
            _plot.Refresh();
            return;
        }

        // ScottPlot 5 Bars API
        var bars = list.Select((c, i) => new ScottPlot.Bar
        {
            Position = i,
            Value = (double)c.TotalAmount,
            FillColor = ScottPlot.Color.FromHex("#4A90D9"),
        }).ToArray();

        _plot.Plot.Add.Bars(bars);

        // X축 업종 이름
        _plot.Plot.Axes.Bottom.TickGenerator =
            new ScottPlot.TickGenerators.NumericManual(
                list.Select((c, i) => new ScottPlot.Tick(i, c.KSymbol)).ToArray()
            );

        _plot.Plot.YLabel("Total (CZK)");
        _plot.Plot.Title("Spending by Category");
        _plot.Plot.Axes.AutoScale();

        _plot.Refresh();
    }

    public void Dispose()
    {
        _plot?.Plot.Clear();
    }
}
