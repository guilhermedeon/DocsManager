import { HttpClient } from '@angular/common/http';
import { Component, output, OutputEmitterRef, Signal, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { ApiBaseUrl } from './app.config';
import { Simplegrid } from "./simplegrid/simplegrid";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Simplegrid],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  updateTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }

  title() {
    return this.titleService.getTitle();
  }

  constructor(private titleService: Title, private httpClient: HttpClient) {
    this.getData();
  }

  getData() {
    this.httpClient.get(`${ApiBaseUrl}/Documents`).subscribe(response => {
      console.log(response);
    });
  }
}
