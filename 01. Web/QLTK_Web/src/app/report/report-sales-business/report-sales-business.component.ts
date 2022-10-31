import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService } from 'src/app/shared';
import { Chart } from 'chart.js';
import { ReportSaleBussinessService } from '../service/report-sales-bussiness.service';
import { provinceMap, originMap } from './map-province';

declare var google: any;
@Component({
  selector: 'app-report-sales-business',
  templateUrl: './report-sales-business.component.html',
  styleUrls: ['./report-sales-business.component.scss'],
})
export class ReportSalesBusinessComponent implements OnInit {
  listTypeChart: any[] = ['Biểu đồ cột', 'Biểu đồ đường'];
  model: any = {
    TypeChart: 'Biểu đồ cột',
  };
  fromDate: String = new Date('01/01/2019').toISOString().slice(0, 10);
  toDate: String = new Date('12/31/2022').toISOString().slice(0, 10);
  public geochartData: any[] = [
    ['State', 'Value'],
    ['Thái Nguyên', 0],
  ];
  public salesJobData: any[] = [];
  salesApplication: any[] = [];
  salesIndustry: any[] = [];
  constructor(
    public appSetting: AppSetting,
    public reportSaleBussinessService: ReportSaleBussinessService,
    private messageService: MessageService
  ) {
    this.geochartData = originMap;
  }

  ngOnInit(): void {
    this.appSetting.PageTitle = 'Báo cáo kết quả kinh doanh theo doanh số';

    //geo chart Viet Nam
    google.load('visualization', '1', {
      packages: ['geochart'],
    });

    google.setOnLoadCallback(this.drawVisualization);
    google.charts.load('ulChart', { packages: ['corechart', 'bar'] });
    google.charts.setOnLoadCallback(this.drawStacked);
    //this.getCurrentDuration();
  }

