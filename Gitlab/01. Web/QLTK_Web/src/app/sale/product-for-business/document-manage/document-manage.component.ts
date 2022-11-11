import { ChangeDetectorRef, Component, Input, OnInit, forwardRef } from '@angular/core';
import { Constants, MessageService, FileProcess } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SaleProductService } from '../sale-product.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-document-manage',
  templateUrl: './document-manage.component.html',
  styleUrls: ['./document-manage.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DocumentManageComponent),
    multi: true
  }
  ],
})
export class DocumentManageComponent implements OnInit {
  @Input() Id: string;

  public _listCatalog;
  get listCatalog(): any {
    return this._listCatalog;
  }
  @Input()
  set listCatalog(val: any) {
    this._listCatalog = val;
  }

  public _listFileSolution;
  get listFileSolution(): any {
    return this._listFileSolution;
  }
  @Input()
  set listFileSolution(val: any) {
    this._listFileSolution = val;
  }

  public _listTechnicalTraning;
  get listTechnicalTraning(): any {
    return this._listTechnicalTraning;
  }
  @Input()
  set listTechnicalTraning(val: any) {
    this._listTechnicalTraning = val;
  }

  public _listSaleTraining;
  get listSaleTraining(): any {
    return this._listSaleTraining;
  }
  @Input()
  set listSaleTraining(val: any) {
    this._listSaleTraining = val;
  }

  public _listUserManual;
  get listUserManual(): any {
    return this._listUserManual;
  }
  @Input()
  set listUserManual(val: any) {
    this._listUserManual = val;
  }

  public _listFixError;
  get listFixError(): any {
    return this._listFixError;
  }
  @Input()
  set listFixError(val: any) {
    this._listFixError = val;
  }

