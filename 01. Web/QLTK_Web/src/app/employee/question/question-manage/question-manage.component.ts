import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { QuestionService } from '../../service/question.service';
import { QuestionCreateComponent } from '../question-create/question-create.component';
import { QuestionGroupCreateComponent } from '../question-group-create/question-group-create.component';

@Component({
  selector: 'app-question-manage',
  templateUrl: './question-manage.component.html',
  styleUrls: ['./question-manage.component.scss']
})
export class QuestionManageComponent implements OnInit {

  items: any;
  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private questionService: QuestionService,
    private dowloadservice: DownloadService,
    private config: Configuration,) {
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }

  questionSearchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Status1: 0,
    Status2: 0,
    OrderBy: 'CreateDate',
    OrderType: true,

    Code: '',
    QuestionGroupId: '',
    Type: null
  };

  questionTypes: any[] = [{ Id: 1, Name: "Câu hỏi Đúng/ sai" }, { Id: 2, Name: "Câu hỏi mở" }]

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo Mã câu hỏi',
    Items: [
      {
        Name: 'Loại câu hỏi',
        FieldName: 'Type',
        Placeholder: 'Chọn loại câu hỏi',
        Type: 'select',
        Data: this.questionTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  questions: any[] = [];

  startIndex: number = 1;

  questionGroupSearchModel: any = {
    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Name: '',
    Code: '',
  };

  questionGroups: any[] = [];
  height = 0;
  expandGroupKeys: any[] = [];
  selectGroupKeys: any[] = [];
  selectedGroupId: any = '';

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý Câu hỏi";
    this.height = window.innerHeight - 140;
    this.searchQuestionGroup();
  }

  searchQuestionGroup() {
    this.questionService.searchQuestionGroup({}).subscribe((data: any) => {
      if (data.ListResult) {
        this.questionGroups = data.ListResult;
        let modelAll: any = {
          Id: '',
          Name: 'Tất cả',
          Code: '',
        }
        this.questionGroups.unshift(modelAll);
        this.questionGroupSearchModel.TotalItems = data.TotalItem;

        this.searchQuestion(this.selectedGroupId);
        this.selectGroupKeys = [this.selectedGroupId];
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedGroupId) {
      this.selectedGroupId = e.selectedRowKeys[0];
      this.searchQuestion(e.selectedRowKeys[0]);
      localStorage.setItem("selectedGroupId", this.selectedGroupId);
    }
  }

  itemClick(e) {
    if (this.selectedGroupId == '' || this.selectedGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm câu hỏi!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateGroup(this.selectedGroupId, false);
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdateGroup(this.selectedGroupId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteQuestionGroup(this.selectedGroupId);
      }
    }
  }

  showCreateUpdateGroup(id: any, isUpdate: boolean) {
    let activeModal = this.modalService.open(QuestionGroupCreateComponent, { container: 'body', windowClass: 'question-group-create-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.id = this.selectedGroupId;
    } else {
      activeModal.componentInstance.parentId = id;
    }

    activeModal.result.then((result) => {
      if (result) {
        this.searchQuestionGroup();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteQuestionGroup(groupId: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa nhóm câu hỏi này không?").then(
      data => {
        this.deleteQuestionGroup(groupId);
      },
      error => {

      }
    );
  }

  deleteQuestionGroup(groupId: any) {
    this.questionService.deleteQuestionGroup({ Id: groupId }).subscribe(
      data => {
        this.searchQuestionGroup();
        this.messageService.showSuccess('Xóa nhóm câu hỏi thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  searchQuestion(groupId: any) {
    this.questionSearchModel.QuestionGroupId = this.selectedGroupId;
    this.questionService.searchQuestion(this.questionSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.questions = data.ListResult;
        this.questionSearchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteQuestion(id: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa câu hỏi này không?").then(
      data => {
        this.deleteQuestion(id);
      },
      error => {

      }
    );
  }

  deleteQuestion(id: any) {
    this.questionService.deleteQuestion({ Id: id }).subscribe(
      data => {
        this.searchQuestionGroup();
        this.messageService.showSuccess('Xóa câu hỏi thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showQuestion(id: string) {
    let activeModal = this.modalService.open(QuestionCreateComponent, { container: 'body', windowClass: 'question-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.groupId = this.selectedGroupId;
    activeModal.componentInstance.isView = true;
    activeModal.result.then((result) => {
      if (result) {
        this.searchQuestion(this.selectedGroupId);
      }
    }, (reason) => {
    });
  }

  clear() {
    this.questionSearchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Status1: 0,
      Status2: 0,
      OrderBy: 'CreateDate',
      OrderType: true,

      Code: '',
      QuestionGroupId: '',
      Type: null
    };
    this.searchQuestion(this.selectedGroupId);
  }

  showCreateUpdate(id: any) {
    let activeModal = this.modalService.open(QuestionCreateComponent, { container: 'body', windowClass: 'question-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.groupId = this.selectedGroupId;

    activeModal.result.then((result) => {
      if (result) {
        this.searchQuestion(this.selectedGroupId);
      }
    }, (reason) => {
    });
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
