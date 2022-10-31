import { Component, OnInit } from '@angular/core';
import { ForecastProjectsService } from '../service/forecast-projects.service';
import { AppSetting, MessageService, Constants } from 'src/app/shared';

@Component({
  selector: 'app-forecast-projects',
  templateUrl: './forecast-projects.component.html',
  styleUrls: ['./forecast-projects.component.scss']
})
export class ForecastProjectsComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    private service: ForecastProjectsService,
  ) { }

  listProject: any[] = [];
  listData: any[] = [];
  total: number;
  totalUnfinishedPlanAll: number;
  totalError: number;
  totalErrorNoSolution: number;
  totalErrorCompareDateline: number;
  ngOnInit() {
    this.appSetting.PageTitle = "Dự báo dự án tương lai";
    this.getForecastProjects()
  }

  getForecastProjects() {
    this.service.getForecastProjects().subscribe((data: any) => {
      this.listProject = data.ListProject;
      this.total = data.Total;
      this.totalUnfinishedPlanAll = data.TotalUnfinishedPlanAll;
      this.totalError = data.TotalError;
      this.totalErrorNoSolution = data.TotalErrorNoSolution;
      this.totalErrorCompareDateline = data.TotalErrorCompareDateline;
    }, error => {
      this.messageService.showError(error);
    });
  }

}
