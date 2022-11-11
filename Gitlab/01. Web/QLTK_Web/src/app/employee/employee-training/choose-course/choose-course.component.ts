import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { EmployeeTrainingService } from '../../service/employee-training.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, AppSetting } from 'src/app/shared';
import * as moment from 'moment';

@Component({
  selector: 'app-choose-course',
  templateUrl: './choose-course.component.html',
  styleUrls: ['./choose-course.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseCourseComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private service: EmployeeTrainingService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  IsRequest: boolean;

  modelSearch: any = {
    Id: '',
    Code: '',
    Name: '',
    ListIdSelect: [],
    ListIdChecked: [],
  }

  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchCourse();
  }

  searchCourse() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchCourse(this.modelSearch).subscribe(data => {
      this.listData = data;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  choose() {
    for (var item of this.listSelect) {
      if (!item.StartDate || !item.EndDate) {
        this.messageService.showMessage("Bạn chưa nhập thời gian cho khóa học!");
        return;
      }
      //var check = moment(item.EndDate).isAfter(moment(item.StartDate))
      var startDate = moment(item.StartDate).format('YYYY-MM-DD');
      var endDate = moment(item.EndDate).format('YYYY-MM-DD');
      if (startDate > endDate) {
        this.messageService.showMessage("Ngày bắt đầu phải sớm hơn ngày kết thúc!");
        return;
      }
    }
    this.activeModal.close(this.listSelect);
  }

  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listData.indexOf(element);
      if (index > -1) {
        this.listData.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listData.push(element);
      }
    });
    this.listData.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  clear() {
    this.modelSearch = {
      Id: '',
      Code: '',
      Name: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.modelSearch.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.ListIdSelectRequest.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    } else {
      this.ListIdSelect.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    }
    this.searchCourse();
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  checkAll(isCheck: any) {
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
