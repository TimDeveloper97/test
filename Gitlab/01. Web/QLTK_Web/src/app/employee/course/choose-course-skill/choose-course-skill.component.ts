import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { CourseService } from '../../service/course.service';

@Component({
  selector: 'app-choose-course-skill',
  templateUrl: './choose-course-skill.component.html',
  styleUrls: ['./choose-course-skill.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseCourseSkillComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private courseService: CourseService,
  ) { }
  listBase: any = [];
  listSelect: any = [];
  isAction: boolean = false;
  IsRequest: boolean;
  ListIdSelectRequest: any = [];
  ListIdSelect: any = [];
  CourseId: string;
  model: any = {
    TotalItem: 0,

    Id: '',
    Name: '',
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }
  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.model.ListIdSelect.push(element);
    });
    this.searchSkill()
  }

  searchSkill() {
    this.listSelect.forEach(element => {
      this.model.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.model.ListIdChecked.push(element.Id);
      }
    });
    this.courseService.SearchCourseSkill(this.model).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.model.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear() {
    this.model = {
      TotalItem: 0,

      Id: '',
      Name: '',
      listBase: [],
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.model.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.ListIdSelectRequest.forEach(element => {
        this.model.ListIdSelect.push(element);
      });
    } else {
      this.ListIdSelect.forEach(element => {
        this.model.ListIdSelect.push(element);
      });
    }
    this.searchSkill();
  }

  addRow() {
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listBase.indexOf(element);
      if (index > -1) {
        this.listBase.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listBase.push(element);
      }
    });
    this.listBase.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  selectedRowKeys: any[] = [];
  isDropDownBoxOpened = false;

  treeView_itemSelectionChanged(e) {
    this.listSelect = e.selectedRowKeys;
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
