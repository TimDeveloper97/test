import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { TaskService } from '../../service/task.service';
import { SubjectSelectSkillComponent } from 'src/app/education/subjects/subject-select-skill/subject-select-skill.component';
import { AnyAaaaRecord } from 'dns';
import { ChooseDocumentComponent } from '../../output-result/choose-document/choose-document.component';
import { SelectMaterialComponent } from 'src/app/education/classroom/select-material/select-material/select-material.component';
import { ChooseOutputResultComponent } from '../choose-output-result/choose-output-result.component';
import { DownloadService } from 'src/app/shared/services/download.service';
import { DocumentViewComponent } from '../../../document/document-view/document-view.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ShowSelectSkillEmployeeComponents } from '../../Employee/show-select-skill-employee/show-select-skill-employee.component';
import { ShowSelectWorkSkillComponent } from '../../show-select-work-skill/show-select-work-skill.component';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Component({
  selector: 'app-task-flow-stage-create',
  templateUrl: './task-flow-stage-create.component.html',
  styleUrls: ['./task-flow-stage-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TaskFlowStageCreateComponent implements OnInit {
  @ViewChild('scrollModule', { static: false }) scrollModule: ElementRef;
  @ViewChild('scrollModuleHeader', { static: false }) scrollModuleHeader: ElementRef;
  @ViewChild('scrollModule1', { static: false }) scrollModule1: ElementRef;
  @ViewChild('scrollModuleHeader1', { static: false }) scrollModuleHeader1: ElementRef;

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    private comboboxService: ComboboxService,
    private taskService: TaskService,
    private modalService: NgbModal,
    private dowloadservice: DownloadService,
    private config: Configuration,
    private routeA: ActivatedRoute,
    public appSetting: AppSetting,
    private router: Router,) { }

  isAction: boolean = false;
  id: any;
  flowStageId: any = null;
  modalInfo = {
    Title: 'Thêm mới công việc',
    SaveText: 'Lưu',
  };

  taskModel: any = {
    Id: '',
    Name: '',
    Code: '',
    IsDesignModule: false,
    Type: 1,
    Description: '',
    TimeStandard: 0,
    DegreeId: '',
    Specialization: '',
    SpecializeId: '',
    WorkTypeRId: '',
    WorkTypeAId: '',
    WorkTypeSId: '',
    WorkTypeCId: '',
    WorkTypeIId: '',
    SBUId: '',
    DepartmentId: '',
    FlowStageId: '',
    PercentValue: 0,
    IsProjectWork: false,

    Skills: [],
    Documents: [],
    Materials: [],
    OutputResults: [],
    InputResults: [],
    TaskWorkTypes :[],
  };
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }]
  flowStages: any[] = [];
  sbus: any[] = [];
  departments: any[] = [];
  workTypes: any[] = [];
  degrees: any = [];
  specializes: any[] = [];
  courses: any[] = [];
  Input =  {
    WorkTypeRId: '',
    WorkTypeAId: '',
    WorkTypeSId: '',
    WorkTypeCId: '',
    WorkTypeIId: '',
    SBUId: '',
    DepartmentId: '',
}
scrollConfig: PerfectScrollbarConfigInterface = {
  suppressScrollX: false,
  suppressScrollY: true,
  minScrollbarLength: 20,
  wheelPropagation: true
};

  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('Id');
    this.routeA.queryParams
      .subscribe(params => {
        this.flowStageId = params.flowId;
      });
    this.getCbbData();
    if (this.id) {
      this.appSetting.PageTitle = 'Chỉnh sửa công việc';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.taskModel.FlowStageId = this.flowStageId;
      this.appSetting.PageTitle = 'Thêm mới công việc';
    }
  }

  ngAfterViewInit() {
    this.scrollModule.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollModule1.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader1.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  getCbbData() {
    let cbbFlowStage = this.comboboxService.getCbbFlowStage();
    let cbbSBU = this.comboboxService.getCbbSBU();
    let cbbWorkType = this.comboboxService.getListWorkType();
    let cbbDegree = this.comboboxService.getCbbDegree();
    let cbbSpecialize = this.comboboxService.getListSpecialize();

    forkJoin([cbbFlowStage, cbbSBU, cbbWorkType, cbbDegree, cbbSpecialize]).subscribe(results => {
      this.flowStages = results[0];
      this.sbus = results[1];
      this.workTypes = results[2];
      this.degrees = results[3];
      this.specializes = results[4];
    });
  }

  getInfo() {
    this.taskService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.taskModel = result;

        this.appSetting.PageTitle = 'Chỉnh sửa công việc - ' + this.taskModel.Code + ' - ' + this.taskModel.Name;
        // this.getCbbDepartment('');
        this.getCourses();
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbDepartment(Id,index) {
    this.taskModel.TaskWorkTypes[index] ={
      Id: this.taskModel.TaskWorkTypes[index].Id,
      SBUId: this.taskModel.TaskWorkTypes[index].SBUId,
      TaskId: this.taskModel.TaskWorkTypes[index].TaskId,
      WorkTypeAId: this.taskModel.TaskWorkTypes[index].WorkTypeAId,
      WorkTypeCId: this.taskModel.TaskWorkTypes[index].WorkTypeCId,
      WorkTypeIId: this.taskModel.TaskWorkTypes[index].WorkTypeIId,
      WorkTypeRId: this.taskModel.TaskWorkTypes[index].WorkTypeRId,
      WorkTypeSId: this.taskModel.TaskWorkTypes[index].WorkTypeSId,
      checkDepartment :true
    }
    this.comboboxService.getCbbDepartmentBySBU(Id).subscribe(data => {
      if (Id != null && Id != "") {
        this.taskModel.TaskWorkTypes[index].DepartmentIds = data;
      } else {
        this.taskModel.TaskWorkTypes[index].DepartmentIds = [];
      }
    }, error => {
      this.messageService.showError(error);
    })

  }

  getCourses() {
    var skillIds = [];
    this.taskModel.Skills.forEach(element => {
      skillIds.push(element.Id);
    });
    this.taskService.getCourses(skillIds).subscribe(data => {
      this.courses = data;
    }, error => {
      this.messageService.showError(error);
    })

  }

  closeModal(isOK: boolean) {
    this.router.navigate(['nhan-vien/quan-ly-cong-viec']);
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  create(isContinue) {
    this.taskService.create(this.taskModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới công việc thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới công việc thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.taskService.update(this.taskModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.taskModel = {
      Id: '',
      Name: '',
      Code: '',
      IsDesignModule: false,
      Type: 1,
      Description: '',
      TimeStandard: 0,
      DegreeId: '',
      Specialization: '',
      SpecializeId: '',
      WorkTypeRId: '',
      WorkTypeAId: '',
      WorkTypeSId: '',
      WorkTypeCId: '',
      WorkTypeIId: '',
      SBUId: '',
      DepartmentId: '',
      FlowStageId: '',
      PercentValue: 0,

      Skills: [],
      Documents: [],
      Materials: [],
      OutputResults: [],
      InputResults: [],
    };
  }

  showSelectSkill() {
    let activeModal = this.modalService.open(ShowSelectWorkSkillComponent, { container: 'body', windowClass: 'select-work-skill-model ', backdrop: 'static' });
    var ListIdSelect = [];
    this.taskModel.Skills.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.taskModel.Skills.push(element);
        });
        this.getCourses();
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteSkill(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kỹ năng ra này không?").then(
      data => {
        this.taskModel.Skills.splice(index, 1);
        this.messageService.showSuccess('Xóa kỹ năng thành công!');
        this.getCourses();
      },
      error => {

      }
    );
  }

  chooseDocument() {
    let activeModal = this.modalService.open(ChooseDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.taskModel.Documents.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code,
            DocumentGroupName: element.DocumentGroupName,
            ListFile: element.ListFile,
            DocumentTypeName: element.DocumentTypeName
          }
          this.taskModel.Documents.push(data);
        });
      }
    }, (reason) => {

    });
  }

  viewDocument(document: any) {
    let activeModal = this.modalService.open(DocumentViewComponent, { container: 'body', windowClass: 'document-viewpdf-file-modal', backdrop: 'static' })
    activeModal.componentInstance.id = document.Id;
    activeModal.componentInstance.documentName = document.Name;
    activeModal.componentInstance.documentCode = document.Code;
    activeModal.result.then((result: any) => {
      if (result) {
      }
    });
  }

  showConfirmDeleteDocument(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.taskModel.Documents.splice(index, 1);
        this.messageService.showSuccess('Xóa tài liệu thành công!');
      },
      error => {

      }
    );
  }

  showSelectMaterial() {
    let activeModal = this.modalService.open(SelectMaterialComponent, { container: 'body', windowClass: 'select-material-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.taskModel.Materials.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.taskModel.Materials.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteMaterial(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn vật tư này không?").then(
      data => {
        this.taskModel.Materials.splice(index, 1);
        this.messageService.showSuccess('Xóa vật tư thành công!');
      },
      error => {

      }
    );
  }

  chooseInputResult() {
    let activeModal = this.modalService.open(ChooseOutputResultComponent, { container: 'body', windowClass: 'choose-outputresult-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.taskModel.InputResults.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.componentInstance.flowStageId = this.taskModel.FlowStageId;
    activeModal.componentInstance.departmentId = this.taskModel.DepartmentId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code,
            SBUName: element.SBUName,
            DepartmentName: element.DepartmentName
          }
          this.taskModel.InputResults.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteInputResult(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá đầu vào này không?").then(
      data => {
        this.taskModel.InputResults.splice(index, 1);
        this.messageService.showSuccess('Xóa đầu vào thành công!');
      },
      error => {

      }
    );
  }

  chooseOutputResult() {
    let activeModal = this.modalService.open(ChooseOutputResultComponent, { container: 'body', windowClass: 'choose-outputresult-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.taskModel.OutputResults.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code,
            SBUName: element.SBUName,
            DepartmentName: element.DepartmentName,
          }
          this.taskModel.OutputResults.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteOutputResult(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kết quả đầu ra này không?").then(
      data => {
        this.taskModel.OutputResults.splice(index, 1);
        this.messageService.showSuccess('Xóa kết quả đầu ra thành công!');
      },
      error => {

      }
    );
  }

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Datashet.forEach(element => {
      listFilePath.push({
        Path: element.Path,
        FileName: element.FileName
      });
    });

    let modelDowload: any = {
      Name: '',
      ListDatashet: []
    }

    modelDowload.Name = "Tài liệu";
    modelDowload.ListDatashet = listFilePath;
    this.dowloadservice.downloadAll(modelDowload).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  addRow() {
    let data = Object.assign({},this.Input);
    if(this.taskModel.TaskWorkTypes.length >0){
      var result = this.taskModel.TaskWorkTypes[this.taskModel.TaskWorkTypes.length-1];
      if(result.DepartmentId =='' && result.SBUId =='' && result.WorkTypeAId =='' 
      && result.WorkTypeCId =='' && result.WorkTypeIId =='' && result.WorkTypeRId =='' 
      && result.WorkTypeSId =='')
      {
        this.messageService.showMessage('Bạn chưa nhập thông tin!');
        return;
      }
      this.taskModel.TaskWorkTypes.push(data);
    }else{
      this.taskModel.TaskWorkTypes=[];
      this.taskModel.TaskWorkTypes.push(data);
    }

  }
  deleteRow(index){
    this.messageService.showConfirm("Bạn có chắc muốn xoá hàng này không?").then(
      data => {
        this. taskModel.TaskWorkTypes.splice(index, 1);
        this.messageService.showSuccess('Xóa  thành công!');
      },
      error => {

      }
    );
  }

}
