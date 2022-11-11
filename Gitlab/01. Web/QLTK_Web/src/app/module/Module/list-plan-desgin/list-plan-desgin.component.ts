import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { ListPlanDesginService } from '../../services/list-plan-desgin.service';

@Component({
  selector: 'app-list-plan-desgin',
  templateUrl: './list-plan-desgin.component.html',
  styleUrls: ['./list-plan-desgin.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ListPlanDesginComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private service: ListPlanDesginService
  ) { }

  @Input() ModuleId;
  StartIndex = 0;
  isAction: boolean = false;
  data:any = {FileElectric:false, FileMechanics:false};
  isSelectAll: false;
  listChecker = [];

  modalInfo = {
    Title: 'Danh sách công việc thiết kế chưa hoàn thành',
    SaveText: 'Lưu',
  };

  model = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'NameWork',
    OrderType: true,
  }
  ngOnInit() {
    this.searchModule(this.ModuleId);
  }


  searchModule(moduleId: string) {
    this.service.searchListPlanDesgin(moduleId).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.data = data;

      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  changeCheckAll() {
    if (this.isSelectAll) {
      this.data.ListResult.forEach(element => {
        element.IsChecked = true;
      });
    } else {
      this.data.ListResult.forEach(element => {
        element.IsChecked = false;
      });
    }
    this.listChecker = [];
    this.data.ListResult.forEach(element => {
      if (element.IsChecked == true) {
        this.listChecker.push(element.Id);
      }
    });
  }

  checkParent() {
    this.listChecker = [];
    this.data.ListResult.forEach(element => {
      if (element.IsChecked == true) {
        this.listChecker.push(element.Id);
      }
    });
  }

  updateListStatus(){
    this.service.updateListCheckStatus(this.listChecker).subscribe(
      data => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật trạng thái công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
