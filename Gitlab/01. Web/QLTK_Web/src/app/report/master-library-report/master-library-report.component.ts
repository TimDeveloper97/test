import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MasterLibraryReportService } from '../service/master-library-report.service';

@Component({
  selector: 'app-master-library-report',
  templateUrl: './master-library-report.component.html',
  styleUrls: ['./master-library-report.component.scss']
})
export class MasterLibraryReportComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public masterLibraryReportService: MasterLibraryReportService
  ) { }

  ngOnInit() {
    this.appSetting.PageTitle = "Tổng hợp dữ liệu thư viện";
    this.searchReportLibrary();
  }

  StartIndex = 0;
  listData: any[] = [];
  model: any={
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'SubjectsCode',
    OrderType: true,

    SubjectCode:'',
    SkillCode:'',
    PracticeCode:'',
    ModuleCode:'',
  }

  searchOptions: any = {
    FieldContentName: 'SubjectsCode',
    Placeholder: 'Tìm kiếm theo mã môn',
    Items: [
      {
        Name: 'Mã kĩ năng',
        FieldName: 'SkillCode',
        Placeholder: 'Nhập mã kĩ năng',
        Type: 'text'
      },
      {
        Name: 'Mã bài thực hành/công đoạn',
        FieldName: 'PracticeCode',
        Placeholder: 'Nhập mã bài thực hành/công đoạn',
        Type: 'text'
      },
      {
        Name: 'Mã module',
        FieldName: 'ModuleCode',
        Placeholder: 'Nhập mã module',
        Type: 'text'
      },
    ]
  };
  searchReportLibrary(){
    this.masterLibraryReportService.SearchMasterLibrary(this.model).subscribe((data: any) => {
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
      OrderBy: 'SubjectCode',
      OrderType: true,
      
      SubjectCode:'',
      SkillCode:'',
      PracticeCode:'',
      ModuleCode:'',
    }
    this.searchReportLibrary();
  }

  ExportExcel(){
    this.masterLibraryReportService.ExportExcel(this.model).subscribe(d => {
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
