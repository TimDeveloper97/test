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
    Title: 'Th??m m???i c??ng vi???c',
    SaveText: 'L??u',
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
  columnName: any[] = [{ Name: 'Code', Title: 'M??' }, { Name: 'Name', Title: 'T??n' }]
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
      this.appSetting.PageTitle = 'Ch???nh s???a c??ng vi???c';
      this.modalInfo.SaveText = 'L??u';
      this.getInfo();
    }
    else {
      this.taskModel.FlowStageId = this.flowStageId;
      this.appSetting.PageTitle = 'Th??m m???i c??ng vi???c';
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

        this.appSetting.PageTitle = 'Ch???nh s???a c??ng vi???c - ' + this.taskModel.Code + ' - ' + this.taskModel.Name;
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
          this.messageService.showSuccess('Th??m m???i c??ng vi???c th??nh c??ng!');
        }
        else {
          this.messageService.showSuccess('Th??m m???i c??ng vi???c th??nh c??ng!');
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
        this.messageService.showSuccess('C???p nh???t c??ng vi???c th??nh c??ng!');
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
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? k??? n??ng ra n??y kh??ng?").then(
      data => {
        this.taskModel.Skills.splice(index, 1);
        this.messageService.showSuccess('X??a k??? n??ng th??nh c??ng!');
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
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? t??i li???u n??y kh??ng?").then(
      data => {
        this.taskModel.Documents.splice(index, 1);
        this.messageService.showSuccess('X??a t??i li???u th??nh c??ng!');
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
    this.messageService.showConfirm("B???n c?? ch???c mu???n v???t t?? n??y kh??ng?").then(
      data => {
        this.taskModel.Materials.splice(index, 1);
        this.messageService.showSuccess('X??a v???t t?? th??nh c??ng!');
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
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? ?????u v??o n??y kh??ng?").then(
      data => {
        this.taskModel.InputResults.splice(index, 1);
        this.messageService.showSuccess('X??a ?????u v??o th??nh c??ng!');
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
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? k???t qu??? ?????u ra n??y kh??ng?").then(
      data => {
        this.taskModel.OutputResults.splice(index, 1);
        this.messageService.showSuccess('X??a k???t qu??? ?????u ra th??nh c??ng!');
      },
      error => {

      }
    );
  }

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Kh??ng c?? file ????? t???i");
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

    modelDowload.Name = "T??i li???u";
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
        this.messageService.showMessage('B???n ch??a nh???p th??ng tin!');
        return;
      }
      this.taskModel.TaskWorkTypes.push(data);
    }else{
      this.taskModel.TaskWorkTypes=[];
      this.taskModel.TaskWorkTypes.push(data);
    }

  }
  deleteRow(index){
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? h??ng n??y kh??ng?").then(
      data => {
        this. taskModel.TaskWorkTypes.splice(index, 1);
        this.messageService.showSuccess('X??a  th??nh c??ng!');
      },
      error => {

      }
    );
  }

}
