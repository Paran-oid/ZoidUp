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

  findFriendsMode: boolean = false;
  filteredFriends: User[] = this.friends;

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
    this.filteredFriends = this.friends;
  }

  Search() {
    const search = this.searchForm.get('search')?.value.toLowerCase();
    console.log(this.friends);
    if (search != '') {
      this.filteredFriends = this.friends.filter((friend) =>
        friend.username.toLowerCase().includes(search)
      );
    } else {
      this.filteredFriends = this.friends;
    }
  }

  DisplayFriend(id: number) {
    let friend = this.friends.find((friend) => friend.id === id);
    this.passUserService.passedUser.next(friend!);
  }
}
