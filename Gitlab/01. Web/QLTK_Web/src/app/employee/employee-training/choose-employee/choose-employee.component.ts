import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { EmployeeTrainingService } from '../../service/employee-training.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, AppSetting } from 'src/app/shared';

@Component({
  selector: 'app-choose-employee',
  templateUrl: './choose-employee.component.html',
  styleUrls: ['./choose-employee.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseEmployeeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private service: EmployeeTrainingService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  courseId: string;
  listSelect: any = [];
  listData: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  IsRequest: boolean;

  employeeModel: any = {
    Id: '',
    CourseId: '',
    Code: '',
    Name: '',
    NameCode: '',
    SBUId: '',
    DepartmentId: '',
    ListIdSelect: [],
    ListIdChecked: [],
    Isreach: ''
  }
  Level : any=[];
  searchOptions: any = {
    FieldContentName: 'NameCode',
    Placeholder: 'Tìm kiếm theo tên/mã nhân viên',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constants.SearchDataType.SBU,
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
        DataType: this.constants.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 0
      },
      {
        Name: 'Tình trạng',
        FieldName: 'Isreach',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constants.EmployeeTraningStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Vị trí',
        FieldName: 'WorkTypeId',
        Placeholder: 'Vị trí',
        Type: 'select',
        DataType: this.constants.SearchDataType.WorkType,
        DisplayName: 'Name',
        ValueName: 'Id'
      }
    ]
  };

  ngOnInit() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.employeeModel.SBUId = currentUser.sbuId;
      this.employeeModel.DepartmentId = currentUser.departmentId;
    }
    this.employeeModel.CourseId = this.courseId;
    this.ListIdSelect.forEach(element => {
      this.employeeModel.ListIdSelect.push(element);
    });
    this.searchEmployee();
  }

  searchEmployee() {
    this.listSelect.forEach(element => {
      this.employeeModel.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.employeeModel.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchEmployee(this.employeeModel).subscribe(data => {
      this.listData = data;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.employeeModel.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listData.indexOf(element);
      if (index > -1) {
        this.listData.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listData.push(element);
      }
    });
    this.listData.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  clear() {
    this.employeeModel = {
      Id: '',
      Code: '',
      Name: '',
      NameCode: '',
      SBUId: '',
      DepartmentId: '',
      ListIdSelect: [],
      ListIdChecked: [],
      Isreach: ''
    }
    this.employeeModel.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.ListIdSelectRequest.forEach(element => {
        this.employeeModel.ListIdSelect.push(element);
      });
    } else {
      this.ListIdSelect.forEach(element => {
        this.employeeModel.ListIdSelect.push(element);
      });
    }
    this.searchEmployee();
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  checkAll(isCheck: any) {
    if (isCheck) {
      this.listData.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }
}
