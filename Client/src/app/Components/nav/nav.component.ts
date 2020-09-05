import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private router:Router ,private authService:AuthService) { }

  ngOnInit(): void {
  }

  logout(){
    this.authService.logout();
    this.router.navigate(['login']);
  }

}
