import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, ComboboxService, Configuration, AppSetting } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { EmployeeWorkTypeService } from '../../service/employee-work-type.service';
import { ShowSelectSkillEmployeeComponent } from '../show-select-skill-employee/show-select-skill-employee.component';
import { forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ChooseDocumentComponent } from '../../output-result/choose-document/choose-document.component';
import { DocumentViewComponent } from '../../../document/document-view/document-view.component';
import { DownloadService } from 'src/app/shared/services/download.service';

@Component({
  selector: 'app-work-type-create',
  templateUrl: './work-type-create.component.html',
  styleUrls: ['./work-type-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WorkTypeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private modalService: NgbModal,
    private workTypeService: EmployeeWorkTypeService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private routeA: ActivatedRoute,
    private dowloadservice: DownloadService,
    private config: Configuration,
    private router: Router,
    public appSetting: AppSetting,
  ) { }
  ModalInfo = {//m
    Title: 'Thêm mới vị trí công việc',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  model: any = {
    Id: '',
    Name: '',
    Quantity: 0,
    SalaryLevelMinId: '',
    SalaryLevelMaxId: '',
    Value: '',
    Materials: [],
    Tasks: [],
    Documents: [],
    SalaryTypeId: null,
    SalaryGroupId: null
  }

  salaryLevels: any[] = [];
  salaryGroups: any[] = [];
  salaryTypes: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];
  columnNameSalary: any[] = [{ Name: 'Code', Title: 'Mã',Format:''  }, { Name: 'Name', Title: 'Tên',Format:'' }, { Name: 'Exten', Title: 'Lương', Format:',##0' }];
  listSBU: any[] = [];
  listDepartment: any[] = [];
  CodeSBU: string;
  CodeDepartment: string;
  flowStages: any[] = [];
  salaryMin: any = "";
  salaryMax: any = "";

  ngOnInit() {
    this.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getCBBData();
    if (this.Id) {
      this.appSetting.PageTitle = 'Chỉnh sửa vị trí công việc';
      this.ModalInfo.SaveText = 'Lưu';
      // this.getWorkTypeInfo();
    }
    else {
      this.appSetting.PageTitle = "Thêm mới vị trí công việc";
    }
  }

  getCBBData() {
    let cbbSalaryLevel = this.comboboxService.getCbbSalaryLevel();
    let cbbSBU = this.comboboxService.getCBBSBU();
    let cbbFlowStage = this.comboboxService.getCbbFlowStage();
    let cbbSalaryGroup = this.comboboxService.getCbbSalaryGroups();
    let cbbSalaryType = this.comboboxService.getCbbSalarytypes();

    forkJoin([cbbSalaryLevel, cbbSBU, cbbFlowStage, cbbSalaryGroup, cbbSalaryType]).subscribe(results => {
      this.salaryLevels = results[0];
      this.listSBU = results[1];
      this.flowStages = results[2];
      this.salaryGroups = results[3];
      this.salaryTypes = results[4];
      if (this.Id) {
        this.getWorkTypeInfo();
      }
    });
  }

  getSalaryLevel(){
    this.comboboxService.getCbbSalaryLevelByGroupType({SalaryGroupId:this.model.SalaryGroupId, SalaryTypeId: this.model.SalaryTypeId}).subscribe(
      data => {
        this.salaryLevels = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  GetCbbDepartment() {
    // this.model.DepartmentId = null;
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
        // this.model.DepartmentId = '';
        this.getName(this.model.SBUId);
        this.getNameDepartment(this.model.DepartmentId);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getName(SBUID) {
    for (var item of this.listSBU) {
      if (item.Id == SBUID) {
        this.CodeSBU = item.Code;
        this.CodeDepartment = null;
      }
    }
  }

  getNameDepartment(DepartmentId) {
    for (var item of this.listDepartment) {
      if (item.Id == DepartmentId) {
        this.CodeDepartment = item.Code;
      }
    }

  }

  getSalary(isMin: boolean) {
    if (isMin) {
      if (this.model.SalaryLevelMinId) {
        this.salaryMin = this.salaryLevels.find(a => a.Id == this.model.SalaryLevelMinId).Exten;
      }
    } else {
      if (this.model.SalaryLevelMaxId) {
        this.salaryMax = this.salaryLevels.find(a => a.Id == this.model.SalaryLevelMaxId).Exten;
      }
    }
  }

  getWorkTypeInfo() {
    this.workTypeService.getInforWorkType({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.appSetting.PageTitle = 'Chỉnh sửa vị trí công việc - ' + this.model.Code + ' - ' + this.model.Name;
      this.listBase = data.ListWorkTypeSkill;
      this.getSalary(true);
      this.getSalary(false);
      this.GetCbbDepartment();
      this.getNameDepartment(this.model.DepartmentId);
    });
  }

  listBase: any[] = [];

  showSelectSkill() {
    let activeModal = this.modalService.open(ShowSelectSkillEmployeeComponent, { container: 'body', windowClass: 'select-skill-employee-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listBase.forEach(element => {
      ListIdSelect.push(element.Id);
    });
    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listBase.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteSkill(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kỹ năng này không?").then(
      data => {
        this.removeRowSkill(row);
      },
      error => {

      }
    );
  }
  removeRowSkill(row) {
    var index = this.listBase.indexOf(row);
    if (index > -1) {
      this.listBase.splice(index, 1);
    }
  }

  createWorkType(isContinue) {
    this.model.ListWorkTypeSkill = this.listBase;
    this.workTypeService.createWorkType(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Quantity: 0,
            SalaryLevelMinId: '',
            SalaryLevelMaxId: '',
            Value: '',
            Materials: [],
            Tasks: [],
            Documents: []
          };
          this.messageService.showSuccess('Thêm mới vị trí công việc thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới vị trí công việc thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  updateWorkType() {
    this.model.ListWorkTypeSkill = this.listBase;
    this.workTypeService.updateWorkType(this.model).subscribe(
      () => {
        this.closeModal(true);
        this.messageService.showSuccess('Cập nhật vị trí công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  closeModal(isOK: boolean) {
    this.router.navigate(['nhan-vien/quan-ly-vi-tri-cong-viec']);
  }

  save(isContinue: boolean) {
    if (Number(this.model.SalaryMax) <= Number(this.model.SalaryMin)) {
      this.messageService.showMessage("Mức lương tối đa nhỏ hơn mức lương tối thiểu");
      return;
    }


    if (this.Id) {
      this.updateWorkType();
    }
    else {
      this.createWorkType(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  chooseDocument() {
    let activeModal = this.modalService.open(ChooseDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.model.Documents.forEach(element => {
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
            DocumentTypeName: element.DocumentTypeName,
            IsDocumentOfTask: false
          }
          this.model.Documents.push(data);
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
        this.model.Documents.splice(index, 1);
        this.messageService.showSuccess('Xóa tài liệu thành công!');
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

}
