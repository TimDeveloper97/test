import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService, Configuration, FileProcess } from 'src/app/shared';
import { ModuleSketchesService } from '../../services/module-sketches-service';

@Component({
  selector: 'app-sketches-history',
  templateUrl: './sketches-history.component.html',
  styleUrls: ['./sketches-history.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SketchesHistoryComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private moduleSketchesService: ModuleSketchesService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcess: FileProcess,
  ) { }

  ListFileSketches: any[] = [];
  Id: string;
  ngOnInit() {
    this.searchSketchHistory();
  }

  searchSketchHistory() {
    if (this.Id == null) {
      this.messageService.showMessage("Bạn cần lưu file để xem lịch sử")
    }
    else {
      this.moduleSketchesService.getSketchesHistoryInfo({ Id: this.Id }).subscribe((data: any) => {
        this.ListFileSketches = data.ListHistory;
      }, error => {
        this.messageService.showError(error);
      });
    }
  }

  DownloadAFile(file) {
    // if (!path) {
    //   this.messageService.showError("Không có file để tải");
    // }
    // var link = document.createElement('a');
    // link.setAttribute("type", "hidden");
    // link.href = this.config.ServerFileApi + path;
    // link.download = "aaaaa";
    // document.body.appendChild(link);
    // link.focus();
    // link.click();
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  closeModal() {
    this.activeModal.close(false);
  }
}
