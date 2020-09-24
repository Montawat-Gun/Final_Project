import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/Models/User';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user: User

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    })
  }

}
