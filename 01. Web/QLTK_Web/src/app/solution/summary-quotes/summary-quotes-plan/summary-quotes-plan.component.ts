import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from '../../service/summary-quotes.service';
import { ShowSummaryQuotesPlanCreateComponent } from './show-summary-quotes-plan-create/show-summary-quotes-plan-create.component';
import { ShowSummaryQuotesPlanUpdateComponent } from './show-summary-quotes-plan-update/show-summary-quotes-plan-update.component';
declare var $: any;
@Component({
  selector: 'app-summary-quotes-plan',
  templateUrl: './summary-quotes-plan.component.html',
  styleUrls: ['./summary-quotes-plan.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SummaryQuotesPlanComponent implements OnInit {
  constructor(
    private messageService: MessageService,
    public appSetting: AppSetting,
    public constant: Constants,
    private service: SummaryQuotesService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
  ) { }

  Id: string;
  nameFile = "";
  fileToUpload: File = null;
  listData: [];
  startIndex = 1;
  selectedRowKey: any[] = [];
  DepartmentName: string;

  @ViewChild('scrollPlan', { static: false }) scrollPlan: ElementRef;
  @ViewChild('scrollPlanHeader', { static: false }) scrollPlanHeader: ElementRef;
  ngOnInit(): void {
    this.Id = this.route.snapshot.paramMap.get('Id');
    this.service.getQuotationById(this.Id ).subscribe(data => {
      this.appSetting.PageTitle = 'Chỉnh sửa báo giá' + ' - '  + data.data.Code;
      this.DepartmentName = data.departmentName.Name;
    });
    this.getQuotationPlan(this.Id);
  }

  getQuotationPlan(Id: string){
    this.service.getQuotationPlan(Id).subscribe(
      data => {
        this.listData = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showCreate(StepInQuotationId: string){
    let activeModal = this.modalService.open(ShowSummaryQuotesPlanCreateComponent, { container: 'body', windowClass: 'show-summary-quotes-plan-create-model', backdrop: 'static' })
    activeModal.componentInstance.QuotationId = this.Id;
    activeModal.componentInstance.StepInQuotationId = StepInQuotationId;
    activeModal.result.then((result) => {
        this.getQuotationPlan(this.Id);
    },)
  }

  showUpdate(QuotationPlanId: string){
    let activeModal = this.modalService.open(ShowSummaryQuotesPlanUpdateComponent, { container: 'body', windowClass: 'show-summary-quotes-plan-update-model', backdrop: 'static' })
    activeModal.componentInstance.QuotationPlanId = QuotationPlanId;
    activeModal.result.then((result) => {
        this.getQuotationPlan(this.Id);
    },)
  }

  showConfirmDelete(QuotationPlanId: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá công việc này không?").then(
      data => {
        this.delete(QuotationPlanId);
      },
      error => {

      }
    );
    
  }
  delete(QuotationPlanId: string) {
    this.service.DeleteQuotation(QuotationPlanId).subscribe(
      data => {
        this.getQuotationPlan(this.Id);
        this.messageService.showSuccess('Xóa công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
