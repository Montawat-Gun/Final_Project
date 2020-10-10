import { Injectable } from '@angular/core';
import Toastify from 'toastify-js'

@Injectable({
  providedIn: 'root'
})
export class ToastifyService {

  constructor() { }

  show(message: string) {
    Toastify({
      text: message,
      duration: 5000,
      close: true,
      gravity: "bottom", // `top` or `bottom`
      position: 'right', // `left`, `center` or `right`
      backgroundColor: "linear-gradient(to right, #00b09b, #96c93d)",
      stopOnFocus: true, // Prevents dismissing of toast on hover
      onClick: function () { } // Callback after click
    }).showToast();
  }
}
