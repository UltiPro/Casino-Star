import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-info-alert',
  templateUrl: './info-alert.component.html',
  styleUrls: ['./info-alert.component.css']
})
export class InfoAlertComponent {
  @Input()
  styleToInsert: string = "";

  @Input()
  styleToInsert2: string = "";

  @Input()
  message: string = "";

  @Output()
  statusCode = new EventEmitter<null>();

  onClose(): void {
    this.statusCode.emit(null);
  }
}
