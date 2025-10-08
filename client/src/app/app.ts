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

  constructor(private titleService: Title) {}
}
