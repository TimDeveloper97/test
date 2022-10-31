import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Constants, MessageService, DateUtils, Configuration, PermissionService } from 'src/app/shared';
import { ProjectEmployeeService } from '../../service/project-employee.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChooseEmployeeComponent } from '../choose-employee/choose-employee.component';
import { ProjectEmployeeCreateComponent } from '../project-employee-create/project-employee-create.component';
import { ProjectEmployeeUpdateStatusSubsidyHistoryComponent } from '../project-employee-update-status-subsidy-history/project-employee-update-status-subsidy-history.component';

@Component({
  selector: 'app-project-employee',
  templateUrl: './project-employee.component.html',
  styleUrls: ['./project-employee.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectEmployeeComponent implements OnInit {

  @Input() Id: string;

  constructor(
    public constants: Constants,
    private config: Configuration,
    private service: ProjectEmployeeService,
    private modalService: NgbModal,
    private messageService: MessageService,
    public dateUtils: DateUtils,
    public permissionService: PermissionService,
  ) { }


  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'MaxLateTime',
    OrderType: false,
    EmployeeId: '',
    Type:'',
    ProjectId: '',
    EmployeeCode: '',
    DepartmentId: '',
    RoleId: '',
    LateStatus: ''
  }

  pros: any[]=[];
  listRequest: any[] = [];

  listProjectEmployee: any[] = [];
  listProjectImplementation: any[] = [];
  listProjectExternalEmployee: any[] = [];
  listProByEmployeeId: any[] = [];
  EmployeeSelect: '';
  listSelect = [];
  selectIndex = -1;
  selectIndexEx = -1;
  selectIndexImp = -1;
  EmployeeId = '';
  EmployeeName = '';
  EmployeeCode = '';
  ExternalEmployeeName = '';
  EmployeeNameImp = '';
  EmployeeCodeImp = '';
  StartIndex = 1;
  TotalLateEmployee = 0;

  searchOptions: any = {
    FieldContentName: 'EmployeeCode',
    Placeholder: 'Tìm kiếm theo mã/tên nhân viên...',
    Items: [
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constants.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Vị trí trong DA',
        FieldName: 'RoleId',
        Placeholder: 'Vị trí trong DA',
        Type: 'select',
        DataType: this.constants.SearchDataType.Role,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Tình trạng CV',
        FieldName: 'LateStatus',
        Placeholder: 'Tình trạng CV',
        Type: 'select',
        Data: this.constants.LateStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  }

  startIndex = 0;
  ngOnInit(): void {
    this.model.ProjectId = this.Id;
    this.getProjectEmployee();
    this.getProjectExternalEmployee();
    this.SearchEmployee();
  }

  SearchEmployee(){
    this.model.ProjectId = this.Id;
    this.service.getProjectImplementationByProjectId(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listProjectImplementation =  data.ListResult;
        this.model.totalItems = data.TotalItem;
        
        data.ListResult.forEach(element => {
          
            if (element.LatePlan > 0) {
              this.TotalLateEmployee = this.TotalLateEmployee + 1;
            }
          
        });
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getProjectEmployee() {
    this.service.getProjectEmployeeByProjectId(this.Id).subscribe(list => {
      if (list) {
        this.listProjectEmployee = list;
        
        console.log(list);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getProjectExternalEmployee(){
    this.service.getProjectExternalEmployeeByProjectId(this.Id).subscribe(list => {
      if (list) {
        this.listProjectExternalEmployee = list;
        console.log(list);
      }    
    }, error => {
      this.messageService.showError(error);
    });    
  }

  loadParam(EmployeeId, index) {
    if(this.selectIndex != index){
      this.selectIndex = index;
      this.selectIndexEx = -1;
      this.selectIndexImp = -1;
      this.EmployeeName = '';
      this.EmployeeCode = '';
      this.model.Type = 1;
      this.service.getEmployeeName(EmployeeId).subscribe(data => {
          this.EmployeeName = data.Name;
          this.EmployeeCode = data.Code;
      });
      this.searchProjectByEmployeeId(this.listProjectEmployee[index].EmployeeId);
    }
    else{
      this.selectIndex = -1;
      this.listProByEmployeeId = [];
      this.model.Type = '';
    }
    
  }

  loadParamEx(EmployeeId, index) {
    if(this.selectIndexEx != index){
      this.selectIndexEx = index;
      this.selectIndex = -1;
      this.selectIndexImp = -1;
      this.ExternalEmployeeName = '';
      this.model.Type = 2;
      this.service.getExternalEmployeeName(EmployeeId).subscribe(data => {
        this.ExternalEmployeeName = data.Name;
      });
      this.searchProjectByExEmployeeId(this.listProjectExternalEmployee[index].ExternalEmployeeId);
    }
    else{
      this.selectIndexEx = -1;
      this.listProByEmployeeId = [];
      this.model.Type = '';
    }
  }

  searchProjectByEmployeeId(EmployeeId:any){
    this.service.searchProjectByEmployeeId(EmployeeId).subscribe(proByEmployeeId => {
      if (proByEmployeeId) {
        this.listProByEmployeeId = proByEmployeeId;
        console.log(proByEmployeeId);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  searchProjectByExEmployeeId(EmployeeId:any){
    this.service.searchProjectByExEmployeeId(EmployeeId).subscribe(proByEmployeeId => {
      if (proByEmployeeId) {
        this.listProByEmployeeId = proByEmployeeId;
        console.log(proByEmployeeId);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  listAddEmployeeId: any = [];
  showClick(Id){
    let activeModal = this.modalService.open(ChooseEmployeeComponent, { container: 'body', windowClass: 'choose-employee', backdrop: 'static'});
    activeModal.componentInstance.Id = Id;
    this.listProjectEmployee.forEach(element => {
      this.listAddEmployeeId.push(element.EmployeeId);
    });
    this.listProjectExternalEmployee.forEach(element => {
      this.listAddEmployeeId.push(element.ExternalEmployeeId);
    });


    activeModal.componentInstance.listEmployeeId = this.listAddEmployeeId;

    activeModal.result.then((result) => {
      if (result && result.length > 0) {
          this.getProjectEmployee();
          this.getProjectExternalEmployee();
          this.SearchEmployee();
      }
    }, (reason) => {

    });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ProjectEmployeeCreateComponent, { container: 'body', windowClass: 'project-employee-create', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.projectId = this.Id;
    activeModal.result.then((result) => {
      if (result) {
        this.getProjectExternalEmployee();
      }
    },)
  }

  Update(row: {RoleId: any, JobDescription: any, StartTime: any, EndTime: any, Evaluate: any, Status: any, SubsidyStartTime: any, SubsidyEndTime: any, Subsidy: any}, index){
    let activeModal = this.modalService.open(ProjectEmployeeUpdateStatusSubsidyHistoryComponent, { container: 'body', windowClass: 'project-employee-update-status-subsidy-history', backdrop: 'static' })
    activeModal.componentInstance.SubsidyModel = row ? Object.assign({}, row) : null;
    
    activeModal.result.then((result) => {
      if (result) {
        this.getProjectEmployee();
        this.getProjectExternalEmployee();
        this.SearchEmployee();
      }
    },)
  }

  showDeleteProjectEmployee(Id: string){
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhân sự này không?").then(
      data => {
        this.service.deleteProjectDeleteEmployee({Id : Id}).subscribe(
          data=>{
            this.getProjectEmployee();
            this.getProjectExternalEmployee();
            this.SearchEmployee();
            this.messageService.showSuccess('Xóa nhân sự thành công!');
          });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  ExportExcel() {
    this.service.ExportExcel(this.model).subscribe(d => {
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
  setPermissions(id : any,Checked :any){
    var model ={
      Id  : id ,
      Checked : Checked
    }
    this.service.updateHasContractPlanPermit(model).subscribe(result=>{
    })
  }
}
