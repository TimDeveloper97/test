import {
  Component,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren,
  ViewEncapsulation,
} from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import {
  Label,
  monkeyPatchChartJsLegend,
  monkeyPatchChartJsTooltip,
  BaseChartDirective,
} from 'ng2-charts';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ProjectProductService } from '../../service/project-product.service';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Data } from '@angular/router';
import { sampleTime } from 'rxjs-compat/operator/sampleTime';

@Component({
  selector: 'app-dashboard-plan',
  templateUrl: './dashboard-plan.component.html',
  styleUrls: ['./dashboard-plan.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardPlanComponent implements OnInit {
  // Pie Tỉ lệ hoàn thành của dự án
  @ViewChildren(BaseChartDirective) charts:
    | QueryList<BaseChartDirective>
    | undefined;

  ngAfterViewInit() {
    console.log(this.charts.toArray()[1].chart.options);
  }

  public chartPlugins = [pluginDataLabels];
  public barChartLabels: Label[] = [
    'Hoàn thành',
    'Đang triển khai',
    'Chưa triển khai',
  ];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartOptions1: ChartOptions = {};
  public barChartOptions2: ChartOptions = {};
  public barChartOptionsPie: ChartOptions = {
    responsive: true,
    tooltips: {
      enabled: false,
    },
    legend: {
      position: 'right',
    },
    maintainAspectRatio: true,
    plugins: {
      datalabels: {
        formatter: (value) => {
          let sum = 0;
          this.doughnutChartData.forEach((number) => {
            sum = sum + number;
          });
          let percentage = ((value * 100) / sum).toFixed(2) + '%';
          return percentage;
        },
        color: '#000000',
      },
    },
  };
  public barChartData: ChartDataSets[] = [
    { data: [], label: 'Tỉ lệ hoàn thành của dự án' },
  ];

  public doughnutChartData: number[] = [0, 0, 0];
  public doughnutChartType: ChartType = 'doughnut';
  public pieChartType: ChartType = 'pie';
  public pieChartColors = [
    {
      backgroundColor: ['#00CC00', '#CCCC66', '#FF0000'],
    },
  ];
  //horizontabar Tổng hợp tình trạng công việc của dự án
  public barChartType1: ChartType = 'horizontalBar';

  public barChartData1: ChartDataSets[] = [
    {
      data: [0, 0, 0],
      barThickness: 20,
      stack: 'a',
      backgroundColor: ['#0033CC', '#FF6600', '#CC66CC'],
    },
  ];
  public barChartLabels1: string[] = [
    'SL CV hoàn thành',
    'SL CV đang triển khai',
    'SL CV chưa triển khai',
  ];

  //horizontabar Tổng hợp vấn đề tồn đọng của dự án
  public barChartType2: ChartType = 'horizontalBar';

  public barChartData2: ChartDataSets[] = [
    {
      data: [0,0, 0, 0],
      barThickness: 20,
      stack: 'a',
      backgroundColor: ['#FF6600', '#CC66CC', '#FFFF00'],
    },
  ];
  public barChartLabels2: string[] = [
    'SL chưa có hành động',
    'SL hoàn thành',
    'SL đang triển khai',
    'SL chưa triển khai',
  ];

  TotalWork  =0;
  TotalError =0;
  //
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constant: Constants,
    public appSetting: AppSetting,
    private projectProductService: ProjectProductService
  ) {
    monkeyPatchChartJsTooltip();
    monkeyPatchChartJsLegend();
  }
  isAction: boolean = false;
  listProject: any = [];

  ListStageOfProject: any[] = [];
  DoneRatioOfProject: string;

  listPlan: any = [];
  totalDay: any;
  daysOfMonth: any = [];
  selectDateId = -1;
  dayOfWeek: any = [];
  listProjectProduct: any = [];
  listExplanedId: any = [];
  projectIdSelect: any;
  items: any;
  suppliers: any = [];

  mode = 'single';
  ListResult: any[] = [];
  ListStage: any[] = [];
  ListPlan: any[] = [];
  ListPlanResult: any[] = [];
  ProjectPlan ={
    ProductPlans :[],
    StageModels: []
  }

  WorkLate = 0;
  WorkEmptyStart = 0;
  WorkEmptyEnd = 0;
  WorkIncurred = 0;
  idProject: string;
  Name: string;

  ngOnInit(): void {
    this.getProjectProductStatusPerform();
    this.getNumberErrorOfProject();
    this.GetWorkOfProject();
    this.GetRatioOfProject();
    this.GetImplementationPlanVersusReality();
    this.GetNumberWorkInProject();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  getProjectProductStatusPerform() {
    this.projectProductService
      .getProjectProductStatusPerform(this.idProject)
      .subscribe(
        (data) => {
          this.ProjectPlan = data;
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
  }

  //tổng hợp vấn đề tồn đọng của dự án
  getNumberErrorOfProject() {
    this.projectProductService
      .getNumberErrorOfProject(this.idProject)
      .subscribe((data) => {
        if (data) {
          this.barChartData2[0].data = [
            data.RiskNoAction,
            data.Done,
            data.Implementation,
            data.NoImplementation,
          ];
          this.barChartOptions2 = {
            responsive: true,
            legend: {
              display: false,
            },
            scales: {
              xAxes: [
                {
                  ticks: {
                    beginAtZero: true,
                    precision: 0,
                    min: 0,
                    max:
                      parseInt(data.RiskNoAction ?? 0) +
                      parseInt(data.DoneRatio ?? 0) +
                      parseInt(data.Implementation ?? 0) +
                      parseInt(data.NoImplementation ?? 0),
                    fontColor: '#000000	'
                  },
                },
              ],
            },
          };
          this.charts?.toArray()[2]?.chart.update();
          this.TotalError =parseInt(data.RiskNoAction ?? 0) +
          parseInt(data.DoneRatio ?? 0) +
          parseInt(data.Implementation ?? 0) +
          parseInt(data.NoImplementation ?? 0);
        }
      });
  }
  //Tổng hợp tình trạng công việc của dự án
  GetWorkOfProject() {
    this.projectProductService
      .GetWorkOfProject(this.idProject)
      .subscribe((data) => {
        if (data) {
          this.barChartData1[0].data = [
            data.WorkDone,
            data.WorkImplement,
            data.WorkNoImplement,
          ];
          this.barChartOptions1 = {
            responsive: true,
            legend: {
              display: false,
            },
            scales: {
              xAxes: [
                {
                  ticks: {
                    beginAtZero: true,
                    precision: 0,
                    min: 0,
                    max:
                      parseInt(data.WorkDone ?? 0) +
                      parseInt(data.WorkImplement ?? 0) +
                      parseInt(data.WorkNoImplement ?? 0),
                    fontColor: '#000000	'
                  },
                },
              ],
            },
          };
          this.charts?.toArray()[1]?.chart.update();
          this.TotalWork = parseInt(data.WorkDone ?? 0) +
          parseInt(data.WorkImplement ?? 0) +
          parseInt(data.WorkNoImplement ?? 0);
        }
      });
  }
  //Tỷ lệ hoàn thành dự án
  GetRatioOfProject() {
    this.projectProductService
      .GetRatioOfProject(this.idProject)
      .subscribe((data) => {
        if (data) {
          this.doughnutChartData = [
            data.Done,
            data.Implementation,
            data.NoImplementation,
          ];
        }
      });
  }

  //Kế hoạch thực hiện so với thực tế
  GetImplementationPlanVersusReality() {
    this.projectProductService
      .GetImplementationPlanVersusReality(this.idProject)
      .subscribe((data) => {
        if (data) {
          this.ListStageOfProject = data.PlanImplementRealities;
          this.DoneRatioOfProject = data.DoneRatio;
        }
      });
  }

  //đếm số lượn công việc
  GetNumberWorkInProject() {
    this.projectProductService
      .GetNumberWorkInProject(this.idProject)
      .subscribe((data) => {
        if (data) {
          this.WorkLate = data.WorkLate;
          this.WorkEmptyStart = data.WorkEmptyStart;
          this.WorkEmptyEnd = data.WorkEmptyEnd;
          this.WorkIncurred = data.WorkIncurred;
        }
      });
  }
}
