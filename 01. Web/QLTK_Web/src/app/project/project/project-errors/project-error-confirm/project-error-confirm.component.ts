import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, DateUtils, Constants } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ErrorService } from 'src/app/project/service/error.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-project-error-confirm',
  templateUrl: './project-error-confirm.component.html',
  styleUrls: ['./project-error-confirm.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectErrorConfirmComponent implements OnInit {

  constructor(
    private combobox: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public dateUtils: DateUtils,
    private serviceError: ErrorService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private router: Router,
    private activeModal: NgbActiveModal,
    private routeA: ActivatedRoute,
    public constants: Constants,
    public fileProcess: FileProcess,
  ) { }

  Id: string;
  planStartDate: any;
  planFinishDate: any;
  actualStartDate: any;
  actualFinishDate: any;
  listErrorGroup: any[] = [];
  listDepartment: any[] = [];
  listDepartmentProcess: any[] = [];
  listEmployee: any[] = [];
  listEmployees: any[] = [];
  listEmployeeFixBy: any[] = [];
  listProject: any[] = [];
  listModule: any[] = [];
  listStage: any[] = [];
  listFile: any[] = [];
  listHistory: any[] = [];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  thumbnailActions: NgxGalleryOptions[];
  columnModule: any[] = [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã vấn đề' }, { Name: 'Name', Title: 'Tên vấn đề' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnType: any[] = [{Name: 'Name', Title: 'Tên loại vấn đề' }];
  columnDeparterment: any[] = [{ Name: 'Code', Title: 'Mã bộ phận' }, { Name: 'Name', Title: 'Tên bộ phận' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  listImage = [];
  listType: any[] = [
    { Id: 1, Name: 'Lỗi' },
    { Id: 2, Name: 'Vấn đề' },
  ]
  model: any = {
    Id: '',
    Subject: '',
    Code: '',
    Description: '',
    ErrorGroupId: '',
    ErrorGroupName: '',
    DepartmentId: '',
    DepartmentName: '',
    AuthorId: '',
    AuthorName: '',
    ErrorBy: '',
    PlanStartDate: '',
    PlanFinishDate: '',
    ObjectId: '',
    ModuleErrorVisualId: '',
    DepartmentProcessId: '',
    StageId: '',
    FixBy: '',
    ProjectId: '',
    ProjectName: '',
    Status: '',
    Solution: '',
    Note: '',
    ErrorCost: '',
    ActualStartDate: '',
    ActualFinishDate: '',
    Type: null,
    strHistory: '',
    ListImage: [],
    ListFile: [],
  }

  moduleModel: any = {
    ProjectId: '',
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
    this.fileProcess.FilesDataBase = [];
    this.appSetting.PageTitle = "Xác nhận vấn đề";
    this.model.Id = this.Id;
    this.getCbbDepartment();
    this.getCbbDepartmentProcess();
    this.getListErrorGroup();
    this.getListProject();
    this.getCbbStage();
    this.getListEmployee();
    this.galleryOptions = [
      {
        height: '400px',
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
      this.getErrorInfo();
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
      this.getCbbEmployee(this.model.DepartmentId);
      this.getCbbEmployeeFixBy(this.model.DepartmentProcessId);
      this.getCbbModule(this.model.ProjectId);
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
        let dateArray4 = data.PlanFinishDate.split('T')[0];
        let dateValue4 = dateArray4.split('-');
        let tempDateFromV4 = {
          'day': parseInt(dateValue4[2]),
          'month': parseInt(dateValue4[1]),
          'year': parseInt(dateValue4[0])
        };
        this.planFinishDate = tempDateFromV4;
      }
      if (data.ActualStartDate != null) {
        let dateArray2 = data.ActualStartDate.split('T')[0];
        let dateValue2 = dateArray2.split('-');
        let tempDateFromV2 = {
          'day': parseInt(dateValue2[2]),
          'month': parseInt(dateValue2[1]),
          'year': parseInt(dateValue2[0])
        };
        this.actualStartDate = tempDateFromV2;
      }
      if (data.ActualFinishDate != null) {
        let dateArray3 = data.ActualFinishDate.split('T')[0];
        let dateValue3 = dateArray3.split('-');
        let tempDateFromV3 = {
          'day': parseInt(dateValue3[2]),
          'month': parseInt(dateValue3[1]),
          'year': parseInt(dateValue3[0])
        };
        this.actualFinishDate = tempDateFromV3;
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

  getCbbDepartmentProcess() {
    this.combobox.getCbbDepartment().subscribe(
      data => {
        this.listDepartmentProcess = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListEmployee() {
    this.combobox.getCbbEmployee().subscribe(
      data => {
        this.listEmployees = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbEmployee(DepartmentId: string) {
    this.combobox.getEmployeeByDepartment(DepartmentId).subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbEmployeeFixBy(DepartmentProcessId: string) {
    this.combobox.getEmployeeByDepartment(DepartmentProcessId).subscribe(
      data => {
        this.listEmployeeFixBy = data;
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

  getCbbModule(ProjectId: string) {
    this.moduleModel.ProjectId = ProjectId;
    this.serviceError.searchModule(this.moduleModel).subscribe((data: any) => {
      if (data) {
        this.listModule = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbbStage() {
    this.combobox.getCbbStage().subscribe(
      data => {
        this.listStage = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  startConfirm() {
    this.model.Status = 3;
    this.model.strHistory = "Đang xử lý";
    this.updateError();
  }

  save() {
    this.updateError();
  }

  warning() {
    if (this.model.ErrorBy == "" || this.model.ErrorBy == null) {
      this.messageService.showMessage("Bạn không được để trống người chịu trách nhiệm");
    } else if (this.model.StageId == "" || this.model.StageId == null) {
      this.messageService.showMessage("Bạn không được để trống công đoạn");
    } else if (this.model.DepartmentProcessId == "" || this.model.DepartmentProcessId == null) {
      this.messageService.showMessage("Bạn không được để trống phòng ban khắc phục");
    } else if (this.model.FixBy == "" || this.model.FixBy == null) {
      this.messageService.showMessage("Bạn không được để trống người khắc phục");
    } else if (this.model.Note == "" || this.model.Note == null) {
      this.messageService.showMessage("Bạn không được để trống nguyên nhân");
    } else if (this.model.Solution == "" || this.model.Solution == null) {
      this.messageService.showMessage("Bạn không được để trống giải pháp");
    } else {
      this.model.Status = 4;
      this.model.strHistory = "Hoàn thành sửa vấn đề";
      this.updateError();
    }
  }

  confirmCancelResultQC() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy kết quả QC?").then(
      data => {
        this.cancelResultQC();
      },
      error => {
        
      }
    );
  }

  cancelResultQC() {
    this.model.Status = 6;
    this.model.strHistory = "Hủy kết quả QC";
    this.updateError();
  }

  confirmDoneQC() {
    this.messageService.showConfirm("Bạn có chắc muốn QC đạt?").then(
      data => {
        this.doneQC();
      },
      error => {
        
      }
    );
  }

  doneQC() {
    if (this.actualFinishDate == "" || this.actualFinishDate == null) {
      this.messageService.showMessage("Bạn không được để trống ngày QC");
    } else{
      this.model.Status = 7;
      this.model.strHistory = "QC đạt";
      this.updateError();
    }

  }

  confirmNotDoneQC() {
    this.messageService.showConfirm("Bạn có chắc muốn QC không đạt?").then(
      data => {
        this.notDoneQC();
      },
      error => {
        
      }
    );
  }

  notDoneQC() {
    this.model.Status = 8;
    this.model.strHistory = "QC không đạt";
    this.updateError();
  }

  confirmCancelCompleteProccessing() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy hoàn thành vấn đề này?").then(
      data => {
        this.cancelCompleteProccessing();
      },
      error => {
        
      }
    );
  }

  cancelCompleteProccessing() {
    this.model.Status = 3;
    this.model.strHistory = "Hủy hoàn thành";
    this.serviceError.cancelCompleteProccessing(this.model).subscribe(
      () => {
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

     // Hoàn thành xử lý
  confirmCompleteProcessing() {
    this.messageService.showConfirm("Bạn có chắc muốn hoàn thành vấn đề này?").then(
      data => {

        if (this.model.ListFile.length <= 0) {
          this.messageService.showMessage("Bạn chưa nhập: Bằng chứng khắc phục.");
        } else {
          if (this.model.FixBy == null || this.model.FixBy == "" || this.model.FixBy == undefined) {
            this.messageService.showMessage("Bạn chưa nhập: Người khắc phục.");
          } else {
            this.completeProcessing();
          }
        }
      },
      error => {
        
      }
    );
  }

  completeProcessing() {
    this.model.Status = 5;
    this.model.strHistory = "Đã xử lý";
    this.updateError();
  }

    // Xác nhận
    confirmRequest() {
      this.messageService.showConfirm("Bạn có chắc muốn xác nhận vấn đề này?").then(
        data => {
          if (this.model.ErrorBy == "" || this.model.ErrorBy == null || this.model.ErrorBy == undefined) {
            this.messageService.showMessage("Bạn chưa nhập đủ các thông tin: Người chịu trách nhiệm.");
          } else {
            if (this.model.DepartmentProcessId == "" || this.model.DepartmentProcessId == null || this.model.DepartmentProcessId == undefined) {
              this.messageService.showMessage("Bạn chưa nhập đủ các thông tin: Bộ phận khắc phục.");
            } else {
              if (this.model.StageId == "" || this.model.StageId == null || this.model.StageId == undefined) {
                this.messageService.showMessage("Bạn chưa nhập đủ các thông tin: Công đoạn xảy ra vấn đề.");
              } else {
                this.confirm();
              }
            }
          }
        },
        error => {
          
        }
      );
    }

    IsDisabledInfoError = false;
    confirm() {
      this.model.Status = 3;
      this.model.strHistory = "Đã xác nhận";
      this.updateError();
      this.IsDisabledInfoError = true;
    }

      // Hủy xác nhận
  confirmCancelConfirm() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy xác nhận vấn đề này?").then(
      data => {
        this.cancelConfirm();
      },
      error => {
        
      }
    );
  }

  cancelConfirm() {
    this.model.Status = 2;
    this.model.strHistory = "Hủy xác nhận";
    this.serviceError.cancelConfirm(this.model).subscribe(
      () => {
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
    this.IsDisabledInfoError = false;
  }

    // Đóng vấn đề
    confirmCloseRequest() {
      this.messageService.showConfirm("Bạn có chắc muốn đóng vấn đề không?").then(
        data => {
          this.cancelRequest();
        },
        error => {
          
        }
      );
    }
    
    closeRequest() {
      this.serviceError.closeError(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Đóng vấn đề vấn đề thành công!');
          this.router.navigate(['du-an/quan-ly-van-de']);
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }

  updateError() {
    this.uploadService.uploadListFile(this.listFile, 'ProblemExist/').subscribe((event: any) => {
      if (event.length > 0) {
        var count = 0;
        this.model.ListFile.forEach(item => {
          if (item.Path == '') {
            item.Path = event[count].FileUrl;
            count++;
          }
        });
      }
      if (this.planStartDate) {
        this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
      }
      if (this.planFinishDate) {
        this.model.PlanFinishDate = this.dateUtils.convertObjectToDate(this.planFinishDate);
      }
      if (this.actualStartDate) {
        this.model.ActualStartDate = this.dateUtils.convertObjectToDate(this.actualStartDate);
      }
      if (this.actualFinishDate) {
        this.model.ActualFinishDate = this.dateUtils.convertObjectToDate(this.actualFinishDate);
      }

      this.serviceError.updateError(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Cập nhật vấn đề thành công!');
        },
        error => {
          this.messageService.showError(error);
        });
    }, error => {
      this.messageService.showError(error);
    });
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

  uploadFileClick($event) {
    var dataProcess = this.fileProcess.getFileOnFileChange($event);
    for (var item of dataProcess) {
      var a = 0;
      for (var ite of this.model.ListFile) {
        if (item.name == ite.FileName) {
          var b = a;
          this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
            data => {
              this.model.ListFile.splice(b, 1);
            },
            error => {
              
            });
        }
        a++;
      }
      if (this.fileProcess.totalbyte > 5000000) {
        this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
        this.fileProcess.FilesDataBase = [];
        this.fileProcess.totalbyte = 0;
      }
      else {
        this.listFile.push(item);
        var file = Object.assign({}, this.fileModel);
        file.FileName = item.name;
        file.FileSize = item.size;
        this.model.ListFile.push(file);
      }
    }
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa bằng chứng khắc phục này không?").then(
      data => {
        this.model.ListFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }

  downloadAFile(file: any) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  confirmCancelRequestQC(){

  }

  confirmRequestQC(){
    
  }
}
