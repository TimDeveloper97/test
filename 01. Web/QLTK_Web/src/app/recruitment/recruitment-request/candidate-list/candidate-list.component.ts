import { Component, OnInit, Input , ViewChild, ElementRef} from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { ApplyService } from '../../services/apply.service';
import { CandidateService } from '../../services/candidate.service';

@Component({
  selector: 'app-candidate-list',
  templateUrl: './candidate-list.component.html',
  styleUrls: ['./candidate-list.component.scss']
})
export class CandidateListComponent implements OnInit {

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
  };

  

  candidates: any[] = []
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
    private candidateService: CandidateService,
    private activeModal: NgbActiveModal,
    private dateUtils: DateUtils) { }

  ngOnInit(): void {

    this.appSetting.PageTitle = "Hồ sơ Ứng viên";
    this.searchCandidate();
  }

  searchCandidate() {
    this.candidateService.searchCandidatesByRecruitmentRequestId(this.Id).subscribe((data: any) => {
      if (data.ListResult) {
        this.candidates = data.ListResult;
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
      Name: '',
      PhoneNumber: "",
      WorkTypeId: '',
      Email: '',
      ApplyDateTo: null,
      ApplyDateFrom: null,
      InterviewDateTo: null,
      InterviewDateFrom: null,
      ApplyDateToV: null,
      ApplyDateFromV: null,
      InterviewDateToV: null,
      InterviewDateFromV: null
    };
    this.searchCandidate();
  }

  showCreateUpdate() {
    this.router.navigate(['tuyen-dung/ho-so-ung-vien/them-moi']);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
