import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { MessageService, AppSetting, Constants, FileProcess, Configuration, ComboboxService, PermissionService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ModuleCreateService } from '../../services/module-create-service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { ModuleGroupChooseStageComponent } from '../../modulegroup/module-group-choose-stage/module-group-choose-stage.component';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { ModuleUpdateContentComponent } from '../module-update-content/module-update-content.component';
import { ListPlanDesginComponent } from '../list-plan-desgin/list-plan-desgin.component';
import { ModuleServiceService } from '../../services/module-service.service';
import { ShowDocumentComponent } from '../show-document/show-document.component';
import { DownloadService } from 'src/app/shared/services/download.service';
@Component({
  selector: 'app-module-update',
  templateUrl: './module-update.component.html',
  styleUrls: ['./module-update.component.scss']
})
export class ModuleUpdateComponent implements OnInit {
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  constructor(
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private moduleCreateService: ModuleCreateService,
    private router: Router,
    private modalService: NgbModal,
    public appSetting: AppSetting,
    public constant: Constants,
    public fileProcessDataSheet: FileProcess,
    private uploadService: UploadfileService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private routeA: ActivatedRoute,
    public permissionService: PermissionService,
    private serviceHistory: HistoryVersionService,
    private service: ModuleServiceService,
    private dowloadService: DownloadService,
  ) { }

  modelModule: any = {
    Id: '',
    ModuleGroupId: '',
    Code: '',
    Name: '',
    Note: '',
    Status: '',
    State: '',
    Specification: '',
    Pricing: 0,
    FileElectric: '',
    ElectricExist: '',
    FileElectronic: '',
    ElectronicExist: '',
    FileMechanics: '',
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
    ManualExist: '',
    IsManual: ''
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
  //treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  isAction: boolean = false;
  currentVersion: string;
  editContent: string;
  ListCbbStage: any[] = [];
  ListStage: any[] = []
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
  Valid: boolean;
  dataStatus: boolean;

  ngOnInit() {
    this.Valid = true;
    this.appSetting.PageTitle = "Cập nhật Module nguồn";
    this.modelModule.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getCBBModuleGroup();
    this.getCBBStage();
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
    if (this.modelModule.Id) {
      this.getById();
    }
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

  check() {
    if (this.modelModule.Name != '' && this.modelModule.Name != undefined && this.modelModule.Code != '' && this.modelModule.Code != undefined
      && this.modelModule.ModuleGroupId != '' && this.modelModule.ModuleGroupId != undefined
      && this.modelModule.Status != '' && this.modelModule.Status != undefined) {
      this.Valid = true;
    } else {
      this.Valid = false;
    }
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
  getById() {
    this.moduleCreateService.getById(this.modelModule).subscribe(
      data => {
        this.modelModule = data;
        this.appSetting.PageTitle = "Cập nhật Module nguồn - " + this.modelModule.Code + " - " + this.modelModule.Name;
        this.currentVersion = this.modelModule.CurrentVersion;
        this.editContent = this.modelModule.EditContent;
        this.ListFileModuleManualDocument = data.ListModuleManualDocument;
        //this.treeBoxValue = data.ModuleGroupId;
        this.ListStage = data.ListStage;
        for (var item of data.ListImage) {
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.ThumbnailPath,
            medium: this.config.ServerFileApi + item.FilePath,
            big: this.config.ServerFileApi + item.FilePath
          });
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  // Combobox Thời gian sản xuất Stage
  getCBBStage() {
    this.comboboxService.getCbbStage().subscribe((data: any) => {
      if (data) {
        this.ListCbbStage = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  selectIndex = -1;
  stageId = '';
  executionTime = 0;
  loadValue(param, index) {
    this.selectIndex = index;
    // this.Value = '';
    if (!param.Id && !param.StageId) {
      this.ListStage[this.selectIndex] = [];
    }
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
      listIdSelect.push(element.StageId);
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
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['module/quan-ly-module']);
  }

  // Cập nhật module
  save(result: any) {
    this.modelModule.ListStage = this.ListStage;
    var validCode = this.checkSpecialCharacter.checkCode(this.modelModule.Code);
    if (validCode) {
      this.checkVersion(result);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.checkVersion(result);
        },
        error => {

        });
    }
  }

  checkVersion(result: any) {
    if (this.modelModule.CurrentVersion != this.currentVersion) {
      this.messageService.showConfirm("Bạn đã thay đổi version. Có lưu hay không?").then(
        data => {
          this.modelModule.CurrentVersion = this.currentVersion;
          this.modelModule.EditContent = this.editContent;
          this.updateModule(result);
        },
        error => {

        }
      );
    }
    else {
      this.updateModule(result);
    }
  }

  updateModule(result: any) {
    localStorage.setItem("selectedModuleGroupId", this.modelModule.ModuleGroupId);
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

      for (var item of this.modelModule.ListModuleManualDocument) {
        if (!item.Path && !item.IsDocument) {
          this.ListFileModuleManualDocument.splice(this.ListFileModuleManualDocument.indexOf(item), 1);
        }
      }
      this.moduleCreateService.updateModule(this.modelModule).subscribe(
        () => {
          //this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật module thành công!');
          if (this.ListImage.length > 0) {
            this.modelModule.ListImage = [];
            this.ListImage = [];
            this.galleryImages = [];
          }
          if (result) {
            this.update(result);
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

  DownloadAFile(file) {
    if (!file.IsDocument) {
      this.fileProcessDataSheet.downloadFileBlob(file.Path, file.FileName);
    } else {
      this.moduleCreateService.getListDocumentFile(file.Id).subscribe(
        data => {
          if (data && data.length > 0) {
            this.downAllDocumentFile(file, data);
          } else {
            this.messageService.showMessage("Không có file để tải");
            return;
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
  }

  downAllDocumentFile(row: any, files: any) {
    var listFilePath: any[] = [];
    files.forEach(element => {
      listFilePath.push({
        Path: element.Path,
        FileName: element.FileName
      });
    });
    var modelDowload = {
      Name: row.FileName,
      ListDatashet: listFilePath
    };

    if (!modelDowload.Name) {
      modelDowload.Name = row.name;
    }

    this.dowloadService.downloadAll(modelDowload).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
      //this.fileProcessDataSheet.downloadFileBlob(data.PathZip, row.FileName);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save(false);
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.modelModule.Id;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Module;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save(result);
      }
    }, (reason) => {
    });
  }

  update(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
        this.getById();
      }, error => {
        this.getById();
        this.messageService.showError(error);
      }
    );
  }

  showUpdateContent() {
    let activeModal = this.modalService.open(ModuleUpdateContentComponent, { container: 'body', windowClass: 'module-update-conten', backdrop: 'static' });
    activeModal.componentInstance.moduleId = this.modelModule.Id;
    activeModal.result.then((result) => {
      if (result) {
        this.getById();
      }
    }, (reason) => {
    });
  }

  isConfirm: boolean = false;
  syncSaleModule(isConfirm: boolean) {
    let list = [];
    list.push(this.modelModule.Id);
    this.service.syncSaleModule(false, isConfirm, list).subscribe(
      data => {
        if (data) {
          this.confirm(data);
        } else {
          this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  confirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.syncSaleModule(true);
      }
    );
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
