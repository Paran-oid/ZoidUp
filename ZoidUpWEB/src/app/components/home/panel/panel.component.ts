import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  OnInit,
  output,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { User } from '../../../models/user/user.model';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { FriendshipService } from '../../../services/friendship.service';
import { Subject } from 'rxjs';
import { PassUserService } from '../../../services/frontend/pass-user.service';
import { SendRequestsService } from '../../../services/frontend/send-requests.service';

@Component({
  selector: 'app-panel',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './panel.component.html',
  styleUrl: './panel.component.scss',
})
export class PanelComponent implements OnInit, OnChanges {
  @Input() currentUser: User | null = null;

  findFriendsMode: boolean = false;
  notificationMode: boolean = false;
  displayDropdown: boolean = false;
  hasRequests: boolean = false;

  @Input() friends: User[] = [];
  selectedItems: User[] = [];
  sentFriendRequests: User[] = [];
  receivedFriendRequests: User[] = [];
  filteredItems: User[] = this.friends;

  @ViewChild('dropdown') dropdown!: ElementRef;
  searchForm: FormGroup = new FormGroup({});
  constructor(
    private fb: FormBuilder,
    public auth: AuthService,
    public friendshipService: FriendshipService,
    private passUserService: PassUserService,
    private sendRequestsService: SendRequestsService
  ) {}

  ngOnInit() {
    this.searchForm = this.fb.group({
      search: [''],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.filteredItems = this.friends;
    if (this.currentUser) {
      this.friendshipService.HasRequests(this.currentUser.id).subscribe({
        next: (result) => {
          this.hasRequests = result;
        },
        error: (error) => {
          console.log(error);
        },
      });
      this.friendshipService
        .GetAllReceivedRequests(this.currentUser.id)
        .subscribe({
          next: (users) => {
            this.receivedFriendRequests = users;
          },
        });
    }
  }

  @HostListener('document:click', ['$event.target']) onClick(target: any) {
    try {
      let dropdown = this.dropdown.nativeElement as HTMLDivElement;
      const clickedInside = dropdown.contains(target);
      if (!clickedInside) {
        this.displayDropdown = false;
      }
    } catch {
      return;
    }
  }

  FetchData(type: string) {
    if (type === 'recommendations') {
      if (!this.findFriendsMode) {
        this.friendshipService
          .GetAllRecommendedFriends(this.currentUser?.id!)
          .subscribe((users) => {
            this.selectedItems = users;
            this.filteredItems = users;
            this.findFriendsMode = true;
          });
      }
      return;
    } else if (type === 'notifications') {
      if (!this.notificationMode) {
        this.friendshipService
          .GetAllReceivedRequests(this.currentUser?.id!)
          .subscribe((users) => {
            this.selectedItems = users;
            this.filteredItems = users;
            this.notificationMode = true;
          });
      }
    }
  }

  LoadNotifications() {}

  GoBack() {
    this.filteredItems = this.friends;
    this.findFriendsMode = false;
    this.notificationMode = false;
  }

  Search() {
    const search = this.searchForm.get('search')?.value.toLowerCase();
    if (search != '') {
      this.filteredItems = this.friends.filter((friend) =>
        friend.username.toLowerCase().includes(search)
      );
    } else {
      return;
    }
  }

  DisplayFriend(id: number) {
    let friend = this.friends.find((friend) => friend.id === id);
    this.passUserService.passedUser.next(friend!);
  }

  SendRequest(receiverID: number, event: Event) {
    // this will hide the current send button and display the below button for unsending requests
    const sendBtn = event.target as HTMLButtonElement;
    const parentEl = sendBtn.parentElement;
    const unsendBtn = parentEl!.lastChild as HTMLButtonElement;

    sendBtn.style.display = 'none';
    unsendBtn.style.display = 'inline';

    this.friendshipService
      .SendRequest(this.currentUser?.id!, receiverID)
      .subscribe({
        next: (response) => {},
        error: (error) => {
          console.log(error);
        },
      });
  }

  UnsendRequest(receiverID: number, event: Event) {
    const unsendBtn = event.target as HTMLButtonElement;
    const parentEl = unsendBtn.parentElement;
    const sendBtn = parentEl!.firstChild as HTMLButtonElement;

    unsendBtn.style.display = 'none';
    sendBtn.style.display = 'inline';

    this.friendshipService
      .UnsendRequest(this.currentUser?.id!, receiverID)
      .subscribe({
        next: (response) => {},
        error: (error) => {},
      });
  }

  GoToSentRequests() {
    let dropdown = this.dropdown.nativeElement as HTMLDivElement;
    dropdown.style.display = 'none';
    this.sendRequestsService.SeeSendRequests();
  }
}
