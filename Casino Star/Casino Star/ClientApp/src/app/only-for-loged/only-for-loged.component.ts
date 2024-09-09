import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-only-for-loged',
  templateUrl: './only-for-loged.component.html',
  styleUrls: ['./only-for-loged.component.css']
})
export class OnlyForLogedComponent {

  constructor(public router: Router) { }
}