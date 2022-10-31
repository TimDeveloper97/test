import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, ComboboxService, DateUtils, AppSetting, Constants } from 'src/app/shared';
import { WorkDiaryService } from '../../service/work-diary.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-work-diary-view',
  templateUrl: './work-diary-view.component.html',
  styleUrls: ['./work-diary-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WorkDiaryViewComponent implements OnInit {

  constructor(
    public dateUtils: DateUtils,
    private workDiaryService: WorkDiaryService,
    private router: Router,
    private routeA: ActivatedRoute,
    private appSetting: AppSetting,
    public contant: Constants,
    private messageService: MessageService
  ) {
  }

  model: any = {};
  workDiaryId = null;

  ngOnInit() {
    this.workDiaryId = this.routeA.snapshot.paramMap.get('Id');
    this.getWorkDiary();
    this.appSetting.PageTitle = 'Xem nhật kí công việc';
  }

  getWorkDiary() {
    this.workDiaryService.getWorkDiaryView(this.workDiaryId).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['nhan-vien/nhat-ky-cong-viec']);
  }
}
