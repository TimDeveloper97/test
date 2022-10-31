import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { FlowStageService } from '../../service/flow-stage.service';
import { ChooseOutputResultComponent } from '../choose-output-result/choose-output-result.component';

@Component({
  selector: 'app-flow-stage-create',
  templateUrl: './flow-stage-create.component.html',
  styleUrls: ['./flow-stage-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FlowStageCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private flowStageService: FlowStageService,
    private comboboxService: ComboboxService,
    public constants: Constants,
    private modalService: NgbModal,) { }

  isAction: boolean = false;
  id: any;
  parentId: any;
  modalInfo = {
    Title: 'Thêm mới dòng chảy',
    SaveText: 'Lưu',
  };

  flowStageModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    ParentId: null,
    OutputResults: []
  }

  flowStages: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã dòng chảy' }, { Name: 'Name', Title: 'Tên dòng chảy' }]

  ngOnInit(): void {
    this.getCbbflowStage();
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa dòng chảy';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.flowStageModel.ParentId = this.parentId;
      this.modalInfo.Title = 'Thêm mới dòng chảy';
    }
  }

  getCbbflowStage() {
    this.comboboxService.getCbbFlowStage().subscribe(data => {
      this.flowStages = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  getInfo() {
    this.flowStageService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.flowStageModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.flowStageService.create(this.flowStageModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới dòng chảy thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới dòng chảy thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.flowStageService.update(this.flowStageModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật dòng chảy thành công!');
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
    let flowStageId = this.flowStageModel.ParentId;
    this.flowStageModel = {
      Id: '',
      Name: '',
      Code: '',
      Note: '',
      ParentId: flowStageId,
      OutputResults: []
    };
    this.getCbbflowStage();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  chooseOutputResult() {
    let activeModal = this.modalService.open(ChooseOutputResultComponent, { container: 'body', windowClass: 'choose-outputresult-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.flowStageModel.OutputResults.forEach(element => {
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
            SBUName: element.SBUName,
            DepartmentName: element.DepartmentName
          }
          this.flowStageModel.OutputResults.push(data);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteOutputResult(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kết quả đầu ra này không?").then(
      data => {
        this.flowStageModel.OutputResults.splice(index, 1);
        this.messageService.showSuccess('Xóa kết quả đầu ra thành công!');
      },
      error => {

      }
    );
  }


}
