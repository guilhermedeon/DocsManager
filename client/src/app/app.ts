import { HttpClient } from '@angular/common/http';
import { Component, output, OutputEmitterRef, Signal, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ButtonDemo } from './buttondemo/buttondemo';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ButtonDemo],
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
    this.httpClient.get('http://localhost:5000/api/Documents/c918a962-b7ea-4d86-8873-88bcb7ad66b5').subscribe(response => {
      console.log(response);
    });
  }
}
