import { Component, OnInit, Input } from '@angular/core';
import { Constants, MessageService, FileProcess, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModuleSketchesService } from '../../services/module-sketches-service';
import { SketchesChooseMaterialComponent } from '../sketches-choose-material/sketches-choose-material.component';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SketchesHistoryComponent } from '../sketches-history/sketches-history.component';
import { Router } from '@angular/router';
import { SketchesImportMaterialComponent } from '../sketches-import-material/sketches-import-material.component';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-sketches',
  templateUrl: './sketches.component.html',
  styleUrls: ['./sketches.component.scss']
})
export class SketchesComponent implements OnInit {
  @Input() moduleId: string;
  constructor(
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    private moduleSketchesService: ModuleSketchesService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private config: Configuration,
    private router: Router,
    private serviceHistory: HistoryVersionService
  ) { }

  modelSketches: any = {
    Id: '',
    ModuleId: '',
    ListSketchMaterialElectronic: [],
    ListSketchMaterialMechanical: [],
    ListFileSketches: [],
    ListHistory: [],
    ListDelete: [],
  }

  fileModel = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    Note: '',
  }
  listFile: any[] = [];
  listSketchFunction: any = [];
  listSketchMaterialElectronic: any = [];
  listSketchMaterialMechanical: any = [];
  sketchesId: string;

  ngOnInit() {
    this.modelSketches.ModuleId = this.moduleId;
    this.getSketchesInfo();
    this.searchSketchesMaterialElectronic();
    this.searchSketchesMaterialMechanical();
  }

  getSketchesInfo() {
    this.moduleSketchesService.getSketchesInfo(this.modelSketches).subscribe(data => {
      this.modelSketches.ListFileSketches = data.ListFileSketches;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  uploadFileClick($event) {
    // this.fileProcess.onFileChange($event);
    // for (var element of this.fileProcess.FilesDataBase) {
    //   var a = 0;
    //   for (var item of this.modelSketches.ListFileSketches) {
    //     if (item.FileName == element.name) {
    //       var b = a;
    //       this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //         data => {
    //           this.modelSketches.ListFileSketches.splice(b, 1);
    //         })
    //     };
    //     a++;
    //   }
    // };
    // if (this.fileProcess.totalbyte > 5000000) {
    //   this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
    // }
    // else {
    //   var file = Object.assign({}, this.fileModel);
    //   file.FileName = this.fileProcess.FilesDataBase[this.fileProcess.FilesDataBase.length - 1].name;
    //   file.FileSize = this.fileProcess.FilesDataBase[this.fileProcess.FilesDataBase.length - 1].size;
    //   this.modelSketches.ListFileSketches.push(file);
    // }
    // this.listFile = this.fileProcess.FilesDataBase;
    // this.fileProcess.FilesDataBase = [];
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.modelSketches.ListFileSketches) {
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
      for (let index = 0; index < this.modelSketches.ListFileSketches.length; index++) {

        if (this.modelSketches.ListFileSketches[index].Id != null) {
          if (file.name == this.modelSketches.ListFileSketches[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.modelSketches.ListFileSketches.splice(index, 1);
            }
          }
        }
        else if (file.name == this.modelSketches.ListFileSketches[index].name) {
          isExist = true;
          if (isReplace) {
            this.modelSketches.ListFileSketches.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.modelSketches.ListFileSketches.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.modelSketches.ListFileSketches.push(file);
    }
  }

  updateSketchesFile() {
    var listFileUpload = [];
    this.modelSketches.ListFileSketches.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    this.uploadService.uploadListFile(this.modelSketches.ListFileSketches, 'SketchesFile/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.Note = listFileUpload[index].Note;
          this.modelSketches.ListFileSketches.push(file);
        });
      }
      for (var item of this.modelSketches.ListFileSketches) {
        if (item.Path == null || item.Path == "") {
          this.modelSketches.ListFileSketches.splice(this.modelSketches.ListFileSketches.indexOf(item), 1);
        }
      }
      this.moduleSketchesService.addFileSketches(this.modelSketches).subscribe(
        () => {
          this.messageService.showSuccess('Lưu file thành công!');
          this.getSketchesInfo();
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    this.updateSketchesFile();
  }

  showConfirmDeleteFile(index, row) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.modelSketches.ListFileSketches.splice(index, 1);
        this.modelSketches.ListDelete.push(row);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }

  // Hiển thị popup chọn material
  showSelectMaterial(IsRequest) {
    if (!IsRequest) {
      let activeModal = this.modalService.open(SketchesChooseMaterialComponent, { container: 'body', windowClass: 'sketcheschoosematerialcomponent-model', backdrop: 'static' });
      var ListMaterialIdSelect = [];
      this.listSketchMaterialElectronic.forEach(element => {
        ListMaterialIdSelect.push(element.Id);
      });

      activeModal.componentInstance.listIdSelect = ListMaterialIdSelect;
      activeModal.componentInstance.isRequest = IsRequest;
      activeModal.result.then((result) => {
        if (result && result.length > 0) {
          result.forEach(element => {
            this.listSketchMaterialElectronic.push(element);
          });
        }
      }, (reason) => {

      });
    } else {
      let activeModal = this.modalService.open(SketchesChooseMaterialComponent, { container: 'body', windowClass: 'sketcheschoosematerialcomponent-model', backdrop: 'static' });
      var ListMaterialMechanicalIdSelect = [];
      this.listSketchMaterialMechanical.forEach(element => {
        ListMaterialMechanicalIdSelect.push(element.Id);
      });

      activeModal.componentInstance.listIdMechanicalSelect = ListMaterialMechanicalIdSelect;
      activeModal.componentInstance.isRequest = IsRequest;
      activeModal.result.then((result) => {
        if (result && result.length > 0) {
          result.forEach(element => {
            this.listSketchMaterialMechanical.push(element);
          });
        }
      }, (reason) => {

      });
    }

  }

  // Xoá bảng Vật tư điện - điện tử
  showConfirmDeleteMaterialElectronic(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa vật tư này không?").then(
      data => {
        this.deleteMaterialElectronic(model);
      },
      error => {
        
      }
    );
  }

  deleteMaterialElectronic(Id: string) {
    this.moduleSketchesService.deleteSketchMaterialElectronic({ Id: Id }).subscribe(
      data => {
        this.searchSketchesMaterialElectronic();
        this.messageService.showSuccess('Xóa vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  // Xoá bảng Vật tư cơ khí
  showConfirmDeleteMaterialMechanical(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa vật tư này không?").then(
      data => {
        this.deleteMaterialMechanical(model);
      },
      error => {
        
      }
    );
  }

  deleteMaterialMechanical(Id: string) {
    this.moduleSketchesService.deleteSketchMaterialMechanical({ Id: Id }).subscribe(
      data => {
        this.searchSketchesMaterialMechanical();
        this.messageService.showSuccess('Xóa vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  DownloadAFile(file) {
    // if (!path) {
    //   this.messageService.showError("Không có file để tải");
    // }
    // var link = document.createElement('a');
    // link.setAttribute("type", "hidden");
    // link.href = this.config.ServerFileApi + path;
    // link.download = "aaaaa";
    // document.body.appendChild(link);
    // link.focus();
    // link.click();

    this.fileProcess.downloadFileBlob(file.Path, file.FileName);

  }

  showSketchVersion(Id: string) {
    let activeModal = this.modalService.open(SketchesHistoryComponent, { container: 'body', windowClass: 'sketcheshistorycomponent-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
  }


  closeModal(isOK: boolean) {
    this.router.navigate(['module/quan-ly-module']);
  }

  modelSketchMaterialElectronic: any = {
    Id: '',
    ModuleId: '',
    Note: '',
    Quantity: '',
    MaterialName: '',
    MaterialId: '',
    Leadtime: '',
  }

  modelSketchMaterialMechanical: any = {
    Id: '',
    ModuleId: '',
    Note: '',
    Quantity: '',
    MaterialName: '',
    MaterialId: '',
    Leadtime: '',
  }

  searchSketchesMaterialElectronic() {
    this.modelSketchMaterialElectronic.ModuleId = this.moduleId;
    this.moduleSketchesService.searchSketchesMaterialElectronic(this.modelSketchMaterialElectronic).subscribe((data: any) => {
      if (data.ListResult) {
        //this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listSketchMaterialElectronic = data.ListResult;
        this.modelSketchMaterialElectronic.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchSketchesMaterialMechanical() {
    this.modelSketchMaterialMechanical.ModuleId = this.moduleId;
    this.moduleSketchesService.searchSketchesMaterialMechanical(this.modelSketchMaterialMechanical).subscribe((data: any) => {
      if (data.ListResult) {
        //this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listSketchMaterialMechanical = data.ListResult;
        this.modelSketchMaterialMechanical.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }



  showImportPopup() {
    let activeModal = this.modalService.open(SketchesImportMaterialComponent, { container: 'body', windowClass: 'import-material', backdrop: 'static' })
    activeModal.componentInstance.ModuleId = this.moduleId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSketchesMaterialElectronic();
        this.searchSketchesMaterialMechanical();
      }
    }, (reason) => {
    });
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.moduleId;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Module;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save();
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
