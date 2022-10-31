import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

import { AppSetting, Configuration, FileProcess, MessageService, Constants, ComboboxService } from 'src/app/shared';

import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { JobServiceService } from '../../service/job-service.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ListSubjectsJobChooseComponent } from '../list-subjects-job-choose/list-subjects-job-choose.component';
import { ProductService } from 'src/app/device/services/product.service';

@Component({
  selector: 'app-job-project-attach',
  templateUrl: './job-project-attach.component.html',
  styleUrls: ['./job-project-attach.component.scss']
})
export class JobProjectAttachComponent implements OnInit {

  @ViewChild('fileInput',{static:false}) fileInput: ElementRef;
  @ViewChild('scrollPracticeHeader',{static:false}) scrollPracticeHeader: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;
  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    public fileProcess: FileProcess,
    private productService: ProductService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private routeA: ActivatedRoute,
    private uploadService: UploadfileService,
    private jobService: JobServiceService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    public activeModal: NgbActiveModal,
  ) {
    
   }

  startIndex = 1;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listJob: any[] = [];
  listGroupJob = [];
  listDegree = [];
  FilesDataBase: any[] = [];
  listFile: any[] = [];
  SubjectName: string;
  logUserId: string;
  Id: string;
  check : any;

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  StartIndex = 1;
  selectIndex =-1;
  productIndex =-1;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Description: '',
    Documents: '',
    DegreeId: '',
    JobGroupId: '',
    SubjectId: '',
    JobId: '',
    SubjectName: '',
    ListJobSubject: [],
    ListJobAttach: [],
    ListSubject: [],
    ListFile: [],
    ListClassRoom :[],
  }
  ListFile = [];

  fileModel = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }
  totalAmount = 0;
  MaxLeadTimeModule = 0;
  TotalModuleNoPrice = 0;
  TotalModuleNoLeadTime = 0;

  listModule: any [];
  ListProduct : any [];
  listBase: any = [];
  listSelect: any = [];
  IsRequest: boolean;
  JobModel: string;
  listSubjectJob: any[] = [];
  listSubject: any[] = [];

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa nghề";
    this.model.JobId = this.routeA.snapshot.paramMap.get('Id');
    this.check =false;
    if (this.model.JobId) {
      this.check =true;
      this.getJobInfo();
    }
  }


  getJobInfo() {
    this.jobService.getJobInfor({ Id: this.model.JobId }).subscribe(data => {
      this.model = data;
      this.listSubject = this.model.ListSubject;
      this.model.ListFile = this.model.ListJobAttach;
    });
  }

  // danh mục tài liệu

  uploadFileClick($event) {
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
  save() {
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
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (item.Path == null || item.Path == "") {
          this.model.ListFile.splice(this.model.ListFile.indexOf(item), 1);
        }
      }
      this.model.ListJobSubject = this.listSubject;
      this.jobService.update(this.model).subscribe((event: any) => {
        this.messageService.showSuccess("Cập nhật nghề thành công!");
        this.close(true);
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
