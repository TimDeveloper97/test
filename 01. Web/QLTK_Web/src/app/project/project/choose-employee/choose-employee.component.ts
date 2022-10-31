import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, AppSetting, DateUtils, Configuration } from 'src/app/shared';
import { ProjectEmployeeService } from '../../service/project-employee.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-choose-employee',
  templateUrl: './choose-employee.component.html',
  styleUrls: ['./choose-employee.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseEmployeeComponent implements OnInit {

  @Input() Id: string;
 
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private config: Configuration,
    public constants: Constants,
    public appset: AppSetting,
    private service: ProjectEmployeeService,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    private route: ActivatedRoute
  ) { }

  modelSearch: any ={
    Name:'',
    Code:'',
    Email:'',
    Status: '',
    ListEmployeeId:[],
    DepartermentId:'',

  }

  model: any = {
    ProjectId: '',
    Type: 1,
  }

  searchOptions ={
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhân viên...',
    Items: [
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban....',
        Type: 'select',
        DataType: this.constants.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  }

  searchOptionsEx ={
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhân viên ngoài...',
  }

  modelAddPE: any ={
    RoleId: '',
    startTime: '',
    endTime: '',
    JobDescription: '',
    Status: '',
    Subsidy: '',
    subsisdyStartTime: '',
    subsidyEndTime: '',
  }

  listEmployee = [];
  listExEmployee = [];
  listRole = [];
  listEmployeeSelect = [];
  listEmployeeId: any = [];
  listAddPE = [];
  listProByEmployeeId: any[] = [];
  selectIndex = -1;
  selectIndexEX = -1;
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;
  StartIndex = 1;
  EmployeeName = '';
  EmployeeCode = '';
  ExternalEmployeeName = '';
  DescriptionRole = '';

  subsidyStartTime: any = null;
  subsidyEndTime: any = null;
  startTime: any = null;
  endTime: any = null;
  

  ngOnInit(): void {
    
    this.model.ProjectId = this.Id;
    console.log(this.Id);
    this.modelSearch.ListEmployeeId = this.listEmployeeId;
    this.modelSearch.Status = 1;
    this.changeObjectType();
    this.changeObjectTypeEx();
    this.getRole();

    this.route.queryParams
      .subscribe(params => {
        console.log(params);
        this.model.RoleId = params.roleId;
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
  checkAllEx(isCheck: any){
    if (isCheck) {
      this.listExEmployee.forEach(element => {
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
    if (this.model.Type == 1) {
      this.listEmployee.forEach(element => {
        if (element.Checked) {
          element.Quantity = 1;
          element.Subsidy=0;
          this.listEmployeeSelect.push(element);
        }
      });
      this.listEmployeeSelect.forEach(element => {
        var index = this.listEmployee.indexOf(element);
        if (index > -1) {
          this.listEmployee.splice(index, 1);
        }
      });
    }

    else if (this.model.Type == 2) {
      this.listExEmployee.forEach(element => {
        if (element.Checked) {
          element.Quantity = 1;
          element.Subsidy=0;
          this.listEmployeeSelect.push(element);
        }
      });
      this.listEmployeeSelect.forEach(element => {
        var index = this.listExEmployee.indexOf(element);
        if (index > -1) {
          this.listExEmployee.splice(index, 1);
        }
      });
    }
  }

  removeRow(){
    if (this.model.Type == 1) {
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
    else if (this.model.Type == 2) {
      this.listEmployeeSelect.forEach(element => {
        if (element.Checked) {
          this.listExEmployee.push(element);
        }
      });
      this.listExEmployee.forEach(element => {
        var index = this.listEmployeeSelect.indexOf(element);
        if (index > -1) {
          this.listEmployeeSelect.splice(index, 1);
        }
      });
    }
    
  }

  changeRole(RoleId: any){
    this.service.getDescriptionRoleById(RoleId).subscribe((data: any) => {
      if (data) {
        this.DescriptionRole = data.Description;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  
  save(){
    var isValid = true;
    if (this.model.Type == 1) {
      this.listEmployeeSelect.forEach(element => {
        if (element.RoleId == null || !element.startTime || !element.endTime)
        {
          this.messageService.showSuccess('Vui nhập đủ thông tin!');
          isValid = false;
        }
        else{
          if(element.subsidyStartTime){
            element.subsidyStartTime = this.dateUtils.convertObjectToDate(element.subsidyStartTime);
          }
          if(element.subsidyEndTime){
            element.subsidyEndTime = this.dateUtils.convertObjectToDate(element.subsidyEndTime);
          }
          if(element.startTime){
            element.startTime = this.dateUtils.convertObjectToDate(element.startTime); 
          }
          if(element.endTime){
            element.endTime = this.dateUtils.convertObjectToDate(element.endTime); 
          }
          if(!element.JobDescription){
            element.JobDescription = this.DescriptionRole; 
          }
  
          element.Type = 1;
          element.ProjectId = this.Id;
          element.Status = 1;
          element.Evaluate = 5;
          this.listEmployeeSelect = this.listEmployeeSelect;
        }
      });

      if(isValid){
        this.service.addMoreProjectEmployee(this.listEmployeeSelect).subscribe(
          data => {
            this.messageService.showSuccess('Thêm hồ sơ ứng tuyển thành công!');
            this.activeModal.close(this.listEmployeeSelect);
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
      
    }

    if (this.model.Type == 2) {
      this.listEmployeeSelect.forEach(element => {
        if (element.RoleId == null || !element.startTime || !element.endTime)
        {
          this.messageService.showSuccess('Vui nhập đủ thông tin!');
          isValid = false;
        }
        else{
          if(element.subsidyStartTime){
            element.subsidyStartTime = this.dateUtils.convertObjectToDate(element.subsidyStartTime);
          }
          if(element.subsidyEndTime){
            element.subsidyEndTime = this.dateUtils.convertObjectToDate(element.subsidyEndTime);
          }
          if(element.startTime){
            element.startTime = this.dateUtils.convertObjectToDate(element.startTime); 
          }
          if(element.endTime){
            element.endTime = this.dateUtils.convertObjectToDate(element.endTime); 
          }
          if(!element.JobDescription){
            element.JobDescription = this.DescriptionRole; 
          }
          element.Type = 2;
          element.ProjectId = this.Id;
          element.Status = 1;
          element.Evaluate = 5;
          
          this.listEmployeeSelect = this.listEmployeeSelect;
        }
      });

      if(isValid){
        this.service.addMoreProjectEmployee(this.listEmployeeSelect).subscribe(
          data => {
            this.messageService.showSuccess('Thêm hồ sơ ứng tuyển thành công!');
            this.activeModal.close(this.listEmployeeSelect);
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
    }
  }

  clear(){
    this.modelSearch = {
      Name:'',
      Code:'',
      Email:'',
      ListEmployeeId:[],
    }
    this.changeObjectType();
    this.changeObjectTypeEx();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


  loadParam(EmployeeId, index) {
    if(this.selectIndex != index){
      this.selectIndex = index;
      this.EmployeeName = '';
      this.EmployeeCode = '';
      this.service.getEmployeeName(EmployeeId).subscribe(data => {
          console.log(data.Name);
          this.EmployeeName = data.Name;
          this.EmployeeCode = data.Code;
      });
      this.searchProjectByEmployeeId(EmployeeId);
    }
    else{
      this.selectIndex = -1;
      this.listProByEmployeeId = [];
      this.EmployeeName = '';
      this.EmployeeCode = '';
    }
      
  }

  loadParamEX(EmployeeId, index) {
    if(this.selectIndexEX != index){
      this.selectIndexEX = index;
      this.ExternalEmployeeName = '';
      this.service.getExternalEmployeeName(EmployeeId).subscribe(data => {
        console.log(data.Name);
        this.ExternalEmployeeName = data.Name;
      });
      this.searchProjectByEmployeeId(EmployeeId);
    }
    else{
      this.selectIndexEX = -1;
      this.listProByEmployeeId = [];
      this.ExternalEmployeeName = '';
    }
      
  }

  searchProjectByEmployeeId(employeeId: any){
    this.service.searchProjectByEmployeeId(employeeId).subscribe(proByEmployeeId => {
      if (proByEmployeeId) {
        this.listProByEmployeeId = proByEmployeeId;
        console.log('Đây: ' + proByEmployeeId);
      }
    }, error => {
      this.messageService.showError(error);
    });
    this
  }

  changeObjectTypeEx(){
    this.selectIndex = -1;
    this.selectIndexEX = -1;
    this.listProByEmployeeId = [];
    this.EmployeeName = '';
    this.EmployeeCode = '';
    this.ExternalEmployeeName = '';
    this.listEmployeeSelect.forEach(item => {
      this.modelSearch.ListEmployeeId.push(item.Id);
    })
    this.service.getListExEmployee(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listExEmployee = data;
        this.listExEmployee.forEach((element, index) => {
          element.Index = index + 1;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  changeObjectType(){
    this.selectIndex = -1;
      this.selectIndexEX = -1;
      this.listProByEmployeeId = [];
      this.EmployeeName = '';
      this.EmployeeCode = '';
      this.ExternalEmployeeName = '';
      this.listEmployeeSelect.forEach(item => {
        this.modelSearch.ListEmployeeId.push(item.Id);
      })
      this.service.getListEmployee(this.modelSearch).subscribe((data: any) => {
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

  getRole(){
    this.service.getRole().subscribe(data => {
      if (data) {
        this.listRole = data;
        console.log(data);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

 
}
