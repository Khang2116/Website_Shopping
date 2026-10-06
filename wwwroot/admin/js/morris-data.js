$(function () {
  Morris.Area({
    element: "morris-area-chart",
    data:
      typeof revenueChartData !== "undefined" && revenueChartData.length
        ? revenueChartData
        : [{ period: "N/A", revenue: 0 }],
    xkey: "period",
    ykeys: ["revenue"],
    labels: ["Doanh thu (đ)"],
    pointSize: 2,
    hideHover: "auto",
    resize: true,
  });
});
