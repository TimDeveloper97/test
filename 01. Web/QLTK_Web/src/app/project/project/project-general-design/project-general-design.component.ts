import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { AppSetting, MessageService, Constants, Configuration } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectGeneralDesignService } from '../../service/project-general-design.service';
import { ProjectGeneralDesignCreateComponent } from '../project-general-design-create/project-general-design-create.component';

@Component({
  selector: 'app-project-general-design',
  templateUrl: './project-general-design.component.html',
  styleUrls: ['./project-general-design.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectGeneralDesignComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    private service: ProjectGeneralDesignService,
    public constant: Constants
  ) { }
  //@Input() Id: string;
  startIndex = 0;
  projectProductId: string;
  projectId: string;
  listData: any[] = [];
  listManager: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'CreateIndex',
    OrderType: true,
    ProjectId: '',
    ProjectProductId: ''
  }

  ngOnInit() {
    this.model.ProjectId = this.projectId;
    this.model.ProjectProductId = this.projectProductId;
    this.searchProjectGeneralDesign();
  }

  searchProjectGeneralDesign() {
    this.service.searchProjectGeneralDesign(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'CreateIndex',
      OrderType: true,
    }
    this.searchProjectGeneralDesign();
  }

  showConfirmDeleteProjectGeneralDesign(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tổng hợp này không?").then(
      data => {
        this.deleteProjectGeneralDesign(Id);
      },
      error => {
        
      }
    );
  }

  deleteProjectGeneralDesign(Id: string) {
    this.service.deleteProjectGeneralDesign({ Id: Id }).subscribe(
      data => {
        this.searchProjectGeneralDesign();
        this.messageService.showSuccess('Xóa tổng hợp thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel(Id: string) {
    this.model.Id = Id;
    this.service.exportExcelManage(this.model).subscribe(d => {
      if (d != "ModuleError") {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerApi + d;
        link.download = 'Download.docx';
        document.body.appendChild(link);
        // link.focus();
        link.click();
        document.body.removeChild(link);
      }
      else {
        this.messageService.showMessage("Module có lỗi nên không thể xuất Excel!");
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  exportExcelBOM(Id: string) {
    this.model.Id = Id;
    this.service.exportExcelBOM(this.model).subscribe(d => {
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

  showCreateUpdate(Id: string) {
    if (!Id) {
      this.service.checkApproveStatus(this.model.ProjectProductId).subscribe(
        data => {
          if (data) {
            this.show(Id);
          } else {
            this.messageService.showMessage("Bạn chưa phê duyệt lần tổng hợp gần nhất");
          }
        },
        error => {
          this.messageService.showError(error);
        });
    } else {
      this.show(Id);
    }
  }

  show(Id: string) {
    let activeModal = this.modalService.open(ProjectGeneralDesignCreateComponent, { container: 'body', windowClass: 'project-ganeral-design-create', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.projectId = this.projectId;
    activeModal.componentInstance.projectProductId = this.projectProductId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectGeneralDesign();
      }
    }, (reason) => {
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(true);
  }
}
