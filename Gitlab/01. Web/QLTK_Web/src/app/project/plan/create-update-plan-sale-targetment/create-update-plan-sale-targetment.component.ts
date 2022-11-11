import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ProjectPaymentService } from '../../service/project-payment.service';
import { ScheduleProjectService } from '../../service/schedule-project.service';

@Component({
  selector: 'app-create-update-plan-sale-targetment',
  templateUrl: './create-update-plan-sale-targetment.component.html',
  styleUrls: ['./create-update-plan-sale-targetment.component.scss']
})
export class CreateUpdatePlanSaleTargetmentComponent implements OnInit {

  PlanId :string;
  ProjectId :string;
  constructor(
    private messageService: MessageService,
    public constant: Constants,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    private paymentService : ProjectPaymentService
  ) { }
  ModalInfo = {
    Title: 'Chọn tiến độ thu tiền công việc',
    SaveText: 'Lưu',
  };
  isAction: boolean = false;
  listPayment : any[] = []
  Payments = [{ Name: 'Name', Title: 'Tên lần thanh toán' }];
  model: any = {
    PlanId :'',
    PaymentId :''
  }
  ngOnInit(): void {
    this.model.PlanId = this.PlanId;
    this.GetAllPayments(this.ProjectId);
    this.GetPaymentByPlanId(this.model.PlanId);
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
  save() {
    this.paymentService.UpdatePlanPayment(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật thành công!');
        this.paymentService.UpdatePlanDate(this.model).subscribe(
          data => {
          },
          error => {
            this.messageService.showError(error);
          });
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      });
  }
  GetAllPayments(projectId : string){
    this.paymentService.SearchPayment(projectId).subscribe(
      data => {
          this.listPayment =data.PaymentModels;
      },
      error => {
        this.messageService.showError(error);
      });
  }
  GetPaymentByPlanId(PlanId : string){
    this.paymentService.GetPaymentByPlanId(PlanId).subscribe(
      data => {
        if(data !=null){
          this.model =data;
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
