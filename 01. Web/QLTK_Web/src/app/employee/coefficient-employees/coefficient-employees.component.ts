import { Component, OnInit, ViewChild, ElementRef, OnDestroy, AfterViewInit} from '@angular/core';
import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { EmployeePresentService } from 'src/app/report/service/employee-present.service';

@Component({
  selector: 'app-coefficient-employees',
  templateUrl: './coefficient-employees.component.html',
  styleUrls: ['./coefficient-employees.component.scss']
})
export class CoefficientEmployeesComponent implements OnInit, OnDestroy,AfterViewInit {
  @ViewChild('scrollHeaderCoefficient',{static:false}) scrollHeaderCoefficient: ElementRef;
  @ViewChild('scrollCoefficient',{static:false}) scrollCoefficient: ElementRef;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    private service: EmployeePresentService,
    private comboboxService: ComboboxService,
  ) { }

  heightTable: number;
  date = new Date();
  listCoefficient: any[] = [];
  model: any = {
    Date: '',
    SBUId: '',
    DepartmentId: '',
    Code: '',
    Year: this.date.getFullYear(),
    ProjectId: '',
    ModuleGroupId: ''
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã và tên nhân viên',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 1
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 0
      },
      {
        Placeholder: 'Năm',
        Name: 'Năm',
        FieldName: 'Year',
        Type: 'numberYear',
      },
    ]
  };

  ngOnInit() {  
    this.heightTable = window.innerHeight - 280;
    this.appSetting.PageTitle = "Hệ số năng lực nhân viên";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.model.SBUId = currentUser.sbuId;
      this.model.DepartmentId = currentUser.departmentId;
    }
    this.coefficientEmployee();
  }

  ngAfterViewInit(){
    this.scrollHeaderCoefficient.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollCoefficient.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollHeaderCoefficient.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  coefficientEmployee() {
    this.service.coefficientEmployee(this.model).subscribe((data: any) => {
      this.listCoefficient = data.ListData;
    }, error => {
      this.messageService.showError(error);
    });
  }

  updateCoefficientEmployee() {
    var modelUpdate = {
      SBUId: this.model.SBUId,
      DepartmentId: this.model.DepartmentId,
      Year: this.model.Year,
      ListData: this.listCoefficient
    }
    this.service.updateCoefficientEmployee(modelUpdate).subscribe(
      data => {
        this.messageService.showSuccess("Cấu hình hệ số nhân viên thành công!");
      }, error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {

      SBUId: '',
      DepartmentId: '',
      Code: '',
      ProjectId: '',
      ModuleGroupId: ''
    }
    this.model.Date = new Date();
    this.coefficientEmployee();
  }
}
