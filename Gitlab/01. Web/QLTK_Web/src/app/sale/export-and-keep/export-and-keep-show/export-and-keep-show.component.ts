import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, OnDestroy, AfterViewInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { ProductService } from 'src/app/device/services/product.service';
import { Constants, Configuration, MessageService, ComboboxService, DateUtils, AppSetting } from 'src/app/shared';
import { ExportAndKeepService } from '../service/export-and-keep.service';

@Component({
  selector: 'app-export-and-keep-show',
  templateUrl: './export-and-keep-show.component.html',
  styleUrls: ['./export-and-keep-show.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ExportAndKeepShowComponent implements OnInit, OnDestroy, AfterViewInit {

  constructor(
    public constant: Constants,
    public config: Configuration,
    private productService: ProductService,
    private messageService: MessageService,
    public combobox: ComboboxService,
    private activeModal: NgbActiveModal,
    public service: ExportAndKeepService,
    private modalService: NgbModal,
    public dateUtil: DateUtils,
    public router: Router,
    private routeA: ActivatedRoute,
    public appSetting: AppSetting,

  ) { }

  model: any = {

  }

  startIndex = 1;
  startIndexCheck = 1;
  listProduct = [];
  listProductCheck = [];
  listEmployee = [];
  listCustomer = [];
  ExpiredDateV: any;
  createDate: any;
  minDateNotificationV: any;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];

  modelCustomer: any = {
    Id: ''
  }

  user: any;
  id: string;
  productHeight = 200;
  @ViewChild('scrollProducChose', { static: false }) scrollProducChose: ElementRef;
  @ViewChild('scrollProducChoseHeader', { static: false }) scrollProducChoseHeader: ElementRef;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Xem chi tiết xuất giữ";
    this.user = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.id = this.routeA.snapshot.paramMap.get('id');
    if (this.id) {
      this.GetExportAndKeep();
    }
    this.productHeight = window.innerHeight - 180;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.productHeight = window.innerHeight - 180;
  }

  ngAfterViewInit() {
    this.scrollProducChose.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollProducChoseHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollProducChose.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  GetExportAndKeep() {
    this.service.GetExportAndKeepViewById(this.id).subscribe(
      data => {
        this.listProductCheck = data.ListExportAndKeepDetail;
        var index = 1;
        this.listProductCheck.forEach(element => {
          element.index = index++;
        });
        this.model = data;
        this.ExpiredDateV = this.dateUtil.convertDateToObject(this.model.ExpiredDate);
        this.createDate = this.dateUtil.convertDateToObject(this.model.CreateDate);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showComfirmmanumitExportAndKeep() {
    this.messageService.showConfirm("Bạn có chắc giải phóng xuất giữ này không?").then(
      data => {
        this.manumitExportAndKeep();
      }
    );
  }

  manumitExportAndKeep() {
    this.service.ManumitExportAndKeep(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Giải phóng xuất giữ thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showComfirSoldExportAndKeep() {
    this.messageService.showConfirm("Bạn có chắc cập nhật trạng thái sang đã bán không?").then(
      data => {
        this.SoldExportAndKeep();
      }
    );
  }

  SoldExportAndKeep() {
    this.service.SoldExportAndKeep(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật trạng thái xuất giữ đã bán thành công!');
        this.closeModal(true);

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    if (this.model.Status == 5 || this.model.Status == 6) {
      this.router.navigate(['kinh-doanh/danh-sach-xuat-giu']);
    }
    else {
      this.router.navigate(['kinh-doanh/lich-su-xuat-giu']);
    }
  }

}
