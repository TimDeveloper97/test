import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, TiviComboboxService } from 'src/app/shared';

@Component({
  selector: 'app-tivi-report-error-list',
  templateUrl: './tivi-report-error-list.component.html',
  styleUrls: ['./tivi-report-error-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TiviReportErrorListComponent implements OnInit {

  constructor(
    private comboboxService: TiviComboboxService,
    public appSetting: AppSetting,) { }

  departments: any[] = [];

  ngOnInit(): void {
    this.appSetting.PageTitle = "Báo cáo Vấn đề tồn đọng theo phòng ban";
    this.getDepartments();
  }

  getDepartments() {
    this.comboboxService.getCbbDepartmentUse().subscribe((data: any) => {
      this.departments = data;
    });
  }

}
