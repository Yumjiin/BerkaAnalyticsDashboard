using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.DTOs;

namespace YJI.Berka.Dashboard.Renderers;

public class SpendingTrendRender : IDisposable
{
    private WpfPlot? _plot;

    public void Render(WpfPlot plot, IEnumerable<SpendingTrendDto> data)
    {
        _plot = plot;
        _plot.Plot.Clear();

        var list = data.ToList();
        if (list.Count == 0)
        {
            _plot.Refresh();
            return;
        }

        // X축: 날짜 → OADate(double) 변환
        // Y축: 일별 총 지출액
        double[] xs = list.Select(d => d.Date.ToOADate()).ToArray();
        double[] ys = list.Select(d => (double)d.TotalAmount).ToArray();

        // 라인 차트
        var line = _plot.Plot.Add.Scatter(xs, ys);
        line.Color = ScottPlot.Color.FromHex("#4A90D9");
        line.LineWidth = 1.5f;
        line.MarkerSize = 0;

        // 이상 탐지 날짜 빨간 수직선
        foreach (var point in list.Where(d => d.IsAnomaly))
        {
            var vl = _plot.Plot.Add.VerticalLine(point.Date.ToOADate());
            vl.Color = ScottPlot.Color.FromHex("#E53935").WithAlpha(0.4f);
            vl.LineWidth = 1f;
        }

        // X축 날짜 형식
        _plot.Plot.Axes.DateTimeTicksBottom();

        // 축 레이블
        _plot.Plot.XLabel("Date");
        _plot.Plot.YLabel("Total Expenses (CZK)");

        _plot.Refresh();
    }

    public void Dispose()
    {
        _plot?.Plot.Clear();
    }
}
