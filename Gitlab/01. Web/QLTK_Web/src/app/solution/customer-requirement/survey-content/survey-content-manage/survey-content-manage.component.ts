import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService, ComboboxService } from 'src/app/shared';
import { Router, ActivatedRoute } from '@angular/router';
import { SurveyContentService } from '../../service/survey-content.service';
import { SurveyContentCreateComponent } from '../survey-content-create/survey-content-create.component';

@Component({
  selector: 'app-survey-content-manage',
  templateUrl: './survey-content-manage.component.html',
  styleUrls: ['./survey-content-manage.component.scss']
})
export class SurveyContentManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private router: Router,
    public constant: Constants,
    private surveyContentService: SurveyContentService,
    private comboboxService: ComboboxService,
    private routeA: ActivatedRoute,
  ) { }

  // searchOptions: any = {
  //   FieldContentName: 'Code', 
  //   Placeholder: 'Tìm kiếm theo tên yêu cầu khách hàng hoặc số YCKH',
  //   Items: [
  //     {
  //       Name: 'Tên khách hàng',
  //       FieldName: 'CustomerId',
  //       Type: 'dropdown',
  //       SelectMode: 'single',
  //       DataType: this.constant.SearchDataType.Customer,
  //       Columns: [{ Name: 'Name', Title: 'Tên khách hàng' }],
  //       DisplayName: 'Name',
  //       ValueName: 'Name',
  //       Placeholder: 'Chọn Tên khách hàng',
  //     },
  //     {
  //       Name: 'Tình trạng',
  //       FieldName: 'Status',
  //       Placeholder: 'Tình trạng ',
  //       Type: 'select',
  //       Data: this.constant.CustomerRequirementStatus,
  //       DisplayName: 'Name',
  //       ValueName: 'Id'
  //     },
  //   ]
  // };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Code: '',
    Name: '',
    Content: '',
    Result: '',
    Status: null
  };

  listRequest: any[] = [];
  listUser: any[] = [];
  listUserId = [];
  userId = '';
  startIndex = 0;

  ngOnInit(): void {
    this.searchModel.SurveyId = this.routeA.snapshot.paramMap.get('Id');
    this.appSetting.PageTitle = "Quản lý nội dung khảo sát";
    this.search();
  }

  search() {
    this.surveyContentService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.searchModel.TotalItems = data.TotalItem;
        this.listRequest = data.ListResult;
        this.listUser = data.listUser;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }


  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nội dung khảo sát này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.surveyContentService.delete(Id).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa nội dung khảo sát thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SurveyContentCreateComponent, { container: 'body', windowClass: 'survey-content-create-modal', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.surveyId = this.searchModel.SurveyId;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Code: '',
      Name: '',
      Status: null
    }
    this.search();

  }
  onSelectionChanged(e: any) {

  }

}
