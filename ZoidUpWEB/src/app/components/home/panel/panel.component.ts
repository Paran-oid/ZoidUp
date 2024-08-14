import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  output,
  Output,
  SimpleChanges,
} from '@angular/core';
import { User } from '../../../models/user/user.model';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { FriendshipService } from '../../../services/friendship.service';
import { Subject } from 'rxjs';
import { PassUserService } from '../../../services/frontend/pass-user.service';

@Component({
  selector: 'app-panel',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './panel.component.html',
  styleUrl: './panel.component.scss',
})
export class PanelComponent implements OnInit, OnChanges {
  @Input() friends: User[] = [];
  @Input() currentUser: User | null = null;
  filteredItems: User[] = this.friends;
  findFriendsMode: boolean = false;

  searchForm: FormGroup = new FormGroup({});
  constructor(
    private fb: FormBuilder,
    public auth: AuthService,
    public friendshipService: FriendshipService,
    private passUserService: PassUserService
  ) {}

  ngOnInit() {
    this.searchForm = this.fb.group({
      search: [''],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.filteredItems = this.friends;
  }

  LoadRecommendations() {
    if (!this.findFriendsMode) {
      this.friendshipService
        .GetAllRecommendedFriends(this.currentUser?.id!)
        .subscribe((users) => {
          this.filteredItems = users;
          this.findFriendsMode = true;
        });
    } else {
      this.filteredItems = this.friends;
      this.findFriendsMode = false;
    }
  }

  SearchFriends() {
    const search = this.searchForm.get('search')?.value.toLowerCase();
    if (search != '') {
      this.filteredItems = this.friends.filter((friend) =>
        friend.username.toLowerCase().includes(search)
      );
    } else {
      this.filteredItems = this.friends;
    }
  }

  SearchRecommendedFriends() {
    const search = this.searchForm.get('search')?.value.toLowerCase();
    if (search != '') {
      this.filteredItems = this.filteredItems.filter((user) =>
        user.username.toLowerCase().includes(search)
      );
    } else {
      return;
    }
  }

  DisplayFriend(id: number) {
    let friend = this.friends.find((friend) => friend.id === id);
    this.passUserService.passedUser.next(friend!);
  }
}
