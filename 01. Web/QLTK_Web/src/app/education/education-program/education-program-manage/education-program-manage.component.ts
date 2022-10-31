import { Component, OnInit } from '@angular/core';

import { AppSetting, MessageService, Configuration, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EducationProgramService } from '../../service/education-program.service';
import { EducationProgramCreateComponent } from '../education-program-create/education-program-create.component';

@Component({
  selector: 'app-education-program-manage',
  templateUrl: './education-program-manage.component.html',
  styleUrls: ['./education-program-manage.component.scss']
})
export class EducationProgramManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private educationProgramService: EducationProgramService,
    public constant: Constants,
    public comboboxService: ComboboxService
  ) { }

  ListJob = [];
  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    JobId: '',
    Id: '',
    Name: '',
    ListJob: [],
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã chương trình đào tạo',
    Items: [
      {
        Name: 'Tên môn',
        FieldName: 'Name',
        Placeholder: 'Nhập tên chương trình đào tạo',
        Type: 'text'
      },
      {
        Name: 'Nghề học',
        FieldName: 'JobId',
        Placeholder: 'Trình độ',
        Type: 'select',
        DataType: this.constant.SearchDataType.Job,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý chương trình đào tạo";
    this.getJob();
    this.searchEducationProgram();
  }

searchEducationProgram() {
    this.educationProgramService.searchEducationProgram(this.model).subscribe((data: any) => {
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
      ListJob: [],
      JobId:'',
    }
    this.searchEducationProgram();
  }
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(EducationProgramCreateComponent, { container: 'body', windowClass: 'EducationProgram-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchEducationProgram();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteEducationProgram(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chương trình này không?").then(
      data => {
        this.deleteEducationProgram(Id);
      },
      error => {
        
      }
    );
  }

  deleteEducationProgram(Id: string) {
    this.educationProgramService.deleteEducationProgram({ Id: Id }).subscribe(
      data => {
        this.searchEducationProgram();
        this.messageService.showSuccess('Xóa chương trình thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ExportExcel() {
    this.educationProgramService.ExportExcel(this.model).subscribe(d => {
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

  getJob() {
    this.comboboxService.getListJob().subscribe(
      data => {
        this.ListJob = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
