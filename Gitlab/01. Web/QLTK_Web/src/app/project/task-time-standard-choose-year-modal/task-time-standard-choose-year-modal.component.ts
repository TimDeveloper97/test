import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-task-time-standard-choose-year-modal',
  templateUrl: './task-time-standard-choose-year-modal.component.html',
  styleUrls: ['./task-time-standard-choose-year-modal.component.scss']
})
export class TaskTimeStandardChooseYearModalComponent implements OnInit {
  listYear = [];
  fileName = "";
  fileToUpload: File = null;
  model = {
    DateFrom : '',
    DateTo: '',
    FileImport: null,
    IsCalculate: false,
    IsExportExcel: true,
    IsImport: false
  };

  constructor(
    private activeModal: NgbActiveModal,
  ) { }

  ngOnInit() {
    this.getYear();
  }

  getYear(){
    var currentYear = new Date().getFullYear();
    for(var i = 2000; i <= currentYear; i++)
    {
      this.listYear.push(i);
    }
  }

  exportExcel(){
    this.activeModal.close(this.model);
  }

  closeModal() {
    this.activeModal.close(false);
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.fileName = files.item(0).name;
    this.model.FileImport = files.item(0);
  }

  changeImport(){
    if(this.model.IsImport)
    {
      this.model.IsExportExcel = false;
      this.model.IsCalculate = false;
    }
  }

  changeExport(){
    if(this.model.IsExportExcel)
    {
      this.model.IsImport = false;
    }
  }

  changeCalculate(){
    if(this.model.IsCalculate)
    {
      this.model.IsImport = false;
    }
  }
}
