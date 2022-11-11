import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AppSetting, MessageService } from 'src/app/shared';
import { Chart } from 'chart.js';
import { ReportSaleBussinessService } from '../service/report-sales-bussiness.service';
import { provinceMap, originMap, barChartData } from './map-province';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalEmployeeComponent } from '../modal-employee/modal-employee.component';
import { ModalDepartmentComponent } from '../modal-department/modal-department.component';

declare var google: any;
@Component({
  selector: 'app-report-sales-business',
  templateUrl: './report-sales-business.component.html',
  styleUrls: ['./report-sales-business.component.scss'],
})
export class ReportSalesBusinessComponent implements OnInit {
  listTypeChart: any[] = ['Biểu đồ cột', 'Biểu đồ đường'];
  viewMode: any[] = [
    { text: 'Theo tháng', value: 1 },
    { text: 'Theo quý', value: 2 },
    { text: 'Theo năm', value: 3 },
  ];
  model: any = {
    TypeChart: 'Biểu đồ cột',
    ViewMode: 1,
  };
  fromDate: String = new Date('01/01/2021').toISOString().slice(0, 10);
  toDate: String = new Date('12/31/2022').toISOString().slice(0, 10);
  public geochartData: any[] = [
    ['State', 'Value'],
    ['Thái Nguyên', 0],
  ];
  geochartTooltip: any[] = ['10 tỷ', '20 tỷ', '30 tỷ'];
  salesJobData: any[] = [];
  salesApplication: any[] = [];
  salesIndustry: any[] = [];
  salesEmployee: any[] = [];
  salesDepartment: any[] = [];
  salesEmployeeLine: any = {};
  salesDepartmentLine: any = {};
  departmentId: String;
  employeesCode: any[] = [];
  departmentCode: any[] = [];
  isDebug: Boolean = true;
  constructor(
    public appSetting: AppSetting,
    public reportSaleBussinessService: ReportSaleBussinessService,
    private messageService: MessageService,
    private modalService: NgbModal
  ) {
    this.geochartData = originMap;
  }

