import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils, MessageService, Constants } from 'src/app/shared';
import { ShowChooseEmployeeComponent } from 'src/app/sale/sale-group/show-choose-employee/show-choose-employee.component';
import { SelectMaterialComponent } from 'src/app/education/classroom/select-material/select-material/select-material.component';
import { ProjectProductService } from '../../service/project-product.service';

@Component({
  selector: 'app-copy-qc-check-list',
  templateUrl: './copy-qc-check-list.component.html',
  styleUrls: ['./copy-qc-check-list.component.scss']
})
export class CopyQcCheckListComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    public constant: Constants,
    private service: ProjectProductService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới lượt khảo sát',
    SaveText: 'Lưu',
  };
  listRequestId: any[] = [];
  listUserId: any[] = [];
  listProjectProductId: any[] = [];
  listCreate: any[] = [];
  listExpert: any[] = [];
  Survey: any[] = [];
  listSurveyTool: any[] = [];
  listDA: any[] = [];
  isAction: boolean = false;
  Id: string;
  row: any = {};
  check: any [] = []
  isAdd = false;
  projectId : string;
  projectProductId : string;

  model: any = {
    Id: '',
    ProjectId: '',
    ProjectProductId:'',
    ListProjectProductId : [],
    ListCheck: [],
  }

  userModel: any = {
    UserId:'',
    SurveyId: '',
  }

  ChangeStepModel = {
    Id: ''
  }
  listData: any[] = []
  listTemp: any[] = [];
  listSurvey: any[] = [];
  surveyDateView = null;



  ngOnInit() {
    this.model.ListCheck = this.check;
    this.model.ProjectId = this.projectId;
    this.model.ProjectProductId = this.projectProductId;
      this.ModalInfo.Title = 'Sao chép tiêu chuẩn';
      this.ModalInfo.SaveText = 'Lưu';
      this.getById();

  }
  getById(){
    // this.service.searchProjectProduct(this.model).subscribe((data: any) => {
    //   if (data.ListResult) {
    //     this.listExpert = data.ListResult;
    //     this.listExpert.forEach(element => {
          
    //         element.IsCheck = false;
    //         this.listDA.push(element);
          
    //     });
    //   }
    // },
    //   error => {
    //     this.messageService.showError(error);
    //   });
      this.service.GetProjectProductCopyQC(this.model).subscribe((data: any) => {
        if (data) {
          this.listExpert = data;
          this.listExpert.forEach(element => {
            
              element.IsCheck = false;
              this.listDA.push(element);
            
          });
        }
      },
        error => {
          this.messageService.showError(error);
        });
  }

  save(){
    this.listDA.forEach(element => {
      if(element.IsCheck){
        this.model.ListProjectProductId.push(element.Id)
      }
    });
    this.service.copyQCCheckList(this.model).subscribe(
      data => {

          this.messageService.showSuccess('Sao chép thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }




  closeModal(isOK: boolean) {
    this.activeModal.close({ isAdd: true, modelTemp: this.listCreate });
  }
  onEditingStart(e){
      e.cancel = !e.data.IsNeedQC;
      if(this.projectProductId==e.data.Id){
        e.cancel = true;
      }
  }

 

}
