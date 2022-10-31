import { Component, OnInit, Input } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Configuration, Constants } from 'src/app/shared';
import { ModuleServiceService } from '../../services/module-service.service';
import { ModuleChooseDesignerTabComponent } from '../module-choose-designer-tab/module-choose-designer-tab.component';

@Component({
  selector: 'app-module-designer-tab',
  templateUrl: './module-designer-tab.component.html',
  styleUrls: ['./module-designer-tab.component.scss']
})
export class ModuleDesignerTabComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private service: ModuleServiceService,
    public constant: Constants
  ) { }

  listData: any = [];
  listEmployee: any = [];
  modelDesigner: any = {
    Id: '',
    ModuleId: '',
    EmployeeId: '',
    ListEmployee: []
  }

  ngOnInit() {
    this.modelDesigner.ModuleId = this.Id;
    this.getDesigners();
  }

  showClick() {
    let activeModal = this.modalService.open(ModuleChooseDesignerTabComponent, { container: 'body', windowClass: 'module-designer-model', backdrop: 'static' });
    activeModal.componentInstance.ModuleId = this.modelDesigner.ModuleId;
    var ListIdSelect = [];
    this.listEmployee.forEach(element => {
      ListIdSelect.push(element.Id);
    });
    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result) {
        setTimeout(() => {
          // this.getDesignerInfo();
        }, 200);
      }
    }, (reason) => {
    });
  }

  getDesigners() {
    this.service.getDesigners(this.Id).subscribe(data => {
      this.listEmployee = data;
    },
    error => {
      this.messageService.showError(error);
    });
  }

  showConfirmDeleteDesigner(Id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhân viên thiết kế này không?").then(
      data => {
        this.deleteModuleDesigner(Id);
      },
      error => {
        
      }
    );
  }

  deleteModuleDesigner(Id: string) {
    this.modelDesigner.Id = Id;
    this.service.DeleteDesigner(this.modelDesigner).subscribe(
      data => {
        // this.getDesignerInfo();
        this.messageService.showSuccess('Xóa nhà thiết kế thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  deleteDesigner(model:any) {
    var index = this.listEmployee.indexOf(model);
    if (index > -1) {
      this.listEmployee.splice(index, 1);
    }
  }

  exportExcel() {
    this.service.exportExcel(this.modelDesigner).subscribe(d => {
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
}
