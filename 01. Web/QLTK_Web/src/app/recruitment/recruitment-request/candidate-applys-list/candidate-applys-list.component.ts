import { Component, OnInit, Input , ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { ApplyService } from '../../services/apply.service';

@Component({
  selector: 'app-candidate-applys-list',
  templateUrl: './candidate-applys-list.component.html',
  styleUrls: ['./candidate-applys-list.component.scss']
})
export class CandidateApplysListComponent implements OnInit {

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,

    Name: '',
    PhoneNumber: "",
    WorkTypeId: '',
    Email: '',
    ApplyDateTo: null,
    ApplyDateFrom: null,
    ApplyDateToV: null,
    ApplyDateFromV: null,
    Status: null,
    ProfileStatus: null,
    InterviewStatus: null,
  };


  applys: any[] = []
  startIndex: number = 1;
  isAction: boolean = false;
  @Input() Id: string;
  @ViewChild('fileInputDocument') inputFile: ElementRef

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,
    private modalService: NgbModal,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private applyService: ApplyService,
    private activeModal: NgbActiveModal,
    private dateUtils: DateUtils) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý ứng tuyển";
    this.searchApplys();
  }

  searchApplys() {
    this.applyService.searchApplysByRecruitmentRequestId(this.Id).subscribe((data: any) => {
      if (data.ListResult) {
        this.applys = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,

      Name: null,
      PhoneNumber: null,
      WorkTypeId: null,
      Email: null,
      ApplyDateTo: null,
      ApplyDateFrom: null,
      InterviewDateTo: null,
      InterviewDateFrom: null,
      ApplyDateToV: null,
      ApplyDateFromV: null,
      InterviewDateToV: null,
      InterviewDateFromV: null
    };
    this.searchApplys();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
