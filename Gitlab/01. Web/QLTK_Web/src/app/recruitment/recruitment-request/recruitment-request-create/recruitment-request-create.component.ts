import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { ComboboxService, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { RecruitmentRequestService } from '../../services/recruitment-request.service';

@Component({
  selector: 'app-recruitment-request-create',
  templateUrl: './recruitment-request-create.component.html',
  styleUrls: ['./recruitment-request-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RecruitmentRequestCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private recruitmentRequestService: RecruitmentRequestService,
    private combobox: ComboboxService,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private dateUtils: DateUtils,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
  ) { }

  modalInfo = {
    Title: 'Thêm mới kênh tuyển dụng',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;
  listDepartment: any[] = [];
  listWorkType: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];
  columnNameWorkType: any[] = [{ Name: 'Name', Title: 'Tên vị trí' }];

  requestModel: any = {
    Id: '',
    Code: '',
    PromulgateDate: '',
    Type: 1,
    Index: 0,
    ListAttach: []
  };

  fileTemplate: any = {
    Id: '',
    Note: null,
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  
  ngOnInit(): void {
    this.getCbbDepartment();
    this.getCbbWorkType();
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa Yêu cầu tuyển dụng';
      this.modalInfo.SaveText = 'Lưu';
      this.getById();
    }
    else {
      this.generateCode();
      this.modalInfo.Title = 'Thêm mới Yêu cầu tuyển dụng';
    }
  }

  generateCode() {
    this.recruitmentRequestService.generateCode().subscribe((data: any) => {
      if (data) {
        this.requestModel.Code = data.Code;
        this.requestModel.Index = data.Index;
      }
    });
  }

  getCbbDepartment() {
    this.comboboxService.getCbbDepartment().subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbWorkType() {
    this.comboboxService.getListWorkType().subscribe(
      data => {
        this.listWorkType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getById() {
    this.recruitmentRequestService.getById(this.id).subscribe(
      data => {
        this.requestModel = data;
        if (this.requestModel.RecruitmentDeadline) {
          this.requestModel.RecruitmentDeadline = this.dateUtils.convertDateToObject(this.requestModel.RecruitmentDeadline);
        }

        if (this.requestModel.RequestDate) {
          this.requestModel.RequestDate = this.dateUtils.convertDateToObject(this.requestModel.RequestDate);
        }

        if (this.requestModel.ApprovalDate) {
          this.requestModel.ApprovalDate = this.dateUtils.convertDateToObject(this.requestModel.ApprovalDate);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCompanySalaryById() {
    this.recruitmentRequestService.getWorkTypeSalaryById(this.requestModel.WorkTypeId).subscribe(
      data => {
        this.requestModel.MinCompanySalary = data.MinCompanySalary;
        this.requestModel.MaxCompanySalary = data.MaxCompanySalary;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.recruitmentRequestService.create(this.requestModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới Yêu cầu tuyển dụng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới Yêu tuyển dụng thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.recruitmentRequestService.update(this.id, this.requestModel).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật Yêu cầu tuyển dụng thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    var listFileUpload = [];
    this.requestModel.ListAttach.forEach((document, index) => {
      if (document.File) {
        document.File.index = index;
        listFileUpload.push(document.File);
      }
    });

    if (this.requestModel.RecruitmentDeadline) {
      this.requestModel.RecruitmentDeadline = this.dateUtils.convertObjectToDate(this.requestModel.RecruitmentDeadline);
    }

    if (this.requestModel.RequestDate) {
      this.requestModel.RequestDate = this.dateUtils.convertObjectToDate(this.requestModel.RequestDate);
    }

    if (this.requestModel.ApprovalDate) {
      this.requestModel.ApprovalDate = this.dateUtils.convertObjectToDate(this.requestModel.ApprovalDate);
    }

    if(this.requestModel.ApprovalDate < this.requestModel.RequestDate)
    {
      this.messageService.showMessage("Ngày phê duyệt không được nhỏ hơn ngày yêu cầu!");
    }
    else{
      if (listFileUpload.length > 0 || this.fileProcess.FilesDataBase) {
        this.uploadService.uploadListFile(listFileUpload, 'RecruitmentRequest/').subscribe(
          data => {
            if (data && data.length > 0) {
              listFileUpload.forEach((item, index) => {
                this.requestModel.ListAttach[item.index].FilePath = data[index].FileUrl;
              });
            }
            if (this.id) {
              this.update()
            }
            else {
              this.create(isContinue);
            }
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
      else {
        if (this.id) {
          this.update()
        }
        else {
          this.create(isContinue);
        }
      }
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.requestModel = {
      Code: '',
      Type: 1,
      Index: 0,
      ListAttach: []
    };
    this.generateCode();
  }

  uploadFile($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.requestModel.ListAttach) {
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
      for (let index = 0; index < this.requestModel.ListAttach.length; index++) {

        if (this.requestModel.ListAttach[index].Id != null) {
          if (file.name == this.requestModel.ListAttach[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.requestModel.ListAttach.splice(index, 1);
            }
          }
        }
        else if (file.name == this.requestModel.ListAttach[index].name) {
          isExist = true;
          if (isReplace) {
            this.requestModel.ListAttach.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        this.requestModel.ListAttach.push(docuemntTemplate);
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
      this.requestModel.ListAttach.push(docuemntTemplate);
    }
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  showConfirmDeleteFile(document, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.deleteFile(document, index);
      },
      error => {

      }
    );
  }

  deleteFile(document, index) {
    if (document.Id) {
      document.IsDelete = true;
    }
    else {
      this.requestModel.ListAttach.splice(index, 1);
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  cancelStatus() {
    this.recruitmentRequestService.cancelStatus(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật trạng thái thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  nextStatus() {
    this.recruitmentRequestService.nextStatus(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật trạng thái thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  backStatus() {
    this.recruitmentRequestService.backStatus(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật trạng thái thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmCancelStatus() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy yêu cầu tuyển dụng này không?").then(
      data => {
        this.cancelStatus();
      },
      error => {

      }
    );
  }


}
