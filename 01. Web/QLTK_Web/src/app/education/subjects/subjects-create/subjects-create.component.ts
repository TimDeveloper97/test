import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Configuration, Constants, FileProcess, ComboboxService } from 'src/app/shared';

import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SubjectsService } from '../../service/subjects.service';
import { SelectClassRoomComponent } from '../select-class-room/select-class-room/select-class-room.component';
import { SubjectSelectSkillComponent } from '../subject-select-skill/subject-select-skill.component';

@Component({
  selector: 'app-subjects-create',
  templateUrl: './subjects-create.component.html',
  styleUrls: ['./subjects-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SubjectsCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private subjectsService: SubjectsService,
    private config: Configuration,
    public constant: Constants,
    private modalService: NgbModal,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private comboboxService: ComboboxService,
    public fileProcess: FileProcess,
    public uploadfileService: UploadfileService,
  ) { }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa môn học';
      this.ModalInfo.SaveText = 'Lưu';
      this.getSubjectsInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới môn học";
    }
    this.getClassRoom();
    this.getDegree();
  }

  columnNameDegree: any[] = [{ Name: 'Code', Title: 'Mã trình độ' }, { Name: 'Name', Title: 'Tên trình độ' }];
  ModalInfo = {
    Title: 'Thêm mới môn học',
    SaveText: 'Lưu',
  };
  DegreeId: String;
  isAction: boolean = false;
  Id: string;
  ListFile = [];
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    RoomTypeId: '',
    DegreeId: '',
    SkillName: '',
    Documents: '',
    Address: '',
    ListSkill: [],
    ListMaterial: [],
    ListFile: [],
    ListClassRoom: []
  }

  fileListFileModuleManualDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
  }
  dateNow = new Date();

  getSubjectsInfo() {
    this.subjectsService.getSubjects({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.listBase = data.ListClassRoom;
      this.ListFile = data.ListFile;
      this.listSkill = data.ListSkill;
      // this.listBase = data.ListCourseSkill;
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }



  saveAndContinue() {
    this.save(true);
  }

  createSubjects(isContinue) {
    this.model.ListClassRoom = this.listBase;
    this.model.ListSkill = this.listSkill;
    var listFileUpload = [];
    this.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    this.uploadfileService.uploadListFile(this.ListFile, 'Subjects/').subscribe((event: any) => {
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
        this.addSubjects(isContinue);
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.addSubjects(isContinue);
          },
          error => {
            
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  addSubjects(isContinue) {
    this.model.ListClassRoom = this.listBase;
    this.model.ListSkill = this.listSkill;
    this.subjectsService.createSubjects(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
            DegreeId: '',
            RoomTypeId: '',
            SkillName: '',
            Address: '',
            ListSkill: [],
            ListMaterial: [],
            ListFile: [],
            ListClassRoom: []
          };
          this.listBase = [];
          this.messageService.showSuccess('Thêm mới môn học thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới môn học thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateSubjects();
    }
    else {
      this.createSubjects(isContinue);
    }
  }

  saveSubjects() {
    this.subjectsService.updateSubjects(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật môn học thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  updateSubjects() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    var listFileUpload = [];

    this.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    this.uploadfileService.uploadListFile(this.ListFile, 'Subjects/').subscribe((event: any) => {
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
        this.saveSubjects();
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
            this.saveSubjects();
          },
          error => {
            
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });

  }
  ListClassRoom = [];
  getClassRoom() {
    this.comboboxService.getListClassRoom().subscribe(
      data => {
        this.ListClassRoom = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  listBase: any[] = [];
  showSelectClassRoom() {
    let activeModal = this.modalService.open(SelectClassRoomComponent, { container: 'body', windowClass: 'select-ClassRoom-model', backdrop: 'static' });
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

  showConfirmDeleteClassRoom(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phòng học này không?").then(
      data => {
        this.removeRowClassRoom(row);
      },
      error => {
        
      }
    );
  }
  removeRowClassRoom(row) {
    var index = this.listBase.indexOf(row);
    if (index > -1) {
      this.listBase.splice(index, 1);
    }
  }

  ListDegree = [];
  getDegree() {
    this.comboboxService.getListDegree().subscribe(
      data => {
        this.ListDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
  listSkill: any[] = [];

  showSelectSkill() {
    let activeModal = this.modalService.open(SubjectSelectSkillComponent, { container: 'body', windowClass: 'subject-select-skill-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listSkill.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listSkill.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteSkill(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phòng học này không?").then(
      data => {
        this.removeRowSkill(row);
      },
      error => {
        
      }
    );
  }
  removeRowSkill(row) {
    var index = this.listSkill.indexOf(row);
    if (index > -1) {
      this.listSkill.splice(index, 1);
    }
  }
}
