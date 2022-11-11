import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { CandidateService } from '../../services/candidate.service';
import { CandidatePassedComponent } from "../candidate-passed/candidate-passed.component";

@Component({
  selector: 'app-candidate-apply-tab',
  templateUrl: './candidate-apply-tab.component.html',
  styleUrls: ['./candidate-apply-tab.component.scss']
})
export class CandidateApplyTabComponent implements OnInit {
  @Input() candidateId: string;
  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private router: Router,
    public fileProcess: FileProcess,
    public constant: Constants,
    private comboboxService: ComboboxService,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    private candidateService: CandidateService) { }

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
  };
  applys: any[] = [];

  ngOnInit(): void {
    this.getApplys() ;
  }

  getApplys() {
    this.candidateService.getApplys(this.candidateId).subscribe(
      data => {
        this.applys = data; 
        this.searchModel.TotalItems = this.applys.length;     
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showCreateUpdate() {

    let activeModal = this.modalService.open(CandidatePassedComponent, { container: 'body', windowClass: 'candidate-passed-model', backdrop: 'static' })
    activeModal.componentInstance.id = this.candidateId;
    activeModal.result.then((result) => {
      if (result) {
        this.getApplys();
      }
    }, (reason) => {
    });
  }

  showUpdate(Id : string) {

    let activeModal = this.modalService.open(CandidatePassedComponent, { container: 'body', windowClass: 'candidate-passed-model', backdrop: 'static' })
    activeModal.componentInstance.id = this.candidateId;
    activeModal.componentInstance.candidateapplyId = Id;

    activeModal.result.then((result) => {
      if (result) {
        this.getApplys();
      }
    }, (reason) => {
    });
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['tuyen-dung/ung-tuyen']);
  }

}
