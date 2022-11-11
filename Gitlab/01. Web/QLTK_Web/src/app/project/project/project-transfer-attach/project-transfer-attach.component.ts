import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, Configuration, MessageService, Constants, FileProcess, DateUtils } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ProjectTransferAttachService } from '../../service/project-transfer-attach.service';

import { forkJoin } from 'rxjs';
import { ConfirmTransferComponent } from '../confirm-transfer/confirm-transfer.component';
import { RouterLinkWithHref } from '@angular/router';


@Component({
  selector: 'app-project-transfer-attach',
  templateUrl: './project-transfer-attach.component.html',
  styleUrls: ['./project-transfer-attach.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectTransferAttachComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private uploadService: UploadfileService,
    public constant: Constants,
    public fileProcess: FileProcess,
    public activeModal: NgbActiveModal,
    private service: ProjectTransferAttachService,
    private modalService: NgbModal,
    private dateUtils: DateUtils,

  ) { }

  dateNow = new Date();
  userName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
  listFile: any[] = [];
  model: any = {
    ProjectId: '',
    ListFile: [],
    ListProductTranfer: [],
    FileId: '',
    FileName: ''
  }
  listTemp: any[] = [];
  fileModel: any = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    CreateByName: '',
    CreateDate: Date,
  }

  ngOnInit() {
    this.fileProcess.FilesDataBase = [];
    this.model.ProjectId = this.Id;
    this.searchProjectTransferAttach();
    this.StatusTrangerProduct();
    this.getProjectProductToTranfer();
  }

  listProduct: any[] = [];

  searchProjectTransferAttach() {
    this.service.searchProjectTransferAttach({ ProjectId: this.Id }).subscribe((data: any) => {
      if (data.ListResult) {
        this.model.ListFile = data.ListResult;
        this.model.ListFile.forEach(element => {
          element.SignDateV = this.dateUtils.convertDateToObject(element.SignDate);
        });
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  uploadFileClick($event) {
    var fileData = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileData) {
      isExist = false;
      for (var ite of this.model.ListFile) {
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
          this.updateFileAndReplaceTranfer(fileData, true);
        }, error => {
          this.updateFileAndReplaceTranfer(fileData, false);
        });
    }
    else {
      this.updateFileTranfer(fileData);
    }
  }

  updateFileAndReplaceTranfer(fileTranfers, isReplace) {
    var isExist = false;
    let idtemp: '';
    for (var file of fileTranfers) {
      for (let index = 0; index < this.model.ListFile.length; index++) {
        if (file.name == this.model.ListFile[index].FileName) {
          isExist = true;
          if (isReplace) {
            // this.model.ListFile[index].IsDelete = true;
            idtemp = this.model.ListFile[index].Id;
            this.model.ListFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        var files = Object.assign({}, this.fileTransfer);
        files.Id = idtemp;
        files.FileName = file.name;
        files.FileSize = file.size;
        files.IsFileUpload = true;
        files.File = file;
        files.CreateDate = this.dateNow;
        files.IsDelete = false
        this.model.ListFile.push(files);
      }
    }
  }

  fileTransfer = {
    Id: '',
    FilePath: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: '',
    IsFileUpload: false,
    CreateDate: null,
    File: null,
    IsDelete: false,
    SignDate: null,
    SignDateV: null,
    NumberOfReport: ''

  };

  updateFileTranfer(fileTranfers) {

    for (var file of fileTranfers) {
      var fileTransfer = Object.assign({}, this.fileTransfer);
      fileTransfer.FileName = file.name;
      fileTransfer.FileSize = file.size;
      fileTransfer.IsFileUpload = true;
      fileTransfer.File = file;
      fileTransfer.CreateDate = this.dateNow;
      fileTransfer.IsDelete = false
      this.model.ListFile.push(fileTransfer);
    }
    let index = this.model.ListFile.length - 1;
    this.loadParam(index, this.model.ListFile[index].FileName, '');
  }


  addFile() {

    let listFile: any[] = [];
    let listFileUpload: any[] = [];


    this.model.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        document.Index = index;
        listFile.push(document.File);
        listFileUpload.push(document);
      }
      if (document.SignDateV) {
        document.SignDate = this.dateUtils.convertObjectToDate(document.SignDateV);
      }
    });


    this.uploadService.uploadListFile(listFile, 'ProjectTransferAttach/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          this.model.ListFile[listFileUpload[index].Index].FilePath = item.FileUrl;
        });
      }

      // if (event.length > 0) {
      //   event.forEach((item, index) => {
      //     var file = Object.assign({}, this.fileModel);
      //     file.FileName = item.FileName;
      //     file.FileSize = item.FileSize;
      //     file.Path = item.FileUrl;
      //     file.Type = item.Type;
      //     this.model.ListFile.push(file);
      //   });
      // }
      // for (var item of this.model.ListFile) {
      //   if (item.Path == null || item.Path == "") {
      //     this.model.ListFile.splice(this.model.ListFile.indexOf(item), 1);
      //   }
      // }
      this.service.addProjectTransferAttach(this.model).subscribe(
        () => {
          this.model.ListFile = [];
          this.messageService.showSuccess('Cập nhật biên bản chuyển giao thành công!');
          this.searchProjectTransferAttach();
          this.StatusTrangerProduct();

          let activeModal = this.modalService.open(ConfirmTransferComponent, { container: 'body', windowClass: 'confirm-transfer-model', backdrop: 'static' })
          activeModal.componentInstance.ProjectId = this.Id;
          activeModal.result.then((result) => {
            if (result) {
              this.searchProjectTransferAttach();
            }
            this.loadParam(this.selectIndex, this.model.ListFile[this.selectIndex].FileName, this.model.ListFile[this.selectIndex].Id);
          }, (reason) => {
          });

        }, error => {
          this.messageService.showError(error);
        });
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    let index = 0
    if (this.selectIndex == -1) {
      this.model.ListProductTranfer = [];
    }
    else {
      this.model.ListProductTranfer = [];
      this.listProjectProduct.forEach(element => {
        if (element.Checked && !element.IsDisabled) {
          this.model.ListProductTranfer.push(element);
        }
      });
    }
    this.addFile();
  }

  showConfirmDeleteFile(index, Id) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa biên bản chuyển giao này không?").then(
      data => {
        if (Id == "" || Id == null) {
          this.model.ListFile.splice(index, 1);
        }
        else {
          this.model.ListFile[index].IsDelete = true;
        }
        this.messageService.showSuccess("Xóa file thành công!");
        this.selectIndex = -1;
        this.loadParam('', '', '');
      },
      error => {

      }
    );
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  listProjectProductId: any[] = [];

  totalProduct: number;
  totalProjectTranferProduct: number;
  statusProduct: '';

  StatusTrangerProduct() {
    this.service.StatusTrangerProduct(this.Id).subscribe(data => {
      if (data) {
        this.totalProduct = data.TotalProduct;
        this.totalProjectTranferProduct = data.TotalProjectTranferProduct
        this.statusProduct = data.Status;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  FileIdSelect: '';
  selectIndex = -1;

  loadParam(index, FileName, Id) {

    this.selectIndex = index;
    this.FileIdSelect = Id;
    this.model.FileId = Id;

    this.model.FileName = FileName;
    forkJoin([
      this.service.getProjectProductToTranfer(this.Id, this.FileIdSelect)]
    ).subscribe(([data1]) => {
      //this.listTemp = [];
      this.listProjectProduct = data1;
      this.listProjectProduct.forEach(element => {
        if (element.ProjectTransferAttachId != Id && element.Checked) {
          element.IsDisabled = true;
        } else {
          element.IsDisabled = false;
        }
      });
    });

  }

  listProjectProduct: any[] = [];

  getProjectProductToTranfer() {
    this.service.getProjectProductToTranfer(this.Id, this.FileIdSelect).subscribe(data => {
      if (data) {
        this.listProjectProduct = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  // getListShow() {
  //   this.listProjectProduct.forEach(element => {
  //     this.listProjectProductId.forEach(el => {
  //       if (element.Id == el) {
  //         element.Checked = true;
  //       }
  //     });
  //   });
  // }

  StartIndex = 1;
  checked = false;

  selectAllFunction() {
    if (this.checked) {
      this.listProjectProduct.forEach(element => {
        element.Checked = true;
        // this.listTemp.push(element);
      });
    } else {
      this.listProjectProduct.forEach(element => {
        element.Checked = false;
        //this.listTemp = [];
      });
    }
  }

  selectFunction(event) {
    this.listProjectProduct.forEach(element => {
      if (event.Id == element.Id) {
        if (event.Checked) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      }
    });

  }

}
