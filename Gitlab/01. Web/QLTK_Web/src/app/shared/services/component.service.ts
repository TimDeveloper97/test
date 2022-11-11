import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

import { ImportExcelComponent } from '../component/import-excel/import-excel.component';
import { ChooseFolderShareModalComponent } from '../component/choose-folder-share-modal/choose-folder-share-modal.component';
import { ChooseFileModalComponent } from '../component/choose-file-modal/choose-file-modal.component';
import { NTSModalService } from './ntsmodal.service';

@Injectable({
    providedIn: 'root'
})
export class ComponentService {

    constructor(private modalService: NgbModal, private ntsModalService: NTSModalService,
        public router: Router,
        private toastr: ToastrService) {

    }

    showImportExcel(templatePath: string, isData): Observable<any> {
        var importExcel = new Observable(observer => {
            const activeModal = this.modalService.open(ImportExcelComponent, { container: 'body', backdrop: 'static' });
            activeModal.componentInstance.templatePath = templatePath;
            activeModal.componentInstance.isData = isData;
            activeModal.result.then((result) => {
                observer.next(result);
                observer.unsubscribe();
            }, (reason) => {
                observer.next(false);
                observer.unsubscribe();
            });
        });

        return importExcel;
    }

    showImportExcelCallback(getData: any, isData): Observable<any> {
        var importExcel = new Observable(observer => {
            const activeModal = this.modalService.open(ImportExcelComponent, { container: 'body', backdrop: 'static' });
            activeModal.componentInstance.isGetData = true;
            activeModal.componentInstance.getData = getData;
            activeModal.componentInstance.isData = isData;
            activeModal.result.then((result) => {
                observer.next(result);
                observer.unsubscribe();
            }, (reason) => {
                observer.next(false);
                observer.unsubscribe();
            });
        });

        return importExcel;
    }

    showChooseFolder(type: number, designType: number, id: string): Observable<any> {
        var chooseFolder = new Observable(observer => {
            const activeModal = this.modalService.open(ChooseFolderShareModalComponent, { container: 'body', windowClass: 'choose-folder-share-modal', backdrop: 'static' });
            activeModal.componentInstance.type = type;
            activeModal.componentInstance.designType = designType;
            activeModal.componentInstance.id = id;
            activeModal.result.then((result) => {
                observer.next(result);
                observer.unsubscribe();
            }, (reason) => {
                observer.next(false);
                observer.unsubscribe();
            });
        });

        return chooseFolder;
    }

    showChooseFile(templatePath: string): Observable<any> {
        var chooseFolderFile = new Observable(observer => {
            const activeModal = this.modalService.open(ChooseFileModalComponent, { container: 'body', windowClass: 'choose-file-modal' , backdrop: 'static'});
            activeModal.componentInstance.templatePath = templatePath;
            activeModal.result.then((result) => {
                observer.next(result);
                observer.unsubscribe();
            }, (reason) => {
                observer.next(false);
                observer.unsubscribe();
            });
        });

        return chooseFolderFile;
    }
}