  public _listOtherDocument;
  get listOtherDocument(): any {
    return this._listOtherDocument;
  }
  @Input()
  set listOtherDocument(val: any) {
    this._listOtherDocument = val;
  }
  constructor(
    public constants: Constants,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private _cd: ChangeDetectorRef,
    public saleProductService: SaleProductService,
  ) { }
  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listCatalog = value;
    this.listFileSolution = value;
    this.listTechnicalTraning = value;
    this.listSaleTraining = value;
    this.listUserManual = value;
    this.listFixError = value;
    this.listOtherDocument = value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.modelListDocument = value;
    } else {
      this.modelListDocument = null;
    }

    this._cd.markForCheck();
  }
  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this._cd.markForCheck();
  }

  model: any = {
    Id: '',
    SaleProductId: '',
    ListFile: []
  }
  modelListDocument = {
    listCatalog: [],
    listFileSolution: [],
    listTechnicalTraning: [],
    listSaleTraining: [],
    listUserManual: [],
    listFixError: [],
    listOtherDocument: [],
  };
  fileModel: any = {
    Id: '',
    FileName: '',
    FileSize: '',
    FilePath: '',
    Note: '',
    Type: 0,
    File: null,
  }

  ngOnInit() {
    this.model.SaleProductId = this.Id;
    this.getDocumentInfo(this.Id);
  }

  getDocumentInfo(id) {
    this.saleProductService.getDocumentByProductId(id).subscribe((data: any) => {
      if (data) {
        data.forEach(element => {
          if (element.Type == 1) {
            this.listFileSolution.push(element)
            this.modelListDocument.listFileSolution.push(element)
          }
          if (element.Type == 2) {
            this.listCatalog.push(element)
            this.modelListDocument.listCatalog.push(element)
          }
          if (element.Type == 3) {
            this.listTechnicalTraning.push(element)
            this.modelListDocument.listTechnicalTraning.push(element)
          }
          if (element.Type == 4) {
            this.listSaleTraining.push(element)
            this.modelListDocument.listSaleTraining.push(element)
          }
          if (element.Type == 5) {
            this.listUserManual.push(element)
            this.modelListDocument.listUserManual.push(element)
          }
          if (element.Type == 6) {
            this.listFixError.push(element)
            this.modelListDocument.listFixError.push(element)
          }
          if (element.Type == 7) {
            this.listOtherDocument.push(element)
            this.modelListDocument.listOtherDocument.push(element)
          }
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  uploadFileSolution($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listFileSolution) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listFileSolution, 1);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listFileSolution, 1);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listFileSolution, 1);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listFileSolution, 1);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.modelListDocument.listFileSolution, 1);
      this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listFileSolution, 1);
    }
    this._onChange(this.modelListDocument);
  }

  uploadFileCatalog($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listCatalog) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listCatalog, 2);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listCatalog, 2);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listCatalog, 2);
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.modelListDocument.listCatalog, 2);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listCatalog, 2);
      this.updateFileManualDocument(fileDataSheet, this.modelListDocument.listCatalog, 2);
    }
    this._onChange(this.modelListDocument);
  }
  uploadFileTechnicalTraining($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listTechnicalTraning) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listTechnicalTraning, 3);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listTechnicalTraning, 3);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listTechnicalTraning, 3);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listTechnicalTraning, 3);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listTechnicalTraning, 3);
      this.updateFileManualDocument(fileDataSheet, this.modelListDocument.listTechnicalTraning, 3);
    }
    this._onChange(this.modelListDocument);
  }

  uploadFileSaleTraining($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listSaleTraining) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listSaleTraining, 4);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listSaleTraining, 4);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listSaleTraining, 4);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listSaleTraining, 4);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listSaleTraining, 4);
      this.updateFileManualDocument(fileDataSheet, this.modelListDocument.listSaleTraining, 4);
    }
    this._onChange(this.modelListDocument);
  }
  uploadFileUserManual($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listUserManual) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listUserManual, 5);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listUserManual, 5);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listUserManual, 5);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listUserManual, 5);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listUserManual, 5);
      this.updateFileManualDocument(fileDataSheet, this.modelListDocument.listUserManual, 5);
    }
    this._onChange(this.modelListDocument);
  }

  uploadFileFixError($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listFixError) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listFixError, 6);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listFixError, 6);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listFixError, 6);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listFixError, 6);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listFixError, 6);
      this.updateFileManualDocument(fileDataSheet, this.modelListDocument.listFixError, 6);
    }
    this._onChange(this.modelListDocument);
  }

  uploadFileOrther($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listOtherDocument) {
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
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.listOtherDocument, 7);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listOtherDocument, 7);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false, this.listOtherDocument, 7);
          this.updateFileAndReplaceManualDocument(fileDataSheet, true, this.modelListDocument.listOtherDocument, 7);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet, this.listOtherDocument, 7);
      this.updateFileManualDocument(fileDataSheet, this.modelListDocument.listOtherDocument, 7);
    }
    this._onChange(this.modelListDocument);
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace, listFile: any[], type: number) {
    var isExist = false;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < listFile.length; index++) {

        if (listFile[index].Id != null) {
          if (file.name == listFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              listFile.splice(index, 1);
            }
          }
        }
        else if (file.name == listFile[index].name) {
          isExist = true;
          if (isReplace) {
            listFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        var fileDocument = Object.assign({}, this.fileModel);
        fileDocument.FileName = file.name;
        fileDocument.FileSize = file.size;
        fileDocument.File = FileReader;
        file.Type = type;
        file.File = file
        listFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments, listFile: any[], type: number) {
    for (var file of fileManualDocuments) {
      file.Type = type;
      file.File = file;
      listFile.push(file);
    }
  }

  showConfirmDeleteFileSolution(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file giải pháp này không?").then(
      data => {
        this.listFileSolution.splice(index, 1);
        this.modelListDocument.listFileSolution.splice(index, 1);
        this._onChange(this.modelListDocument);
      },
      error => {

      }
    );
  }

  showConfirmDeleteCatalog(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file Catolog này không?").then(
      data => {
        this.listCatalog.splice(index, 1);
        this.modelListDocument.listCatalog.splice(index, 1);
        this._onChange(this.modelListDocument);
      },
      error => {

      }
    );
  }

  showConfirmDeleteTechnicalTraining(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đào tạo kỹ thuật này không?").then(
      data => {
        this.listTechnicalTraning.splice(index, 1);
        this.modelListDocument.listTechnicalTraning.splice(index, 1);
        this._onChange(this.modelListDocument);
      }
    );
  }

  showConfirmDeleteSaleTraining(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đào tạo sale này không?").then(
      data => {
        this.listSaleTraining.splice(index, 1);
        this.modelListDocument.listSaleTraining.splice(index, 1);
        this._onChange(this.modelListDocument);
      },
      error => {

      }
    );
  }

  showConfirmDeleteUserManual(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file hướng dẫn sử dụng này không?").then(
      data => {
        this.listUserManual.splice(index, 1);
        this.modelListDocument.listUserManual.splice(index, 1);
        this._onChange(this.modelListDocument);
      }
    );
  }

  showConfirmDeleteFixError(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file sửa lỗi này không?").then(
      data => {
        this.listFixError.splice(index, 1);
        this.modelListDocument.listFixError.splice(index, 1);
        this._onChange(this.modelListDocument);
      },
      error => {

      }
    );
  }

  showConfirmOrther(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa tài liệu khác này không?").then(
      data => {
        this.listOtherDocument.splice(index, 1);
        this.modelListDocument.listOtherDocument.splice(index, 1);
        this._onChange(this.modelListDocument);
      },
      error => {

      }
    );
  }

  downloadFile(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }
}
