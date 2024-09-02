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
import { User } from '../../../models/main/user.model';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../services/backend/auth.service';
import { RequestService } from '../../../services/backend/request.service';
import { Subject, timeInterval } from 'rxjs';
import { PassUserService } from '../../../services/frontend/pass-user.service';
import { SendRequestsService } from '../../../services/frontend/send-requests.service';
import { CookieService } from 'ngx-cookie-service';
import { NotificationService } from '../../../services/frontend/notification.service';
import { SpinnerService } from '../../../services/frontend/spinner.service';
import { MessageService } from '../../../services/backend/message.service';

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
  selectedFriend: User | null = null;
  selectedItems: User[] = [];
  sentFriendRequests: User[] = [];
  receivedFriendRequests: User[] = [];
  filteredItems: User[] = this.friends;

  @ViewChild('dropdown') dropdown!: ElementRef;
  searchForm: FormGroup = new FormGroup({});
  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    public spinnerService: SpinnerService,
    public RequestService: RequestService,
    private passUserService: PassUserService,
    private sendRequestsService: SendRequestsService,
    private cookieService: CookieService,
    private notificationService: NotificationService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.searchForm = this.fb.group({
      search: [''],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.filteredItems = this.friends;
    if (this.currentUser) {
      this.RequestService.HasRequests(this.currentUser?.id!).subscribe(
        (response) => {
          this.hasRequests = response;
        }
      );
      this.RequestService.GetAllReceivedRequests(this.currentUser.id).subscribe(
        {
          next: (users) => {
            this.receivedFriendRequests = users;
          },
        }
      );
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
        this.RequestService.GetAllRecommendedFriends(
          this.currentUser?.id!
        ).subscribe((users) => {
          this.selectedItems = users;
          this.filteredItems = users;
          this.findFriendsMode = true;
        });
      }
      return;
    } else if (type === 'notifications') {
      if (!this.notificationMode) {
        this.RequestService.GetAllReceivedRequests(
          this.currentUser?.id!
        ).subscribe((users) => {
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
    if (friend == this.selectedFriend) {
      this.passUserService.passedUser.next(null);
      this.selectedFriend = null;
    } else {
      this.selectedFriend = friend!;
      this.passUserService.passedUser.next(friend!);
      this.messageService.LoadMessages(this.currentUser?.id!, id).subscribe();
    }
    this.cookieService.delete('first_time');
  }

  ReturnHours(time: string) {
    const today: any = new Date();
    const previousDate: any = new Date(time);
    const diffTime = Math.abs(today - previousDate);

    let difference = Math.floor(diffTime / (1000 * 60 * 60 * 24));
    let result = Math.floor(difference).toString() + ' day(s) ago';
    if (difference == 0) {
      result =
        Math.floor(diffTime / (1000 * 60 * 60)).toString() + 'hour(s) ago';
      if (difference == 0) {
        const diffMinutes =
          Math.floor(diffTime / (1000 * 60)).toString() + 'minute(s) ago';
        if (difference == 0) {
          result = 'Online';
        }
      }
    }
    const final = result != 'Online' ? 'was logged in:' + result : result;
    return final;
  }

  SendRequestRecommended(receiverId: number, event: Event) {
    // this will hide the current send button and display the below button for unsending requests
    const sendBtn = event.target as HTMLButtonElement;
    const parentEl = sendBtn.parentElement;
    const unsendBtn = parentEl!.lastChild as HTMLButtonElement;

    sendBtn.style.display = 'none';
    unsendBtn.style.display = 'inline';

    this.RequestService.SendRequest(
      this.currentUser?.id!,
      receiverId
    ).subscribe({
      next: (response) => {
        this.notificationService.Success('Friend request sent');
      },
      error: (error) => {},
    });
  }
  AddFriend(senderId: number) {
    this.RequestService.SendRequest(this.currentUser?.id!, senderId).subscribe({
      next: (response) => {
        const index = this.filteredItems.findIndex(
          (user) => user.id === senderId
        );
        const friend = this.filteredItems.at(index);
        this.notificationService.Info(
          `You are now friends with ${friend?.username}!`
        );
        this.friends.push(friend!);
        this.filteredItems.splice(index, 1);
        this.CheckIfHasRequests();
      },
      error: (error) => {},
    });
  }
  UnacceptRequest(senderId: number) {
    this.RequestService.UnsendRequest(
      senderId,
      this.currentUser?.id!
    ).subscribe({
      next: (response) => {
        const index = this.receivedFriendRequests.findIndex(
          (user) => user.id === senderId
        );
        this.filteredItems.splice(index, 1);
        this.CheckIfHasRequests();
        this.notificationService.Info('Unaccepted request');
      },
      error: (error) => {},
    });
  }

  UnsendRequest(receiverId: number, event: Event) {
    const unsendBtn = event.target as HTMLButtonElement;
    const parentEl = unsendBtn.parentElement;
    const sendBtn = parentEl!.firstChild as HTMLButtonElement;

    unsendBtn.style.display = 'none';
    sendBtn.style.display = 'inline';

    this.RequestService.UnsendRequest(
      this.currentUser?.id!,
      receiverId
    ).subscribe({
      next: (response) => {
        this.notificationService.Info('Friend request unsent');
      },
      error: (error) => {},
    });
  }

  GoToSentRequests() {
    let dropdown = this.dropdown.nativeElement as HTMLDivElement;
    dropdown.style.display = 'none';
    this.GoBack();
    this.sendRequestsService.SeeSentRequests(this.currentUser?.id!);
  }
  Logout() {
    this.spinnerService.isLoading.next(true);
    this.notificationService.isDisplayed.next(false);
    setTimeout(() => {
      this.authService.Logout();
      this.spinnerService.isLoading.next(false);
    }, 3000);
  }

  CheckIfHasRequests() {
    this.RequestService.HasRequests(this.currentUser?.id!).subscribe(
      (response) => {
        this.hasRequests = response;
      }
    );
  }
}
