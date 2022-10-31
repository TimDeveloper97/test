import { Component, OnInit, ElementRef, ViewChild, ViewEncapsulation } from '@angular/core';

import { MessageService, Constants, AppSetting, ComboboxService } from 'src/app/shared';
import { FileProcess } from 'src/app/shared/common/file-process';
import { NsMaterialGroupService } from '../../services/ns-material-group.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ns-material-group-create',
  templateUrl: './ns-material-group-create.component.html',
  styleUrls: ['./ns-material-group-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NsMaterialGroupCreateComponent implements OnInit {
  listAlphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
  listAlphabetBind = [];
  listSymbol = [];
  rowTable = [];
  quantityParam = 0;
  preCode = '';
  selectIndex = -1;
  value = '';
  description = '';
  listManufacture = [];
  listNSMaterialType = [];
  listFile = [];
  index = 1;
  DateNow = new Date();
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  valueModel = {
    MaterialParameterId: '',
    Value: '',
    Description: ''
  }

  model: any = {
    ManufactureId: '',
    Code: '',
    Name: '',
    Description: '',
    ListParameter: [],
    ListFile: []
  };

  fileModel = {
    // Id: '',
    // Path: '',
    // FileName: '',
    // FileSize: '',
    // IsDelete: false
    Id: '',
    FilePath: '',
    FileName: '',
    FileSize: '',
    CreateBy: '',
    CreateDate: new Date(),
    UpdateBy: '',
    UpdateDate: ''
  }

  @ViewChild('fileInput') fileInput: ElementRef;
  constructor(
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private nsMaterialGroupService: NsMaterialGroupService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private router: Router,
    public constants: Constants,
    public appset: AppSetting,
  ) { }

  ngOnInit() {
    this.appset.PageTitle = "Nhóm vật tư phi tiêu chuẩn";
    this.getCbbManufacture();
    this.getCbbNSMaterialType();
  }

  quantityParamChange() {
    if (this.quantityParam > 26) {
      this.messageService.showMessage("Số lượng thông số chỉ được phép nhập từ 1-26");
    }
    else {
      this.listAlphabetBind = [];
      this.rowTable = [];
      for (var i = 0; i < this.quantityParam; i++) {
        this.listAlphabetBind.push(this.listAlphabet[i]);
        var obj = { Code: this.listAlphabet[i], Name: '', Unit: '', ConnectCharacter: '', ListValue: [] };
        this.rowTable.push(obj);
      }
    }
  }

  genCode() {
    this.listSymbol = [];
    this.listAlphabetBind.forEach(item => {
      var symbol = (<HTMLInputElement>document.getElementsByName("Symbol" + item)[0]).value;
      this.listSymbol.push(symbol);
    });
    var code = this.preCode;
    for (var i = 0; i < this.quantityParam; i++) {
      code += this.listSymbol[i] + "[" + this.listAlphabetBind[i] + "]";
    }
    this.model.Code = code;
  }

  loadValue(index) {
    this.selectIndex = index;
  }

  checkValue = true;
  addRowValue() {
    this.checkValue = true;
    if (this.rowTable[this.selectIndex].ListValue.length > 0) {
      this.rowTable[this.selectIndex].ListValue.forEach(item => {
        if (item.Value == this.value) {
          this.checkValue = false;
        }
      });
    }
    if (this.value == '') {
      this.messageService.showMessage("Bạn không được để trống giá trị");
    }
    else if (this.value.length > 50) {
      this.messageService.showMessage("Giá trị vượt quá 50 ký tự cho phép, vui lòng kiểm tra lại");
    }
    else if (!this.checkValue) {
      this.messageService.showMessage("Giá trị đã tồn tại trong thông số, hãy kiểm tra lại");
    }
    else {
      var valueM = Object.assign({}, this.valueModel);
      valueM.Value = this.value;
      valueM.Description = this.description;
      this.rowTable[this.selectIndex].ListValue.push(valueM);
      this.value = '';
      this.description = '';
    }
  }

  getCbbManufacture() {
    this.comboboxService.getCbbManufacture().subscribe(
      data => {
        this.listManufacture = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbNSMaterialType() {
    this.comboboxService.getCbbNSMaterialType().subscribe(
      data => {
        this.listNSMaterialType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save() {
    var checkNameCount = false;
    var strTemp = '';
    var valueIsEmpty = false;
    var paramIsNull = false;
    if (this.rowTable.length > 0) {
      this.rowTable.forEach(item => {
        strTemp = item.Name;
        if (strTemp == '') {
          paramIsNull = true;
        }
        else {
          var count = 0;
          this.rowTable.forEach(item => {
            if (strTemp == item.Name) {
              count++;
            }
            if (count > 1) {
              checkNameCount = true;
            }
          });
        }
      });
    }
    if (paramIsNull) {
      this.messageService.showMessage("Bạn không được để trống Tên thông số");
    }
    else if (checkNameCount) {
      this.messageService.showMessage("Tên thông số đã tồn tại trong nhóm vật tư, hãy kiểm tra lại");
    }
    else if (this.model.Code == "") {
      this.messageService.showMessage("Bạn không được để trống Mã thông số");
    }
    else if (this.model.Name == "") {
      this.messageService.showMessage("Bạn không được để trống Tên");
    }
    else if (this.model.ManufactureId == "") {
      this.messageService.showMessage("Bạn phải chọn Hãng");
    }
    else {
      for (var i = 0; i < this.rowTable.length; i++) {
        this.rowTable[i].ConnectCharacter = this.listSymbol[i];
      }

      this.model.ListParameter = this.rowTable;
      this.uploadService.uploadListFile(this.model.ListFile, 'NSMaterialGroup/').subscribe((event: any) => {
        if (event.length > 0) {
          event.forEach(item => {
            var file = Object.assign({}, this.fileModel);
            file.FileName = item.FileName;
            file.FileSize = item.FileSize;
            file.FilePath = item.FileUrl;
            this.model.ListFile.push(file);
          });
        }
        for (var item of this.model.ListFile) {
          if (item.FilePath == null || item.FilePath == "") {
            this.model.ListFile.splice(this.model.ListFile.indexOf(item), 1);
          }
        }
        this.nsMaterialGroupService.createNSMaterialGroup(this.model).subscribe((event: any) => {
          this.messageService.showSuccess("Thêm mới vật tư phi tiêu chuẩn thành công!");
          this.close();
        }, error => {
          this.messageService.showError(error);
        });
      }, error => {
        this.messageService.showError(error);
      });

    }

  }

  listFiles = [];
  uploadFileClick($event) {
    // this.fileProcess.onFileChange($event);
    // if (this.fileProcess.totalbyte > 5000000) {
    //   this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
    // }
    // else {
    //   this.listFile = this.fileProcess.FilesDataBase;
    // }

    //   var dataProcess = this.fileProcess.getFileOnFileChange($event);
    //   for (var item of dataProcess) {
    //     var a = 0;
    //     for (var ite of this.model.ListFile) {
    //       if (ite.FileName == item.name) {
    //         var b = a;
    //         this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //           data => {
    //             this.model.ListFile.splice(b, 1);
    //             this.listFiles.splice(b, 1);
    //           });
    //       }
    //       a++;
    //     }
    //     this.listFiles.push(item);
    //     var file = Object.assign({}, this.fileModel);
    //     file.FileName = item.name;
    //     file.FileSize = item.size;
    //     this.model.ListFile.push(file);
    //   }
    // }

    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
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
      for (let index = 0; index < this.model.ListFile.length; index++) {

        if (this.model.ListFile[index].Id != null) {
          if (file.name == this.model.ListFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.model.ListFile.splice(index, 1);
            }
          }
        }
        else if (file.name == this.model.ListFile[index].name) {
          isExist = true;
          if (isReplace) {
            this.model.ListFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.model.ListFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.model.ListFile.push(file);
    }
  }


  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.model.ListFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }

  deleteValue(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá giá này?").then(
      data => {
        if (data == true) {
          this.rowTable[this.selectIndex].ListValue.splice(index, 1);
          this.messageService.showSuccess("Xóa giá trị thông số vật tư thành công!");
        }
      },
      error => {
        
      }
    );
  }

  close() {
    this.router.navigate(['vat-tu/nhom-vat-tu-phi-tieu-chuan']);
  }
}
