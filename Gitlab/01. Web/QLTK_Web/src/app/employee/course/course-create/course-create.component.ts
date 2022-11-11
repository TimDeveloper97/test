import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, FileProcess, DateUtils, Configuration } from 'src/app/shared';
import { CourseService } from '../../service/course.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ChooseCourseSkillComponent } from '../choose-course-skill/choose-course-skill.component';
import { ChooseCourseEmployeeComponent } from '../choose-course-employee/choose-course-employee.component';
import { DxTreeListComponent } from 'devextreme-angular';

@Component({
  selector: 'app-course-create',
  templateUrl: './course-create.component.html',
  styleUrls: ['./course-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseCreateComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private messageService: MessageService,
    private courseService: CourseService,
    public constant: Constants,
    public fileProcess: FileProcess,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public uploadfileService: UploadfileService,
    public dateUtils: DateUtils,
    private config: Configuration,
  ) { }
  modalInfo = {
    Title: 'Thêm mới khóa học',
    SaveText: 'Lưu',
  };
  columnName: any[] = [{ Name: 'Code', Title: 'Mã khóa học' }, { Name: 'Name', Title: 'Tên khóa học' }]
  isAction: boolean = false;
  Id: string;
  treeBoxValue: string[];
  ListFile = [];
  minDateNotificationV: null;
  endCourse: any;
  startCourse: any;
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Status: false,
    StudyTime: 0,
    DeviceForCourse: '',
    ListFile: [],
    ListCourseSkill: [],
    StartDate: '',
    EndDate: '',
    disableDate: false,
    WorkSkillId:''
  }
  listData: [];
  results : any = {
    Id: '',
  }
  listCourseSkill = [];
  listBase: any[] = [];
  listFileCourse = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
  }
  Valid = false;
  disableData: false;
  ngOnInit() {
    this.searchSkill();
    this.getListParentCourse();
    if (this.Id) {
      this.Valid = true;
      this.model.disableData = this.disableData;
      this.modalInfo.SaveText = 'Lưu';
      this.getInfor();
    }
    else {
      this.modalInfo.Title = "Thêm mới khóa học";
    }
  }

  getListParentCourse() {
    this.courseService.getListParentCourse().subscribe(data => {
      this.results = data;
      if(this.Id){
        this.results.forEach(element =>{
          if(element.Id ==this.Id){
            this.results = this.results.filter(item => item.Id !== this.Id);
          }
        });
      }
    }, error => {
      this.messageService.showError(error);
    }
    );
  }

  getInfor() {
    this.courseService.getInforCourse({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.modalInfo.Title = 'Chỉnh sửa khóa học - ' + this.model.Code + " - " + this.model.Name;
      this.listData = data.ListData;
      this.ListFile = data.ListFile;
      this.treeBoxValue = data.ListId;
      this.selectedRowKeys = this.treeBoxValue;
      if (data.StartDate != null) {
        this.startCourse = this.dateUtils.convertDateToObject(data.StartDate);;
      }
      if (data.EndDate != null) {
        this.endCourse = this.dateUtils.convertDateToObject(data.EndDate);;
      }
    }, error => {
      this.messageService.showError(error);
    }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  StartIndex = 1;
  uploadFileClick($event) {
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

  createCourse(isContinue) {
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    var listFileUpload = [];
    this.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    this.uploadfileService.uploadListFile(this.ListFile, 'Course/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.listFileCourse);
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
      this.courseService.createCourse(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {
              Id: '',
              Name: this.model.Name,
              Code: this.model.Code,
              Status: false,
              StudyTime: 0,
              DeviceForCourse: '',
              ListFile: [],
              ListCourseSkill: [],
              StartDate: '',
              EndDate: ''
            };
            this.ListFile = [];
            this.messageService.showSuccess('Thêm mới phòng học thành công!');
          }
          else {
            this.listBase = [];
            this.messageService.showSuccess('Thêm mới phòng học thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }


  updateCourse() {
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    var listFileUpload = [];

    this.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });

    this.uploadfileService.uploadListFile(this.ListFile, 'Course/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.listFileCourse);
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
      this.courseService.updateCourse(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật khóa học thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateCourse();
    }
    else {
      this.createCourse(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  searchSkill() {
    this.model.WorkSkillId = this.Id;
    this.courseService.SearchCourseSkill(this.model).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
    }, error => {
      this.messageService.showError(error);
    })
  }

  selectedRowKeys: any[] = [];
  isDropDownBoxOpened = false;

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys;
    this.model.ListCourseSkill = e.selectedRowsData;
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;

  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  showConfirmDeleteFile(row) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.ListFile.splice(row, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }


  checkValid() {
    if (this.model.Name != "" && this.model.Name != null && this.model.Name != undefined) {
      this.Valid = true;
    }
    if (this.model.Code != "" && this.model.Code != null && this.model.Code != undefined) {
      this.Valid = true;
    }
  }

}
