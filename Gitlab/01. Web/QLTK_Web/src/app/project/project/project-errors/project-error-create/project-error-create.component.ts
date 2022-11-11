import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, DateUtils, Constants } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ErrorService } from 'src/app/project/service/error.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectErrorConfirmComponent } from '../project-error-confirm/project-error-confirm.component';

@Component({
  selector: 'app-project-error-create',
  templateUrl: './project-error-create.component.html',
  styleUrls: ['./project-error-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectErrorCreateComponent implements OnInit {

  constructor(
    private combobox: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    private activeModal: NgbActiveModal,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    private serviceError: ErrorService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private router: Router,
    private routeA: ActivatedRoute,
    public constant: Constants
  ) { }

  Id: string;
  listFile: any[]=[];
  projectId: string;
  planStartDate: any;
  planFinishDate: any;
  listErrorGroup: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listProject: any[] = [];
  listModule: any[] = [];
  listProduct: any[] = [];
  listStage: any[] = [];
  columnModule: any[] = [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã vấn đề' }, { Name: 'Name', Title: 'Tên vấn đề' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnType: any[] = [{Name: 'Name', Title: 'Tên loại vấn đề' }];
  columnDeparterment: any[] = [{ Name: 'Code', Title: 'Mã bộ phận' }, { Name: 'Name', Title: 'Tên bộ phận' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  thumbnailActions: NgxGalleryOptions[];
  listImage = [];
  listType: any[] = [
    { Id: 1, Name: 'Lỗi' },
    { Id: 2, Name: 'Vấn đề' },
  ]

  modalInfo = {
    Title: 'Thêm mới vấn đề',
    SaveText: 'Lưu',
  };

  model: any = {
    Id: '',
    Subject: '',
    Code: '',
    Description: '',
    ErrorGroupId: '',
    DepartmentId: '',
    AuthorId: '',
    ErrorBy: '',
    PlanStartDate: null,
    PlanFinishDate: null,
    ObjectId: '',
    ModuleErrorVisualId: '',
    DepartmentProcessId: '',
    StageId: '',
    FixBy: '',
    ProjectId: '',
    Status: 1,
    strHistory: '',
    Type: null,
    Price: 0,
    ListImage: [],
    ObjectType: 1
  }

  fileImage = {
    Id: '',
    MaterialId: '',
    Path: '',
    ThumbnailPath: ''
  }

  fileModel = {
    Id: '',
    ErrorId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }

  ngOnInit() {
    this.model.Id = this.Id;
    this.model.ProjectId = this.projectId;
    this.getCbbDepartment();
    this.getListErrorGroup();
    this.getListProject();
    this.getCbbEmployee();
    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        previewCloseOnClick: true,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
    if (this.model.Id) {
      this.modalInfo.Title = "Chỉnh sửa vấn đề tồn đọng";
      this.getErrorInfo();
    } else {
      let now = new Date();
      let planStartDate = { day: now.getDate(), month: now.getMonth() + 1, year: now.getFullYear() };
      this.planStartDate = planStartDate;
      this.model.AuthorId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    }
  }

  deleteImage(event, index): void {
    let imageDelete = this.galleryImages[index].medium;
    let indexDelete;
    this.model.ListImage.forEach(element => {
      let url = this.config.ServerFileApi + element.ThumbnailPath;
      if (url == imageDelete) {
        indexDelete = this.model.ListImage.indexOf(element);
      }
    });
    this.galleryImages.splice(index, 1);
    this.model.ListImage.splice(indexDelete, 1);
  }

  getErrorInfo() {
    this.serviceError.getErrorInfo(this.model).subscribe(data => {
      this.model = data;
      this.listFile = data.ListFile;
      //this.getCbbEmployee(this.model.DepartmentId);
      this.getCbbModule();
      if (data.PlanStartDate != null) {
        let dateArray1 = data.PlanStartDate.split('T')[0];
        let dateValue1 = dateArray1.split('-');
        let tempDateFromV1 = {
          'day': parseInt(dateValue1[2]),
          'month': parseInt(dateValue1[1]),
          'year': parseInt(dateValue1[0])
        };
        this.planStartDate = tempDateFromV1;
      }
      if (data.PlanFinishDate != null) {
        let dateArray2 = data.PlanFinishDate.split('T')[0];
        let dateValue2 = dateArray2.split('-');
        let tempDateFromV2 = {
          'day': parseInt(dateValue2[2]),
          'month': parseInt(dateValue2[1]),
          'year': parseInt(dateValue2[0])
        };
        this.planFinishDate = tempDateFromV2;
      }
      for (var item of data.ListImage) {
        this.galleryImages.push({
          small: this.config.ServerFileApi + item.ThumbnailPath,
          medium: this.config.ServerFileApi + item.Path,
          big: this.config.ServerFileApi + item.Path
        });
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbbDepartment() {
    this.combobox.getCbbDepartment().subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }



  getCbbEmployee() {
    this.combobox.getCbbEmployee().subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListErrorGroup() {
    this.combobox.getListErrorGroup().subscribe(
      data => {
        this.listErrorGroup = data;
        if (!this.Id) {
          if (this.listErrorGroup.length > 0) {
            let defaultGroup = data.filter((t: any) => t.Code === "VĐ99");
            if (defaultGroup.length > 0) {
              this.model.ErrorGroupId = defaultGroup[0].Id;
            }
          }
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListProject() {
    this.combobox.getListProject().subscribe(
      data => {
        this.listProject = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeObjectType() {
    this.model.ObjectId = null;
    this.getCbbModule();
  }

  getCbbModule() {
    if (this.model.ObjectType == 1) {
      this.serviceError.searchModule(this.model).subscribe((data: any) => {
        if (data) {
          this.listModule = data;
        }
      }, error => {
        this.messageService.showError(error);
      });
    }
    if (this.model.ObjectType == 2) {
      this.serviceError.searchProject(this.model).subscribe(
        data => {
          this.listProduct = data;
        },
        error => {
          this.messageService.showError(error)
        }
      )
    }
  }

  save(isContinue: boolean) {
    if (this.planStartDate) {
      this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
    }
    else {
      this.model.PlanStartDate = null;
    }

    if (this.planFinishDate) {
      this.model.PlanFinishDate = this.dateUtils.convertObjectToDate(this.planFinishDate);
    }
    else {
      this.model.PlanFinishDate = null;
    }

    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      if (this.model.Id == '') {
        this.model.strHistory = "Tạo vấn đề"
        this.model.Status = 1;
        this.createError(isContinue);
      } else {
        this.model.strHistory = "Cập nhật vấn đề";
        this.updateError();
      }
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          if (this.model.Id == '') {
            this.model.strHistory = "Tạo vấn đề"
            this.model.Status = 1;
            this.createError(isContinue);
          } else {
            this.model.strHistory = "Cập nhật vấn đề";
            this.updateError();
          }
        },
        error => {

        });
    }
  }

  showConfirmRequest() {
    this.messageService.showConfirm("Bạn có chắc muốn yêu cầu xác nhận vấn đề này?").then(
      data => {
        this.confirmRequest();
      },
      error => {

      }
    );
  }

  confirmRequest() {
    if (!this.model.ProjectId) {
      this.messageService.showMessage("Bạn không được để trống dự án");
    }   

    else if (!this.model.AuthorId) {
      this.messageService.showMessage("Bạn không được để trống người chịu trách nhiệm");
    }
    else {
      if (this.planStartDate) {
        this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
      }
      else {
        this.model.PlanStartDate = null;
      }
  
      if (this.planFinishDate) {
        this.model.PlanFinishDate = this.dateUtils.convertObjectToDate(this.planFinishDate);
      }
      else {
        this.model.PlanFinishDate = null;
      }
      this.serviceError.confirmRequest(this.model).subscribe(
        data => {
          this.messageService.showSuccess('Yêu cầu xác nhận vấn đề thành công!');
          this.closeModal();
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
  }
  getCodeProblem(type){
    this.combobox.getCodeProblem(type).subscribe(
      data => {
        this.model.Code = data.Code;
      }, error =>{
        this.messageService.showError(error);
      });
  }

  changeCodeProblemandError(){
    if(this.model.Type == 1){
      this.getCodeProblem(1);
    }
    if(this.model.Type  == 2){
      this.getCodeProblem(2);
    }
  }

  showConfirm(Id: string) {
    let activeModal = this.modalService.open(ProjectErrorConfirmComponent, { container: 'body', windowClass: 'project-error-confirm', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {
    });
  }

  createError(isContinue) {
    let files = [];
    this.listFile.forEach((item, index) => {
      if (item.File) {
        item.File.Index = index;
        files.push(item.File);
      }
    });

    if (files.length > 0) {
      this.uploadService.uploadListFile(files, 'ProblemExistDocument/').subscribe((event: any) => {
        files.forEach((item, index) => {
          this.listFile[item.Index].Path = event[index].FileUrl;
        });
        this.model.ListFile = this.listFile;
        this.serviceError.createError(this.model).subscribe(
          data => {
            if (isContinue) {
              this.model = {
                ListImage: [],
                Status: 1
              };
              this.listImage = [];
              this.planStartDate = '';
              this.planFinishDate = '';
              this.galleryImages = [];
              this.messageService.showSuccess('Thêm mới vấn đề thành công!');
            }
            else {
              this.messageService.showSuccess('Thêm mới vấn đề thành công!');
              this.closeModal();
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
    else {
      this.serviceError.createError(this.model).subscribe(
        data => {
          if (isContinue) {
            this.model = {
              ListImage: [],
              Status: 1
            };
            this.listImage = [];
            this.planStartDate = '';
            this.planFinishDate = '';
            this.galleryImages = [];
            this.messageService.showSuccess('Thêm mới vấn đề thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới vấn đề thành công!');
            this.closeModal();
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
  }

  updateError() {
    let files = [];
    this.listFile.forEach((item, index) => {
      if (item.File) {
        item.File.Index = index;
        files.push(item.File);
      }
    });

    if (files.length > 0) {
      this.uploadService.uploadListFile(files, 'ProblemExistDocument/').subscribe((event: any) => {
        files.forEach((item, index) => {
          this.listFile[item.Index].Path = event[index].FileUrl;
        });
        this.model.ListFile = this.listFile;
        this.serviceError.updateError(this.model).subscribe(
          () => {
            this.closeModal();
            this.messageService.showSuccess('Cập nhật vấn đề thành công!');
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }, error => {
        this.messageService.showError(error);
      });
    }
    else {
      this.serviceError.updateError(this.model).subscribe(
        () => {
          this.closeModal();
          this.messageService.showSuccess('Cập nhật vấn đề thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }

  }

  closeModal() {
    this.activeModal.close(true);
  }

  uploadFileClickImage($event) {
    this.listImage = [];
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.listImage.push(item);
    }

    this.uploadService.uploadListFile(this.listImage, 'ProblemExist/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.Path = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
          this.model.ListImage.push(file);
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.FileUrlThum,
            medium: this.config.ServerFileApi + item.FileUrlThum,
            big: this.config.ServerFileApi + item.FileUrl
          });
        });
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

   // Hủy yêu cầu xác nhận
   confirmCancelRequest() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy yêu cầu xác nhận không?").then(
      data => {
        this.cancelRequest();
      },
      error => {

      }
    );
  }

  cancelRequest() {
    this.serviceError.cancelRequest(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật vấn đề thành công!');
        this.router.navigate(['du-an/quan-ly-van-de']);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  uploadFileClick($event) {
    var fileDataSheet = this.fileProcessImage.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listFile) {
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
    let documentTemplate;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.listFile.length; index++) {

        if (this.listFile[index].Id != null) {
          if (file.name == this.listFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.listFile.splice(index, 1);
            }
          }
        }
        else if (file.name == this.listFile[index].name) {
          isExist = true;
          if (isReplace) {
            this.listFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        documentTemplate = Object.assign({}, this.fileModel);
        documentTemplate.File = file;
        documentTemplate.FileName = file.name;
        documentTemplate.FileSize = file.size;
        this.listFile.push(documentTemplate);
      }
    }
  }

  updateFileManualDocument(files) {
    let documentTemplate;
    for (var file of files) {
      documentTemplate = Object.assign({}, this.fileModel);
      documentTemplate.File = file;
      documentTemplate.FileName = file.name;
      documentTemplate.FileSize = file.size;
      this.listFile.push(documentTemplate);
    }
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa bằng chứng khắc phục này không?").then(
      data => {
        if (this.listFile[index].Id) {
          this.listFile.splice(index, 1);
        }
        else {
          this.listFile[index].IsDelete = true;
        }
        // this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {

      }
    );
  }

  downloadAFile(path: string) {
    if (!path) {
      this.messageService.showError("Không có file để tải");
      return;
    }
    var link = document.createElement('a');
    link.setAttribute("type", "hidden");
    link.href = this.config.ServerFileApi + path;
    link.download = "aaaaa";
    document.body.appendChild(link);
    // link.focus();
    link.click();
    document.body.removeChild(link);
  }
}
