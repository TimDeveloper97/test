import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MasterEmployeeLevelService } from '../service/master-employee-level.service';

@Component({
  selector: 'app-master-employee-level',
  templateUrl: './master-employee-level.component.html',
  styleUrls: ['./master-employee-level.component.scss']
})
export class MasterEmployeeLevelComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public masterEmployeeLevelService: MasterEmployeeLevelService
  ) { }
  model: any={
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'CouseCode',
    OrderType: true,

    SubjectCode:'',
    SkillCode:'',
    PracticeCode:'',
    ModuleCode:'',
    JobPositionName:''
  }
  StartIndex = 0;
  listData: any[] = [];
  searchOptions: any = {
    FieldContentName: 'CouseCode',
    Placeholder: 'Tìm kiếm theo mã khóa học',
    Items: [
      // {
      //   Name: 'Mã khóa học',
      //   FieldName: 'CouseCode',
      //   Placeholder: 'Nhập mã khóa học',
      //   Type: 'text'
      // },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo master level nhân sự";
    this.searchMasterEmployee();
  }
  searchMasterEmployee(){
    this.masterEmployeeLevelService.SearchEmployeeLevel(this.model).subscribe((data: any) => {
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

  clear(){
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'EmployeeCode',
      OrderType: true,
  
      SubjectCode:'',
      SkillCode:'',
      PracticeCode:'',
      ModuleCode:'',
      JobPositionName:''
    }
    this.searchMasterEmployee();
  }

  ExportExcel(){
    this.masterEmployeeLevelService.ExportExcel(this.model).subscribe(d => {
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
}
