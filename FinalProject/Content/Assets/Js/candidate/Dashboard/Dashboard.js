$(document).ready(function () {
    $.getJSON("User/GetPie", function (data) {



        Highcharts.chart('chartPie', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'Source CV'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.0f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.0f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                name: 'total',
                colorByPoint: true,
                data: [{
                    name: 'Jobstreet',
                    y: data.Jobstreet
                }, {
                    name: 'JobsDB',
                    y: data.JobsID
                }, {
                    name: 'JobsID',
                    y: data.JobsDB
                }, {
                    name: 'Joblike',
                    y: data.Joblike
                }, {
                    name: 'Top Karir',
                    y: data.TopKarir
                }, {
                    name: 'KarirPad',
                    y: data.KarirPad
                }, {
                    name: 'Karir2.com',
                    y: data.Karir2
                }]
            }]
        });
    });
});


$(document).ready(function () {
    $.getJSON("User/GetBar", function (data) {


        Highcharts.chart('chartBar', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Total Stock VS Total Called CV'
            },
            xAxis: {
                categories: [
                    'Java',
                    'PHP',
                    'Ruby',
                    'VB.NET'
                ],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Total'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{
                name: 'Stock CV',
                data: [data.PreJava, data.PrePHP, data.PreRuby, data.PreVB]

            }, {
                name: 'Called CV',
                data: [data.CallJava, data.CallPHP, data.CallRuby, data.CallVB]

            }]
        });
    });
});