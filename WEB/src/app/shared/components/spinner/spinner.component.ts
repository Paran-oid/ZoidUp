import { Component, Input } from '@angular/core';
import { SpinnerService } from '../../../services/frontend/spinner.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-spinner',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.scss',
  providers: [SpinnerService],
})
export class SpinnerComponent {
  @Input() isLoading: boolean = false;
}
