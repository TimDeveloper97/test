import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Constants, FileProcess, Configuration, ComboboxService, PermissionService ,AppSetting} from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ClassRoomService } from '../../service/class-room.service';
import { SelectSkillComponent } from '../select-skill/select-skill/select-skill.component';
import { SelectMaterialComponent } from '../select-material/select-material/select-material.component';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SelectModuleComponent } from '../select-module/select-module.component';
import { SelectProductComponent } from '../select-product/select-product.component';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectPracticeComponent } from '../select-practice/select-practice.component';
import { ShowSelectProductComponent } from '../show-select-product/show-select-product.component';
import { ShowSelectModuleComponent } from '../show-select-module/show-select-module.component';

@Component({
  selector: 'app-class-room-create',
  templateUrl: './class-room-create.component.html',
  styleUrls: ['./class-room-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ClassRoomCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private classRoomService: ClassRoomService,
    private config: Configuration,
    public constant: Constants,
    private modalService: NgbModal,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private comboboxService: ComboboxService,
    public fileProcess: FileProcess,
    public uploadfileService: UploadfileService,
    public permissionService: PermissionService,
    private routeA: ActivatedRoute,
    private router: Router,
    private appSetting : AppSetting
  ) { }
  RoomTypeId: string;
  ngOnInit() {
    this.Id = this.routeA.snapshot.paramMap.get('Id');
    if (this.Id) {

      this.ModalInfo.SaveText = 'Lưu';
      this.getClassRoomInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới phòng học";
      this.model.RoomTypeId = this.RoomTypeId;
    }
    this.getRoomType();
  }

  ModalInfo = {
    Title: 'Thêm mới phòng học',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  ListRoomType = [];
  ListFile = [];
  ListModule = [];
  ListProduct =[];
  ListPractice = [];
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    RoomTypeId: '',
    SkillName: '',
    Address: '',
    ListModule: [],
    ListPractice:[],
    ListProduct:[],
    ListMaterial: [],
    ListFile: [],
  }

  fileListFileModuleManualDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
  }

  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  getClassRoomInfo() {
    this.classRoomService.getClassRoom({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.appSetting.PageTitle = 'Quản lý Phòng học - ' + this.model.Code + " - " + this.model.Name;
      this.listData = data.ListMaterial;
      this.ListFile = data.ListFile;
      this.ListModule = data.ListModule;
      this.ListProduct = data.ListProduct;
      this.ListPractice = data.ListPractice;
    });
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['phong-hoc/quan-ly-phong-hoc']);
  }

  saveAndContinue() {
    this.save(true);
  }

  createClassRoom(isContinue) {
  if(this.model.Code == '' || this.model.Code == null){
    this.messageService.showError("Mã không được bỏ trống!");
  }

  if(this.model.Name == '' || this.model.Name == null){
    this.messageService.showError("Tên không được bỏ trống!");
  }

  if(this.model.RoomTypeId == '' || this.model.RoomTypeId == null){
    this.messageService.showError("Loại phòng học/ line sản xuất không được bỏ trống!");
  }

    this.model.ListMaterial = this.listData;
    this.model.ListFile = this.ListFile;
    this.model.ListModule = this.ListModule;
    this.model.ListProduct = this.ListProduct;
    this.model.ListPractice = this.ListPractice;
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    this.uploadfileService.uploadListFile(this.ListFile, 'ClassRoom/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileListFileModuleManualDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      if (validCode) {
        this.addClassRoom(isContinue);
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.addClassRoom(isContinue);
          },
          error => {
            
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  addClassRoom(isContinue) {
    this.classRoomService.createClassRoom(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
            RoomTypeId: '',
            SkillName: '',
            Address: '',
            ListSkill: [],
            ListMaterial: [],
            ListFile: [],
          };
          this.messageService.showSuccess('Thêm mới phòng học thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới phòng học thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateClassRoom();
    }
    else {
      this.createClassRoom(isContinue);
    }
  }

  saveClassRoom() {
    this.classRoomService.updateClassRoom(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật phòng học thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateClassRoom() {
    //kiểm tra ký tự đặc việt trong Mã
    this.model.ListMaterial = this.listData;
    this.model.ListModule = this.ListModule;
    this.model.ListProduct = this.ListProduct;
    this.model.ListPractice = this.ListPractice;
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    this.uploadfileService.uploadListFile(this.ListFile, 'ClassRoom/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileListFileModuleManualDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (item.Path == null || item.Path == "") {
          this.ListFile.splice(this.ListFile.indexOf(item), 1);
        }
      }
      if (validCode) {
        this.saveClassRoom();
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
            this.saveClassRoom();
          },
          error => {
            
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  getRoomType() {
    this.comboboxService.getListRoomType().subscribe(
      data => {
        this.ListRoomType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  listBase: any[] = [];
  showSelectSkill() {
    let activeModal = this.modalService.open(SelectSkillComponent, { container: 'body', windowClass: 'select-skill-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
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

  // Chọn vật tư
  listData: any[] = [];
  showSelectMaterial() {
    let activeModal = this.modalService.open(SelectMaterialComponent, { container: 'body', windowClass: 'select-material-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {

    });
  }

  // Chọn module
  showSelectModule() {
    let activeModal = this.modalService.open(SelectModuleComponent, { container: 'body', windowClass: 'select-module-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {

    });
  }

  SelectModule() {
    let activeModal = this.modalService.open(ShowSelectModuleComponent, { container: 'body', windowClass: 'show-select-module-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];

    this.ListModule.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.ListModule.push(element);
        });
      }
    }, (reason) => {

    });
  }

  // Chọn thiết bị
  showSelectProduct() {
    let activeModal = this.modalService.open(SelectProductComponent, { container: 'body', windowClass: 'select-product-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {

    });
  }

  SelectProduct() {
    let activeModal = this.modalService.open(ShowSelectProductComponent, { container: 'body', windowClass: 'show-select-product-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];

    this.ListProduct.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.ListProduct.push(element);
        });
      }
    }, (reason) => {

    });
  }

  // Chọn bài thực hành
  showSelectPractice() {
    let activeModal = this.modalService.open(SelectPracticeComponent, { container: 'body', windowClass: 'select-practice-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];

    this.ListPractice.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.ListPractice.push(element);
        });
      }
    }, (reason) => {

    });
  }

  // Xóa vật tư
  showConfirmDeleteMaterial(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư này không?").then(
      data => {
        this.removeRowMaterial(row);
      },
      error => {
        
      }
    );
  }

  removeRowMaterial(row) {
    var index = this.listData.indexOf(row);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  // Xóa module
  showConfirmDeleteModule(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá module này không?").then(
      data => {
        this.removeRowMaterial(row);
      },
      error => {
        
      }
    );
  }

  removeRowModule(row) {
    var index = this.listData.indexOf(row);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  showConfirmDeleteModules(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá module này không?").then(
      data => {
        this.removeMaterial(row);
      },
      error => {
        
      }
    );
  }

  removeMaterial(row) {
    var index = this.ListModule.indexOf(row);
    if (index > -1) {
      this.ListModule.splice(index, 1);
    }
  }

  // Xóa thiết bị
  showConfirmDeleteProduct(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị này không?").then(
      data => {
        this.removeRowMaterial(row);
      },
      error => {
        
      }
    );
  }
  
  removeProduct(row) {
    var index = this.ListProduct.indexOf(row);
    if (index > -1) {
      this.ListProduct.splice(index, 1);
    }
  }

  showConfirmDeleteProducts(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị này không?").then(
      data => {
        this.removeProduct(row);
      },
      error => {
        
      }
    );
  }
  
  removeRowProduct(row) {
    var index = this.listData.indexOf(row);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  // Xóa bài thực hành
  showConfirmDeletePractice(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài thực hành này không?").then(
      data => {
        this.removeRowPractice(row);
      },
      error => {
        
      }
    );
  }
  
  removeRowPractice(row) {
    var index = this.ListPractice.indexOf(row);
    if (index > -1) {
      this.ListPractice.splice(index, 1);
    }
  }

  //UPLOAD FILE
  StartIndex = 1;
  uploadFileClick($event) {
    //   var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    //   for (var item of fileDataSheet) {
    //     var a=0;
    //     for(var ite of this.ListFile){
    //       if(ite.Id != null){
    //         if(item.name == ite.FileName){
    //           var b =a;
    //           this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //             data => {
    //               // this.model.ListFile.splice(b, 1);
    //               this.ListFile.splice(b, 1);
    //             });
    //         }
    //       }
    //       else{
    //         if(item.name == ite.name){
    //           var b =a;
    //           this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //             data => {
    //               // this.model.ListFile.splice(b, 1);
    //               this.ListFile.splice(b, 1);
    //             });
    //         }
    //       }

    //       a ++;
    //     }
    //     item.IsFileUpload = true;
    //     this.ListFile.push(item);
    //   }
    // }
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.ListFile) {
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
      for (let index = 0; index < this.ListFile.length; index++) {

        if (this.ListFile[index].Id != null) {
          if (file.name == this.ListFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.ListFile.splice(index, 1);
            }
          }
        }
        else if (file.name == this.ListFile[index].name) {
          isExist = true;
          if (isReplace) {
            this.ListFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.ListFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.ListFile.push(file);
    }
  }
  
  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.ListFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }
}