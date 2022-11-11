import { Injectable, NgZone } from '@angular/core';
import { BroadcastEventListener } from '../signalr/broadcast.event.listener';
declare var $: any;
export declare type CallbackFn = (...args: any[]) => void;
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private connection: any;
  private proxy: any;
  private _zone: NgZone;
  private _logging: boolean;
  private _lisEvents: string[];
  private _listeners: { [eventName: string]: CallbackFn[] };

  public constructor(
    zone: NgZone, ) {
    this._listeners = {};
    this._lisEvents = [];
    this._zone = zone;
  }

  public initSignalR(url) {
    if (!this.connection) {
      var options = {
        transport: 'webSockets'
      };
      this.connection = $.hubConnection(url, options);
      // this.connection.qs = { 'UserId': currentUser.userid, 'userName': currentUser.userName };
      this.proxy = this.connection.createHubProxy('MyHub');

      const callback: CallbackFn = (...args: any[]) => {
        this.run(() => {
          this.log('Disconnected to Processing Hub');
          for (const event of this._lisEvents) {
            for (const callbackEvent of this._listeners[event]) {
              callbackEvent();
            }
          }
        }, true);
      };

      this.connection.disconnected(callback);

      this.connection.stateChanged(function (change) {
        if (change.newState === $.signalR.connectionState.reconnecting) {
          this.log("liveFeed is reconnecting!");
        }
        else if (change.newState === $.signalR.connectionState.connected) {
          this.log("liveFeed is connected!");
        }
      });
    }
  }

  public startConnection(): Observable<any> {
    var studentsObservable = new Observable(observer => {
      if (this.connection && this.connection.state === $.signalR.connectionState.disconnected) {
        this.connection.start().done((data: any) => {
          this.log('Connected to Processing Hub');
          observer.next(true);
          observer.unsubscribe();
        }).catch((error: any) => {
          this.log('Hub error -> ' + error);
          observer.next(false);
          observer.unsubscribe();
        });
      }
    });

    return studentsObservable;
  }

  public disconnected() {
    this.log('Disconnected to Processing Hub');
  }

  public disConnect() {
    if (this.connection) {
      this.connection.stop();
      this.log('disConnect to Processing Hub');
    }

    this.connection = null;
  }

  public listen<T>(listener: BroadcastEventListener<T>, isCheckDis: Boolean): void {
    if (listener == null) {
      throw new Error('Failed to listen. Argument \'listener\' can not be null');
    }

    const callback: CallbackFn = (...args: any[]) => {
      this.run(() => {
        let casted: T = null;
        if (args.length > 0) {
          casted = args[0] as T;
        }
        this.log('SignalRConnection.proxy.on invoked. Calling listener next() ...');
        listener.next(casted);
        this.log('listener next() called.');
      }, true);
    };

    this.setListener(callback, listener, isCheckDis);
  }

  public invoke(action, data): Observable<any> {
    var invokeObservable = new Observable(observer => {
      if (this.connection && this.connection.state === $.signalR.connectionState.disconnected) {
        this.startConnection().subscribe(isStart => {
          if (isStart) {
            this.proxy.invoke(action, data);
          }
          observer.next(isStart);
          observer.unsubscribe();
        });
      } else {
        this.proxy.invoke(action, data);
        observer.next(true);
        observer.unsubscribe();
      }
    });

    return invokeObservable;
  }

  private setListener<T>(callback: CallbackFn, listener: BroadcastEventListener<T>, isCheckDis: Boolean) {
    this.log(`SignalRConnection: Starting to listen to server event with name ${listener.event}`);
    this.proxy.on(listener.event, callback);

    if (this._listeners[listener.event] == null) {
      this._listeners[listener.event] = [];
    }

    this._listeners[listener.event].push(callback);

    if (this._lisEvents.indexOf(listener.event) == -1 && isCheckDis) {
      this._lisEvents.push(listener.event);
    }
  }

  public stopListening<T>(listener: BroadcastEventListener<T>): void {
    if (listener == null) {
      throw new Error('Failed to listen. Argument \'listener\' can not be null');
    }

    this.log(`SignalRConnection: Stopping listening to server event with name ${listener.event}`);
    if (!this._listeners[listener.event]) {
      this._listeners[listener.event] = [];
    }

    for (const callback of this._listeners[listener.event]) {
      this.proxy.off(listener.event, callback);
    }

    this._listeners[listener.event] = [];
  }

  private log(...args: any[]) {
    if (this._logging === false) {
      return;
    }
    console.log(args.join(', '));
  }

  private run(func: () => void, inZone: boolean) {
    if (inZone) {
      this._zone.run(() => func());
    } else {
      this._zone.runOutsideAngular(() => func());
    }
  }
}
