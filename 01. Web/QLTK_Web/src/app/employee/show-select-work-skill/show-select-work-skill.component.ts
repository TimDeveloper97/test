import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting, ComboboxService } from 'src/app/shared';
import { WorkSkillService } from 'src/app/employee/service/work-skill.service';

@Component({
  selector: 'app-show-select-work-skill',
  templateUrl: './show-select-work-skill.component.html',
  styleUrls: ['./show-select-work-skill.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class ShowSelectWorkSkillComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private workSkillService: WorkSkillService,
    private comboboxService: ComboboxService,

  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  listBase: any = [];
  listSelect: any = [];
  isAction: boolean = false;
  IsRequest: boolean;
  ListIdSelectRequest: any = [];
  ListIdSelect: any = [];
  WorkSkillId: string;
  listWorkType: any[] = [];
  model: any = {
    TotalItem: 0,
    Id: '',
    Name: '',
    EmployeeId: '',
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false,
    WorkTypeId: ''
  }
  WorkType: string;
  Id: string;
  CheckedAll = false;

  ngOnInit() {
    this.WorkType = this.model.WorkTypeId;
    this.Id = this.model.EmployeeId;
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
    this.workSkillService.searcSelecthWorkSkill(this.model).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.model.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  checkAll(isCheck: any) {
    if (isCheck) {
      this.listBase.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  chooseSkill(row) {
    let allCheck = true;
    if (!row.Checked && this.CheckedAll) {
      this.CheckedAll = false;
    } else {

      this.listBase.forEach(skill => {
        if (!skill.Checked) {
          allCheck = false;
        }
      });
      if (allCheck) {
        this.CheckedAll = true;
      }
    }

  }

  clear() {
    this.model = {
      TotalItem: 0,

      Id: '',
      Name: '',

      ListIdSelect: [],
      ListIdChecked: [],
      IsRequest: false
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
    this.model.EmployeeId = this.Id;
    this.model.WorkTypeId = this.WorkType;
    this.searchSkill();
  }

  addRow() {
    this.CheckedAll = false;
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
    this.listSelect.forEach(element => {
      element.Mark = 10;
      element.Grade = 0;
    });
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
