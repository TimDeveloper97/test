import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ImportConfigService } from '../services/import-config.service';

@Component({
  selector: 'app-import-config',
  templateUrl: './import-config.component.html',
  styleUrls: ['./import-config.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ImportConfigComponent implements OnInit {
  documentConfig: any = {
    Step: 2,
    Documents: []
  }

  isAll = false;
  isLoad = true;

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    public messageService: MessageService,
    private importConfigService: ImportConfigService
  ) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = 'Cấu hình tài liệu';
    this.getConfigByType();
  }

  getConfigByType() {
    this.isLoad = true;
    this.documentConfig.Documents = [];
    this.isAll = false;
    this.importConfigService.getConfigByType({ Step: this.documentConfig.Step }).subscribe((result: any) => {      
      this.documentConfig.Documents = result;
      if (this.documentConfig.Documents.length > 0) {
        this.isAll = this.documentConfig.Documents.filter(item => item.checked).length == this.documentConfig.Documents.length;
      }
      this.isLoad = false;
    }, error => {

      this.messageService.showError(error);
    });
  }

  updateConfig() {
    this.documentConfig.Documents = this.documentConfig.Documents.filter(item =>item.Name != undefined && item.Name != '' && item.Name.trim() != '');
    this.importConfigService.updateConfig(this.documentConfig).subscribe((result: any) => {
      this.messageService.showSuccess('Cập nhật cầu hình tài liệu thành công!');
    }, error => {
      this.messageService.showError(error);
    });
  }

  createConfig() {
    this.documentConfig.Documents.push({});
  }

  showConfirmDelete(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.deleteConfig(index);
      }
    );
  }

  deleteConfig(index: number) {
    this.documentConfig.Documents.splice(index, 1);
  }

  checkItem(document) {
    if (!document.IsRequired) {
      this.isAll = false;
    }
    else {
      this.isAll = this.documentConfig.Documents.filter(item => item.IsRequired).length == this.documentConfig.Documents.length;
    }
  }

  checkAll() {
    this.documentConfig.Documents.forEach(item => {
      item.IsRequired = this.isAll;
    });
  }
}
