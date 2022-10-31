import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Configuration, Constants, MessageService } from 'src/app/shared';
import { WorktypeInterviewService } from '../../service/worktype-interview.service';
import { forkJoin } from 'rxjs';
import { ChooseQuestionComponent } from '../choose-question/choose-question.component';
import { DownloadService } from 'src/app/shared/services/download.service';

@Component({
  selector: 'app-worktype-interview-create',
  templateUrl: './worktype-interview-create.component.html',
  styleUrls: ['./worktype-interview-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WorktypeInterviewCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    private workTypeInterviewService: WorktypeInterviewService,
    private comboboxService: ComboboxService,
    private modalService: NgbModal,
    private dowloadservice: DownloadService,
    private config: Configuration,) { }

  isAction: boolean = false;
  id: string = '';
  workTypeId: string;
  modalInfo = {
    Title: 'Thêm mới câu hỏi',
    SaveText: 'Lưu',
  };

  departments: any[] = [];
  sbus: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];

  workTypeInterviewModel: any = {
    Id: '',
    WorkTypeId: '',
    Name: '',
    DepartmentId: '',
    SBUId: '',
    Questions: []
  };

  codeSBU: any;
  codeDepartment: any;

  ngOnInit(): void {
    this.getCbbData();
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa Lần phỏng vẩn';
      this.modalInfo.SaveText = 'Lưu';
    }
    else {
      this.workTypeInterviewModel.WorkTypeId = this.workTypeId;
      this.modalInfo.Title = 'Thêm mới lần phỏng vấn';
    }
  }

  getCbbData() {
    let cbbSBU = this.comboboxService.getCBBSBU();
    forkJoin([cbbSBU]).subscribe(results => {
      this.sbus = results[0];
      if (this.id) {
        this.getInfo();
      }
    });
  }

  getCbbDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.workTypeInterviewModel.SBUId).subscribe(
      data => {
        this.departments = data;
        this.getNameDepartment(this.workTypeInterviewModel.DepartmentId);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getName(SBUId) {
    for (var item of this.sbus) {
      if (item.Id == SBUId) {
        this.codeSBU = item.Code;
        this.codeDepartment = '';
      }
    }
  }

  getNameDepartment(DepartmentId) {
    for (var item of this.departments) {
      if (item.Id == DepartmentId) {
        this.codeDepartment = item.Code;
      }
    }

  }

  getInfo() {
    this.workTypeInterviewService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.workTypeInterviewModel = result;
        this.getName(this.workTypeInterviewModel.SBUId);
        this.getCbbDepartment();
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.workTypeInterviewService.create(this.workTypeInterviewModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới câu hỏi thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới câu hỏi thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.workTypeInterviewService.update(this.workTypeInterviewModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật câu hỏi thành công!');
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

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.workTypeInterviewModel = {
      Id: '',
      WorkTypeId: '',
      Name: '',
      DepartmentId: '',
      SBUId: '',
      Questions: []
    };
    this.codeSBU = '';
    this.codeDepartment = '';
  }

  chooseQuestion() {
    let activeModal = this.modalService.open(ChooseQuestionComponent, { container: 'body', windowClass: 'choose-question-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.workTypeInterviewModel.Questions.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.workTypeInterviewModel.Questions.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteQuestion(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa câu hỏi này không?").then(
      data => {
        if (this.workTypeInterviewModel.Questions.length > 0) {
          this.workTypeInterviewModel.Questions.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Datashet.forEach(element => {
      listFilePath.push({
        Path: element.FilePath,
        FileName: element.FileName
      });
    });

    let modelDowload: any = {
      Name: '',
      ListDatashet: []
    }

    modelDowload.Name = "Tài liệu";
    modelDowload.ListDatashet = listFilePath;
    this.dowloadservice.downloadAll(modelDowload).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

}
