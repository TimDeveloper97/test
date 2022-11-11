import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, ComboboxService, Constants } from 'src/app/shared';
import { ModuleGroupService } from '../../services/module-group-service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-module-group-choose-stage',
  templateUrl: './module-group-choose-stage.component.html',
  styleUrls: ['./module-group-choose-stage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleGroupChooseStageComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private moduleGroupService: ModuleGroupService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    public combobox: ComboboxService,
  ) { }

  isAction: boolean = false;
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  modelsearch: any = {

    DepartmentId: '',
    Code: '',
    Name: '',
    ListIdSelect: [],
    ListIdChecked: [],
    
  }
  listBase: any = [];
  listSelect: any = [];
  ListIdSelect: any = [];
  ListDepartment: any = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }];

  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });
    this.searchStage();
    this.getListDepaterment();
  }

  getListDepaterment() {
    this.combobox.getCbbDepartment().subscribe(data => {
      this.ListDepartment = data;
    });
  }

  searchStage() {
    this.listSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelsearch.ListIdChecked.push(element.Id);
      }
    });
    this.moduleGroupService.SearchStage(this.modelsearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelsearch.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear() {
    this.modelsearch = {
      DepartmentId: '',
      Code: '',
      Name: '',


      ListIdSelect: [],
      ListIdChecked: [],
    }

    this.ListIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });

    this.searchStage();
  }

  addRow() {
    var index = 1;
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
    for (var item of this.listBase) {
      item.Index = index;
      index++;
    }
  }

  removeRow() {
    var index = 1;
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
    for (var item of this.listBase) {
      item.Index = index;
      index++;
    }
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  CloseModal() {
    this.activeModal.close(false);
  }

  checkAll(isCheck) {
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

}