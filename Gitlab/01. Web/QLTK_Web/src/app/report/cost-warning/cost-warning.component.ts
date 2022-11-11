import { Component, OnInit } from '@angular/core';
import { CostWarningService } from '../service/cost-warning.service';
import { AppSetting, MessageService, Constants } from 'src/app/shared';

@Component({
  selector: 'app-cost-warning',
  templateUrl: './cost-warning.component.html',
  styleUrls: ['./cost-warning.component.scss']
})
export class CostWarningComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    private service: CostWarningService,
  ) { }

  listCostWarning: any[] = [];
  listYear: any[] = [];
  listMonth: any[] = [];
  totalEstimatedCost: number;
  totalRealCost: number;
  totalNextMonth: number;
  totalStatusCost: number;
  totalCost: number;
  date = new Date();
  model: any = {
    // TimeType: '0',
    // DateFromV: null,
    // DateToV: null,
    // DateFrom: null,
    // DateTo: null,
    Year: null,
    // Month: this.date.getMonth() + 1,
    ListCost: []
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Cảnh báo chi phí";
    this.loadMonth();
    this.loadYear();
    this.initModel();
    this.searchCost();
  }

  searchCost() {
    this.service.searchCost({ Year: this.model.Year }).subscribe((data: any) => {
      this.listCostWarning = data.ListResult;
      this.totalEstimatedCost = data.TotalEstimatedCost;
      this.totalRealCost = data.TotalRealCost;
      this.totalNextMonth = data.TotalNextMonth;
      this.totalStatusCost = data.TotalStatusCost;
      this.totalCost = data.TotalCost;
    }, error => {
      this.messageService.showError(error);
    });
  }

  createCost() {
    this.model.ListCost = this.listCostWarning;
    this.service.createCost(this.model).subscribe((data: any) => {
      this.messageService.showSuccess("Cập nhật danh sách thành công!");
      this.searchCost();
    }, error => {
      this.messageService.showError(error);
    });
  }

  loadMonth() {
    for (var month = 1; month < 13; month++)
      this.listMonth.push(month);
  }

  loadYear() {
    for (var year = 2000; year <= new Date().getFullYear(); year++) {
      this.listYear.push(year);
    }
  }

  createObjectDate(year: number, month: number, day: number) {
    var objectDate = new ObjectDate();
    objectDate.day = day;
    objectDate.month = month;
    objectDate.year = year;

    return objectDate;
  }

  initModel() {
    this.model.Year = new Date().getFullYear();
    this.model.DateFromV = new ObjectDate();

    // Set ngày từ
    this.model.DateFromV = this.createObjectDate(parseInt(this.model.Year), 1, 1);

    // Set ngày đến
    this.model.DateToV = this.createObjectDate(parseInt(this.model.Year), 12, 31);
  }

  TotalEstimatedCosts() {
    this.totalEstimatedCost = 0;
    this.totalStatusCost = 0;
    this.totalNextMonth = 0;

    this.listCostWarning.forEach(element => {
      this.totalEstimatedCost += element.EstimatedCost;
      this.totalStatusCost += element.EstimatedCost - element.RealCost;
    });

    var group = this.listCostWarning.filter(t => t.RealCost > 0);
    var count = group.length;
    var medium = (this.totalStatusCost / (12 - count));
    this.listCostWarning.forEach(element => {
      if (element.RealCost > 0) {
        element.NextMonthCost = 0;
      } else { element.NextMonthCost = medium; }
      this.totalNextMonth += element.NextMonthCost;
    });
  }

  TotalRealCost() {
    this.totalRealCost = 0;
    this.totalStatusCost = 0;
    this.totalNextMonth = 0;

    this.listCostWarning.forEach(element => {
      this.totalRealCost += element.RealCost;
      this.totalStatusCost += element.EstimatedCost - element.RealCost;
    });

    var group = this.listCostWarning.filter(t => t.RealCost > 0);
    var count = group.length;
    var medium = (this.totalStatusCost / (12 - count));
    this.listCostWarning.forEach(element => {
      if (element.RealCost > 0) {
        element.NextMonthCost = 0;
      } else { element.NextMonthCost = medium; }
      this.totalNextMonth += element.NextMonthCost;
    });
  }

}

export class ObjectDate {
  day: Number;
  month: Number;
  year: Number;
}