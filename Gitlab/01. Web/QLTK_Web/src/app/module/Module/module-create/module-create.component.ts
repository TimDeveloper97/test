import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { MessageService, AppSetting, Constants, FileProcess, Configuration, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ModuleCreateService } from '../../services/module-create-service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

import { ModuleGroupChooseStageComponent } from '../../modulegroup/module-group-choose-stage/module-group-choose-stage.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowDocumentComponent } from '../show-document/show-document.component';

@Component({
  selector: 'app-module-create',
  templateUrl: './module-create.component.html',
  styleUrls: ['./module-create.component.scss']
})
export class ModuleCreateComponent implements OnInit {

  constructor(
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private moduleCreateService: ModuleCreateService,
    private router: Router,
    public appSetting: AppSetting,
    public constant: Constants,
    public fileProcessDataSheet: FileProcess,
    private uploadService: UploadfileService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private modalService: NgbModal,
  ) { }
  modelModule: any = {
    Id: '',
    ModuleGroupId: '',
    Code: '',
    Name: '',
    Note: '',
    Status: 2,
    State: '',
    Specification: '',
    Pricing: 0,
    FileElectric: false,
    ElectricExist: '',
    FileElectronic: false,
    ElectronicExist: '',
    FileMechanics: false,
    MechanicsExist: '',
    IsSoftware: false,
    SoftwareExist: '',
    IsHMI: false,
    HMIExist: '',
    IsPLC: false,
    PLCExist: '',
    CurrentVersion: '',
    ThumbnailPath: '',
    IsFilm: false,
    FilmExist: '',
    Description: '',
    EditContent: '',
    ListStage: [],
    ListModuleManualDocument: [],
    ListImage: [],
    IsManual: false,
    ManualExist: ''
  }

  modelModuleProductionTime: any = {
    Id: '',
    StageId: '',
    ExecutionTime: ''
  }

  temp = {
    Id: '',
    StageId: '',
    ExecutionTime: 0
  };

