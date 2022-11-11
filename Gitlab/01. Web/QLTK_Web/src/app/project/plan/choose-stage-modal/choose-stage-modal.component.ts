import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { StageService } from 'src/app/module/services/stage.service';
import { AppSetting, Constants, FileProcess, MessageService } from 'src/app/shared';
import { PlanService } from '../../service/plan.service';
import { ScheduleProjectService } from '../../service/schedule-project.service';


@Component({
  selector: 'app-choose-stage-modal',
  templateUrl: './choose-stage-modal.component.html',
  styleUrls: ['./choose-stage-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseStageModalComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private service: StageService,
    private planService: PlanService,
    private scheduleProjectService: ScheduleProjectService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  IsRequest: boolean;
  Id: any;
  ProjectId: any;
  Data: any;

  model: any = {
    Id: '',
    ProjectId: '',
    ListIdSelect: []
  }

  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.model.ListIdSelect.push(element);
    });
    this.searchStage();
  }

  searchStage() {
    this.service.searchListStage(this.Id).subscribe(data => {
      this.listData = data.ListResult;
      this.model.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  choose() {
    this.model.Id = this.Id;
    this.model.ProjectId = this.ProjectId;
    this.listData.forEach((element: { Checked: any; Id: any; }) => {
      if (element.Checked) {
        this.model.ListIdSelect.push(element.Id);
      }
    });

    this.scheduleProjectService.createStage(this.model).subscribe(data => {
      this.activeModal.close(data);
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear() {
    this.model = {
      Id: '',
      Code: '',
      Name: '',
      ListIdChecked: []
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
    this.searchStage();
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK);
  }

  checkAll(isCheck) {
    if (isCheck) {
      this.listData.forEach(element => {
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
