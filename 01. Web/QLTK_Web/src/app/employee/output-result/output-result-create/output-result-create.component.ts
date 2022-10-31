import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { OutputResultService } from '../../service/output-result.service';
import { ChooseFlowStageComponent } from '../choose-flow-stage/choose-flow-stage.component';
import { forkJoin } from 'rxjs';
import { ChooseDocumentComponent } from '../choose-document/choose-document.component';

@Component({
  selector: 'app-output-result-create',
  templateUrl: './output-result-create.component.html',
  styleUrls: ['./output-result-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class OutputResultCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private outputResultService: OutputResultService,
    public constants: Constants,
    private modalService: NgbModal,
    private comboboxService: ComboboxService,) { }

  isAction: boolean = false;
  id: any;
  modalInfo = {
    Title: 'Thêm mới kết quả đầu ra',
    SaveText: 'Lưu',
  };

  outputResultModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    SBUid: '',
    DepartmentId: '',
    FlowStages: [],
    Documents: []
  }

  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];
  subs: any[] = [];
  departments: any[] = [];

  ngOnInit(): void {
    this.getCbbData();
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa kết quả đầu ra';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới kết quả đầu ra';
    }
  }

  getCbbData() {
    let cbbSBU = this.comboboxService.getCbbSBU();
    forkJoin([cbbSBU]).subscribe(results => {
      this.subs = results[0];
    });

  }

  getCbbDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.outputResultModel.SBUId).subscribe(data => {
      if (this.outputResultModel.SBUId != null && this.outputResultModel.SBUId != "") {
        this.departments = data;
      } else {
        this.departments = [];
      }
    }, error => {
      this.messageService.showError(error);
    })
  }

  getInfo() {
    this.outputResultService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.outputResultModel = result;
        this.getCbbDepartment();
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.outputResultService.create(this.outputResultModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới kết quả đầu ra thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới kết quả đầu ra thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.outputResultService.update(this.outputResultModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật kết quả đầu ra thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.outputResultModel = {
      Id: '',
      Name: '',
      Code: '',
      Note: '',
      SBUid: '',
      DepartmentId: '',
      FlowStages: [],
      Document: []
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  chooseOutputResult() {
    let activeModal = this.modalService.open(ChooseFlowStageComponent, { container: 'body', windowClass: 'choose-flowstage-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.outputResultModel.FlowStages.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code,
          }
          this.outputResultModel.FlowStages.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteOutputResult(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá dòng chảy này không?").then(
      data => {
        this.outputResultModel.FlowStages.splice(index, 1);
        this.messageService.showSuccess('Xóa dòng chảy thành công!');
      },
      error => {

      }
    );
  }

  chooseDocument() {
    let activeModal = this.modalService.open(ChooseDocumentComponent, { container: 'body', windowClass: 'choose-document-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.outputResultModel.Documents.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code,
            DocumentGroupName: element.DocumentGroupName
          }
          this.outputResultModel.Documents.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteDocument(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.outputResultModel.Documents.splice(index, 1);
        this.messageService.showSuccess('Xóa tài liệu thành công!');
      },
      error => {

      }
    );
  }

}
