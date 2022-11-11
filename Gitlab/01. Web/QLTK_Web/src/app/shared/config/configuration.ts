import { Injectable } from '@angular/core';
import { IAppConfig } from './IAppConfig';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class Configuration {
    public ServerApi: string;
    public ServerFileApi: string;
    public ServerWithApiUrl: string;
    public ServerSignalRUrl: string;
    public ClientId: string;
    public ClientSecret: string;
    public ServerFileApiUrl: string;
    public ServerDownloadImage: string;
    public Version: string;
    public SVNVersion: string;
    public BuildDate:string;

    constructor(private http: HttpClient) { }

    load() {
        const jsonFile = 'assets/config/configuration.json';
        return new Promise<void>((resolve, reject) => {
            this.http.get(jsonFile).toPromise().then((response: IAppConfig) => {
                let settings = (<IAppConfig>response);
                this.ServerApi = settings.ServerApi;
                this.ServerFileApi = settings.ServerFileApi;
                this.ClientId = settings.ClientId;
                this.ServerWithApiUrl = settings.ServerWithApiUrl;
                this.ServerSignalRUrl = settings.ServerSignalRUrl;
                this.ClientSecret = settings.ClientSecret;
                this.ServerFileApiUrl = settings.ServerFileApiUrl;
                this.ServerDownloadImage = settings.ServerDownloadImage;
                this.Version = settings.Version;
                this.SVNVersion = settings.SVNVersion;
                this.BuildDate = settings.BuildDate;
                resolve();
            }).catch((response: any) => {
                reject(`Could not load file '${jsonFile}': ${JSON.stringify(response)}`);
            });
        });
    }
}