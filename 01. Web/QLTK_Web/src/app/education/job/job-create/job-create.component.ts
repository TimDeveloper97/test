import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AppSetting, Configuration, MessageService, Constants, FileProcess, ComboboxService } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

import { JobServiceService } from '../../service/job-service.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ListSubjectsJobChooseComponent } from '../list-subjects-job-choose/list-subjects-job-choose.component';
import { UploadfileService } from 'src/app/upload/uploadfile.service';

@Component({
  selector: 'app-job-create',
  templateUrl: './job-create.component.html',
  styleUrls: ['./job-create.component.scss'],
})
export class JobCreateComponent implements OnInit {

  @ViewChild('fileInput',{static:false}) fileInput: ElementRef;

  
  @ViewChild('scrollModuleHeader', { static: false }) scrollModuleHeader: ElementRef;
  @ViewChild('scrollModule', { static: false }) scrollModule: ElementRef;
  @ViewChild('scrollModule1', { static: false }) scrollModule1: ElementRef;
  @ViewChild('scrollModuleHeader1', { static: false }) scrollModuleHeader1: ElementRef;
  @ViewChild('scrollModule2', { static: false }) scrollModule2: ElementRef;
  @ViewChild('scrollModuleHeader2', { static: false }) scrollModuleHeader2: ElementRef;
  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    private modalService: NgbModal,
    private uploadService: UploadfileService,
    private jobService: JobServiceService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    private routeA: ActivatedRoute,
    public activeModal: NgbActiveModal,
  ) { }

  startIndex = 1;
  StartIndex =1;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listJob: any[] = [];
  listGroupJob = [];
  listDegree = [];
  FilesDataBase: any[] = [];
  ListJobSubject = [];
  ListFile = [];
  SubjectName: string;
  logUserId: string;
  GroupId: string;
  totalAmount : number =0;
  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  ModalInfo = {
    Title: 'Thêm mới công việc',
    SaveText: 'Lưu',

  };
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnNameDegree: any[] = [{ Name: 'Code', Title: 'Mã trình độ' }, { Name: 'Name', Title: 'Tên trình độ' }];
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    DegreeId: '',
    JobGroupId: '',
    SubjectId: '',
    JobId: '',
    SubjectName: '',
    ListJobSubject: [],
    ListJobAttach: [],
    ListFile:[],
  }

  fileModel = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }
  listBase: any = [];
  listSelect: any = [];
  IsRequest: boolean;
  JobModel: string;
  listSubjectJob: any = [];
  JobGroupId: any [] = [];
  ListClassRoom : any [] =[];
  ListClassRoomProduct : any [] =[];
  ListModuleProductModel : any [] =[];
  ngOnInit() {
    this.appSetting.PageTitle = "Thêm mới nghề";
    this.getListDegree();
    this.getListGroupJob();
    this.model.JobGroupId = this.routeA.snapshot.paramMap.get('GroupId');
    
  }
  ngAfterViewInit() {
    this.scrollModule.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollModule1.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader1.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollModule2.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader2.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }
  getListGroupJob() {
    this.comboboxService.getCBBListGroupJob().subscribe(
      data => {
        this.listGroupJob = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListDegree() {
    this.comboboxService.getListDegree().subscribe(
      data => {
        this.listDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  // danh mục môn học công nghệ
  //Hiển thì popup chọn 
  showSelectTestCeiteria(IsRequest) {
    let activeModal = this.modalService.open(ListSubjectsJobChooseComponent, { container: 'body', windowClass: 'listsubjectsjobchoose-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listSubjectJob.forEach(element => {
      ListIdSelect.push(element.Id);
    });
    

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listSubjectJob.push(element);
        });
        var model = {
          ListSubject:[]
        }
        if(this.listSubjectJob.length >0){
          this.listSubjectJob.forEach(element =>{
            model.ListSubject.push(element);
          });
          this.jobService.getSubjectInfo(model).subscribe(data => {
            this.ListClassRoom = data.ListClassRoom;
            this.ListClassRoomProduct = data.ListClassRoomProductModel;
            this.ListModuleProductModel = data.ListModuleProduct;
            this.totalAmount = 0;
            this.ListModuleProductModel.forEach(mod => {
            this.totalAmount += mod.Price * mod.Qty;
          });
          });
        }
      }
    }, (reason) => {

    });
  }

  showConfirmdeleteSubject(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa môn học này không?").then(
      data => {
          this.deleteSubject(model);
      },
      error => {
        
      }
    );
  }
  
  deleteSubject(model) {
    var index = this.listSubjectJob.indexOf(model);
    if (index > -1) {
      this.listSubjectJob.splice(index, 1);
    }
  }

  getListSubjectInfo() {
    this.jobService.GetJobInfo({ Id: this.model.JobId }).subscribe(
      data => {
        this.JobModel = data.Id;
        this.model.Id = data.Id;
        this.searchSubject();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchSubject() {
    this.jobService.SearchSubject({ Id: this.model.JobId }).subscribe(data => {
      this.listSubjectJob = data.ListResult;
    }, error => {
      this.messageService.showError(error);
    })
  }

  uploadFileClick($event) {
    // var dataProcess = this.fileProcess.getFileOnFileChange($event);
    // for (var item of dataProcess) {
    //   var a = 0;
    //   for (var ite of this.model.ListFile) {
    //     if (ite.FileName == item.name) {
    //       var b = a;
    //       this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //         data => {
    //           this.model.ListFile.splice(b, 1);
    //           this.ListFile.splice(b, 1);
    //         });
    //     }
    //     a++;
    //   }
    //   this.ListFile.push(item);
    //   var file = Object.assign({}, this.fileModel);
    //   file.FileName = item.name;
    //   file.FileSize = item.size;
    //   this.model.ListFile.push(file);
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

  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue: boolean) {
    this.model.ListJobAttach = [];
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    var listFileUpload = [];

    this.model.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    this.uploadService.uploadListFile(this.model.ListFile, 'JobAttach/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListJobAttach.push(file);
        });
      }

      this.model.ListJobSubject = this.listSubjectJob;
      this.jobService.Create(this.model).subscribe((event: any) => {
        this.messageService.showSuccess("Thêm mới nghề thành công!");
        if (isContinue) {
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
            DegreeId: '',
            JobGroupId: '',
            SubjectId: '',
            JobId: '',
            SubjectName: '',
            ListJobSubject: [],
            ListJobAttach: [],
          }
          this.listSubjectJob = [];
          this.ListFile = [];
          this.fileProcess.FilesDataBase = [];
        }
        else {
          this.close(true);
        }
      }, error => {
        this.messageService.showError(error);
      });
    }, error => {
      this.messageService.showError(error);
    });

  }
  close(isOK: boolean) {
    this.router.navigate(['phong-hoc/quan-ly-nghe']);
  }
}
