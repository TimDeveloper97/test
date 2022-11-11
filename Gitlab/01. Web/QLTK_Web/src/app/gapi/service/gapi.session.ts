import { Injectable, EventEmitter } from "@angular/core";
import { AppRepositoryService } from "../service/app-repository.service";
const CLIENT_ID = "107884621530-on10n5fa3ekmrdnqre8ugdbcotcl05ap.apps.googleusercontent.com";
const API_KEY = "AIzaSyBajCa98N7X8W5B8DP7CrXcYkPl6Ov1OGg";
// const CLIENT_ID = "555360871314-3mt6o4m5a27issnbf43hvmvf5m41oqi5.apps.googleusercontent.com";
// const API_KEY = "AIzaSyBvj0ErK_JDsCeMrQeoC2G-QTY1Q3m4CBI";
const DISCOVERY_DOCS = ["https://www.googleapis.com/discovery/v1/apis/drive/v3/rest"];
var SCOPES = 'https://www.googleapis.com/auth/drive';

@Injectable()
export class GapiSession {
    googleAuth: gapi.auth2.GoogleAuth;

    constructor(
        private appRepository: AppRepositoryService

    ) {
    }

    initClient() {
        return new Promise((resolve,reject)=>{
            gapi.load('client:auth2', () => {
                return gapi.client.init({
                    apiKey: API_KEY,
                    clientId: CLIENT_ID,
                    discoveryDocs: DISCOVERY_DOCS,
                    scope: SCOPES,
                }).then(() => {                   
                    this.googleAuth = gapi.auth2.getAuthInstance();
                    resolve(true);
                });
            });
        });
        
    }
    get isSignedIn(): boolean {
        return this.googleAuth.isSignedIn.get();
    }

    signIn() {
        return this.googleAuth.signIn({
            prompt: 'consent'
        }).then((googleUser: gapi.auth2.GoogleUser) => {
            this.appRepository.User.add(googleUser.getBasicProfile());
        });
    }

    signOut(): void {
        this.googleAuth.signOut();
        
    }
}