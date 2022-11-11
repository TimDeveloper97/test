import { Injectable } from '@angular/core';

import { MessageService, } from '../services/message.service';
import { DownloadService, } from '../services/download.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FileProcess {
    constructor(private messageService: MessageService, private downloadService: DownloadService) {

    }

    filesModel: any[] = [];
    totalbyte: number = 0;
    FilesDataBase: any[] = [];
    fileModel: any = {};
    FileDataBase: any = null;
    onFileChange(event) {
        if (event.target.files && event.target.files.length > 0) {
            let files = event.target.files;

            for (let i = 0; i < files.length; i++) {
                const element = files[i];
                // if (element.size < 52428800) {

                // }
                // else {
                //     this.messageService.showMessage('Dung lượng file không được quá 50MB!');
                // }
                this.totalbyte += element.size;
                this.FilesDataBase.push(element);
                let reader = new FileReader();
                reader.readAsDataURL(element);
                reader.onload = () => {
                    var filer = {
                        Name: element.name,
                        DataURL: reader.result,
                        Size: element.size
                    }
                    this.filesModel.push(filer);
                };
                reader.onprogress = (data) => {
                    if (data.lengthComputable) {
                        var progress = parseInt(((data.loaded / data.total) * 100).toString(), 10);
                        console.log(progress);
                    }
                }
            }

        }
    }

    getFileOnFileChange(event) {
        let files = [];
        if (event.target.files && event.target.files.length > 0) {
            files = event.target.files;
            for (let i = 0; i < files.length; i++) {
                const element = files[i];
            }

        }

        return files;
    }

    public async onAFileChange(event) {
        if (event.target.files && event.target.files.length > 0) {
            let files = event.target.files;
            this.totalbyte = files[0].size;
            this.FileDataBase = files[0];
            let reader = new FileReader();
            reader.readAsDataURL(files[0]);
            
            var promise = new Promise(resolve => {
                reader.onload = () => {
                    var filer = {
                        Name: files[0].name,
                        DataURL: reader.result,
                        Size: files[0].size
                    }
                    this.fileModel = (filer);
                };
            });
            await promise;
        }
    }

    readDataFile(event): Observable<any> {
        const fileObservable = new Observable(observer => {

            if (event.target.files && event.target.files.length > 0) {

                let element = event.target.files[0];

                let reader = new FileReader();
                reader.readAsDataURL(element);
                reader.onload = () => {
                    var filer = {
                        Name: element.name,
                        DataURL: reader.result,
                        Size: element.size
                    }

                    observer.next({ File: element, Data: filer.DataURL, Name: element.name });
                    observer.unsubscribe();
                };
                reader.onprogress = (data) => {

                }
            }
        });

        return fileObservable;
    }

    fileExist(urlToFile) {
        var xhr = new XMLHttpRequest();
        xhr.open('HEAD', urlToFile, false);
        xhr.send();
        if (xhr.status == 404) {
            console.log("file not found");
            return false;
        } else {
            return true;
        }
    }

    removeFiles(file) {
        let index = this.filesModel.indexOf(file);
        if (index > -1) {
            this.filesModel.splice(index, 1);
            this.FilesDataBase.splice(index, 1);
        }
    }

    removeFile() {
        this.fileModel = {};
        this.FileDataBase = null;
    }

    downloadFile(file) {
        var link = document.createElement("a");
        link.href = file.DataURL;
        link.download = file.Name;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    downloadFileBlob(path, fileName) {
        this.downloadService.downloadFileBlob(path, fileName).subscribe(data => {
            var blob = new Blob([data], { type: 'octet/stream' });
            var url = window.URL.createObjectURL(blob);
            this.downloadFile({ Name: fileName, DataURL: url });
        }, error => {
            const blb = new Blob([error.error], { type: "text/plain" });
            const reader = new FileReader();

            reader.onload = () => {
                this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
            };
            // Start reading the blob as text.
            reader.readAsText(blb);
        });
    }

    downloadFileBlobNew(path, fileName) {
        this.downloadService.downloadFileBlobNew(path, fileName).subscribe(data => {
            var blob = new Blob([data], { type: 'octet/stream' });
            var url = window.URL.createObjectURL(blob);
            this.downloadFile({ Name: fileName, DataURL: url });
        }, error => {
            const blb = new Blob([error.error], { type: "text/plain" });
            const reader = new FileReader();

            reader.onload = () => {
                this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
            };
            // Start reading the blob as text.
            reader.readAsText(blb);
        });
    }
}