  getCurrentDuration() {
    this.fromDate && this.toDate && this.getData();
  }
  getData() {
    this.reportSaleBussinessService
      .salesTargetRegion({ from: this.fromDate, to: this.toDate })
      .subscribe(
        (data) => {
          this.geochartData = this.makeGeoChartArray(data);
          this.drawVisualization();
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
    this.reportSaleBussinessService
      .salesJob({ from: this.fromDate, to: this.toDate })
      .subscribe(
        (data) => {
          this.salesJobData = this.makeSaleJobData(data);
          this.drawStacked();
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
    this.reportSaleBussinessService
      .salesApplication({ from: this.fromDate, to: this.toDate })
      .subscribe(
        (data) => {
          this.salesApplication = this.makeSaleJobData(data);
          this.drawStacked();
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
    this.reportSaleBussinessService
      .salesIndustry({ from: this.fromDate, to: this.toDate })
      .subscribe(
        (data) => {
          this.salesIndustry = this.makeSaleJobData(data);
          this.drawStacked();
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
    this.reportSaleBussinessService
      .salesDepartments({
        from: this.fromDate,
        to: this.toDate,
      })
      .subscribe(
        (data) => {
          console.log(data);
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
  }

  makeGeoChartArray(resData) {
    let data = [['Tỉnh', 'Doanh số']];
    let referMap = provinceMap;
    resData.map((rd) => {
      let temp = referMap[+rd.Code];
      referMap = referMap.filter((r) => r != temp);
      data = [...data, [temp, rd.Sale]];
    });
    referMap = referMap.filter((m) => m != '');
    referMap.map((r) => {
      data = [...data, [r, '0']];
    });
    return data;
  }
  makeSaleJobData(resData) {
    let data = [['Doanh số', 'Doanh số đạt được', 'Doanh số đăng ký']];
    resData.map((rd) => {
      data = [...data, [rd.Name, rd.Reality, rd.Target]];
    });
    return data;
  }
  drawVisualization() {
    let tempData = this.geochartData;
    var data = google.visualization.arrayToDataTable(tempData);
    var opts = {
      region: 'VN',
      displayMode: 'regions',
      resolution: 'provinces',
      colorAxis: {
        colors: ['#594545', '#F0FF42', '#54B435', '#2146C7', '#E0144C'],
      },
      backgroundColor: '#81d4fa',
      datalessRegionColor: '#f8bbd0',
      defaultColor: '#f5f5f5',
      keepAspectRatio: false,
      legend: {
        fontSize: 16,
      },
    };
    var geochart = new google.visualization.GeoChart(
      document.getElementById('visualization')
    );
    geochart.draw(data, opts);
  }
  drawStacked() {
    let salesJobData = google.visualization.arrayToDataTable(this.salesJobData);
    let salesApplicationData = google.visualization.arrayToDataTable(
      this.salesApplication
    );
    let salesIndustryData = google.visualization.arrayToDataTable(
      this.salesIndustry
    );
    let data = google.visualization.arrayToDataTable([
      ['City', '2010 Population', '2000 Population'],
      ['New York City, NY', 8175000, 8008000],
      ['Los Angeles, CA', 3792000, 3694000],
      ['Chicago, IL', 2695000, 2896000],
      ['Houston, TX', 2099000, 1953000],
      ['Philadelphia, PA', 1526000, 1517000],
    ]);
    let salesJobOptions = {
      title: 'Biểu đồ doanh số theo lĩnh vực sản xuất',
      chartArea: { width: '65%', top: 50, left: 100 },
      height: 250,
      width: 200,
      legend: { position: 'none' },
      isStacked: true,
      hAxis: {
        minValue: 0,
        ticks: [0, 3000000000, 6000000000, 9000000000, 12000000000],
      },
      titleTextStyle: {
        fontSize: 12,
      },
      vAxis: {
        textStyle: {
          fontSize: 6,
        },
        gridlines: { count: 4 },
      },
    };
    let salesApplicationOptions = {
      title: 'Biểu đồ doanh số theo ứng dụng',
      chartArea: { width: '65%', top: 50, left: 100 },
      height: 250,
      width: 200,
      legend: { position: 'none' },
      isStacked: true,
      hAxis: {
        minValue: 0,
        ticks: [0, 3000000000, 6000000000, 9000000000, 12000000000],
      },
      titleTextStyle: {
        fontSize: 12,
      },
      vAxis: {
        textStyle: {
          fontSize: 6,
        },
        gridlines: { count: 4 },
      },
    };
    let salesIndustryOptions = {
      title: 'Biểu đồ doanh số theo ngành hàng',
      chartArea: { width: '65%', top: 50, left: 100 },
      height: 250,
      width: 200,
      legend: { position: 'none' },
      isStacked: true,
      hAxis: {
        minValue: 0,
        ticks: [0, 3000000000, 6000000000, 9000000000, 12000000000],
      },
      titleTextStyle: {
        fontSize: 12,
      },
      vAxis: {
        textStyle: {
          fontSize: 6,
        },
        gridlines: { count: 4 },
      },
    };
    var chart = new google.visualization.BarChart(
      document.getElementById('ulChart')
    );
    var chart2 = new google.visualization.BarChart(
      document.getElementById('ulChart2')
    );
    var chart3 = new google.visualization.BarChart(
      document.getElementById('ulChart3')
    );
    var chart4 = new google.visualization.BarChart(
      document.getElementById('dlChart')
    );
    var chart5 = new google.visualization.BarChart(
      document.getElementById('dlChart2')
    );
    chart.draw(salesJobData, salesJobOptions);
    this.salesApplication.length > 0
      ? chart2.draw(salesApplicationData, salesApplicationOptions)
      : (document.getElementById('ulChart2').innerHTML =
          '<h3>Không có dữ liệu</h3>');
    this.salesIndustry.length > 0
      ? chart3.draw(salesIndustryData, salesIndustryOptions)
      : (document.getElementById('ulChart3').innerHTML =
          '<h3>Không có dữ liệu</h3>');
    chart4.draw(data, salesJobOptions);
    chart5.draw(data, salesJobOptions);
  }
  changeTypeChart() {
    if (this.model.TypeChart == 'Biểu đồ đường') {
      google.charts.load('current', { packages: ['corechart', 'line'] });
      google.charts.setOnLoadCallback(this.drawCurveTypes);
    } else {
      google.charts.load('ulChart', { packages: ['corechart', 'bar'] });
      google.charts.setOnLoadCallback(this.drawStacked);
    }
  }
  drawCurveTypes() {
    var data = new google.visualization.DataTable();
    data.addColumn('number', 'X');
    data.addColumn('number', 'Dogs');
    data.addColumn('number', 'Cats');

    data.addRows([
      [0, 10, 20],
      [1, 10, 5],
      [2, 23, 15],
      [3, 17, 9],
      [4, 18, 10],
      [5, 9, 5],
      [6, 11, 3],
      [7, 27, 19],
      [8, 33, 25],
      [9, 40, 32],
      [10, 32, 24],
      [11, 35, 27],
      [12, 30, 22],
      [13, 40, 32],
      [14, 42, 34],
      [15, 47, 39],
      [16, 44, 36],
      [17, 48, 40],
      [18, 52, 44],
      [19, 54, 46],
      [20, 42, 34],
      [21, 55, 47],
      [22, 56, 48],
      [23, 57, 49],
      [24, 60, 52],
      [25, 50, 42],
      [26, 52, 44],
      [27, 51, 43],
      [28, 49, 41],
      [29, 53, 45],
      [30, 55, 47],
      [31, 60, 52],
      [32, 61, 53],
      [33, 59, 51],
      [34, 62, 54],
      [35, 65, 57],
      [36, 62, 54],
      [37, 58, 50],
      [38, 55, 47],
      [39, 61, 53],
      [40, 64, 56],
      [41, 65, 57],
      [42, 63, 55],
      [43, 66, 58],
      [44, 67, 59],
      [45, 69, 61],
      [46, 69, 61],
      [47, 70, 62],
      [48, 72, 64],
      [49, 68, 60],
      [50, 66, 58],
      [51, 65, 57],
      [52, 67, 59],
      [53, 70, 62],
      [54, 71, 63],
      [55, 72, 64],
      [56, 73, 65],
      [57, 75, 67],
      [58, 70, 62],
      [59, 68, 60],
      [60, 64, 56],
      [61, 60, 52],
      [62, 65, 57],
      [63, 67, 59],
      [64, 68, 60],
      [65, 69, 61],
      [66, 70, 62],
      [67, 72, 64],
      [68, 75, 67],
      [69, 80, 72],
    ]);

    var options = {
      hAxis: {
        title: 'Time',
      },
      vAxis: {
        title: 'Popularity',
      },
      series: {
        1: { curveType: 'function' },
      },
    };

    var chart = new google.visualization.LineChart(
      document.getElementById('dlChart')
    );
    var chart2 = new google.visualization.LineChart(
      document.getElementById('dlChart2')
    );
    chart.draw(data, options);
    chart2.draw(data, options);
  }
}
