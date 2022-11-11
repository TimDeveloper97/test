import { Component, OnInit } from '@angular/core';

import { AppSetting, MessageService, Constants, Configuration, ComboboxService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ExpertService } from '../service/expert.service';
import { ExpertCreateComponent } from '../expert-create/expert-create.component';

@Component({
  selector: 'app-expert-manage',
  templateUrl: './expert-manage.component.html',
  styleUrls: ['./expert-manage.component.scss']
})
export class ExpertManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private expertService: ExpertService,
    public constant: Constants,
    public comboboxService: ComboboxService
  ) { }
 
  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    SpecializeId: '',
    DegreeId: '',
    DegreeName: '',
    WorkPlaceId: '',
    Status: true,
    BankName: '',
    BankAccount: '',
    BankAccountName: '',
    ListBank: []
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã chuyên gia',
    Items: [
      {
        Name: 'Tên chuyên gia',
        FieldName: 'Name',
        Placeholder: 'Nhập tên chuyên gia',
        Type: 'text'
      },
      {
        Name: 'Tên tài khoản',
        FieldName: 'BankAccountName',
        Placeholder: 'Nhập tên tài khoản',
        Type: 'text'
      },
      {
        Name: 'Chuyên môn',
        FieldName: 'SpecializeId',
        Placeholder: 'Chuyên môn',
        Type: 'select',
        DataType: this.constant.SearchDataType.Specialize,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Đơn vị công tác',
        FieldName: 'WorkPlaceId',
        Placeholder: 'Đơn vị công tác',
        Type: 'select',
        DataType: this.constant.SearchDataType.WorkPlace,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Trình độ',
        FieldName: 'DegreeId',
        Placeholder: 'Trình độ',
        Type: 'select',
        DataType: this.constant.SearchDataType.Degree,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý Chuyên gia";
    this.searchExpert();
    this.getSpecialize();
    this.getWorkPlace();
    this.getDegree();
  }

  searchExpert() {
    this.expertService.searchExpert(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
      Description: '',
      SpecializeName: '',
      DegreeId: '',
      DegreeName: '',
      WorkplaceName: '',
      Status: true,
      BankName: '',
      BankAccount: '',
      BankAccountName: '',
      ListBank:[],
      WorkPlaceId:'',
      SpecializeId:''
    }
    this.searchExpert();
  }
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ExpertCreateComponent, { container: 'body', windowClass: 'Expert-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchExpert();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteExpert(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chuyên gia này không?").then(
      data => {
        this.deleteExpert(Id);
      },
      error => {
        
      }
    );
  }

  deleteExpert(Id: string) {
    this.expertService.deleteExpert({ Id: Id }).subscribe(
      data => {
        this.searchExpert();
        this.messageService.showSuccess('Xóa chuyên gia thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ExportExcel() {
    this.expertService.ExportExcel(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  ListSpecialize: any = [];
  getSpecialize() {
    this.comboboxService.getListSpecialize().subscribe(
      data => {
        this.ListSpecialize = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  ListWorkPlace: any = [];
  getWorkPlace() {
    this.comboboxService.getListWorkPlace().subscribe(
      data => {
        this.ListWorkPlace = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  ListDegree: any = [];
  getDegree() {
    this.comboboxService.getListDegree().subscribe(
      data => {
        this.ListDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
