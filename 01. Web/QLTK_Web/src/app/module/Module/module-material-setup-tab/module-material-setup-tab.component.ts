import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { MessageService, FileProcess, Constants, AppSetting, Configuration } from '../../../shared';
import { ModuleServiceService } from '../../services/module-service.service';
import { ModuleCreateService } from '../../services/module-create-service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ModuleMaterialService } from '../../services/module-material.service';
import { ProductMaterialsService } from 'src/app/device/services/product-materials.service';
import { MaterialService } from 'src/app/material/services/material-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';

@Component({
  selector: 'app-module-material-setup-tab',
  templateUrl: './module-material-setup-tab.component.html',
  styleUrls: ['./module-material-setup-tab.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleMaterialSetupTabComponent implements OnInit {

  @Input() Id: string
  constructor(
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private router: Router,
    private config: Configuration,
    private serviceProductmaterials: ProductMaterialsService,
    private uploadService: UploadfileService,
    private serviceModuleMaterial: ModuleServiceService,
    private moduleCreateService: ModuleCreateService,
    private moduleMaterialService: ModuleMaterialService,
    private materialService: MaterialService,
    private modalService: NgbModal,
    private serviceHistory: HistoryVersionService
  ) { }

  listData: any[] = [];
  ListFileSetup = [];
  ListFileDataSheet = [];
  ListFileModuleManualDocument = [];
  manufactureId: string;
  indexDataSheet: number;

  // moduleMaterialModel: any = {
  //   Id: '',
  //   ModuleId: '',
  //   MaterialId: '',
  //   MaterialCode: '',
  //   MaterialName: ''
  // }
  modelModule: any = {
    ModuleId: ''
  }

  modelModuleMaterial: any = {
    Id: '',
    ListFileSetup: [],
    ListFileDataSheet: [],
    ModuleId: '',
    ManufactureId: '',
    MaterialId: '',
  }

  model: any = { ListDatashet: [] }

  fileListFileModuleManualDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: ''
  }

  fileSetup: any = {
    Id: '',
    FilePath: '',
    FileName: '',
    Size: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: ''
  }

  ngOnInit() {
    this.modelModule.ModuleId = this.Id;
    this.modelModuleMaterial.ModuleId = this.Id;
    this.searchModuleMaterial();
  }

  searchModuleMaterial() {
    this.serviceModuleMaterial.searchModuleMaterialsSetup(this.modelModule).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        if (this.selectIndex != -1) {
          this.loadParam(this.selectIndex, this.listData[this.selectIndex].Id, this.listData[this.selectIndex].ManufacturerId, this.listData[this.selectIndex].MaterialId);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  fileDataSheet = {
    Id: '',
    FilePath: '',
    FileName: '',
    Size: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: ''
  }

  save() {
    this.uploadService.uploadListFile(this.modelModuleMaterial.ListFileSetup, 'FileStepUp/').subscribe((data: any) => {
      if (data.length > 0) {
        data.forEach((item, index) => {
          var file = Object.assign({}, this.fileSetup);
          file.FileName = item.FileName;
          file.Size = item.FileSize;
          file.FilePath = item.FileUrl;
          this.modelModuleMaterial.ListFileSetup.push(file);
        });
      }

      for (var item of this.modelModuleMaterial.ListFileSetup) {
        if (item.FilePath == undefined && item.FilePath == null || item.FilePath == "") {
          this.modelModuleMaterial.ListFileSetup.splice(this.modelModuleMaterial.ListFileSetup.indexOf(item), 1);
        }
      }

      this.uploadService.uploadListFile(this.modelModuleMaterial.ListFileDataSheet, 'DataSheet/').subscribe((data: any) => {
        if (data.length > 0) {
          data.forEach((item, index) => {
            var file = Object.assign({}, this.fileDataSheet);
            file.FileName = item.FileName;
            file.Size = item.FileSize;
            file.FilePath = item.FileUrl;
            this.modelModuleMaterial.ListFileDataSheet.push(file);
          });
        }
        for (var item of this.modelModuleMaterial.ListFileDataSheet) {
          if (item.FilePath == undefined && item.FilePath == null || item.FilePath == "") {
            this.modelModuleMaterial.ListFileDataSheet.splice(this.modelModuleMaterial.ListFileDataSheet.indexOf(item), 1);
          }
        }

        this.moduleMaterialService.UpdateFileModuleMaterial(this.modelModuleMaterial).subscribe(
          data => {
            if (this.ListFileDesign3D.length > 0) {
              this.modelModuleMaterial.ListFileSetup = [];
              this.ListFileSetup = [];
            }
            if (this.ListFileDataSheet.length > 0) {
              this.modelModuleMaterial.ListFileDataSheet = [];
              this.ListFileDataSheet = [];
            }
            this.messageService.showSuccess('Cập nhật vật tư thành công!');
            this.searchModuleMaterial();

          },
          error => {
            this.messageService.showError(error);
          }
        );

      }, error => {
        this.messageService.showError(error);
      });
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.router.navigate(['module/quan-ly-module']);
  }

  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  DownloadAFileDataSheet(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  DownloadAllFileSetup(Setup: any) {
    if (Setup.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Setup.forEach(element => {
      listFilePath.push({
        Path: element.FilePath,
        FileName: element.FileName
      });
    });
    this.model.Name = "Setup";
    this.model.ListDatashet = listFilePath;
    this.serviceProductmaterials.downAllDocumentProcess(this.model).subscribe(data => {
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

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Datashet.forEach(element => {
      listFilePath.push({
        Path: element.FilePath,
        FileName: element.FileName
      });
    });
    this.model.Name = "Datasheet";
    this.model.ListDatashet = listFilePath;
    this.serviceProductmaterials.downAllDocumentProcess(this.model).subscribe(data => {
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

  selectIndex = -1;
  ListFileDesign3D: any = [];
  DateNow = new Date();
  UserName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;

  Idtemp: string;
  loadParam(index, Id: string, ManufactureId: string, MaterialId: string) {
    if (this.Idtemp == Id) {

    }
    else {
      // for (var item of this.listData[index].ListDatashet) {
      //   if (item.FilePath == undefined && item.FilePath == null || item.FilePath == "") {
      //     this.listData[index].ListDatashet.splice(this.listData[index].ListDatashet.indexOf(item), 1);
      //   }
      // }
      // for (var item of this.listData[index].ListFileSetup) {
      //   if (item.FilePath == undefined && item.FilePath == null || item.FilePath == "") {
      //     this.listData[index].ListFileSetup.splice(this.listData[index].ListFileSetup.indexOf(item), 1);
      //   }
      // }
      this.searchModuleMaterial();
    }

    this.selectIndex = index;
    this.modelModuleMaterial.ManufactureId = ManufactureId;
    this.modelModuleMaterial.Id = Id;
    this.modelModuleMaterial.MaterialId = MaterialId;
    this.modelModuleMaterial.ListFileDataSheet = this.listData[index].ListDatashet;
    this.modelModuleMaterial.ListFileSetup = this.listData[index].ListFileSetup;
    this.Idtemp = Id;
  }


  uploadFileClickSetup($event, ManufactureId, MaterialId, Id) {
    var fileSetUp = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileSetUp) {
      isExist = false;
      for (var ite of this.modelModuleMaterial.ListFileSetup) {
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
          this.updateFileAndReplaceManualDocument(fileSetUp, true);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileSetUp, false);
        });
    }
    else {
      this.updateFileSetUp(fileSetUp);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;

    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.modelModuleMaterial.ListFileSetup.length; index++) {

        if (this.modelModuleMaterial.ListFileSetup[index].Id != null) {
          if (file.name == this.modelModuleMaterial.ListFileSetup[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.modelModuleMaterial.ListFileSetup.splice(index, 1);
            }
          }
        }
        else if (file.name == this.modelModuleMaterial.ListFileSetup[index].name) {
          isExist = true;
          if (isReplace) {
            this.modelModuleMaterial.ListFileSetup.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.modelModuleMaterial.ListFileSetup.push(file);
      }
    }
  }

  updateFileSetUp(filesSetUp) {
    for (var file of filesSetUp) {
      file.IsFileUpload = true;
      file.FilePath = null;
      this.modelModuleMaterial.ListFileSetup.push(file);
    }
  }

  uploadFileClickDatasheet($event, ManufactureId, MaterialId, Id) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.modelModuleMaterial.ListFileDataSheet) {
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
          this.updateFileAndReplaceDataSheet(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceDataSheet(fileDataSheet, false);
        });
    }
    else {
      this.updateFileDataSheet(fileDataSheet);
    }
  }

  updateFileAndReplaceDataSheet(fileDataSheet, isReplace) {
    var isExist = false;

    for (var file of fileDataSheet) {
      for (let index = 0; index < this.modelModuleMaterial.ListFileDataSheet.length; index++) {

        if (this.modelModuleMaterial.ListFileDataSheet[index].Id != null) {
          if (file.name == this.modelModuleMaterial.ListFileDataSheet[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.modelModuleMaterial.ListFileDataSheet.splice(index, 1);
            }
          }
        }
        else if (file.name == this.modelModuleMaterial.ListFileDataSheet[index].name) {
          isExist = true;
          if (isReplace) {
            this.modelModuleMaterial.ListFileDataSheet.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.modelModuleMaterial.ListFileDataSheet.push(file);
      }
    }
  }

  updateFileDataSheet(filesDataSheet) {
    for (var file of filesDataSheet) {
      file.IsFileUpload = true;
      file.FilePath = null;
      this.modelModuleMaterial.ListFileDataSheet.push(file);
    }
  }

  showConfirmDeleteFileSetup(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.modelModuleMaterial.ListFileSetup.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  showConfirmDeleteFileDataSheet(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.modelModuleMaterial.ListFileDataSheet.splice(index, 1);
      },
      error => {
        
      }
    );
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
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Module;
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
