import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportApplicationPresentService } from '../service/report-application-present.service';

@Component({
  selector: 'app-report-application-present',
  templateUrl: './report-application-present.component.html',
  styleUrls: ['./report-application-present.component.scss']
})
export class ReportApplicationPresentComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public serviceReportApplicationPresent: ReportApplicationPresentService

  ) { }

  ngOnInit() {
    this.a = new Date()
    this.model.Year = this.a.getFullYear();
    this.appSetting.PageTitle = "Báo cáo theo ngành hàng, ứng dụng hiện tại";
    this.loadYear();
    this.GetReportApplicationPresent();
  }
  a = null;
  model = {
    Year: '',
  }
  TotalProjectToNotFinish: number;
  TotalProjectFinish: number;

  TotalSolutionUse: number;
  TotalSolutionToProject: number;

  TotalSolutionNotToProject: number;
  TotalSolutionStop: number;
  TotalSolutionCancel: number;

  list_Project: any[] = [];
  list_Solution: any[] = [];
  listYear: any[] = [];

  GetReportApplicationPresent() {
    this.serviceReportApplicationPresent.SearchApplicationPresent(this.model).subscribe((data: any) => {
      this.TotalProjectToNotFinish = data.TotalProjectToNotFinish;
      this.TotalProjectFinish = data.TotalProjectFinish;

      this.TotalSolutionUse = data.TotalSolutionUse;
      this.TotalSolutionToProject = data.TotalSolutionToProject;

      this.TotalSolutionNotToProject = data.TotalSolutionNotToProject;
      this.TotalSolutionStop = data.TotalSolutionStop;
      this.TotalSolutionCancel = data.TotalSolutionCancel;

      this.list_Solution = data.Solutions;
      this.list_Project = data.Projects;

    });
  }

  loadYear() {
    for (var year = 2017; year <= new Date().getFullYear(); year++) {
      this.listYear.push(year);
    }
  }
}
