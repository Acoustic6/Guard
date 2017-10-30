import { Component, Input } from '@angular/core';

@Component({
  selector: 'guard-header',
  styleUrls: ['./guard-header.component.scss'],
  templateUrl: './guard-header.component.html'
})
export class GuardHeaderComponent {
  @Input() title: string = '';
  @Input() message: string = '';
}
