import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Configuration, FileProcess, ComboboxService, DateUtils, Constants } from 'src/app/shared';
import { ProjectEmployeeService } from '../../service/project-employee.service';
@Component({
  selector: 'app-project-employee-update-status-subsidy-history',
  templateUrl: './project-employee-update-status-subsidy-history.component.html',
  styleUrls: ['./project-employee-update-status-subsidy-history.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectEmployeeUpdateStatusSubsidyHistoryComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private dateUtil: DateUtils,
    private messageService: MessageService,
    private service: ProjectEmployeeService,
  ) { }

  model: any = {
    Id: '',
    Subsidy: '',
    SubsidyStartTime: null,
    SubsidyEndTime: null
  }

  SubsidyModel: any = null;
  subsidyStartTime: any = null;
  subsidyEndTime: any = null;
  startTime: any = null;
  endTime: any = null;
  listRole = [];
  DescriptionRole = '';

  // subsidyStartTimeUpdate: any = null;
  // subsidyEndTimeUpdate: any = null;
  subsidyUpdate: any;
  id: any;
  StartIndex = 1;
  OldId = '';


  ngOnInit(): void {
    this.SubsidyModel = this.SubsidyModel;
    this.OldId = this.SubsidyModel.RoleId;
    if (this.SubsidyModel.SubsidyStartTime) {
      this.subsidyStartTime = this.dateUtil.convertDateToObject(this.SubsidyModel.SubsidyStartTime);
    }
    if (this.SubsidyModel.SubsidyEndTime) {
      this.subsidyEndTime = this.dateUtil.convertDateToObject(this.SubsidyModel.SubsidyEndTime);
    }
    if (this.SubsidyModel.StartTime) {
      this.startTime = this.dateUtil.convertDateToObject(this.SubsidyModel.StartTime);
    }
    if (this.SubsidyModel.EndTime) {
      this.endTime = this.dateUtil.convertDateToObject(this.SubsidyModel.EndTime);
    }

    console.log(this.SubsidyModel.Id);
    this.id = this.SubsidyModel.Id;
    this.getSubsidyHistory();
    this. getRole();
  }

  listSubsidyHistory: any[] = [];
  getSubsidyHistory() {
    this.service.getSubsidyHistory(this.id).subscribe(list => {
      if (list) {
        this.listSubsidyHistory = list;
        console.log(list);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  

  update(){
    if(this.subsidyStartTime){
      this.SubsidyModel.SubsidyStartTime = this.dateUtil.convertObjectToDate(this.subsidyStartTime)
    }
    if(this.subsidyEndTime){
      this.SubsidyModel.SubsidyEndTime = this.dateUtil.convertObjectToDate(this.subsidyEndTime)
    }
    if(this.startTime){
      this.SubsidyModel.StartTime = this.dateUtil.convertObjectToDate(this.startTime)
    }
    if(this.endTime){
      this.SubsidyModel.EndTime = this.dateUtil.convertObjectToDate(this.endTime)
    }
    if(this.SubsidyModel.RoleId != this.OldId){
      this.SubsidyModel.JobDescription = this.SubsidyModel.JobDescription;
    }
    // 
    this.service.updateSubsidyPE(this.SubsidyModel).subscribe(result=>{
      this.messageService.showSuccess('Cập nhật thành công!');
      this.activeModal.close(this.SubsidyModel);
    })
  }

  save(){
    if(this.subsidyStartTime){
      this.SubsidyModel.SubsidyStartTime = this.dateUtil.convertObjectToDate(this.subsidyStartTime)
    }
    if(this.subsidyEndTime){
      this.SubsidyModel.SubsidyEndTime = this.dateUtil.convertObjectToDate(this.subsidyEndTime)
    }
    this.service.AddSubsidyHistory(this.SubsidyModel).subscribe( result =>{
     this.getSubsidyHistory();
      this.messageService.showSuccess('Thêm lịch sử thành công!');
    })
  }

  closeModal() {
    this.activeModal.close(this.SubsidyModel);
  }

  getRole(){
    this.service.getRole().subscribe(data => {
      if (data) {
        this.listRole = data;
        console.log("Lấy list role: "+data);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  changeRole(RoleId: any){
    this.service.getDescriptionRoleById(RoleId).subscribe((data: any) => {
      if (data) {
        this.DescriptionRole = data.Description;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
}
