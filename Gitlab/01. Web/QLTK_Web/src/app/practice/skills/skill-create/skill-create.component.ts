import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Configuration, MessageService, AppSetting, Constants, FileProcess, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SkillService } from '../service/skill.service';
import { ChoosePraciteComponent } from '../choose-pracite/choose-pracite.component';

@Component({
  selector: 'app-skill-create',
  templateUrl: './skill-create.component.html',
  styleUrls: ['./skill-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SkillCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private modalService: NgbModal,
    private messageService: MessageService,
    private serviceSkill: SkillService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public constants: Constants,
    public appset: AppSetting,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService
  ) { }

  ModalInfo = {
    Title: 'Thêm mới kỹ năng',
    SaveText: 'Lưu',
  };

  StartIndex = 1;
  isAction: boolean = false;
  Id: string;
  SkillGroupId: string;
  listData: any = [];
  listSkill: any[] = []
  listSkillGroup = [];
  listDegree = [];
  ListFile = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnNameDegree: any[] = [{ Name: 'Code', Title: 'Mã trình độ' }, { Name: 'Name', Title: 'Tên trình độ' }];
  model: any = {
    Id: '',
    SkillGroupId: null,
    DegreeId: '',
    Name: '',
    Code: '',
    Note: '',
    Description: '',
    ListData: [],
    ListFile: [],
  }

  fileModel = {
    Id: '',
    SkillId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
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
      this.model.SkillGroupId = this.SkillGroupId;
    }
  }

  getSkillInfo() {
    this.serviceSkill.getSkill({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  getCbSkilldGroup() {
    this.combobox.getCbbSkillGroup().subscribe(
      data => {
        this.listSkillGroup = data;
        // this.model.SkillGroupId = this.listSkillGroup[0].Id;
        // this.model.SkillGroupId = this.listSkillGroup;
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
        //this.model.DegreeId = this.listDegree[0].Id;
        // this.model.DegreeId = this.listDegree;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getPracticeSkill() {
    this.serviceSkill.getPracticeSkillInfo(this.model).subscribe((data: any) => {
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
      ListIdSelect.push(element.PracticeId);
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

  supCreate(isContinue) {
    this.serviceSkill.createSkill(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            SkillGroupId: null,
            DegreeId: '',
            Name: '',
            Code: '',
            Note: '',
            Description: '',
            ListData: [],
            ListFile: [],
          };
          this.listData = [];
          this.ListFile = [];
          this.messageService.showSuccess('Thêm kỹ năng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm kỹ năng thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createSkill(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    this.model.ListData = this.listData;
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
          this.ListFile.splice(this.ListFile.indexOf(item), 1);
        }
      }
      if (validCode) {
        this.supCreate(isContinue);
      } else {
        this.supCreate(isContinue);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.messageService.showMessage("Có lỗi trong quá trình xử lý");
    }
    else {
      this.createSkill(isContinue);
    }
  }
  uploadFileClick($event) {
    //   var dataProcess = this.fileProcess.getFileOnFileChange($event);
    //   for (var item of dataProcess) {
    //     var a = 0;
    //     for (var ite of this.model.ListFile) {
    //       if (ite.FileName == item.name) {
    //         var b = a;
    //         this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //           data => {
    //             this.model.ListFile.splice(b, 1);
    //             this.ListFile.splice(b, 1);
    //           });
    //       }
    //       a++;
    //     }
    //     this.ListFile.push(item);
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

  showConfirmDelete(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa bài thực hành này không?").then(
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

  DownloadAFile(path: string) {
    if (!path) {
      this.messageService.showError("Không có file để tải");
    }
    this.serviceSkill.DownloadAFile({ Path: path }).subscribe(() => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + path;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, error => {
      this.messageService.showError(error);
    });
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

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
