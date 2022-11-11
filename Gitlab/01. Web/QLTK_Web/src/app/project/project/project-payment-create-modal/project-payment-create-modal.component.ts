import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils, MessageService } from 'src/app/shared';
import { ProjectPaymentService} from 'src/app/project/service/project-payment.service';

@Component({
  selector: 'app-project-payment-create-modal',
  templateUrl: './project-payment-create-modal.component.html',
  styleUrls: ['./project-payment-create-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectPaymentCreateModalComponent implements OnInit {

  TotalPlanAmount : 0;
  constructor(
    private activeModal: NgbActiveModal,
    private dateUtils: DateUtils,
    private service: ProjectPaymentService,
    public messageService: MessageService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới thanh toán',
    SaveText: 'Lưu',
  };

  ProjectId :string;
  model: any = {

    Id: '',
    Name:'',
    ProjectId:'',
    PlanPaymentDate: null,
    PlanAmount:'',
    ActualPaymentDate1: null,
    ActualAmount1:'',
    ActualPaymentDate2: null,
    ActualAmount2:'',
    ActualPaymentDate3: null,
    ActualAmount3:'',
    ActualPaymentDate4: null,
    ActualAmount4:'',
    ActualPaymentDate5: null,
    ActualAmount5:'',
    PaymentCondition:'',
    PaymentMilestone: null,
    MoneyCollectionTime: '',
    AdjustmentDate: null
  }

   //#region
   TotalWithPaymentModel:{
    PaymentModels : [],
    TotalPlanAmount: 0,
    ActualPlanAmount:0,
  }

  Id: any;
  ngOnInit(): void {
    if(this.Id){
      this.ModalInfo.Title='Chỉnh sửa thông tin';
      this.getPaymentInfor();
    }else{
      this.ModalInfo.Title = "Thêm thanh toán";
    }
  }

  getPaymentInfor() {
    this.service.SearchPaymentById({ Id: this.Id }).subscribe(data => {
      this.model = data;
      if (this.model.AdjustmentDate) {
        this.model.AdjustmentDate = this.dateUtils.convertDateToObject(this.model.AdjustmentDate);
      }

      if (this.model.PaymentMilestone) {
        this.model.PaymentMilestone = this.dateUtils.convertDateToObject(this.model.PaymentMilestone);
      }

      if (this.model.PlanPaymentDate) {
        this.model.PlanPaymentDate = this.dateUtils.convertDateToObject(this.model.PlanPaymentDate);
      }
     
      if (this.model.ActualPaymentDate1) {
        this.model.ActualPaymentDate1 = this.dateUtils.convertDateToObject(this.model.ActualPaymentDate1);
      }
      
      if (this.model.ActualPaymentDate2) {
        this.model.ActualPaymentDate2 = this.dateUtils.convertDateToObject(this.model.ActualPaymentDate2);
      }
      
      if (this.model.ActualPaymentDate3) {
        this.model.ActualPaymentDate3 = this.dateUtils.convertDateToObject(this.model.ActualPaymentDate3);
      }
      
      if (this.model.ActualPaymentDate4) {
        this.model.ActualPaymentDate4 = this.dateUtils.convertDateToObject(this.model.ActualPaymentDate4);
      }
      
      if (this.model.ActualPaymentDate5) {
        this.model.ActualPaymentDate5 = this.dateUtils.convertDateToObject(this.model.ActualPaymentDate5);
      }
    });
  }

  getPayment() {
    this.service.SearchPayment(this.model.ProjectId).subscribe(
      data => {
        this.TotalPlanAmount = data.TotalPlanAmount;
        this.TotalWithPaymentModel=data;

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save() {
    this.createPayment();
  }

  createPayment(){
    if (this.model.AdjustmentDate) {
      this.model.AdjustmentDate = this.dateUtils.convertObjectToDate(this.model.AdjustmentDate);
    }
    if (this.model.PaymentMilestone) {
      this.model.PaymentMilestone = this.dateUtils.convertObjectToDate(this.model.PaymentMilestone);
    }
    if (this.model.PlanPaymentDate) {
      this.model.PlanPaymentDate = this.dateUtils.convertObjectToDate(this.model.PlanPaymentDate);
    }
    if (this.model.ActualPaymentDate1) {
      this.model.ActualPaymentDate1 = this.dateUtils.convertObjectToDate(this.model.ActualPaymentDate1);
    }
    if (this.model.ActualPaymentDate2) {
      this.model.ActualPaymentDate2 = this.dateUtils.convertObjectToDate(this.model.ActualPaymentDate2);
    }

    if (this.model.ActualPaymentDate3) {
      this.model.ActualPaymentDate3 = this.dateUtils.convertObjectToDate(this.model.ActualPaymentDate3);
    }

    if (this.model.ActualPaymentDate4) {
      this.model.ActualPaymentDate4 = this.dateUtils.convertObjectToDate(this.model.ActualPaymentDate4);
    }

    if (this.model.ActualPaymentDate5) {
      this.model.ActualPaymentDate5 = this.dateUtils.convertObjectToDate(this.model.ActualPaymentDate5);
    }

    this.model.ProjectId=this.ProjectId;
    this.service.UpdatePayment(this.model).subscribe(
      data =>{
        this.getPayment();
        this.activeModal.close(true);
      },
      error =>{
      }
    );
  }
  closeModal() {
    this.activeModal.close();
  }

}
