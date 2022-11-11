import { Injectable } from '@angular/core';
import { FileInfo, MIME_TYPE_FOLDER } from "../model/fileInfo";
import { Observable } from 'rxjs';
import { resolve, reject } from 'q';
declare var UploaderForGoogleDrive;
@Injectable({
    providedIn: 'root'
})
export class FileRepositoryService {

    create(parentId: string, folderName: string) {
        var folder = {
            name: folderName,
            mimeType: MIME_TYPE_FOLDER,
            parents: [parentId]
        };
        return new Promise((resolve, reject) => {
            gapi.client.drive.files.create({
                resource: folder,
                fields: "id, name, mimeType, modifiedTime, size"
            }).then((res) => {
                resolve(FileInfo.fromGoogleFile(res.result))
            },
                err => {
                    reject(err);
                });
        });
    }

    delete(fileId: string) {
        return gapi.client.drive.files.delete({
            fileId: fileId
        });
    }

    getFiles(folderId: string) {
        return gapi.client.drive.files.list({
            pageSize: 100,
            fields: "nextPageToken, files(id, name, mimeType, modifiedTime, size)",
            q: `'${folderId}' in parents and trashed = false`
        }).then((res) => {
            let files: FileInfo[] = [];
            res.result.files.forEach((file) => files.push(FileInfo.fromGoogleFile(file)));
            return files;
        });
    }

    importFile(parentId: string, file: FileInfo, onComplete: any) {
        var contentType = file.Blob.type || 'application/octet-stream';
        var metadata = {
            name: file.Blob.name,
            mimeType: contentType,
            parents: [parentId]
        };

        var uploader = new UploaderForGoogleDrive({
            file: file.Blob,
            token: gapi.auth2.getAuthInstance().currentUser.get().getAuthResponse().access_token,
            metadata: metadata,
            onComplete: onComplete,
            fields: 'id',
            params: {
                convert: false,
                ocr: false
            }

        });

        uploader.upload();
    }

    // importFile(parentId: string, name: string, blob: Blob) {
    //     const boundary = 'hintdesk';
    //     const delimiter = "\r\n--" + boundary + "\r\n";
    //     const close_delim = "\r\n--" + boundary + "--";

    //     var reader = new FileReader();
    //     reader.readAsBinaryString(blob);
    //     reader.onload = function (e) {
    //         var contentType = blob.type || 'application/octet-stream';
    //         var metadata = {
    //             name: name,
    //             mimeType: contentType,
    //             parents: [parentId]
    //         };

    //         var base64Data = btoa(reader.result.toString());
    //         var multipartRequestBody =
    //             delimiter +
    //             'Content-Type: application/json\r\n\r\n' +
    //             JSON.stringify(metadata) +
    //             delimiter +
    //             'Content-Type: ' + contentType + '\r\n' +
    //             'Content-Transfer-Encoding: base64\r\n' +
    //             '\r\n' +
    //             base64Data +
    //             close_delim;

    //         return gapi.client.request({
    //             'path': '/upload/drive/v3/files',
    //             'method': 'POST',
    //             'params': { 'uploadType': 'multipart' },
    //             'headers': {
    //                 'Content-Type': 'multipart/mixed; boundary="' + boundary + '"'
    //             },
    //             'body': multipartRequestBody
    //         });
    //     }
    // }
    download(fileId: string) {
        return gapi.client.drive.files.get({
            fileId: fileId,
            alt: 'json',
            fields: '*',

        }).then(res => {
            window.open(res.result.webContentLink);
        });
    }
}
