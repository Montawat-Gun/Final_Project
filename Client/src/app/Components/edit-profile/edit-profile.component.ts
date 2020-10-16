import { HttpEventType } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { User } from 'src/app/Models/User';
import { ToastifyService } from 'src/app/Services/toastify.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  @Input() user: User;

  userToEdit: (any);
  passwordToEdit: (any) = {};
  confirmPassword: string = '';

  errors: string[];
  passwordErrors: string[];

  completeMessage: string = '';

  isDisableUploadButton: boolean = true;
  isShowCrop: boolean = false;

  selectedFile = null;

  imageChangedEvent: any = '';
  croppedImage = null;

  constructor(private userService: UserService, private toastify: ToastifyService) { }

  @ViewChild('closeButton') closeButton;

  ngOnInit(): void {
    this.userToEdit = {
      id: this.user.id,
      username: this.user.username,
      birthdate: this.user.birthDate,
      gender: this.user.gender,
      description: this.user.description,
      email: this.user.email,
      imageUrl: this.user.imageUrl
    }
    this.croppedImage = this.user.imageUrl;
  }

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
    if (event.target.files.length <= 0) {
      this.isDisableUploadButton = true;
      this.isShowCrop = false;
      this.croppedImage = this.user.imageUrl;
    } else {
      this.isShowCrop = true;
    }
  }
  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.base64;
    this.selectedFile = this.base64ToFile(this.croppedImage, this.imageChangedEvent.target.files[0].name);
  }
  imageLoaded() {
    // show cropper
  }
  cropperReady() {
    // cropper ready
    this.isDisableUploadButton = false;
  }
  loadImageFailed() {
    this.croppedImage = this.userToEdit.imageUrl;
  }

  saveEdit() {
    this.errors = []
    this.completeMessage = ''
    this.userService.updateUser(this.userToEdit).subscribe(response => {
      this.user = response
      this.toastify.show("Yor have changed your info.");
    },
      error => {
        error.error.forEach(e => {
          this.errors.push(e.description);
        });
      });
  }

  savePasswordEdit() {
    this.passwordErrors = []
    this.completeMessage = ''
    if (this.passwordToEdit.newPassword != this.confirmPassword) {
      this.passwordErrors.push("New password and Confirm password not match!");
      return;
    }
    this.userService.updateUserPassword(this.passwordToEdit).subscribe(response => {
      this.toastify.show("You have changed your password.");
      this.passwordToEdit.currentPassword = null;
      this.passwordToEdit.newPassword = null;
      this.confirmPassword = null;
    },
      error => {
        error.error.forEach(e => {
          this.passwordErrors.push(e.description);
        });
      });
  }

  isButtonEditDisable() {
    if (this.userToEdit.username === this.user.username &&
      this.userToEdit.email === this.user.email &&
      this.userToEdit.description === this.user.description) {
      return true;
    }
  }

  isButtonPasswordEditDisable() {
    if (this.passwordToEdit.currentPassword === undefined ||
      this.passwordToEdit.newPassword === undefined) {
      return true;
    } else if (this.passwordToEdit.currentPassword === '' ||
      this.passwordToEdit.newPassword === '' ||
      this.confirmPassword === '') {
      return true;
    }
    return false;
  }

  onUploadImage() {
    this.completeMessage = ''
    const fd = new FormData();
    fd.append('File', this.selectedFile);
    this.userService.uploadUserImage(fd).subscribe(events => {
      if (events.type === HttpEventType.UploadProgress) {
        if (events.loaded == events.total)
          this.toastify.show("You have changed your image.");
      }
      this.selectedFile = null;
      this.imageChangedEvent = ''
    });
  }

  base64ToFile(data, filename) {
    const arr = data.split(',');
    const mime = arr[0].match(/:(.*?);/)[1];
    const bstr = atob(arr[1]);
    let n = bstr.length;
    let u8arr = new Uint8Array(n);

    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }
    return new File([u8arr], filename, { type: mime });
  }

}
