import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, MessageService, FileProcess, Constants, AppSetting, ComboboxService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { SkillService } from '../service/skill.service';
import { ChoosePraciteComponent } from '../choose-pracite/choose-pracite.component';

@Component({
  selector: 'app-skill-update',
  templateUrl: './skill-update.component.html',
  styleUrls: ['./skill-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SkillUpdateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private config: Configuration,
    private messageService: MessageService,
    private serviceSkill: SkillService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public constants: Constants,
    public appset: AppSetting,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService
  ) { }
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnNameDegree: any[] = [{ Name: 'Code', Title: 'Mã trình độ' }, { Name: 'Name', Title: 'Tên trình độ' }];
  ModalInfo = {
    Title: 'Chỉnh sửa kỹ năng',
    SaveText: 'Lưu',
  };
  StartIndex = 1;
  isAction: boolean = false;
  Id: string;
  listData: any = [];
  listFile: any[] = [];
  ListPractice: any = [];
  listProductStandard: any[] = []
  listSkill: any[] = []
  listSkillGroup = [];
  listDegree = [];
  ListFile = [];

  model: any = {
    Id: '',
    SkillGroupId: '',
    DegreeId: '',
    Name: '',
    Code: '',
    Note: '',
    Description: '',
    ListFile: [],
    ListData: [],
  }

  fileModel = {
    Id: '',
    SkillId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }

  modelPractice: any = {
    Id: '',
    SkillId: '',
    PracticeId: '',
    ListPractice: []
  }

  ngOnInit() {
    this.getCbSkilldGroup();
    this.getCbDegree();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa kỹ năng';
      this.ModalInfo.SaveText = 'Lưu';
      this.getSkillInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới kỹ năng";
    }
  }

  getSkillInfo() {
    this.serviceSkill.getSkill({ Id: this.Id }).subscribe(data => {
      //this.ListHistory = data.resultInfo.ListHistory;
      this.model = data;
      this.listData = data.ListData;
      this.ListFile = data.ListFile;
    }, error => {
      this.messageService.showError(error);
    }
    );
  }

  getCbSkilldGroup() {
    this.combobox.getCbbSkillGroup().subscribe(
      data => {
        this.listSkillGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbDegree() {
    this.combobox.getCbbDegree().subscribe(
      data => {
        this.listDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getPracticeSkill() {
    this.serviceSkill.getPracticeSkillInfo(this.modelPractice).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showClick() {
    let activeModal = this.modalService.open(ChoosePraciteComponent, { container: 'body', windowClass: 'ChoosePracite-model', backdrop: 'static' });
    activeModal.componentInstance.SkillId = this.model.Id;
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

  supUpdate() {
    this.serviceSkill.updateSkill(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật kỹ năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateSkill() {
    this.model.ListData = this.listData;
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    var listFileUpload = [];
    this.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    this.uploadService.uploadListFile(this.model.ListFile, 'Skill/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (item.Path == null || item.Path == "") {
          this.model.ListFile.splice(this.model.ListFile.indexOf(item), 1);
        }
      }
      if (validCode) {
        this.supUpdate();
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.supUpdate();
          },
          error => {
            
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });
  }


  save(isContinue: boolean) {
    if (this.Id) {
      this.updateSkill();
    }
    else {
      this.messageService.showMessage("Có lỗi trong quá trình xử lý");
    }
  }

  showConfirmDelete(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa bài thực hành/công đoạn này không?").then(
      data => {
        this.deletePractice(model);
      },
      error => {
        
      }
    );
  }

  deletePractice(model:any) {
    var index = this.listData.indexOf(model);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

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

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
