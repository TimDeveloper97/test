import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, ComboboxService, Constants, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IndustryService } from '../../services/industry.service';
import { IndustryCreateComponent } from '../industry-create/industry-create.component';

@Component({
  selector: 'app-industry-manage',
  templateUrl: './industry-manage.component.html',
  styleUrls: ['./industry-manage.component.scss']
})
export class IndustryManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private config :  Configuration,
    private industryService : IndustryService
  ) { }

  startIndex = 1;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Code: '',
    Name: '',
    Description:'',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã ngành',
    Items: [
      {
        Name: 'Tên ngành',
        FieldName: 'Name',
        Placeholder: 'Nhập tên tiêu chí',
        Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý ngành hàng";
    this.searchIndustrys();
  }

  searchIndustrys() {
    this.industryService.searchIndustry(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
    }
    this.searchIndustrys();
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá ngành này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.industryService.deleteIndustry({ Id: Id }).subscribe(
      data => {
        this.searchIndustrys();
        this.messageService.showSuccess('Xóa ngành thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(IndustryCreateComponent, { container: 'body', windowClass: 'Industry-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchIndustrys();
      }
    }, (reason) => {
    });
  }
}
