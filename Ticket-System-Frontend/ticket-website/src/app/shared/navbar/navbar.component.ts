import { Component } from '@angular/core';
import {AuthenticationService} from "../authentication.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {

  constructor(private authenticationService: AuthenticationService,
              private router: Router) {
  }

  checkIfLoggedIn(): boolean {
    // @ts-ignore
    if(JSON.parse(localStorage.getItem('currentUser')) === null){
      return false;
    } else {
      return true;
    }
  }

  logout(): void{
    this.router.navigate(['/']).then(r => this.authenticationService.logout());
  }
}
