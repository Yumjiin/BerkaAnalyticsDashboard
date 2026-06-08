using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.DTOs;

namespace YJI.Berka.Dashboard.Renderers;

public class MonthlySpendingRender : IDisposable
{
    private WpfPlot? _plot;

    public void Render(WpfPlot plot, IEnumerable<MonthlySpendingDto> data)
    {
        _plot = plot;
        _plot.Plot.Clear();

        var list = data.ToList();
        if (list.Count == 0)
        {
            _plot.Refresh();
            return;
        }

        // ScottPlot 5 Bars API
        var bars = list.Select((item, i) => new ScottPlot.Bar
        {
            Position = i,
            Value = (double)item.TotalAmount,
            FillColor = (item.MomChangeRate ?? 0) >= 0
                ? ScottPlot.Color.FromHex("#4A90D9")
                : ScottPlot.Color.FromHex("#E53935"),
        }).ToArray();

        _plot.Plot.Add.Bars(bars);

        // X축 월 레이블
        _plot.Plot.Axes.Bottom.TickGenerator =
            new ScottPlot.TickGenerators.NumericManual(
                list.Select((item, i) => new ScottPlot.Tick(i, item.Label)).ToArray()
            );

        _plot.Plot.Axes.Bottom.TickLabelStyle.Rotation = 45;
        _plot.Plot.YLabel("Total (CZK)");
        _plot.Plot.Title("Monthly Spending");
        _plot.Plot.Axes.AutoScale();

        _plot.Refresh();
    }

    public void Dispose()
    {
        _plot?.Plot.Clear();
    }
}
