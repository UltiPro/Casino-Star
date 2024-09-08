import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent {
  @Input()
  styleToInsert: string = "";

  @Input()
  message: string = "";

  @Output()
  statusCode = new EventEmitter<null>();

  onClose(): void {
    this.statusCode.emit(null);
  }
}