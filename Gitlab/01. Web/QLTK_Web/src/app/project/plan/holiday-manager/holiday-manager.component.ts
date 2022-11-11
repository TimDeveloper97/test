import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { HolidayService } from '../../service/holiday.service';

@Component({
  selector: 'app-holiday-manager',
  templateUrl: './holiday-manager.component.html',
  styleUrls: ['./holiday-manager.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HolidayManagerComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private messageService: MessageService,
    private holidayService: HolidayService
  ) { }

  HolidayModel: any = {
    Year: '',
    FullDate: ''
  }

  ListHoliday: any = [];
  ListCBBYear: any = [];
  ListDay: any = [1, 2, 3, 4, 5, 6, 7];
  ngOnInit() {
    var dateNow = new Date();
    this.HolidayModel.Year = dateNow.getFullYear();
    this.getCalendar();
    this.appSetting.PageTitle = "Cấu hình thông tin ngày nghỉ";
    this.getYear();
  }

  getYear() {
    var listYear = new Date();
    var list = (listYear.getFullYear() + 5 - 1970);
    for (var i = 0; i <= list; i++) {
      this.ListCBBYear.push(1970 + i);
    }
    this.ListCBBYear.sort(function (a, b) { return b - a });
  }

  getCalendar() {
    this.holidayService.getCalendarOfYear(this.HolidayModel).subscribe((data: any) => {
      if (data) {
        this.ListHoliday = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    let listSelected = [];
    this.ListHoliday.forEach(month => {
      month.ListDayOfMonth.forEach(element => {
        if (element.IsChecked) {
          listSelected.push(element);
        }
      });

    });

    this.holidayService.createHoliday({ Year: this.HolidayModel.Year, ListDayOfMonth: listSelected }).subscribe((data: any) => {
      this.messageService.showSuccess('Lưu cấu hình thông tin ngày nghỉ thành công!');
    }, error => {
      this.messageService.showError(error);
    });
  }
}
