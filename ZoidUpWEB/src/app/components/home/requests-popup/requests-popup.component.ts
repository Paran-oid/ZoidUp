import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { FriendshipService } from '../../../services/friendship.service';
import { RequestUserDTO, User } from '../../../models/user/user.model';
import { CommonModule } from '@angular/common';
import { FormatRequestDate } from '../../../shared/functions/FormatRequestDate.function';
import { SendRequestsService } from '../../../services/frontend/send-requests.service';

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
    private friendshipService: FriendshipService,
    private sendRequestsService: SendRequestsService
  ) {}

  ngOnInit(): void {
    this.friendshipService.sentRequests$.subscribe((requests) => {
      this.requests = requests!;
      if (requests == null) {
        return;
      }
      if (this.requests.length > 0) {
        this.isEmpty = false;
      }
    });
  }

  UnsendRequest(receiverID: number) {
    const index = this.requests.findIndex((req) => req.id === receiverID);
    this.requests.splice(index, 1);

    this.friendshipService
      .UnsendRequest(this.currentUser?.id!, receiverID)
      .subscribe({
        next: (response) => {
          console.log(response);
        },
      });
  }

  CloseTab() {
    this.sendRequestsService.LeaveSendRequests();
  }
}
