import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Constants, DateUtils, MessageService } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { DocumentService } from '../services/document.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ChooseProductToolComponent } from 'src/app/tool/choose-product-tool/choose-product-tool.component';
import { ProductChooseModuleComponent } from 'src/app/device/product-choose-module/product-choose-module.component';
import { ChooseDepartmentComponent } from '../choose-department/choose-department.component';
import { ChooseWorktypeComponent } from '../choose-worktype/choose-worktype.component';
import { ChooseWorkComponent } from '../choose-work/choose-work.component';
import { NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-document-create',
  templateUrl: './document-create.component.html',
  styleUrls: ['./document-create.component.scss']
})
export class DocumentCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appSetting: AppSetting,
    private comboboxService: ComboboxService,
    public dateUtils: DateUtils,
    private documentService: DocumentService,
    private router: Router,
    private routeA: ActivatedRoute,
    private modalService: NgbModal,) { }

  @ViewChild(NgSelectComponent) ngSelectComponent: NgSelectComponent;
  isAction: boolean = false;
  id: any;

  documentModel: any = {
    Id: '',
    DocumentGroupId: '',
    DocumentTypeId: '',
    Name: '',
    Code: '',
    Note: '',
    Version: '',
    CompilationType: 1,
    CompilationEmployeeId: '',
    CompilationSuppliserId: '',
    PromulgateDate: '',
    PromulgateLastDate: '',
    DepartmentId: '',
    EmployeeId: '',
    ReviewDateFrom: '',
    ReviewDateTo: '',
    Price: '',
    Keywords: [],
    Description: '',
    ApproveWorkTypeId: '',
    Status: 2,
    Devices: [],
    Modules: [],
    WorkTypes: [],
    Departments: [],
    Works: [],
    DocumentTags: []
  }

  documentGroups: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  employees: any[] = [];
  columnEmployeeName: any[] = [{ Name: 'Code', Title: 'Mã Nhân viên' }, { Name: 'Name', Title: 'Tên Nhân vien' }];
  columnSupplierName: any[] = [{ Name: 'Code', Title: 'Mã NCC' }, { Name: 'Name', Title: 'Tên NCC' }];
  reviewDateFrom: any = null;
  reviewDateTo: any = null;
  promulgateDate: any = null;
  promulgateLastDate: any = null;
  documnetTypes: any[] = [];
  suppliers: any[] = [];
  workTypes: any[] = [];
  departments: any[] = [];
  documentTags: any[] = [];

  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('Id');
    this.getCbbData();
    if (!this.id) {
      this.appSetting.PageTitle = "Thêm mới tài liệu";
      this.documentModel.DocumentGroupId = localStorage.getItem("selectedDocumentGroupId");

    } else {
      this.appSetting.PageTitle = "Chỉnh sửa tài liệu";
      this.documentModel.Id = this.id;
    }

  }

  getCbbData() {
    let cbbDocumentGroup = this.comboboxService.getDocumentGroup();
    let cbbDocumnentType = this.comboboxService.getDocumentType();
    let cbbEmployee = this.comboboxService.getCbbEmployee();
    let cbbSupplier = this.comboboxService.getCBBSupplier();
    let cbbDepartment = this.comboboxService.getCbbDepartment();
    let cbbWorkType = this.comboboxService.getListWorkType();
    let tags = this.documentService.getDocumentTags();

    forkJoin([cbbDocumentGroup, cbbDocumnentType, cbbEmployee, cbbSupplier, cbbDepartment, cbbWorkType, tags]).subscribe(results => {
      this.documentGroups = results[0];
      this.documnetTypes = results[1];
      this.employees = results[2];
      this.suppliers = results[3];
      this.departments = results[4];
      this.workTypes = results[5];
      this.documentTags = results[6];

      if (this.id) {
        this.getInfo();
      }
    });
  }

  getInfo() {
    this.documentService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.documentModel = result;
        this.appSetting.PageTitle = "Chỉnh sửa tài liệu - " + this.documentModel.Code + " - " + this.documentModel.Name;
        if (this.documentModel.PromulgateDate != null && this.documentModel.PromulgateDate != "") {
          this.promulgateDate = this.dateUtils.convertDateToObject(this.documentModel.PromulgateDate);;
        }

        if (this.documentModel.PromulgateLastDate != null && this.documentModel.PromulgateLastDate != "") {
          this.promulgateLastDate = this.dateUtils.convertDateToObject(this.documentModel.PromulgateLastDate);;
        }

        if (this.documentModel.ReviewDateFrom != null && this.documentModel.ReviewDateFrom != "") {
          this.reviewDateFrom = this.dateUtils.convertDateToObject(this.documentModel.ReviewDateFrom);;
        }

        if (this.documentModel.ReviewDateTo != null && this.documentModel.ReviewDateTo != "") {
          this.reviewDateTo = this.dateUtils.convertDateToObject(this.documentModel.ReviewDateTo);;
        }

      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {

    if (this.promulgateDate != null && this.promulgateDate != "") {
      this.documentModel.PromulgateDate = this.dateUtils.convertObjectToDate(this.promulgateDate)
    }

    if (this.promulgateLastDate != null && this.promulgateLastDate != "") {
      this.documentModel.PromulgateLastDate = this.dateUtils.convertObjectToDate(this.promulgateLastDate)
    }

    if (this.reviewDateFrom != null && this.reviewDateFrom != "") {
      this.documentModel.ReviewDateFrom = this.dateUtils.convertObjectToDate(this.reviewDateFrom)
    }

    if (this.reviewDateTo != null && this.reviewDateTo != "") {
      this.documentModel.ReviewDateTo = this.dateUtils.convertObjectToDate(this.reviewDateTo)
    }

    let isOk = true;
    if (this.documentModel.PromulgateDate != null && this.documentModel.PromulgateDate != "" && this.documentModel.PromulgateLastDate != null && this.documentModel.PromulgateLastDate != "") {
      let promulgateDateValidate = new Date(this.documentModel.PromulgateDate);
      let promulgateLastDateValidate = new Date(this.documentModel.PromulgateLastDate);
      if (promulgateDateValidate > promulgateLastDateValidate) {
        this.messageService.showMessage("Ngày Phê duyệt lần đầu lớn hơn ngày ban hành hiện tại!");
        isOk = false;
      }
    }

    let reviewDateFromValidate = new Date(this.documentModel.ReviewDateFrom);
    let reviewDateToValidate = new Date(this.documentModel.ReviewDateTo);
    if (reviewDateFromValidate > reviewDateToValidate) {
      this.messageService.showMessage("Thời gian bắt đầu hiệu lực không được lớn hơn thời gian kết thúc hiệu lực!");
      isOk = false;
    }

    this.ngSelectComponent.itemsList.items.forEach(tag => {
      if (tag.value['IsNew']) {
        this.documentModel.DocumentTags.push(tag.value['Name']);
      }
    });

    if (isOk) {
      if (!this.id) {
        this.createDocument(isContinue);
      } else {
        this.updateDocument();
      }
    }

  }

  saveAndContinue() {
    this.save(true);
  }

  createDocument(isContinue: boolean) {
    this.documentService.create(this.documentModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới tài liệu thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới tài liệu thành công!');
          this.closeModal();
          // this.router.navigate(['nhan-vien/quan-ly-tai-lieu/them-moi', data]);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateDocument() {
    this.documentService.update(this.documentModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật tài liệu thành công!');
        this.closeModal();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  cancelPromulgate() {
    this.documentService.cancelPromulgate(this.documentModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Hủy ban hành thành công!');
        this.getInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  reviewDocument() {
    this.documentService.reviewDocument(this.documentModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Đổi trạng thái review thành công!');
        this.getInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.documentModel = {
      Id: '',
      DocumentGroupId: '',
      DocumentTypeId: '',
      Name: '',
      Code: '',
      Note: '',
      Version: '',
      CompilationType: 1,
      CompilationEmployeeId: '',
      CompilationSuppliserId: '',
      PromulgateDate: '',
      PromulgateLastDate: '',
      DepartmentId: '',
      EmployeeId: '',
      ReviewDateFrom: '',
      ReviewDateTo: '',
      Price: '',
      Keywords: [],
      Description: '',
      ApproveWorkTypeId: '',
      Status: 2,
      Devices: [],
      Modules: [],
      WorkTypes: [],
      Departments: [],
      Works: [],
      DocumentTags: []
    };
    this.reviewDateFrom = null;
    this.reviewDateTo = null;
    this.promulgateDate = null;
    this.promulgateLastDate = null;
  }

  chooseDevice() {
    let activeModal = this.modalService.open(ChooseProductToolComponent, { container: 'body', windowClass: 'select-product-tool-model', backdrop: 'static' })
    var ListIdSelect = [];
    this.documentModel.Devices.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code
          }
          this.documentModel.Devices.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteDevices(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị này không?").then(
      data => {
        this.documentModel.Devices.splice(index, 1);
        this.messageService.showSuccess('Xóa thiết bị thành công!');
      },
      error => {

      }
    );
  }

  chooseModule() {
    let activeModal = this.modalService.open(ProductChooseModuleComponent, { container: 'body', windowClass: 'productchoosemodulecomponent-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.documentModel.Modules.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code
          }
          this.documentModel.Modules.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteModule(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá module này không?").then(
      data => {
        this.documentModel.Modules.splice(index, 1);
        this.messageService.showSuccess('Xóa module thành công!');
      },
      error => {

      }
    );
  }

  chooseDepartment() {
    let activeModal = this.modalService.open(ChooseDepartmentComponent, { container: 'body', windowClass: 'choose-department-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.documentModel.Departments.forEach(element => {
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
            SBUId: element.SBUId,
            SBUName: element.SBUName,
          }
          this.documentModel.Departments.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteDepartment(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phòng ban này không?").then(
      data => {
        this.documentModel.Departments.splice(index, 1);
        this.messageService.showSuccess('Xóa phòng ban thành công!');
      },
      error => {

      }
    );
  }

  chooseWorkType() {
    let activeModal = this.modalService.open(ChooseWorktypeComponent, { container: 'body', windowClass: 'choose-worktype-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.documentModel.WorkTypes.forEach(element => {
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
          }
          this.documentModel.WorkTypes.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteWorkType(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vị trí công việc này không?").then(
      data => {
        this.documentModel.WorkTypes.splice(index, 1);
        this.messageService.showSuccess('Xóa vị trí công việc thành công!');
      },
      error => {

      }
    );
  }

  chooseTask() {
    let activeModal = this.modalService.open(ChooseWorkComponent, { container: 'body', windowClass: 'choose-work-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.documentModel.Works.forEach(element => {
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
          }
          this.documentModel.Works.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteTask(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá công việc này không?").then(
      data => {
        this.documentModel.Works.splice(index, 1);
        this.messageService.showSuccess('Xóa công việc thành công!');
      },
      error => {

      }
    );
  }

  closeModal() {
    this.router.navigate(['tai-lieu/quan-ly-tai-lieu']);
  }

  onNewPromugate() {
    this.getInfo();
  }

  addTagFn(name) {
    return { Name: name, tag: true, IsNew: true };
  }

}