  // Biến xử lý cho Combobox Nhóm module
  ListModuleGroup: any[] = [];
  ListModuleGroupId: any[] = []; // xổ toàn bộ nhóm cha con
  // treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  isAction: boolean = false;
  ListStage: any[] = []
  lengtCode: number;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  fileListFileModuleManualDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: '',
  }

  ListFileModuleManualDocument = [];

  ListImage = [];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  fileImage = {
    Id: '',
    ModuleId: '',
    FileName: '',
    FilePath: '',
    ThumbnailPath: '',
    Note: ''
  }
  ModuleGroupCode = '';
  EmployeeName = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeName;
  DepartmentName = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentName;

  ngOnInit() {
    this.appSetting.PageTitle = "Thêm mới Module nguồn";
    this.modelModule.ModuleGroupId = localStorage.getItem("selectedModuleGroupId");
    this.getCBBModuleGroup();
    //this.ListStage.push(this.modelModuleProductionTime);
    this.getListStageByGroupId(this.modelModule.ModuleGroupId);
    this.ModuleGroupCode = localStorage.getItem("ModuleGroupCode")
    if (this.ModuleGroupCode.includes("TPA")) {
      this.modelModule.FileMechanics = true;
      this.modelModule.FileElectric = true;
    } else if (this.ModuleGroupCode.includes("PCB")) {
      this.modelModule.FileElectronic = true;
    }

    this.galleryOptions = [
      {
        height: '350px',
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];

  }

  thumbnailActions: NgxGalleryOptions[];

  deleteImage(event, index): void {
    let imageDelete = this.galleryImages[index].medium;
    let indexDelete;
    this.modelModule.ListImage.forEach(element => {
      let url = this.config.ServerFileApi + element.ThumbnailPath;
      if (url == imageDelete) {
        indexDelete = this.modelModule.ListImage.indexOf(element);
      }
    });
    this.galleryImages.splice(index, 1);
    this.modelModule.ListImage.splice(indexDelete, 1);
  }

  uploadFileClickImage($event) {
    this.ListImage = [];
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.ListImage.push(item);
    }

    this.uploadService.uploadListFile(this.ListImage, 'ImageModule/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.FilePath = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
          this.modelModule.ListImage.push(file);
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.FileUrlThum,
            medium: this.config.ServerFileApi + item.FileUrlThum,
            big: this.config.ServerFileApi + item.FileUrl
          });
        });
      }
    }, error => {
      this.messageService.showError(error);
    });

  }
  // Tải file hướng dẫn sử dụng
  uploadFileClick($event) {
    var fileDataSheet = this.fileProcessDataSheet.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.ListFileModuleManualDocument) {
        if (ite.Id != null) {
          if (file.name == ite.FileName) {
            isExist = true;
          }
        }
        else {
          if (file.name == ite.name) {
            isExist = true;
          }
        }
      }
    }
    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
        data => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.ListFileModuleManualDocument.length; index++) {

        if (this.ListFileModuleManualDocument[index].Id != null) {
          if (file.name == this.ListFileModuleManualDocument[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.ListFileModuleManualDocument.splice(index, 1);
            }
          }
        }
        else if (file.name == this.ListFileModuleManualDocument[index].name) {
          isExist = true;
          if (isReplace) {
            this.ListFileModuleManualDocument.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.ListFileModuleManualDocument.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.ListFileModuleManualDocument.push(file);
    }
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.ListFileModuleManualDocument.splice(index, 1);
      },
      error => {

      }
    );
  }

  //Thêm mới module
  Valid: boolean;

  check() {
    var checkCode = this.modelModule.Code.length;
    if (this.modelModule.Name != '' && this.modelModule.Name != undefined && this.modelModule.Code != '' && this.modelModule.Code != undefined
      && this.modelModule.ModuleGroupId != '' && this.modelModule.ModuleGroupId != undefined
      && this.modelModule.Status != '' && this.modelModule.Status != undefined) {
      this.Valid = true;
    } else {
      this.Valid = false;
    }
  }

  moduleGroupChange() {
    var isExist = false;
    for (var item of this.ListModuleGroup) {
      if (this.modelModule.ModuleGroupId == item.Id && item.Code.includes('TPA')) {
        isExist = true;
      }
    }

    this.modelModule.IsManual = isExist;

    this.check();
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue: boolean) {
    this.modelModule.ListStage = this.ListStage;
    var validCode = this.checkSpecialCharacter.checkCode(this.modelModule.Code);
    if (validCode) {
      this.createModule(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.createModule(isContinue);
        },
        error => {

        });
    }
  }

  createModule(isContinue) {
    var listFileUpload = [];
    this.ListFileModuleManualDocument.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });

    this.uploadService.uploadListFile(this.ListFileModuleManualDocument, 'ModuleManualDocument/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileListFileModuleManualDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.FileType = "1";
          file.Note = listFileUpload[index].Note;
          this.modelModule.ListModuleManualDocument.push(file);
        });
      }
      this.ListFileModuleManualDocument.forEach(element => {
        if (element.IsDocument) {
          this.modelModule.ListModuleManualDocument.push(element);
        }
      });
      this.moduleCreateService.createModule(this.modelModule).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;

            this.modelModule.ModuleGroupId = localStorage.getItem("selectedModuleGroupId");
            for (var item of this.ListModuleGroup) {
              // this.ListModuleGroupId.push(item.Id);
              if (this.modelModule.ModuleGroupId == item.Id) {
                this.modelModule.Code = item.Code;
              }
            }
            //this.modelModule.Code.filter("TPA".includes(this.modelModule.Code));
            this.modelModule.Name = '';
            this.ListImage = [];
            this.ListFileModuleManualDocument = [];
            this.ListStage = [];
            this.galleryImages = [];
            //this.ListStage.push(this.temp);
            this.getListStageByGroupId(this.modelModule.ModuleGroupId);
            this.messageService.showSuccess('Thêm mới module nguồn thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới module nguồn thành công!');
            this.router.navigate(['module/quan-ly-module/chinh-sua-module/' + data]);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['module/quan-ly-module']);
  }

  selectIndex = -1;
  stageId = '';
  executionTime = 0;
  loadValue(index) {
    this.selectIndex = index;
  }

  getListStageByGroupId(ModuleGroupId) {
    this.moduleCreateService.getListStageByGroupModuleId({ ModuleGroupId: ModuleGroupId }).subscribe((data: any) => {
      if (data) {
        this.ListStage = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  confirmDeleteStage(i) {
    this.messageService.showConfirm("Bạn có muốn xóa công đoạn này không?").then(o => {
      if (o) {
        this.ListStage.splice(i, 1);
      }
    },
      error => {

      })
  }

  showStage() {
    let activeModal = this.modalService.open(ModuleGroupChooseStageComponent, { container: 'body', windowClass: 'module-choose-stage-model', backdrop: 'static' });
    var listIdSelect = [];
    this.ListStage.forEach(element => {
      listIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = listIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.ListStage.push(element);
        });
      }
    }, (reason) => {

    });
  }
  // Combobox Nhóm module
  getCBBModuleGroup() {

    this.comboboxService.getCbbModuleGroup().subscribe((data: any) => {
      if (data) {
        this.ListModuleGroup = data.ListResult;
        for (var item of this.ListModuleGroup) {
          this.ListModuleGroupId.push(item.Id);
          if (this.modelModule.ModuleGroupId == item.Id) {
            this.modelModule.Code = item.Code;

            if (item.Code.includes('TPA')) {
              this.modelModule.IsManual = true;
            }
          }
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showDocument() {
    let activeModal = this.modalService.open(ShowDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var listIdSelect = [];
    this.ListFileModuleManualDocument.forEach(element => {
      if (element.IsDocument) {
        listIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.componentInstance.groupCode = this.constant.GroupDocument.Module_DocumentHDSD;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.ListFileModuleManualDocument.push(element);
        });
      }
    }, (reason) => {

    });
  }
}
