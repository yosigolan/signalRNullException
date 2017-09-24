import { Component } from '@angular/core';
import * as signalR from '@aspnet/signalr-client';
import { HttpClient } from '@angular/common/http';

@Component({
  selector:    'app-root',
  templateUrl: './app.component.html',
  styleUrls:   ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(private httpClient: HttpClient) {
    let connection: signalR.HubConnection = new signalR.HubConnection('http://localhost:5000/myHub');

    connection.on('setGeneralData', data => {
      console.log(data);
    });

    connection.on('updateTaskStatus', data => {
      console.log(data);
    });

    connection.start().then(() => {
      let connectionId: string = (<any>connection).connection.connectionId;
      httpClient.post<number>(`http://localhost:5000/api/values?connectionId=${connectionId}`,null).subscribe(
        (data:any)=> {
          console.log('request for data completed');
        },
        (error: any) => {
          console.error('error occured when requesting for data',error);
        }
      );
    }).catch(
      (error: any) => {
        console.error(error);
      });

    connection.onClosed = (error?: Error) => {
      console.log(error);
    };

  }
}
