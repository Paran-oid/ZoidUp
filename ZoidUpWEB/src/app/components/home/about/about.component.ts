import { Component, OnInit } from '@angular/core';
import { User } from '../../../models/user/user.model';
import { PassUserService } from '../../../services/frontend/pass-user.service';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss',
})
export class AboutComponent implements OnInit {
  friend: User | null = null;
  constructor(private passUserService: PassUserService) {}
  ngOnInit(): void {
    this.passUserService.passedUser$.subscribe((friend) => {
      this.friend = friend;
    });
  }
}
