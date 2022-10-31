import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { SaleGroupService } from '../service/sale-group.service';

@Component({
  selector: 'app-show-choose-employee',
  templateUrl: './show-choose-employee.component.html',
  styleUrls: ['./show-choose-employee.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChooseEmployeeComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private saleGroupService: SaleGroupService,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public constant: Constants
  ) { }

  listEmployee = [];

  listEmployeeSelect = [];
  listEmployeeId: any = [];
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;

  modelSearch: any ={
    Name:'',
    Code:'',
    Email:'',
    Status: '',
    ListEmployeeId:[],
    DepartermentId:''
  }
  check  = false;
  searchOptions ={
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhân viên/ mã nhân viên ...',
    Items: [
      {
        Name: 'Email nhân viên',
        FieldName: 'Email',
        Placeholder: 'Nhập email nhân viên ...',
        Type: 'text'
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartermentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng làm việc',
        FieldName: 'Status',
        Placeholder: 'Tình trạng làm việc',
        Type: 'select',
        Data: this.constant.EmployeeStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      }
    ]
  }
  Title : any = 'Chọn nhân viên kinh doanh';
  ngOnInit() {
    this.modelSearch.ListEmployeeId = this.listEmployeeId;
    this.modelSearch.Status = 1;
    this.searchEmployee();
    if(this.check ==true){
      this.Title = 'Chọn nhân viên xem tài liệu dự án';
    }
  }

  imagePath : string;
  searchEmployee(){
    this.listEmployeeSelect.forEach(item => {
      this.modelSearch.ListEmployeeId.push(item.Id);
    })
    this.saleGroupService.getListEmployee(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listEmployee = data;
        this.listEmployee.forEach((element, index) => {
          element.Index = index + 1;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  checkAll(isCheck: any){
    if (isCheck) {
      this.listEmployee.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listEmployeeSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow(){
    this.listEmployee.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listEmployeeSelect.push(element);
      }
    });
    this.listEmployeeSelect.forEach((element,i) => {
      var index = this.listEmployee.indexOf(element);
      if (index > -1) {
        this.listEmployee.splice(index, 1);
      }
      element.Index = i + 1;
    });
    this.listEmployee.forEach((element, index) => {
      element.Index = index + 1;
    });

  }

  removeRow(){
    this.listEmployeeSelect.forEach(element => {
      if (element.Checked) {
        this.listEmployee.push(element);
      }
    });
    this.listEmployee.forEach(element => {
      var index = this.listEmployeeSelect.indexOf(element);
      if (index > -1) {
        this.listEmployeeSelect.splice(index, 1);
      }
    });
  }

  choose(){
    this.activeModal.close(this.listEmployeeSelect);
  }

  clear(){
    this.modelSearch = {
      Name:'',
      Code:'',
      Email:'',
      ListEmployeeId:[],
    }
    this.searchEmployee();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
