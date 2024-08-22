import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../services/frontend/notification.service';
import { CommonModule } from '@angular/common';
import { NotificationModel } from '../../../models/other/notification.model';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.scss',
})
export class NotificationComponent implements OnInit {
  constructor(public notificationService: NotificationService) {}
  model!: NotificationModel;
  ngOnInit() {
    this.notificationService.model$.subscribe((model) => {
      this.model = model;
    });
  }
}
