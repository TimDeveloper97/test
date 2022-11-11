import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, MessageService, DateUtils, Constants } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { ApplyService } from '../../services/apply.service';
@Component({
  selector: 'app-candidate-passed',
  templateUrl: './candidate-passed.component.html',
  styleUrls: ['./candidate-passed.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CandidatePassedComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private applyService: ApplyService,
    public dateUtils: DateUtils,
    public constant: Constants,
  ) { }

  modalInfo = {
    Title: 'Thêm mới vị trí ứng tuyển.',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;
  applyDate: any = null;
  candidateapplyId: string;
  recruitmentRequests: any[] = [];
  listWorkType: any[] = [];

  model: any = {
    Id: '',
    Quantity: 1,
    Pricing: 0,
    customerRequirementId: '',
    WorkTypes: [],
    RecruitmentRequestId: '',
    WorkTypeId: '',
    ApplyDate: '',
    ProfileStatus:0,
  }
  ListMaterial: any = [];
  workTypes: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' },];
  columnNameRequest: any[] = [{ Name: 'Name', Title: 'Mã yêu cầu tuyển dụng' }];

  ngOnInit() {
    this.model.CandidateId = this.id;
    this.modalInfo.SaveText = 'Lưu';

    this.getAllRecruitmentRequests();
    this.getCbbWorkType();

    if(this.candidateapplyId){
      this.getCandidateApplyById();
      this.modalInfo.Title = 'Sửa vị trí ứng tuyển';
    }

  }

  getCandidateApplyById() {
    this.applyService.getCandidateApplyById(this.candidateapplyId).subscribe(
      data => {
        this.model = data;
        if (this.model.ApplyDate) {
          this.applyDate = this.dateUtils.convertDateToObject(this.model.ApplyDate);
        }
        this.getSalaryMaxMin();
        this.getSalaryRecruitmentRequest();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getAllRecruitmentRequests() {
    this.comboboxService.getAllCbbRecruitmentRequest().subscribe(
      data => {
        this.recruitmentRequests = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getSalaryMaxMin() {
    this.applyService.getSalaryLevelById(this.model.WorkTypeId).subscribe(
      data => {
        this.model.MinCompanySalary = data.SalaryLevelMin;
        this.model.MaxCompanySalary = data.SalaryLevelMax;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }



  getSalaryRecruitmentRequest() {
    this.applyService.getSalaryByRequestId(this.model.RecruitmentRequestId).subscribe(
      data => {
        this.model.MinSalary = data.MinSalary;
        this.model.MaxSalary = data.MaxSalary;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbData() {
    let cbbWorkType = this.comboboxService.getListWorkType();

    forkJoin([cbbWorkType]).subscribe(results => {
      this.workTypes = results[0];
    });
  }


  create(isContinue) {
    this.applyService.createCandidateApply(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm hồ sơ ứng tuyển thành công!');
        this.closeModal(true);
        
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.applyService.updateCandidateApply(this.candidateapplyId, this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật hồ sơ ứng tuyển thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  


  save(isContinue: boolean) {
    if (this.applyDate != null && this.applyDate != '') {
      this.model.ApplyDate = this.dateUtils.convertObjectToDate(this.applyDate)
    }
    if(this.candidateapplyId){
      this.update();
    }else{
    this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  getCbbWorkType() {
    this.comboboxService.getListWorkType().subscribe(
      data => {
        this.listWorkType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeRequest() {
    this.getSalaryRecruitmentRequest();
    if (this.model.RecruitmentRequestId) {
      this.applyService.getWorkTypeByRequestId(this.model.RecruitmentRequestId).subscribe(
        data => {
          this.model.WorkTypeId = data;
          this.getSalaryMaxMin();
          
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.model.WorkTypeId = null;
    }
  }

  

}
