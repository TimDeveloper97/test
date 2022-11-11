import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants } from 'src/app/shared';
import { ScheduleProjectService } from '../../service/schedule-project.service';

@Component({
  selector: 'app-plan-copy',
  templateUrl: './plan-copy.component.html',
  styleUrls: ['./plan-copy.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PlanCopyComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    private scheduleProjectService: ScheduleProjectService,
  ) { }

  @ViewChild('scrollPlanCopy') scrollPlanCopy: ElementRef;
  @ViewChild('scrollPlanCopyHeader') scrollPlanCopyHeader: ElementRef;

  planId: string;
  listIdUserId: any[] = [];
  listHeard: any[] = [];
  listData: any[] = [];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];

  id: string;
  userId: string;
  isMain: boolean = false

  model: any = {}

  modelEmployee: any = {
    UserId: null,
    IsMain: false
  }

  ngOnInit() {
    this.getListEmployee();
  }

  ngAfterViewInit() {
    this.scrollPlanCopy.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPlanCopyHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPlanCopy.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  getListEmployee() {
    this.scheduleProjectService.getDataCopy(this.model).subscribe((data: any) => {
      if (data) {
        this.listHeard = data.ListHeard;
        this.listData = data.ListData;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    this.model.ListPlanAdjustment = this.listData;
    this.scheduleProjectService.updatePlanAssignment(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật người phụ trách thành công!');
        this.activeModal.close(true);
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }
}
