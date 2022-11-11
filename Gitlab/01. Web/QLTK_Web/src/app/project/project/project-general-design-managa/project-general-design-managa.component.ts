import { Component, OnInit, Input, ViewChild, ViewEncapsulation } from '@angular/core';
import { DxTreeListComponent } from 'devextreme-angular';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, DateUtils, Constants } from 'src/app/shared';
import { ProjectGeneralDesignService } from '../../service/project-general-design.service';
import { ProjectGeneralDesignCreateComponent } from '../project-general-design-create/project-general-design-create.component';
import { ProjectGeneralDesignComponent } from '../project-general-design/project-general-design.component';

@Component({
  selector: 'app-project-general-design-managa',
  templateUrl: './project-general-design-managa.component.html',
  styleUrls: ['./project-general-design-managa.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectGeneralDesignManagaComponent implements OnInit {

  @Input() Id: string;
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private modalService: NgbModal,
    private messageService: MessageService,
    private service: ProjectGeneralDesignService,
    public dateUtils: DateUtils,
    public constants: Constants,
  ) {
    this.items = [
      { Id: 1, text: 'Lịch sử tổng hợp thiết kế', icon: 'fas fa-file-excel text-success' },
    ];
  }

  items: any;
  listDA: any[] = [];
  projectProductId: '';
  selectedProjectProductId = '';
  listProductGroupId: any[] = [];

  model: any = {
    ProjectId: '',
    ProjectProductId: ''
  }

  ngOnInit() {
    this.model.ProjectId = this.Id;
    this.searchProjectProductExport();
    this.selectedProjectProductId = localStorage.getItem("selectedProjectProductId");
    localStorage.removeItem("selectedProjectProductId");
  }

  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.showProjectGeneralDesign(this.projectProductId);
    }
  }

  onSelectionChanged(e) {
    this.selectedProjectProductId = e.selectedRowKeys[0];
    this.projectProductId = e.selectedRowKeys[0];
  }

  showProjectGeneralDesign(Id: string) {
    let activeModal = this.modalService.open(ProjectGeneralDesignComponent, { container: 'body', windowClass: 'project-general-design', backdrop: 'static' })
    activeModal.componentInstance.projectProductId = Id;
    activeModal.componentInstance.projectId = this.model.ProjectId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectProductExport();
      }
    }, (reason) => {
    });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ProjectGeneralDesignCreateComponent, { container: 'body', windowClass: 'project-ganeral-design-create', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.projectId = this.Id;
    activeModal.componentInstance.projectProductId = this.projectProductId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectProductExport();
      }
    }, (reason) => {
    });
  }

  searchProjectProductExport() {
    this.model.ProjectProductId = this.projectProductId;
    this.service.searchProjectProductExport(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listDA = data.ListResult;
        if (!this.selectedProjectProductId && this.listDA.length > 0) {
          this.selectedProjectProductId = this.listDA[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedProjectProductId];
        for (var item of this.listDA) {
          this.listProductGroupId.push(item.Id);
        }
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  ClickTHTK() {
    let activeModal = this.modalService.open(ProjectGeneralDesignCreateComponent, { container: 'body', windowClass: 'project-ganeral-design-create', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.Id;
    activeModal.componentInstance.projectProductId = this.projectProductId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectProductExport();
      }
    }, (reason) => {
    });
  }

}