  ngOnInit(): void {
    this.appSetting.PageTitle = 'Báo cáo kết quả kinh doanh theo doanh số';

    //geo chart Viet Nam
    google.charts.load('current', {
      packages: ['geochart'],
    });
    google.charts.load('current', { packages: ['corechart'] });
    //google.charts.setOnLoadCallback(() => this.drawStacked());
    google.charts.setOnLoadCallback(() => this.getData());
  }
  // ngAfterViewInit(): void {
  //   this.getData();
  // }
  getCurrentDuration() {
    this.fromDate && this.toDate && this.getData();
  }
  getData() {
    this.reportSaleBussinessService
      .salesTargetRegion({ from: this.fromDate, to: this.toDate })
      .subscribe(
        (data) => {
          this.geochartData = this.makeGeoChartArray(data);
          //console.log(this.geochartData);
          this.drawVisualization();
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
    if (this.isDebug) {
      this.salesJobData = this.makeSaleJobData(barChartData);
      this.salesApplication = this.makeSaleJobData(barChartData);
      this.salesIndustry = this.makeSaleJobData(barChartData);
    } else {
      this.reportSaleBussinessService
        .salesJob({ from: this.fromDate, to: this.toDate })
        .subscribe(
          (data) => {
            this.salesJobData = this.makeSaleJobData(data);
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
            //this.drawStacked();
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
            //this.drawStacked();
          },
          (error) => {
            this.messageService.showError(error);
          }
        );
    }
    this.drawStacked();
  }
  makeRangeTooltip(data) {
    let min = Number.MAX_VALUE;
    let max = 0;
    for (let i = 0; i < data.length; i++) {
      data[i] < min && (min = data[i]);
      data[i] > max && (max = data[i]);
    }
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
    !this.isDebug &&
      resData.forEach((element) => {
        element.Reality /= 1000000000;
        element.Target /= 1000000000;
      });
    let data = [
      [
        'Doanh số',
        { type: 'number' },
        { type: 'string', role: 'tooltip' },
        { type: 'number' },
        { type: 'string', role: 'tooltip' },
        { type: 'string', role: 'annotation' },
      ],
    ];
    resData.map((rd) => {
      let percent = Math.round((rd.Reality / rd.Target) * 100);
      let annotation =
        rd.Reality.toString() +
        '/' +
        rd.Target.toString() +
        '[' +
        percent.toString() +
        '%]';
      let secondStack = rd.Target - rd.Reality > 0 ? rd.Target - rd.Reality : 0;
      data = [
        ...data,
        [
          rd.Name,
          rd.Reality,
          `${rd.Name}\nThực tế: ${rd.Reality} tỷ\nĐăng ký: ${rd.Target} tỷ`,
          secondStack,
          `${rd.Name}\nThực tế: ${rd.Reality} tỷ\nĐăng ký: ${rd.Target} tỷ`,
          annotation,
        ],
      ];
    });
    return data;
  }
  createTooltipContent(name, r, t) {
    return `<div style="width: 120px;
  height: 60px;
  font-size: 10px;
  font-weight: 200;
  background-color: black;
  overflow: hidden;
  position: absolute;
  color: white;"><p style="text-align: center; padding: 0; margin:0">${name}</p><p style="padding: 0; margin:0">DS đạt được: ${r} tỷ đồng</p><p style="padding: 0; margin:0">DS đăng ký: ${t} tỷ đồng</p></div>`;
  }
  checkBarDataNull(inputData) {
    let check = true;
    inputData.forEach((e) => {
      e.Target != 0 && e.Reality != 0 && (check = false);
    });
    return check;
  }
  fakeBarData(inputData) {
    return inputData.map((e) => {
      return {
        Code: e.Code,
        Reality: this.getRandomInt(50),
        Target: this.getRandomInt(50),
      };
    });
  }
  makeSaleEmployeeData(resData) {
    if (this.checkBarDataNull(resData)) {
      resData = this.fakeBarData(resData);
    } else {
      resData.forEach((element) => {
        element.Reality /= 1000000000;
        element.Target /= 1000000000;
      });
    }
    console.log(resData);
    let data = [
      [
        'Doanh số',
        { type: 'number' },
        { type: 'string', role: 'tooltip' },
        { type: 'number' },
        { type: 'string', role: 'tooltip' },
        { type: 'string', role: 'annotation' },
      ],
    ];
    resData.map((rd) => {
      let percent = Math.round((rd.Reality / rd.Target) * 100);
      let annotation =
        rd.Reality.toString() +
        '/' +
        rd.Target.toString() +
        '[' +
        percent.toString() +
        '%]';
      let secondStack = rd.Target - rd.Reality > 0 ? rd.Target - rd.Reality : 0;
      data = [
        ...data,
        [
          rd.Code,
          rd.Reality,
          `${rd.Code}\nThực tế: ${rd.Reality} tỷ\nĐăng ký: ${rd.Target} tỷ`,
          secondStack,
          `${rd.Code}\nThực tế: ${rd.Reality} tỷ\nĐăng ký: ${rd.Target} tỷ`,
          annotation,
        ],
      ];
    });
    return data;
  }
  drawVisualization() {
    var data = google.visualization.arrayToDataTable(this.geochartData);
    var opts = {
      region: 'VN',
      displayMode: 'regions',
      resolution: 'provinces',
      colorAxis: {
        colors: ['#c7f8f2', '#dbe2c4', '#d3b346', '#eebe46', '#d35e57'],
      },
      defaultColor: '#f5f5f5',
      keepAspectRatio: false,
      legend: 'none',
      width: 400,
      height: 500,
    };
    var geochart = new google.visualization.GeoChart(
      document.getElementById('visualization')
    );
    geochart.draw(data, opts);
  }
  getOptions(row) {
    let options = {
      chartArea: { width: '65%', top: 50, left: 40 },
      height: row * 40,
      legend: { position: 'none' },
      isStacked: true,
      hAxis: {
        minValue: 0,
        ticks: [0, 16, 33, 50],
      },
      titleTextStyle: {
        fontSize: 12,
      },
      vAxis: {
        textStyle: {
          fontSize: 8,
        },
        gridlines: { count: 4 },
      },
      bar: {
        width: 15,
      },
      series: {
        0: { color: '#746bf3' },
        1: { color: '#c874cc' },
      },
    };
    return options;
  }
  drawStacked() {
    let salesJobData = google.visualization.arrayToDataTable(this.salesJobData);
    let salesApplicationData = google.visualization.arrayToDataTable(
      this.salesApplication
    );
    let salesIndustryData = google.visualization.arrayToDataTable(
      this.salesIndustry
    );
    let salesJobOptions = this.getOptions(salesJobData.getNumberOfRows());
    let salesApplicationOptions = this.getOptions(
      salesApplicationData.getNumberOfRows()
    );
    let salesIndustryOptions = this.getOptions(
      salesIndustryData.getNumberOfRows()
    );
    var chart = new google.visualization.BarChart(
      document.getElementById('ulChart')
    );
    var chart2 = new google.visualization.BarChart(
      document.getElementById('ulChart2')
    );
    var chart3 = new google.visualization.BarChart(
      document.getElementById('ulChart3')
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
  }
  drawEmployeeChart(inputData) {
    let data = google.visualization.arrayToDataTable(inputData);
    let options = this.getOptions(data.getNumberOfRows());
    let chart4 = new google.visualization.BarChart(
      document.getElementById('dlChart')
    );
    console.log(this.salesEmployee);

    inputData.length > 0
      ? chart4.draw(data, options)
      : (document.getElementById('dlChart').innerHTML =
          '<h3>Không có dữ liệu</h3>');
  }
  changeTypeChart() {
    let chart = document.getElementById('dlChart');
    let chart2 = document.getElementById('dlChart2');
    if (this.model.TypeChart == 'Biểu đồ đường') {
      //google.charts.load('current', { packages: ['corechart', 'line'] });
      //google.charts.setOnLoadCallback(() => this.drawCurveTypes());
      this.drawEmployeeLineChart();
      this.drawDepartmentLineChart();

      chart.style.overflowX = 'scroll';
      chart.style.overflowY = 'hidden';
      chart2.style.overflowX = 'scroll';
      chart2.style.overflowY = 'hidden';
    } else {
      this.drawEmployeeChart(this.salesEmployee);
      this.drawDepartmentChart(this.salesDepartment);
      chart.style.overflowY = 'scroll';
      chart.style.overflowX = 'hidden';
      chart2.style.overflowY = 'scroll';
      chart2.style.overflowX = 'hidden';
    }
  }
  openModal() {
    let activeModal = this.modalService.open(ModalEmployeeComponent, {
      container: 'body',
      windowClass: 'select-Project-model',
      backdrop: 'static',
    });
    activeModal.result.then((result) => {
      if (result.listIdSelect.length > 0) {
        this.departmentId = result.departmentId;
        this.employeesCode = result.listIdSelect;
      }
      this.reportSaleBussinessService
        .salesEmployeeByDepartment({
          from: this.fromDate,
          to: this.toDate,
          departmentId: this.departmentId,
          employeesCode: this.employeesCode,
        })
        .subscribe(
          (data) => {
            this.salesEmployee = this.makeSaleEmployeeData(data);
            if (this.model.TypeChart === 'Biểu đồ cột') {
              this.drawEmployeeChart(this.makeSaleEmployeeData(data));
            } else {
              this.drawEmployeeLineChart();
            }
          },
          (error) => {
            this.messageService.showError(error);
          }
        );
      this.reportSaleBussinessService
        .salesEmployeeByDepartmentLine({
          from: this.fromDate,
          to: this.toDate,
          departmentId: this.departmentId,
          employeesCode: this.employeesCode,
          mode: this.model.ViewMode,
        })
        .subscribe(
          (data) => {
            if (this.checkNullData(data[0].Datas)) {
              data = data.map((d) => {
                return { Code: d.Code, Datas: this.fakeData(d.Datas) };
              });
            }
            this.salesEmployeeLine = this.makeEmployeeLineData(data);
          },
          (error) => {
            this.messageService.showError(error);
          }
        );
    });
  }
  drawDepartmentChart(inputData) {
    let data = google.visualization.arrayToDataTable(inputData);
    let options = this.getOptions(data.getNumberOfRows());
    let chart = new google.visualization.BarChart(
      document.getElementById('dlChart2')
    );
    inputData.length > 0
      ? chart.draw(data, options)
      : (document.getElementById('dlChart2').innerHTML =
          '<h3>Không có dữ liệu</h3>');
  }
  openDepartmentModal() {
    let activeModal = this.modalService.open(ModalDepartmentComponent, {
      container: 'body',
      windowClass: 'select-Project-model',
      backdrop: 'static',
    });
    activeModal.result.then((result) => {
      if (result.listIdSelect.length > 0) {
        this.departmentCode = result.listIdSelect;
        this.reportSaleBussinessService
          .salesDepartments({
            from: this.fromDate,
            to: this.toDate,
            departmentsCode: this.departmentCode,
          })
          .subscribe(
            (data) => {
              this.salesDepartment = this.makeSaleEmployeeData(data);
              if (this.model.TypeChart === 'Biểu đồ cột') {
                this.drawDepartmentChart(this.makeSaleEmployeeData(data));
              } else {
                this.drawDepartmentLineChart();
              }
            },
            (error) => {
              this.messageService.showError(error);
            }
          );
        this.reportSaleBussinessService
          .salesDepartmentLine({
            from: this.fromDate,
            to: this.toDate,
            departmentsCode: this.departmentCode,
            mode: this.model.ViewMode,
          })
          .subscribe(
            (data) => {
              if (this.checkNullData(data[0].Datas)) {
                data = data.map((d) => {
                  return { Code: d.code, Datas: this.fakeData(d.Datas) };
                });
                this.salesDepartmentLine = this.makeEmployeeLineData(data);
              }
            },
            (error) => {
              this.messageService.showError(error);
            }
          );
      }
    });
  }
  changeViewMode() {
    if (this.model.TypeChart === 'Biểu đồ đường') {
      this.reportSaleBussinessService
        .salesEmployeeByDepartmentLine({
          from: this.fromDate,
          to: this.toDate,
          departmentId: this.departmentId,
          employeesCode: this.employeesCode,
          mode: this.model.ViewMode,
        })
        .subscribe(
          (data) => {
            if (this.checkNullData(data[0].Datas)) {
              data = data.map((d) => {
                return { Code: d.Code, Datas: this.fakeData(d.Datas) };
              });
            }
            this.salesEmployeeLine = this.makeEmployeeLineData(data);
            this.drawEmployeeLineChart();
          },
          (error) => {
            this.messageService.showError(error);
          }
        );
      this.reportSaleBussinessService
        .salesDepartmentLine({
          from: this.fromDate,
          to: this.toDate,
          departmentsCode: this.departmentCode,
          mode: this.model.ViewMode,
        })
        .subscribe(
          (data) => {
            if (this.checkNullData(data[0].Datas)) {
              data = data.map((d) => {
                return { Code: d.code, Datas: this.fakeData(d.Datas) };
              });
              this.salesDepartmentLine = this.makeEmployeeLineData(data);
              this.drawDepartmentLineChart();
            }
          },
          (error) => {
            this.messageService.showError(error);
          }
        );
    }
  }
  checkNullData(data) {
    let check = true;
    data.forEach((d) => {
      d.Value != 0 && (check = false);
    });
    return check;
  }
  fakeData(inputData) {
    return inputData.map((e) => {
      return { Code: e.Code, Value: this.getRandomInt(15) };
    });
  }
  getRandomInt(max) {
    return Math.floor(Math.random() * max);
  }
  makeEmployeeLineData(inputData) {
    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Thời gian');
    inputData.forEach((i) => {
      data.addColumn('number', i.Code);
    });
    let rowData = [];
    inputData[0].Datas.forEach((e) => {
      rowData = [...rowData, [e.Code]];
    });
    inputData.forEach((i) => {
      i.Datas.forEach((e, index) => {
        rowData[index].push(e.Value);
      });
    });
    data.addRows(rowData);
    return { data: data, rows: rowData.length };
  }
  drawEmployeeLineChart() {
    let width = this.model.ViewMode == 3 ? 150 : 60;
    var options = {
      legend: 'none',
      hAxis: {
        title: 'Thời gian',
      },
      vAxis: {
        title: 'Doanh số thực (tỷ)',
      },
      chartArea: { width: '90%' },
      width: this.salesEmployeeLine.rows * width,
      height: 300,
    };

    var chart = new google.visualization.LineChart(
      document.getElementById('dlChart')
    );
    chart.draw(this.salesEmployeeLine.data, options);
  }
  drawDepartmentLineChart() {
    let width = this.model.ViewMode == 3 ? 150 : 60;
    var options = {
      legend: 'none',
      hAxis: {
        title: 'Thời gian',
      },
      vAxis: {
        title: 'Doanh số thực (tỷ)',
      },
      chartArea: { width: '90%' },
      width: this.salesDepartmentLine.rows * width,
      height: 300,
    };

    var chart = new google.visualization.LineChart(
      document.getElementById('dlChart2')
    );
    chart.draw(this.salesDepartmentLine.data, options);
  }
}
