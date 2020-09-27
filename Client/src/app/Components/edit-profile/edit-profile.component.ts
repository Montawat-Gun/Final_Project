import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  @Input() user: User;

  userToEdit: (any);

  errors: string[]

  disableButton: boolean = false;

  constructor(private userService: UserService, private route: Router) { }

  ngOnInit(): void {
    this.userToEdit = {
      id: this.user.id,
      username: this.user.username,
      birthdate: this.user.birthdate,
      gender: this.user.gender,
      description: this.user.description,
      email: this.user.email,
      imageUrl: this.user.imageUrl
    }
  }

  saveEdit() {
    this.errors = []
    this.userService.updateUser(this.userToEdit).subscribe(response => this.user = response,
      error => {
        error.error.forEach(e => {
          this.errors.push(e.description);
        });
      });
  }

  isButtonDisable() {
    if (this.userToEdit.username === this.user.username &&
      this.userToEdit.email === this.user.email &&
      this.userToEdit.description === this.user.description) {
      return true;
    }
    return false;
  }

}
