import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { ProjectServiceService } from '../../service/project-service.service';

@Component({
  selector: 'app-show-export-excel-project-plan',
  templateUrl: './show-export-excel-project-plan.component.html',
  styleUrls: ['./show-export-excel-project-plan.component.scss']
})
export class ShowExportExcelProjectPlanComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private messageService: MessageService,
    public service: ProjectServiceService,
    private config: Configuration,
    private activeModal: NgbActiveModal,
  ) { }

  date = new Date();
  model: any = {
    ProjectId: '',
    CustomerId: '',
    StartYear: this.date.getFullYear(),
    EndYear: this.date.getFullYear(),
    Year: this.date.getFullYear(),
  }

  listYear: any[] = [];

  ngOnInit(): void {
    this.loadYear();
  }

  loadYear() {
    this.service.getMinYear().subscribe(
      data => {
        var minYear = data.minYear;
        var maxYear = data.maxYear;
        for (minYear ; minYear <= maxYear; minYear++) {
          this.listYear.push(minYear);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }

  exportExcel() {
    this.service.ExportExcel(this.model).subscribe(d => {
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
}
