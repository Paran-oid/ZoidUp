import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { RequestService } from '../../../services/backend/request.service';
import { RequestUserDTO, User } from '../../../models/user/user.model';
import { CommonModule } from '@angular/common';
import { FormatRequestDate } from '../../../shared/functions/FormatRequestDate.function';
import { SendRequestsService } from '../../../services/frontend/send-requests.service';
import { NotificationService } from '../../../services/frontend/notification.service';

@Component({
  selector: 'app-requests-popup',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './requests-popup.component.html',
  styleUrl: './requests-popup.component.scss',
})
export class RequestsPopupComponent implements OnInit {
  @Input() currentUser: User | null = null;
  requests: RequestUserDTO[] = [];

  isEmpty: boolean = true;
  FormatRequestDate = FormatRequestDate;

  constructor(
    private RequestService: RequestService,
    private sendRequestsService: SendRequestsService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.RequestService.sentRequests$.subscribe((requests) => {
      this.requests = requests!;
      if (requests == null) {
        return;
      }
      if (this.requests.length > 0) {
        this.isEmpty = false;
      }
    });
  }

  UnsendRequest(receiverId: number) {
    const index = this.requests.findIndex((req) => req.id === receiverId);
    this.requests.splice(index, 1);

    this.RequestService.UnsendRequest(
      this.currentUser?.id!,
      receiverId
    ).subscribe({
      next: (response) => {
        this.notificationService.Info('Friend request unsent');
      },
    });
  }

  CloseTab() {
    this.sendRequestsService.LeaveSendRequests();
  }
}
