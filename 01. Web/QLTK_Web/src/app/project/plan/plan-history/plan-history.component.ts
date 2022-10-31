import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting, Configuration } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { PlanService } from '../../service/plan.service';
import { ScheduleProjectService } from '../../service/schedule-project.service';

@Component({
  selector: 'app-plan-history',
  templateUrl: './plan-history.component.html',
  styleUrls: ['./plan-history.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PlanHistoryComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constant: Constants,
    private router: Router,
    private config: Configuration,
    private planService: PlanService,
    private downloadService: DownloadService,
    private scheduleProjectService: ScheduleProjectService,
  ) { }

  projectId: any;
  listData: any[] = [];
  Status: any[] = [
    { Id: false, Name: "Chưa được chấp nhận" },
    { Id: true, Name: "Đã được chấp nhận" }
  ]

  ngOnInit() {
    this.getListPlanHistory();
  }

  getListPlanHistory() {
    this.planService.getListPlanHistory(this.projectId).subscribe(data => {
      this.listData = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  downloadAFile(row: any) {
    // this.getAttachProject(Id);
    if (row.ListDatashet.length > 0) {
      if (row.ListDatashet.length == 1) {
        this.fileProcess.downloadFileBlob(row.ListDatashet[0].Path, row.ListDatashet[0].FileName);
      } else {
        this.downloadService.downloadAll({ Name: "LichSuThayDoi_KeHoachDuAn_Version_" + row.Version, ListDatashet: row.ListDatashet }).subscribe(data => {
          var link = document.createElement('a');
          link.setAttribute("type", "hidden");
          link.href = this.config.ServerFileApi + data.PathZip;
          link.download = 'Download.zip';
          document.body.appendChild(link);
          // link.focus();
          link.click();
          document.body.removeChild(link);
        }, e => {
          this.messageService.showError(e);
        });
      }
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }

  exportExcelProjectPlan(id: string) {
    this.scheduleProjectService.exportExcelProjectPlan({ ProjectId: this.projectId, IsHistory: true, PlanHistoryId: id }).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showView(Id: string) {
    this.router.navigate(['du-an/lich-su-thay-doi-gia-han-hop-dong', Id]);
  }
}
