import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import {ProblemExistCreateChangePlanComponent} from '../problem-exist-create-change-plan/problem-exist-create-change-plan.component'
import { MessageService, Configuration, FileProcess, AppSetting, DateUtils, Constants, ComboboxService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-problem-exist-confirm',
  templateUrl: './problem-exist-confirm.component.html',
  styleUrls: ['./problem-exist-confirm.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProblemExistConfirmComponent implements OnInit {

  constructor(
    private combobox: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public dateUtils: DateUtils,
    private serviceError: ErrorService,
    private router: Router,
    private routeA: ActivatedRoute,
    public constants: Constants,
    public fileProcess: FileProcess,
    private modalService : NgbModal,
  ) { }

  Id: string;
  planStartDate: any;
  planFinishDate: any;
  expectedPlanFinishDate: any;
  actualStartDate: any;
  actualEndDate: any;
  actualFinishDate: any;
  listErrorGroup: any[] = [];
  listDepartment: any[] = [];
  errorAffects: any[] = [];
  listDepartmentProcess: any[] = [];
  listEmployee: any[] = [];
  listEmployees: any[] = [];
  listEmployeeFixBy: any[] = [];
  listProject: any[] = [];
  listModule: any[] = [];
  listProduct: any[] = [];
  listStage: any[] = [];
  listFile: any[] = [];
  listHistory: any[] = [];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  listImage = [];
  columnModule: any[] = [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }];
  columnProduct: any[] = [{ Name: 'Code', Title: 'Mã thiết bị' }, { Name: 'Name', Title: 'Tên thiết bị' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã vấn đề' }, { Name: 'Name', Title: 'Tên vấn đề' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnType: any[] = [{ Name: 'Name', Title: 'Tên loại vấn đề' }];
  columnDeparterment: any[] = [{ Name: 'Code', Title: 'Mã bộ phận' }, { Name: 'Name', Title: 'Tên bộ phận' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  columnStage: any[] = [{ Name: 'Code', Title: 'Mã công đoạn' }, { Name: 'Name', Title: 'Tên công đoạn' }];
  listType: any[] = [
    { Id: 1, Name: 'Lỗi' },
    { Id: 2, Name: 'Vấn đề' },
  ]
  model: any = {
    Id: '',
    Subject: '',
    Code: '',
    Index: 0,
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
    ExpectedPlanFinishDate: '',
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
    Fixs: [],
    ListHistory: [],
    //isChange: Boolean
    isChange: false,
    Reason: '',
    FirstHistory: Boolean,

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

  fixSelectIndex = -1;
  fixSelect: any = {};

  listErrorHistory: any[] = [];
  modelHistory: any = {
    Id: '',
    ErrorId: '',
  }

  ngOnInit() {
    this.fileProcess.FilesDataBase = [];
    this.appSetting.PageTitle = "Xác nhận vấn đề";
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getCbbDepartment();
    this.getCbbDepartmentProcess();
    this.getListErrorGroup();
    this.getListProject();
    this.getCbbStage();
    this.getListEmployee();
    this.getListProduct(); 
    this.getCbbErrorAffect();
    this.galleryOptions = [
      {
        height: '400px',
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
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
    this.modelHistory.ErrorId = this.Id;
  }

  changeCodeProblemandError() {
    if (this.model.Type == 1) {
      this.getCodeProblem(1);
    }
    if (this.model.Type == 2) {
      this.getCodeProblem(2);
    }
  }

  getCodeProblem(type) {
    this.combobox.getCodeProblem(type).subscribe(
      data => {
        this.model.Code = data.Code;
        this.model.Index = data.Index;
      }, error => {
        this.messageService.showError(error);
      });
  }

  
  getErrorInfo() {
    this.serviceError.getErrorInfo(this.model).subscribe(data => {
      this.getCbbEmployeeFixBy();
      this.model = data;
      this.listFile = data.ListFile;
      this.appSetting.PageTitle = "Xác nhận vấn đề - " + data.Code;
      if (this.model.Status != 2) {
        this.IsDisabledInfoError = true;
      }

      //
      console.log(this.model.FirstHistory)
      //

      // this.getListEmployee();
      this.getCbbModule(this.model.ProjectId);
      if (data.PlanStartDate != null) {
        this.planStartDate = this.dateUtils.convertDateToObject(data.PlanStartDate);
      }
      if (data.PlanFinishDate != null) {
        this.planFinishDate = this.dateUtils.convertDateToObject(data.PlanFinishDate);
      }

      if (data.ActualFinishDate != null) {
        this.actualFinishDate = this.dateUtils.convertDateToObject(data.ActualFinishDate);
      }

      for (var item of data.ListImage) {
        this.galleryImages.push({
          small: this.config.ServerFileApi + item.ThumbnailPath,
          medium: this.config.ServerFileApi + item.Path,
          big: this.config.ServerFileApi + item.Path
        });
      }

      if (this.model.Fixs.length > 0) {
        this.selectFix(0);
        var arrDate =[];
        this.model.Fixs.forEach(element => {
          arrDate.push(element.DateTo);
       });
         var len = arrDate.length
         var max = '';
          while (len--) {
               if (arrDate[len] > max) {
                max = arrDate[len];
            }
        }
       this.expectedPlanFinishDate = this.dateUtils.convertDateToObject(max);
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

  getCbbErrorAffect(){
    this.combobox.getCbbErrorAffect().subscribe(
      data => {
        this.errorAffects = data;
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

  getCbbEmployeeFixBy() {
    this.combobox.getEmployeeByDepartment(this.model.DepartmentProcessId).subscribe(
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

  getListProduct() {
    this.combobox.getListProduct().subscribe(
      data => {
        this.listProduct = data;
      },
      error => {
        this.messageService.showError(error)
      }
    )
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

  save() {
    if (this.model.Status == 2) {
      this.updateErrorConfirm();
    }
    else if (this.model.Status == 3) {
      this.updateErrorPlan();
    }
    else if (this.model.Status == 5) {
      this.updateErrorProcess();
    }
    else if (this.model.Status == 6) {
      this.updateErrorQC();
    }
  }

  updateErrorConfirm() {
    if (this.model.Status == 2) {
      this.IsDisabledInfoError = false;
    }

    if (this.planStartDate) {
      this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
    }
    if (this.planFinishDate) {
      this.model.PlanFinishDate = this.dateUtils.convertObjectToDate(this.planFinishDate);
    }

    this.serviceError.updateErrorConfirm(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật vấn đề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  updateErrorPlan() {
    let files = [];
    this.model.Fixs.forEach((element, parentIndex) => {
      element.FixAttachs.forEach((item, index) => {
        if (item.File) {
          item.File.Index = index;
          item.File.ParentIndex = parentIndex;
          files.push(item.File);
        }
      });
    });

    if (files.length > 0) {
      this.uploadService.uploadListFile(files, 'ProblemExist/').subscribe((event: any) => {
        files.forEach((item, index) => {
          this.model.Fixs[item.ParentIndex].FixAttachs[item.Index].Path = event[index].FileUrl;
        });

        this.updateDataErrorPlan();
      }, error => {
        this.messageService.showError(error);
      });
    }
    else {
      this.updateDataErrorPlan();
    }
  }

  updateDataErrorPlan() {
    this.serviceError.updateErrorPlan(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật vấn đề thành công!');
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      });
  }

  updateErrorProcess() {
    let files = [];
    this.model.Fixs.forEach((element, parentIndex) => {
      element.FixAttachs.forEach((item, index) => {
        if (item.File) {
          item.File.Index = index;
          item.File.ParentIndex = parentIndex;
          files.push(item.File);
        }
      });
    });
    if (files.length > 0) {
      this.uploadService.uploadListFile(files, 'ProblemExist/').subscribe((event: any) => {
        files.forEach((item, index) => {
          this.model.Fixs[item.ParentIndex].FixAttachs[item.Index].Path = event[index].FileUrl;
        });
        this.updateDataErrorProcess();
      }, error => {
        this.messageService.showError(error);
      });
    }
    else {
      this.updateDataErrorProcess();
    }
  }


  updateDataErrorProcess() {
    this.serviceError.updateErrorProcess(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật vấn đề thành công!');
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      });
  }

  updateErrorQC() {
    if (this.actualFinishDate) {
      this.model.ActualFinishDate = this.dateUtils.convertObjectToDate(this.actualFinishDate);
    }

    this.serviceError.updateErrorQC(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật vấn đề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-van-de']);
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
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.model.Fixs[this.fixSelectIndex].FixAttachs) {
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
      for (let index = 0; index < this.model.Fixs[this.fixSelectIndex].FixAttachs.length; index++) {

        if (this.model.Fixs[this.fixSelectIndex].FixAttachs[index].Id != null) {
          if (file.name == this.model.Fixs[this.fixSelectIndex].FixAttachs[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.model.Fixs[this.fixSelectIndex].FixAttachs.splice(index, 1);
            }
          }
        }
        else if (file.name == this.model.Fixs[this.fixSelectIndex].FixAttachs[index].name) {
          isExist = true;
          if (isReplace) {
            this.model.Fixs[this.fixSelectIndex].FixAttachs.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileModel);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        this.model.Fixs[this.fixSelectIndex].FixAttachs.push(docuemntTemplate);
      }
    }
  }

  updateFileManualDocument(files) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, this.fileModel);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      this.model.Fixs[this.fixSelectIndex].FixAttachs.push(docuemntTemplate);
    }
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa bằng chứng khắc phục này không?").then(
      data => {
        if (this.model.Fixs[this.fixSelectIndex].Id) {
          // this.model.Fixs[this.fixSelectIndex].FixAttachs.splice(index, 1);
          this.model.Fixs[this.fixSelectIndex].FixAttachs[index].IsDelete = true;
        }
        else {
          this.model.Fixs[this.fixSelectIndex].FixAttachs[index].IsDelete = true;
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

  IsDisabledInfoError = false;

  // Xác nhận
  confirmRequest() {
    this.messageService.showConfirm("Bạn có chắc muốn xác nhận vấn đề này?").then(
      data => {
        if (this.model.ErrorBy == "" || this.model.ErrorBy == null || this.model.ErrorBy == undefined) {
          this.messageService.showMessage("Bạn chưa nhập đủ các thông tin: Người chịu trách nhiệm.");
        } else if (this.model.StageId == "" || this.model.StageId == null || this.model.StageId == undefined) {
          this.messageService.showMessage("Bạn chưa nhập đủ các thông tin: Công đoạn xảy ra vấn đề.");
        } else {
          this.confirm();
        }
      },
      error => {

      }
    );
  }

  confirm() {
    this.serviceError.confirm(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Xác nhận thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
    this.serviceError.cancelConfirm(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Hủy xác nhận thành công!');
        this.IsDisabledInfoError = false;
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  // Đã có kế hoạch
  confirmPlan() {
    this.messageService.showConfirm("Bạn có chắc đã có kế hoạch vấn đề này chưa?").then(
      data => {
        let isFix = false;
        this.model.Fixs.forEach(element => {
          if (element.DepartmentId && element.EmployeeFixId) {
            isFix = true;
          }
        });

        if (!isFix) {
          this.messageService.showMessage("Bạn chưa nhập đủ các thông tin: Bộ phận khắc phục.");
          return;
        }

        if (!this.model.AffectId) {
          this.messageService.showMessage("Bạn chưa chọn yếu tố ảnh hưởng dự án.");
          return;
        }

        this.serviceError.confirmPlan(this.model).subscribe(
          () => {
            this.messageService.showSuccess('Đã có kế hoạch thành công!');
            this.galleryImages = [];
            this.getErrorInfo();
          },
          error => {
            this.messageService.showError(error);
          }
        );

      },
      error => {

      });
  }

  // Hủy đã có kế hoạch
  confirmCancelPlan() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy Đã có kế hoạch vấn đề này?").then(
      data => {
        this.serviceError.cancelConfirmPlan(this.model).subscribe(
          () => {
            this.messageService.showSuccess('Hủy Đã có kế hoạch thành công!');
            this.galleryImages = [];
            this.getErrorInfo();
          },
          error => {
            this.messageService.showError(error);
          }
        );
      },
      error => {

      }
    );
  }

  // Hoàn thành xử lý
  confirmCompleteProcessing() {
    this.messageService.showConfirm("Bạn có chắc Đã xử lý xong vấn đề này?").then(
      data => {
        this.completeProcessing();
      },
      error => {

      }
    );
  }

  completeProcessing() {
    let fixNotFinish = this.model.Fixs.filter(item => {
      return item.Status == 1;
    });

    if (fixNotFinish.length > 0) {
      this.messageService.showMessage('Có bộ phận chưa xử lý xong!');
      return;
    }

    let files = [];
    this.model.Fixs.forEach((element, parentIndex) => {


      element.FixAttachs.forEach((item, index) => {
        if (item.File) {
          item.File.Index = index;
          item.File.ParentIndex = parentIndex;
          files.push(item.File);
        }
      });
    });

    this.uploadService.uploadListFile(files, 'ProblemExist/').subscribe((event: any) => {
      files.forEach((item, index) => {
        this.model.Fixs[item.ParentIndex].FixAttachs[item.Index].Path = event[index].FileUrl;
      });

      this.serviceError.completeProccessing(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Đã xử lý thành công!');
          this.getErrorInfo();
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }

  confirmCancelCompleteProccessing() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy đã xử lý vấn đề này?").then(
      data => {
        this.cancelCompleteProccessing();
      },
      error => {

      }
    );
  }

  cancelCompleteProccessing() {
    this.serviceError.cancelCompleteProccessing(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Hủy đã xử lý thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  confirmDone(){
    this.messageService.showConfirm("Bạn có chắc đã khắc phục triệt để?").then(
      data => {
        this.done();
      },
      error => {

      }
    );
  }

  confirmCancelDone(){
    this.messageService.showConfirm("Bạn có chắc hủy khắc phục triệt để?").then(
      data => {
        this.cancelDone();
      },
      error => {

      }
    );
  }

  done() {
    this.serviceError.done(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Khắc phục triệt để thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  cancelDone() {
    this.serviceError.cancelDone(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Hủy khắc phục triệt để thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  doneQC() {
    this.serviceError.qcOK(this.model).subscribe(
      () => {
        this.messageService.showSuccess('QC đạt vấn đề thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
    this.serviceError.qcNG(this.model).subscribe(
      () => {
        this.messageService.showSuccess('QC không đạt vấn đề thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
    this.serviceError.cancelResultQC(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Hủy kết quả QC vấn đề thành công!');
        this.galleryImages = [];
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
        this.messageService.showSuccess('Hủy yêu cầu xác nhận thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  confirmCancelClose() {
    this.messageService.showConfirm("Bạn có chắc muốn hủy đóng vấn đề này?").then(
      data => {
        this.cancelClose();
      },
      error => {

      }
    );
  }

  cancelClose() {
    this.serviceError.cancelCloseError(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Hủy đóng vấn đề thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  // Đóng vấn đề
  confirmCloseRequest() {
    this.messageService.showConfirm("Bạn có chắc muốn đóng vấn đề không?").then(
      data => {
        this.closeRequest();
      },
      error => {

      }
    );
  }

  closeRequest() {
    this.serviceError.closeError(this.model).subscribe(
      () => {
        this.messageService.showSuccess('Đóng vấn đề thành công!');
        this.galleryImages = [];
        this.getErrorInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  addDepartment() {
    this.model.Fixs.push({
      DepartmentId: null,
      EmployeeFixId: null,
      Solution: null,
      DateFrom: null,
      EstimateTime: 0,
      Status: 1,
      SupportId: null,
      ApproveId: null,
      AdviseId: null,
      NotifyId: null,
      Deadline: 0,
      FixAttachs: [],
      IsChange: true,
    });
    this.selectFix(this.model.Fixs.length - 1);
  }

  confirmDeleteDepartment() {
    this.messageService.showConfirm("Bạn có chắc muốn xóa kế hoạch này không?").then(
      data => {
        if (this.model.Fixs[this.fixSelectIndex].Id) {
          this.model.Fixs[this.fixSelectIndex].IsDelete = true;
        } else {
          this.model.Fixs.splice(this.fixSelectIndex, 1);
        }

        let isExist = false;
        this.model.Fixs.forEach((element, index) => {
          if (!element.IsDelete && !isExist) {
            isExist = true;
            this.selectFix(index);
          }
        });

        if (!isExist) {
          this.fixSelectIndex = -1;
        }
      },
      error => {

      }
    );
  }

  selectFix(index) {
    if (this.fixSelectIndex != index) {
      this.fixSelectIndex = index;
      this.actualStartDate = null;
      if (this.model.Fixs[this.fixSelectIndex].DateFrom) {
        this.actualStartDate = this.dateUtils.convertDateToObject(this.model.Fixs[this.fixSelectIndex].DateFrom);
      }

      this.actualEndDate = null;
      if (this.model.Fixs[this.fixSelectIndex].DateTo) {
        this.actualEndDate = this.dateUtils.convertDateToObject(this.model.Fixs[this.fixSelectIndex].DateTo);
      }

      if(this.model.Status < 3){
        this.model.IsChange = true;
      }else {
        this.model.IsChange = this.model.Fixs[this.fixSelectIndex].IsChange == null? false: this.model.Fixs[this.fixSelectIndex].IsChange;
      }
    }
  }

  selectDepartment() {
    for (var item of this.listDepartment) {
      if (item.Id == this.model.Fixs[this.fixSelectIndex].DepartmentId) {
        this.model.Fixs[this.fixSelectIndex].DepartmentName = item.Name;
      }
    }
  }

  changeFixDate() {
    if (this.actualStartDate) {
      this.model.Fixs[this.fixSelectIndex].DateFrom = this.dateUtils.convertObjectToDate(this.actualStartDate);
    }

    if (this.actualEndDate) {
      this.model.Fixs[this.fixSelectIndex].DateTo = this.dateUtils.convertObjectToDate(this.actualEndDate);
    }
  }

  listChange: any[]=[];

  openChangedPlan(){
    let activeModal = this.modalService.open(ProblemExistCreateChangePlanComponent, { container: 'body', windowClass: 'problem-exist-create-change-plan', backdrop: 'static' })
    activeModal.componentInstance.Reason = this.model.Fixs[this.fixSelectIndex].Reason;
    activeModal.result.then((result) => {
      if (result.isOK) {
        this.model.Fixs[this.fixSelectIndex].Reason = result.reason;
        this.model.Fixs[this.fixSelectIndex].IsChange = true;
        this.model.IsChange = true;
      }
    }, (reason) => {
    });
  }
}