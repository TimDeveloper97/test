import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Constants, DateUtils, FileProcess, MessageService, Configuration } from 'src/app/shared';
import { ApplyService } from '../../services/apply.service';
import { InterviewService } from '../../services/interview.service';
import { MoreInterviewsComponent} from '../more-interviews/more-interviews.component'
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { NgBlockUI, BlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-apply-interview-tab',
  templateUrl: './apply-interview-tab.component.html',
  styleUrls: ['./apply-interview-tab.component.scss']
})
export class ApplyInterviewTabComponent implements OnInit {

  @BlockUI() blockUI: NgBlockUI;

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private router: Router,
    public fileProcess: FileProcess,
    public constant: Constants,
    private comboboxService: ComboboxService,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    private applyService: ApplyService,
    private config: Configuration,
    private signalRService: SignalRService,
    ) { }

  @Input() applyId: string;
  interviewModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
  };
  interviews: any[] = [];modelTest: any = {
    PathFileMaterial: '',
    Module: '',
    SelectedPath: '',
    ApiUrl: '',
    PathDownload: '',
    ModuleCode: '',
    FileName: '',
    ApplyId: '',
    Type: Number
  }

  ngOnInit(): void {
    this. searchInterview();
  }

  searchInterview() {
    this.applyService.getInterviews(this.applyId).subscribe(
      data => {
        this.interviews = data;    
        this.interviewModel.TotalItems= this.interviews.length;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showView(id) {
    this.router.navigate(['tuyen-dung/ho-so-ung-vien/thong-tin-phong-van/'+id]);
  }

  closeModal() {
    this.router.navigate(['tuyen-dung/ung-tuyen']);
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(MoreInterviewsComponent, { container: 'body', windowClass: 'more-interviews-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.ApplyId = this.applyId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchInterview();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa lần phỏng vấn này không?").then(
      data => {
        this.deleteInterview(Id);
      },
      error => {

      }
    );
  }

  deleteInterview(Id: string) {
    this.applyService.delete(Id).subscribe(
      data => {
        this.searchInterview();
        this.messageService.showSuccess('Xóa lần phỏng vấn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  InterviewExport() {
    this.applyService.InterviewExport(this.applyId).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.FileName = "PATK-C.";
        // this.fileProcess.downloadFileBlob(this.config.ServerApi+d, this.modelTest.FileName);
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerApi + d;
        link.download = 'Download.docx';
        document.body.appendChild(link);
        // link.focus();
        link.click();
        document.body.removeChild(link);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  DowloadTemplateToFolder() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('DowloadTemplateToFolder', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }



}
