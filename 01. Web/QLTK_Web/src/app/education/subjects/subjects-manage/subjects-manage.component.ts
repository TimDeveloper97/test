import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { AppSetting, MessageService, Configuration, Constants , ComboboxService} from 'src/app/shared';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SubjectsService } from '../../service/subjects.service';
import { SubjectsCreateComponent } from '../subjects-create/subjects-create.component';

@Component({
  selector: 'app-subjects-manage',
  templateUrl: './subjects-manage.component.html',
  styleUrls: ['./subjects-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SubjectsManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private subjectsService: SubjectsService,
    public constant: Constants,
    public comboboxService: ComboboxService
  ) { }

  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    ClassRoomId: '',
    DegreeName: '',
    ClassRoomName: '',
    DegreeId: '',
    TotalLearningTime: '',
    LearningTheoryTime: '',
    LearningPracticeTime: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã môn hoặc tên môn',
    Items: [
      {
        Name: 'Trình độ',
        FieldName: 'DegreeId',
        Placeholder: 'Trình độ',
        Type: 'select',
        DataType: this.constant.SearchDataType.Degree,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Phòng học',
        FieldName: 'ClassRoomId',
        Placeholder: 'Chọn phòng học',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.ClassRoom,
        Columns: [{ Name: 'Code', Title: 'Mã phòng học' }, { Name: 'Name', Title: 'Tên phòng học' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý môn học";
    this.searchSubjects();
    this.getClassRoom();
    this.getDegree();
  }
  searchSubjects() {
    this.subjectsService.searchSubjects(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
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
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Description: '',
      ClassRoomId: '',
      DegreeName: '',
      ClassRoomName: '',
      DegreeId: '',
      TotalLearningTime: '',
      LearningTheoryTime: '',
      LearningPracticeTime: '',
    }
    this.searchSubjects();
  }
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SubjectsCreateComponent, { container: 'body', windowClass: 'Subjects-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSubjects();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteSubjects(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa môn học này không?").then(
      data => {
        this.deleteSubjects(Id);
      },
      error => {
        
      }
    );
  }

  deleteSubjects(Id: string) {
    this.subjectsService.deleteSubjects({ Id: Id }).subscribe(
      data => {
        this.searchSubjects();
        this.messageService.showSuccess('Xóa môn học thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ExportExcel() {
    this.subjectsService.ExportExcel(this.model).subscribe(d => {
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

  ListClassRoom: any = [];
  getClassRoom() {
    this.comboboxService.getListClassRoom().subscribe(
      data => {
        this.ListClassRoom = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  ListDegree: any = [];
  getDegree() {
    this.comboboxService.getListDegree().subscribe(
      data => {
        this.ListDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
