import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/shared';
import { ExportAndKeepService } from '../service/export-and-keep.service';

@Component({
  selector: 'app-export-and-keep-detail',
  templateUrl: './export-and-keep-detail.component.html',
  styleUrls: ['./export-and-keep-detail.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ExportAndKeepDetailComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constant: Constants,
    private exportAndKeepService: ExportAndKeepService
  ) { }

  model: any = {

  }
  total = 0;
  listData = [];
  startIndex= 1;
  isAction: boolean = false;
  id: string;

  ngOnInit() {
    this.search();
  }

  search(){
    this.exportAndKeepService.GetListExportDetailBySaleProductId(this.id).subscribe(item => {
      if(item.ListResult){
        this.listData = item.ListResult;
        this.total = this.listData.length;
      }
    })
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
