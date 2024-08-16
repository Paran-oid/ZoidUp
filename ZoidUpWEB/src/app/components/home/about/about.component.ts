import { Component, Input, input, OnInit } from '@angular/core';
import { User } from '../../../models/user/user.model';
import { PassUserService } from '../../../services/frontend/pass-user.service';
import { FriendshipService } from '../../../services/friendship.service';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss',
})
export class AboutComponent implements OnInit {
  @Input() currentUser: User | null = null;
  friend: User | null = null;
  constructor(
    private passUserService: PassUserService,
    private friendshipService: FriendshipService
  ) {}
  ngOnInit(): void {
    this.passUserService.passedUser$.subscribe((friend) => {
      this.friend = friend;
    });
  }

  UnfriendUser() {
    var isConfirmed = confirm(
      `Are you sure you want to unfriend ${this.friend?.username}?`
    );
    if (isConfirmed) {
      this.friendshipService
        .RemoveFriend(this.currentUser?.id!, this.friend?.id!)
        .subscribe((response) => {
          window.location.reload();
        });
    }
  }
}
