import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { forkJoin } from 'rxjs';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils, FileProcess } from 'src/app/shared';
import { ScheduleProjectService } from '../../service/schedule-project.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';

@Component({
  selector: 'app-plan-adjustment',
  templateUrl: './plan-adjustment.component.html',
  styleUrls: ['./plan-adjustment.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PlanAdjustmentComponent implements OnInit {


  Status: any[] = [
    { Id: false, Name: "Chưa được chấp nhận" },
    { Id: true, Name: "Đã được chấp nhận" }
  ]

  fileTemplate: any = {
    Id: '',
    Name: "",
    Note: '',
    Type: '',
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,

    Name: '',
    ListAttach: [],
    Status: false,
    Version: 1,
  };

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo mã/tên ứng viên',
    Items: [

    ]
  };

  overallProject: any[] = []
  startIndex: number = 1;
  projectId: string;
  listStageOfProduct: any[] = [];

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private modalService: NgbModal,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private scheduleProjectService: ScheduleProjectService,
    private activeModal: NgbActiveModal,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private dateUtils: DateUtils) { }

  versionName = '';
  ngOnInit(): void {
    
  }

  searchRiskProblemProject() {
    this.searchModel.ProjectId = this.projectId;

    this.scheduleProjectService.searchRiskProblemProject(this.searchModel).subscribe(
      data => {
        this.overallProject = data.ListResult;
        this.listStageOfProduct = data.ListDayChange;
        this.searchModel.TotalItems = data.TotalItem;
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.searchModel = {

    };
    this.searchRiskProblemProject();
  }
  CloseModal() {
    this.activeModal.close(false);
  }
  uploadFile($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.searchModel.ListAttach) {
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
    let docuemntTemplate;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.searchModel.ListAttach.length; index++) {

        if (this.searchModel.ListAttach[index].Id != null) {
          if (file.name == this.searchModel.ListAttach[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.searchModel.ListAttach.splice(index, 1);
            }
          }
        }
        else if (file.name == this.searchModel.ListAttach[index].name) {
          isExist = true;
          if (isReplace) {
            this.searchModel.ListAttach.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        docuemntTemplate.FilePath = file.FilePath;
        this.searchModel.ListAttach.push(docuemntTemplate);
      }
    }
  }

  updateFileManualDocument(files) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, this.fileTemplate);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      this.searchModel.ListAttach.push(docuemntTemplate);
    }
  }

  save() {
    var listFileUpload = [];
    this.searchModel.ListAttach.forEach((document, index) => {
      if (document.File) {
        document.File.index = index;
        listFileUpload.push(document.File);
      }
    });
    if (listFileUpload.length > 0) {
      let filesUpload = this.uploadService.uploadListFile(listFileUpload, 'PlanHistories/');
      forkJoin([filesUpload]).subscribe(results => {
        if (results[0].length > 0) {
          listFileUpload.forEach((item, index) => {
            this.searchModel.ListAttach[item.index].FilePath = results[0][index].FileUrl;
          });

          this.create();
        }
      })
    }
    else {
      this.create();
    }
  }
  create() {
    this.searchModel.ProjectId = this.projectId;
    if (this.searchModel.AcceptDate) {
      this.searchModel.AcceptDate = this.dateUtils.convertObjectToDate(this.searchModel.AcceptDate);
    }
    this.scheduleProjectService.createPlanAdjustment(this.searchModel).subscribe(
      data => {

        this.messageService.showSuccess('Đổi kế hoạch thành công!');
        this.closeModal();

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.activeModal.close(true);
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.searchModel.ListAttach.splice(index, 1);
      },
      error => {

      }
    );
  }

  // downloadAFile(row: any) {
  //   this.fileProcess.downloadFileBlob(row.FilePath, row.FileName);
  // }

}
