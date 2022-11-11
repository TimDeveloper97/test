import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MasterEmployeeService } from '../service/master-employee.service';

@Component({
  selector: 'app-master-employee',
  templateUrl: './master-employee.component.html',
  styleUrls: ['./master-employee.component.scss']
})
export class MasterEmployeeComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public masterEmployeeService: MasterEmployeeService
  ) { }
  model: any={
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'EmployeeCode',
    OrderType: true,

    EmployeeCode:'',
    EmployeeName:'',
  }
  StartIndex = 0;
  listData: any[] = [];
  searchOptions: any = {
    FieldContentName: 'EmployeeCode',
    Placeholder: 'Tìm kiếm theo mã nhân viên',
    Items: [
      {
        Name: 'Tên nhân viên',
        FieldName: 'EmployeeName',
        Placeholder: 'Nhập tên nhân viên',
        Type: 'text'
      },

    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo master nhân sự";
    this.searchMasterEmployee();
  }

  searchMasterEmployee(){
    this.masterEmployeeService.SearchEmployee(this.model).subscribe((data: any) => {
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
  
      EmployeeCode:'',
      EmployeeName:'',
    }
    this.searchMasterEmployee();
  }

  ExportExcel(){
    this.masterEmployeeService.ExportExcel(this.model).subscribe(d => {
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
