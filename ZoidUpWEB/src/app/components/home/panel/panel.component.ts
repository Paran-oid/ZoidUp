import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { User } from '../../../models/user/user.model';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-panel',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './panel.component.html',
  styleUrl: './panel.component.scss',
})
export class PanelComponent implements OnInit {
  @Input() friends: User[] = [];
  filteredFriends: User[] = [];

  searchForm: FormGroup = new FormGroup({});
  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.searchForm = this.fb.group({
      search: [''],
    });
    this.filteredFriends = this.friends;
  }

  Search() {
    const search = this.searchForm.get('search')?.value.toLowerCase();
    if (search != '') {
      this.filteredFriends = this.friends.filter((friend) =>
        friend.username.toLowerCase().includes(search)
      );
    } else {
      this.filteredFriends = this.friends;
    }
  }
}
