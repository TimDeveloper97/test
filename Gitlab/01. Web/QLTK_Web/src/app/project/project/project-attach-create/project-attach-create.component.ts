import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, forwardRef } from '@angular/core';
import { MessageService, Configuration, FileProcess, ComboboxService, DateUtils, Constants } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ProjectAttachService } from '../../service/project-attach.service';
import { ShowChooseEmployeeComponent } from 'src/app/sale/sale-group/show-choose-employee/show-choose-employee.component';

@Component({
  selector: 'app-project-attach-create',
  templateUrl: './project-attach-create.component.html',
  styleUrls: ['./project-attach-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectAttachCreateComponent implements OnInit {

  @ViewChild('scrollModuleHeader', { static: false }) scrollModuleHeader: ElementRef;
  @ViewChild('scrollModule', { static: false }) scrollModule: ElementRef;

  modalInfo = {
    Title: 'Thêm mới tài liệu'
  };

  listCheckAttachs: any[] = [];
  isAction: boolean = false;
  projectAttachModel: any = null;
  projectAttachs: any[] = [];
  PromulgateDateV = null;
  listProjectAttach: any[] = [];
  columnName: any[] = [ { Name: 'Name', Title: 'Tên' },];
  columnCustomers: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  columnSuppliers: any[] = [{ Name: 'Code', Title: 'Mã NCC' }, { Name: 'Name', Title: 'Tên NCC' }];
  listUserId: any[] = [];
  closes: boolean = true;
  projectAttachModel1: any = null;
  listDA = [];
  ParentId : string;
  ProjectId : string;
  AttachId ='';

  fileImportModel: any = {
    Index: 0,
    File: null
  };
  type: any;
  isEdit: boolean = false;
  listFile: any[] = [];
  customers: any[] = [];
  suppliers: any[] = [];
  check: boolean = false;
  templatePath: string = '';

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    private projectAttachService: ProjectAttachService,
    private combobox: ComboboxService,
    private dateUtil: DateUtils,
    private modalService: NgbModal,
    public constant: Constants,
  ) { }

  ngOnInit() {
    this.getCustomers();
    this.getSuppliers();
    this.getListProjectAttach();
    this.getListType();


    if (this.projectAttachModel) {
      this.getProjectAttachInfo();
      this.modalInfo.Title = "Chỉnh sửa tài liệu";
    } else {
      this.projectAttachModel = {
        Id: null,
        Name: null,
        Description: null,
        FileName: null,
        FileSize: null,
        Path: null,
        PromulgateType: 1,
        CustomerId: null,
        SupplierId: null,
        GroupName: null,
        PromulgateDate: null,
        IsRequired: true,
        ListUser: [],
        ParentId : this.ParentId
      };
    }
  }

  ngAfterViewInit() {
    this.scrollModule.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  getCustomers() {
    this.combobox.getListCustomer().subscribe(
      data => {
        this.customers = data;
      },
      error => {
        this.messageService.showError(error);
      });
  }

  getSuppliers() {
    this.combobox.getCBBSupplier().subscribe(
      data => {
        this.suppliers = data;
      },
      error => {
        this.messageService.showError(error);
      });
  }
  getListProjectAttach() {
    this.combobox.getListProjectAttach().subscribe(
      data => {
        this.listProjectAttach = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  uploadFileDocument($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    this.projectAttachModel.File = fileDataSheet[0];
    this.projectAttachModel.FileName = fileDataSheet[0].name;
    this.projectAttachModel.FileSize = fileDataSheet[0].size;
    this.projectAttachModel.CreateDate = new Date();
  }

  saveAndContinue() {
    // this.closes = isClose;
    this.save(true);
  }

  save(isContinue: boolean) {
    this.closes = !isContinue;
    if (this.projectAttachModel.PromulgateType == 1) {
      if (this.projectAttachModel.CustomerId) {
        let customerSelect = this.customers.filter(item => {
          return item.Id == this.projectAttachModel.CustomerId;
        });

        this.projectAttachModel.PromulgateName = customerSelect[0].Name;
        this.projectAttachModel.PromulgateCode = customerSelect[0].Code;
      }
    }
    else if (this.projectAttachModel.PromulgateType == 2) {
      if (this.projectAttachModel.SupplierId) {
        let supplierSelect = this.suppliers.filter(item => {
          return item.Id == this.projectAttachModel.SupplierId;
        });

        this.projectAttachModel.PromulgateName = supplierSelect[0].Name;
        this.projectAttachModel.PromulgateCode = supplierSelect[0].Code;
      }
    }

    this.projectAttachModel.PromulgateDate = null;
    if (this.PromulgateDateV) {
      this.projectAttachModel.PromulgateDate = this.dateUtil.convertObjectToDate(this.PromulgateDateV);
    }

    if (this.projectAttachModel.IsRequired) {
      if(this.projectAttachModel.FileSize != null){
        if ( this.projectAttachModel.Id) {
          //this.closeModals(true);
          this.projectAttachs.forEach((elements,index) => {
            if (this.projectAttachModel.Id == elements.Id) {
              elements = this.projectAttachModel;
              this.projectAttachs.push(elements);
              this.projectAttachs.splice(index, 1);
              if(!isContinue){
                this.activeModal.close(this.projectAttachs);
      
              }  
            }
          });
        }
        else {
          this.projectAttachModel.ProjectId = this.ProjectId;
          this.projectAttachService.CheckNameProjectAttach(this.projectAttachModel).subscribe(
            (data) => {
              this.projectAttachs.push(this.projectAttachModel);
  
              this.projectAttachModel = {
                Id: null,
                Name: null,
                Description: null,
                FileName: null,
                FileSize: null,
                Path: null,
                PromulgateType: 1,
                CustomerId: null,
                SupplierId: null,
                GroupName: null,
                PromulgateDate: null,
                IsRequired : true,
              };
              if(!isContinue){
                this.activeModal.close(this.projectAttachs);
      
              }  
            }, error => {
              this.messageService.showError(error);
            });
              
        }
      }
      else{
        this.messageService.showMessage('Bạn chưa upload file không thể thêm mới!');
        
      }
    }
    else{
      //if(this.projectAttachModel.FileSize == null){
        if ( this.projectAttachModel.Id) {
          //this.closeModals(true);
          this.projectAttachs.forEach((elements,index) => {
            if (this.projectAttachModel.Id == elements.Id) {
              elements = this.projectAttachModel;
              this.projectAttachs.push(elements);
              this.projectAttachs.splice(index, 1);
              if(!isContinue){
                this.activeModal.close(this.projectAttachs);
      
              }  
            }
          });
        }
        else {
          this.projectAttachModel.ProjectId = this.ProjectId;
          this.projectAttachService.CheckNameProjectAttach(this.projectAttachModel).subscribe(
            (data) => {
              this.projectAttachs.push(this.projectAttachModel);
  
              this.projectAttachModel = {
                Id: null,
                Name: null,
                Description: null,
                FileName: null,
                FileSize: null,
                Path: null,
                PromulgateType: 1,
                CustomerId: null,
                SupplierId: null,
                GroupName: null,
                PromulgateDate: null,
                IsRequired : true,
              };
              if(!isContinue){
                this.activeModal.close(this.projectAttachs);
      
              }   
            }, error => {
              this.messageService.showError(error);
            });
 
        }

      // }
      // else{
      //   this.messageService.showMessage('Bạn không được upload file, không thể thêm mới!');
        
      // }
    }
  }

  closeModal() {
    this.activeModal.close(true);
  }

  closeModals(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  showSelectEmployee() {
    let activeModal = this.modalService.open(ShowChooseEmployeeComponent, { container: 'body', windowClass: 'select-employee-model', backdrop: 'static' });

    activeModal.componentInstance.listEmployeeId = this.listUserId;
    activeModal.componentInstance.check = true;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var count =0;
          element.IsNew = true;
          this.projectAttachModel.ListUser.forEach(item =>{
            if(item.Id == element.Id){
              count++;
            }
          });
          if(count==0){
            this.projectAttachModel.ListUser.push(element);
          }
        });
        var j = 1;
        this.projectAttachModel.ListUser.forEach(element => {
          element.index = j++;
          element.IsDelete =false;
        });
      }
    }, (reason) => {
    });
  }

  showComfirmDeleteEmployee(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá người khảo sát này không?").then(
      data => {
        this.projectAttachModel.ListUser[index].IsDelete=true;
        // this.projectAttachModel.ListUser.splice(index, 1);
      },
      error => {

      }
    );
  }

  getListType() {
    this.projectAttachService.getProjectDocType(this.ProjectId).subscribe((data: any) => {
      if (data) {
        this.listDA = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getProjectAttachInfo() {
    this.projectAttachService.getProjectAttachInfo(this.projectAttachModel).subscribe((data: any) => {
      if (data) {
        this.projectAttachModel = data;
        this.isEdit = true;
        if(this.projectAttachModel.PromulgateDate){
          this.PromulgateDateV = this.dateUtil.convertDateToObject(this.projectAttachModel.PromulgateDate);
        }
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

}
