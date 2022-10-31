import { Component, OnInit, OnDestroy } from '@angular/core';

import { SignalRService } from './signalR/signal-r.service';
import { Subscription } from 'rxjs';
import { BroadcastEventListener } from './signalr/broadcast.event.listener';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private sendMessage = new BroadcastEventListener<any>('sendMessage');
  private addMessage = new BroadcastEventListener<any>('addMessage');
  private _notificationSub: Subscription;

  constructor(
    private signalRService: SignalRService,
  ) {

  }

  ngOnInit() {
    this.signalRService.initSignalR("http://localhost:2712/signalr");
    // register the listener
    this.signalRService.listen(this.sendMessage, false);

    // subscribe to event
    this._notificationSub = this.sendMessage.subscribe((message: any) => {
     console.log(message);
    });
    this.signalRService.startConnection();
  }

  ngOnDestroy() {
    this.signalRService.disConnect();
  }
}
