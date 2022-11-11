import { Component, OnInit, Input} from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Configuration, Constants } from 'src/app/shared';

import { ModuleServiceService } from '../../services/module-service.service';
import { ModuleTestCriteriaService } from '../../services/module-test-criteria.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-module-tab-criteria',
  templateUrl: './module-tab-criteria.component.html',
  styleUrls: ['./module-tab-criteria.component.scss']

})

export class ModuleTabCriteriaComponent implements OnInit{
  
  @Input() Id: string;
  @Input() ModuleGroupId: string;

  constructor(
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private moduleService: ModuleServiceService,
    public constant: Constants,
    private testCriteriaModel: ModuleTestCriteriaService,
    private serviceHistory: HistoryVersionService
  ) { }

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
    TestCriteriaGroupId: '',
    TechnicalRequirements: '',
    Note: '',
    ModuleId: '',
    ListTestCriteriaModule: [],
  }
  height = 200;
  idUpdate: string;
  listGroupModule: any[] = [];
  listGroupModuleId: any[] = [];
  listData: any = [];
  listTestCriteriaModule: any = [];

  ngOnInit() {
    this.height = window.innerHeight - 290;
    this.model.ModuleGroupId = this.ModuleGroupId;
    this.model.ModuleId = this.Id;
    this.getListTestCriteria();
  }

  moduleModelId: string;

  deleteTestCeiteria(model:any) {
    var index = this.listTestCriteriaModule.indexOf(model);
    if (index > -1) {
      this.listTestCriteriaModule.splice(index, 1);
      // this.messageService.showSuccess('Xóa tiêu chí thành công!');
    }
  }

  getListTestCriteria() {
    this.testCriteriaModel.getModuleGroupInfo({ Id: this.model.ModuleGroupId }).subscribe(
      data => {
        this.listData = data.ListTestCriteri;
        this.searchTestCriteria();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchTestCriteria() {
    this.testCriteriaModel.getModuleTestCriteriaInfo({ Id: this.model.ModuleId }).subscribe(data => {
      this.listTestCriteriaModule = data.ListTestCriteri;
      for (var item of this.listData) {
        for (var element of this.listTestCriteriaModule) {
          if (element.TestCriteriasId == item.Id) {
            item.Checked = true;
          }
        }
      }
    }, error => {
      this.messageService.showError(error);
    })
  }

  exportExcelCriteria() {
    this.moduleService.ExportExcelCriteria(this.model).subscribe(d => {
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

  save() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.model.ListTestCriteriaModule.push(element.Id);
      }
    });

    this.testCriteriaModel.createTestCriteria(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới module tiêu chí thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
    this.listTestCriteriaModule = [];
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Module;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save();
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

}
