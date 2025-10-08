import { Component, output } from '@angular/core';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'button-demo',
  templateUrl: './buttondemo.html',
  imports: [ButtonModule],
})
export class ButtonDemo {
  output = output<string>();
  number = 1;

  handleClick() {
    this.output.emit(`Button clicked ${this.number++} times`);
  }
}
