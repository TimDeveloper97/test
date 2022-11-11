import { Component, OnInit } from '@angular/core';

import { MessageService, Constants, AppSetting, DateUtils } from 'src/app/shared';
import { GeneralInformationProjectService } from '../service/general-information-project.service';

@Component({
  selector: 'app-general-information-project',
  templateUrl: './general-information-project.component.html',
  styleUrls: ['./general-information-project.component.scss']
})
export class GeneralInformationProjectComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private appSetting: AppSetting,
    private service: GeneralInformationProjectService,
    public constant: Constants,
    public dateUtils: DateUtils,
  ) { }

  listData: any[] = [];
  totalFinish: 0;
  totalBeingImplemented: 0;

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã hoặc tên dự án ...',
    Items: [
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        Permission: ['F060902'],
        RelationIndexTo: 2
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        Permission: ['F060902'],
        RelationIndexFrom: 1
      },
    ]
  };

  projectSearchModel: any = {
    Code: '',
    Name: '',
    DateFrom: '',
    DateTo: '',
    DateToV: '',
    DateFromV: '',
    DepartmentId: '',
    SBUId: ''
  }


  ngOnInit() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.projectSearchModel.SBUId = currentUser.sbuId;
      this.projectSearchModel.DepartmentId = currentUser.departmentId;
    }
    this.appSetting.PageTitle = "Tổng hợp kế hoạch thiết kế";
    this.getListGeneralInformationProject();
  }

  getListGeneralInformationProject() {
    if (this.projectSearchModel.DateFromV) {
      this.projectSearchModel.DateFrom = this.dateUtils.convertObjectToDate(this.projectSearchModel.DateFromV);
    } else {
      this.projectSearchModel.DateFrom = null;
    }
    if (this.projectSearchModel.DateToV) {
      this.projectSearchModel.DateTo = this.dateUtils.convertObjectToDate(this.projectSearchModel.DateToV)
    } else {
      this.projectSearchModel.DateTo = null
    }
    this.service.getListGeneralInformationProject(this.projectSearchModel).subscribe(data => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        this.totalBeingImplemented = data.Status1;
        this.totalFinish = data.Status2;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  clear() {
    this.projectSearchModel = {
      Code: '',
      Name: '',
      DateFrom: '',
      DateTo: '',
      DateToV: this.dateUtils.getFiscalYearEnd(),
      DateFromV: this.dateUtils.getFiscalYearStart(),
      DepartmentId: '',
      SBUId: ''
    }
    this.getListGeneralInformationProject();
  }
}
