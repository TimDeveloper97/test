import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, MessageService } from 'src/app/shared';
import { QuestionService } from '../../service/question.service';

@Component({
  selector: 'app-question-group-create',
  templateUrl: './question-group-create.component.html',
  styleUrls: ['./question-group-create.component.scss']
})
export class QuestionGroupCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private questionService: QuestionService,
    private comboboxService: ComboboxService) { }

  isAction: boolean = false;
  id: any;
  parentId: any;
  modalInfo = {
    Title: 'Thêm mới nhóm câu hỏi',
    SaveText: 'Lưu',
  };

  questionGroupModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    ParentId: null
  }

  questionGroups: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }]

  ngOnInit(): void {
    this.getCbbQuestionGroup();
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm câu hỏi';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.questionGroupModel.ParentId = this.parentId;
      this.modalInfo.Title = 'Thêm mới nhóm câu hỏi';
    }
  }

  getCbbQuestionGroup() {
    this.comboboxService.getQuestionGroup().subscribe(data => {
      this.questionGroups = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  getInfo() {
    this.questionService.getInfoQuestionGroup({ Id: this.id }).subscribe(
      result => {
        this.questionGroupModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.questionService.createQuestionGroup(this.questionGroupModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới nhóm câu hỏi thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm câu hỏi thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.questionService.updateQuestionGroup(this.questionGroupModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm câu hỏi thành công!');
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
    let groupId = this.questionGroupModel.ParentId;
    this.questionGroupModel = {
      Id: '',
      Name: '',
      Code: '',
      Note: '',
      ParentId: groupId
    };
    this.getCbbQuestionGroup();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
