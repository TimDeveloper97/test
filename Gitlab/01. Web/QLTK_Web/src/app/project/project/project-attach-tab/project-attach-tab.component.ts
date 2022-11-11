import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Console } from 'console';
import { type } from 'os';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { AppSetting, Configuration, MessageService, Constants, FileProcess, ComponentService, ComboboxService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ProjectAttachService } from '../../service/project-attach.service';
import { ProjectAttachCreateComponent } from '../project-attach-create/project-attach-create.component';
import { ViewProjectAttachTabComponent } from '../show-project-attach-tab/view-project-attach-tab/view-project-attach-tab.component';
import { ProjectAttachTabTypeComponent } from '../project-attach-tab-type/project-attach-tab-type.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-project-attach-tab',
  templateUrl: './project-attach-tab.component.html',
  styleUrls: ['./project-attach-tab.component.scss']
})
export class ProjectAttachTabComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private uploadService: UploadfileService,
    public constant: Constants,
    public fileProcess: FileProcess,
    public activeModal: NgbActiveModal,
    private projectAttachService: ProjectAttachService,
    private modalService: NgbModal,
    private componentService: ComponentService,
    public comboboxService: ComboboxService,
  ) {
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fas fa-plus' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }
  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  ProjectId: string;
  validMessage: string;
  ParentId: string;
  isValid: boolean = true;
  dateNow = new Date();
  userName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
  listFile: any[] = [];
  listData: any[] = [];
  listProjectAttachTabType = [];
  selectedProjectId = '';
  projectAttchId: '';
  heightLeft = 0;
  expandGroupKeys: any[] = [];
  selectGroupKeys: any[] = [];
  selectedGroupId: any = '';
  model: any = {
    Id: '',
    ProjectId: '',
    JuridicalFiles: [],
    TechnicalFiles: [],
    OtherFiles: [],
  }
  test: any[] = [];
  fileTemplateMaterial = '';
  projectAttachTabTotal: number;
  items: any;

  fileModel: any = {
    Id: '',
    Path: '',
    Name: '',
    Description: '',
    FileName: '',
    FileSize: '',
    CreateByName: '',
    CreateDate: Date,
  }


  fileUpload: any[] = [];
  fileSelect: any = {};
  @ViewChild('fileInputDocument') inputFile: ElementRef

  ngOnInit() {
    this.fileProcess.FilesDataBase = [];
    this.model.ProjectId = this.Id;
    this.searchProjectAttachTabType();
    this.getProjectAttach();
    this.selectedProjectId = localStorage.getItem("selectedProjectId");
    localStorage.removeItem("selectedProjectId");
    this.heightLeft = window.innerHeight - 200;
  }

  getProjectAttach() {
    this.model.projectId = this.Id;
    this.projectAttachService.getProjectAttach(this.model).subscribe((data: any) => {
      if (data) {
        this.model.JuridicalFiles = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }


  uploadFile(row, index) {
    this.fileSelect = row;
    this.inputFile.nativeElement.click();
  }

  uploadFileDocument($event: any) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    this.fileSelect.File = fileDataSheet[0];
    this.fileSelect.FileName = fileDataSheet[0].name;
    this.fileSelect.FileSize = fileDataSheet[0].size;
    this.fileSelect.CreateDate = new Date();
    this.fileSelect.CreateByName = this.userName;
  }

  createUpdateProjectAttach(type: number, row: { Name: any; SupplierId: any; CustomerId: any; FileName: any; FileSize: any; File: any; Description: any; PromulgateType: any; GroupName: any; PromulgateDate: any; PromulgateName: any; PromulgateCode: any; }) {
    // if(Id != null){
    // this.getAttachProject(Id);
    // }
    let activeModal = this.modalService.open(ProjectAttachCreateComponent, { container: 'body', windowClass: 'project-attach-create', backdrop: 'static' })
    activeModal.componentInstance.projectAttachModel = row ? Object.assign({}, row) : null;
    if (type == 1) {
      activeModal.componentInstance.projectAttachs = this.model.JuridicalFiles;
      activeModal.componentInstance.listCheckAttachs = this.model.JuridicalFiles;
      activeModal.componentInstance.ParentId = this.ParentId;
      activeModal.componentInstance.ProjectId = this.Id;
    }
    else if (type == 2) {
      activeModal.componentInstance.projectAttachs = this.model.TechnicalFiles;
      activeModal.componentInstance.listCheckAttachs = this.model.TechnicalFiles;
      activeModal.componentInstance.ParentId = this.ParentId;
      activeModal.componentInstance.ProjectId = this.Id;
    }
    else if (type == 3) {
      activeModal.componentInstance.projectAttachs = this.model.OtherFiles;
      activeModal.componentInstance.listCheckAttachs = this.model.OtherFiles;
      activeModal.componentInstance.ParentId = this.ParentId;
      activeModal.componentInstance.ProjectId = this.Id;
    }
    activeModal.result.then((results) => {
      if (results) {
        this.save();
      }
    }, (reason) => {
    });
  }





  save() {
    this.saveProjectAttach();
  }

  saveProjectAttach() {
    this.fileUpload = [];
    this.setFileUpload(this.model.JuridicalFiles, 'JuridicalFiles');
    if (this.fileUpload.length > 0) {
      let questionFiles = this.uploadService.uploadListFilePDF(this.fileUpload, 'DocumentFile/');
      forkJoin([questionFiles]).subscribe(results => {
        if (results[0].length > 0) {
          // results[0].forEach(item => {
          //   this.fileUpload.forEach(f =>{
          //     var file = this.model.JuridicalFiles[f.Index];
          //     file.Path = item.FileUrl;
          //     file.PDFLinkFile = item.FilePDFUrl;
          //   });
          //   // var file = this.model.JuridicalFiles.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
          //   // file.Path = item.FileUrl;
          //   // file.PDFLinkFile = item.FilePDFUrl;
          // });
            this.fileUpload.forEach(f =>{
              var file = this.model.JuridicalFiles[f.Index];
              results[0].forEach(item => {
                if(file.FileName == item.FileName && file.FileSize == item.FileSize ){
                  file.Path = item.FileUrl;
                  file.PDFLinkFile = item.FilePDFUrl;
                }
              })

            });
            // var file = this.model.JuridicalFiles.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
            // file.Path = item.FileUrl;
            // file.PDFLinkFile = item.FilePDFUrl;
        }
        this.projectAttachService.addProjectAttach(this.model).subscribe(
          () => {
              var check = false;
            this.model.JuridicalFiles.forEach( element =>{
              if(element.Id == null){
                check=true;
              }
            })
            if(check){
              this.messageService.showSuccess('Cập nhật tài liệu thành công!');
            }
            this.getProjectAttach();
          }, error => {
            this.messageService.showError(error);
          });
        // this.updateDocumentFile();
      }, error => {
        this.messageService.showError(error);
      });
    }
    else{
      this.projectAttachService.addProjectAttach(this.model).subscribe(
            () => {
                var check = false;
              this.model.JuridicalFiles.forEach( element =>{
                if(element.Id == null){
                  check=true;
                }
              })
              if(check){
                this.messageService.showSuccess('Cập nhật tài liệu thành công!');
              }
              this.getProjectAttach();
            }, error => {
              this.messageService.showError(error);
            });
    }
    // this.uploadService.uploadListFile(this.fileUpload, 'ProjectAttach/').subscribe((event: any) => {
    //   this.fileUpload.forEach((item, index) => {
    //     if (item.Type == 'JuridicalFiles') {
    //       this.model.JuridicalFiles[item.Index].Path = event[index].FileUrl;
    //     }
    //   });

    //   this.projectAttachService.addProjectAttach(this.model).subscribe(
    //     () => {
    //         var check = false;
    //       this.model.JuridicalFiles.forEach( element =>{
    //         if(element.Id == null){
    //           check=true;
    //         }
    //       })
    //       if(check){
    //         this.messageService.showSuccess('Cập nhật tài liệu thành công!');
    //       }
    //       this.getProjectAttach();
    //     }, error => {
    //       this.messageService.showError(error);
    //     });
    // }, error => {
    //   this.messageService.showError(error);
    // });
  }

  setFileUpload(files: any[], type: string) {
    files.forEach((item: { File: { Index: any; Type: any; }; }, index: any) => {
      if (item.File) {
        item.File.Index = index;
        item.File.Type = type;
        this.fileUpload.push(item.File);
      }
    });
  }

  showConfirmDeleteDocument(row: any, index: any, type: number) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa tài liệu này không?").then(
      data => {
        if (row) {
          row.IsDelete = true;
          this.saveProjectAttach();
        }
        else {
          if (type == 1) {
            this.model.JuridicalFiles.splice(index, 1);
          }
          else if (type == 2) {
            this.model.TechnicalFiles.splice(index, 1);
          } else if (type == 3) {
            this.model.OtherFiles.splice(index, 1);
          }
          this.saveProjectAttach();
        }
        this.messageService.showSuccess("Xóa tài liệu thành công!");
      },
      error => {

      }
    );
  }

  showConfirmDeleteFile(row: { Path: any; File: any; FileName: any; FileSize: any; CreateDate: any; CreateByName: any; }) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file tài liệu đính kèm này không?").then(
      data => {
        row.Path = null;
        row.File = null;
        row.FileName = null;
        row.FileSize = null;
        row.CreateDate = null;
        row.CreateByName = null;
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {

      }
    );
  }

  downloadAFile(file: { Path: any; FileName: any; }) {
    // this.getAttachProject(Id);
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  exportExcel() {
    this.projectAttachService.exportExcel(this.Id).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }
  showImportExcel() {
    this.projectAttachService.getGroupInTemplate(this.fileModel).subscribe(d => {
      this.fileTemplateMaterial = this.config.ServerApi + d;
      this.componentService.showImportExcel(this.fileTemplateMaterial, false).subscribe(data => {
        if (data) {
          this.projectAttachService.importFileProjectAttach(data, this.Id).subscribe(
            data => {
              this.getProjectAttach();
              this.searchProjectAttachTabType();
              this.messageService.showSuccess('Import tài liệu!');
            },
            error => {
              this.messageService.showError(error);
            });
        }
      });
    }, e => {
      this.messageService.showError(e);
    });
  }

  showProject(Id: string) {
    let activeModal = this.modalService.open(ViewProjectAttachTabComponent, { container: 'body', windowClass: 'view-project-attach-tab-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.getProjectAttach();
      }
    }, (reason) => {
    });
  }

  showCreateUpdateType(Id: string, isUpdate: Boolean) {
    let activeModal = this.modalService.open(ProjectAttachTabTypeComponent, { container: 'body', windowClass: 'project-attach-tab-type-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.id = Id;
      activeModal.componentInstance.ProjectId = this.Id;
    } else {
      activeModal.componentInstance.parentId = Id;
      activeModal.componentInstance.ProjectId = this.Id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectAttachTabType();
      }
    }, (reason) => {
    });
  }

  searchProjectAttachTabType() {
    this.comboboxService.GetProjectAttachTabType(this.Id).subscribe((data: any) => {
      if (data) {
        this.listProjectAttachTabType = data;
        this.projectAttachTabTotal = this.listProjectAttachTabType.length;
        // this.setSelectGroup();

        let modelAll: any = {
          Id: '',
          Name: 'Tất cả',
          Code: '',
        }
        this.listProjectAttachTabType.unshift(modelAll);
        this.selectGroupKeys = [this.selectedGroupId];
        // this.getProjectAttach();
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null) {
      this.ParentId = e.selectedRowKeys[0];
      this.model.ParentId = this.ParentId;
      this.getProjectAttach();
      // this.moduleGroupId = e.selectedRowKeys[0];
      // localStorage.setItem("selectedModuleGroupId", this.selectedModelGroupId);
      //this.modelProduct.Status = this.Status;
    }
  }

  itemClick(e) {
    if (!this.ParentId) {
      this.messageService.showMessage("Đây không phải chủng loại!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateType(this.ParentId, false);
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdateType(this.ParentId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmType(this.ParentId);
      }
    }
  }

  showConfirmType(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chủng loại này không?").then(
      data => {
        this.deleteType(row);
      },
      error => {

      }
    );
  }


  deleteType(row) {
    this.projectAttachService.deleteType(row).subscribe(
      data => {
        this.searchProjectAttachTabType();
        this.messageService.showSuccess('Xóa chủng loại thành công!');
        this.ngOnInit();
      },
      error => {
        this.messageService.showError(error);
      });
  }

}

