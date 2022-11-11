import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NsMaterialGroupService } from '../../services/ns-material-group.service';
import { MessageService, FileProcess, Constants, AppSetting, ComboboxService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';

@Component({
  selector: 'app-ns-material-group-update',
  templateUrl: './ns-material-group-update.component.html',
  styleUrls: ['./ns-material-group-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NsMaterialGroupUpdateComponent implements OnInit {

  listAlphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
  listAlphabetBind = [];
  rowTable = [];
  count = 0;
  change = false;
  // temp = 0;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  model: any = {
    Id: '',
    ManufactureId: '',
    Code: '',
    Name: '',
    Description: '',
    ListParameter: [],
    ListFile: []
  };
  quantityParam = 0;
  preCode = '';
  listManufacture = [];
  listNSMaterialType = [];
  selectIndex = -1;
  index = 1;
  listSymbol = [];
  value = '';
  description = '';
  valueModel = {
    MaterialParameterId: '',
    Value: '',
    Description: ''
  }

  fileModel = {
    // Id: '',
    // Path: '',
    // FileName: '',
    // FileSize: '',
    // IsDelete: false,
    // CreateDate: new Date()
    Id: '',
    FilePath: '',
    FileName: '',
    FileSize: '',
    CreateBy: '',
    CreateDate: new Date(),
    UpdateBy: '',
    UpdateDate: ''
  }
  DateNow = new Date();
  constructor(
    private nsMaterialGroupService: NsMaterialGroupService,
    private messageService: MessageService,
    private router: Router,
    private routeA: ActivatedRoute,
    private comboboxService: ComboboxService,
    public fileProcess: FileProcess,
    public uploadService: UploadfileService,
    public constants: Constants,
    public appset: AppSetting,
  ) { }


  ngOnInit() {
    this.appset.PageTitle = "Nhóm vật tư phi tiêu chuẩn";
    this.getCbbManufacture();
    this.getCbbNSMaterialType();
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    if (this.model.Id) {
      this.getById();
    }
  }

  getById() {
    this.nsMaterialGroupService.getById(this.model).subscribe(
      data => {
        this.model = data;
        var strSplit = '';
        if (this.model.ListParameter[0].ConnectCharacter != '') {
          strSplit = this.model.Code.split(this.model.ListParameter[0].ConnectCharacter);
        }
        else {
          strSplit = this.model.Code.split('[');
        }
        if (strSplit.length > 0) {
          this.preCode = strSplit[0];
        }
        this.quantityParam = this.model.ListParameter.length;
        this.count = this.model.ListParameter.length;
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  loadValue(index) {
    this.selectIndex = index;
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

  checkValue = true;
  addRowValue() {
    this.checkValue = true;
    if (this.model.ListParameter[this.selectIndex].ListValue.length > 0) {
      this.model.ListParameter[this.selectIndex].ListValue.forEach(item => {
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
      this.model.ListParameter[this.selectIndex].ListValue.push(valueM);
      this.value = '';
      this.description = '';
    }
  }

  update() {
    var checkNameCount = false;
    var valueIsEmpty = false;
    var strTemp = '';
    var paramIsNull = false;
    if (this.model.ListParameter.length > 0) {
      this.model.ListParameter.forEach(item => {
        strTemp = item.Name;
        if (strTemp == '') {
          paramIsNull = true;
        }
        else {
          var count = 0;
          this.model.ListParameter.forEach(item => {
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
    else if (valueIsEmpty) {
      this.messageService.showMessage("Tồn tại thông số chưa có giá trị, hãy kiểm tra lại");
    }
    else if (this.change) {
      this.messageService.showMessage("Bạn chưa tạo lại quy tắc");
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
    // else if(this.model.Name == null){
    //   this.messageService.showMessage("Bạn không được để trống Tên thông số");
    // }
    else if (this.model.ManufactureId == null) {
      this.messageService.showMessage("Bạn phải chọn Hãng");
    }
    else {
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
        this.nsMaterialGroupService.updateNSMaterialGroup(this.model).subscribe((event: any) => {
          this.messageService.showSuccess("Chỉnh sửa nhóm vật tư phi tiêu chuẩn thành công!");
          this.close();
        }, error => {
          this.messageService.showError(error);
        });
      }, error => {
        this.messageService.showError(error);
      });
    }

    // this.uploadService.uploadListFile(this.ListFileDataSheet, 'DataSheet/').subscribe((event: any) => {
    //   if (event.length > 0) {
    //     event.forEach(item => {
    //       var file = Object.assign({}, this.fileDataSheet);
    //       file.FileName = item.FileName;
    //       file.Size = item.FileSize;
    //       file.FilePath = item.FileUrl;
    //       this.modelMaterial.ListFileDataSheet.push(file);
    //     });
    //   }

  }

  genCode() {
    this.model.Code = this.preCode;
    for (var i = 0; i < this.model.ListParameter.length; i++) {
      this.model.Code += this.model.ListParameter[i].ConnectCharacter + "[" + this.model.ListParameter[i].Code + "]";
    }
    this.change = false;
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
        if (data) {
          this.model.ListParameter[this.selectIndex].ListValue.splice(index, 1);
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

  quantityParamChange() {
    if (this.quantityParam > 26) {
      this.messageService.showMessage("Số lượng thông số chỉ được phép nhập từ 1-26");
    }
    else {
      if (this.count != this.quantityParam) {
        this.change = true;
        if (this.count < this.quantityParam) {
          for (var i = this.count; i < this.quantityParam; i++) {
            var obj = { Code: this.listAlphabet[i], Name: '', Unit: '', ConnectCharacter: '-', ListValue: [] };
            this.model.ListParameter.push(obj);
          }
        }
        else {
          var numberCharRemove = this.count - this.quantityParam;
          for (i = 0; i < numberCharRemove; i++) {
            this.model.ListParameter.pop();
          }
        }
      }
      this.count = this.quantityParam;
    }

  }

  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }
}
