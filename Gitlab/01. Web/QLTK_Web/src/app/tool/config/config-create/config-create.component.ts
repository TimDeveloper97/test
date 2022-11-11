import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, DateUtils } from 'src/app/shared';
import { ConfigService } from '../../services/config.service';

@Component({
  selector: 'app-config-create',
  templateUrl: './config-create.component.html',
  styleUrls: ['./config-create.component.scss']
})
export class ConfigCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ConfigService,
    public dateUtils: DateUtils,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới cấu hình',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  value1 = '';
  value2 = null;
  startTimeV = null;
  dateFrom = null;

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Value: null,
    ValueType: 0,
  }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa cấu hình';
      this.ModalInfo.SaveText = 'Lưu';
      this.getConfigInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới cấu hình";
    }
  }

  getConfigInfo() {
    this.service.getConfigInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      if (this.model.ValueType == 1) {
        this.value1 = this.model.Value;
      } else if (this.model.ValueType == 2) {
        this.value2 = this.model.Value;
      } else if (this.model.ValueType == 3) {
        let dateArray = this.model.Value.split('T')[0];
        let dateValue = dateArray.split('-');
        let tempDateFromV = {
          'day': parseInt(dateValue[2]),
          'month': parseInt(dateValue[1]),
          'year': parseInt(dateValue[0])
        };
        this.dateFrom = tempDateFromV;
      } else if (this.model.ValueType == 4) {
        this.startTimeV = this.dateUtils.convertTimeToObject(this.model.Value);
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  updateConfig() {
    if (this.model.ValueType == 1) {
      this.model.Value = this.value1;
    } else if (this.model.ValueType == 2) {
      this.model.Value = this.value2;
    } else if (this.model.ValueType == 3) {
      this.model.Value = this.dateUtils.convertObjectToDate(this.dateFrom);
    } else if (this.model.ValueType == 4) {
      this.model.Value = this.dateUtils.convertObjectToTime(this.startTimeV);
    }
    this.service.updateConfig(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm tiêu chuẩn sản phẩm thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateConfig();
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
