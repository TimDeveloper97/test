import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { AuthenticationService } from '../services'; 
declare var $: any;
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  message: string;
  returnUrl: string
  model: any = {}
  constructor(private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,  
    private titleservice: Title,
  ) { }

  ngOnInit() {
    // reset login status
    this.authenticationService.logout();

    this.titleservice.setTitle("PHẦN MỀM QUẢN LÝ HỆ THỐNG");
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login() {
    this.authenticationService.login(this.model).subscribe(
      data => {
        // login successful if there's a jwt token in the response
        if (data && data.access_token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          data.LoginDate = new Date();
          localStorage.setItem('qltkcurrentUser', JSON.stringify(data)); 
          this.router.navigate(['/qltk']);
        }
      },
      error => {
        if (error.status == 0) {
          this.message = "Không kết nối server";
        } else {
          this.message = error.error.error_description;
        }
      }
    );
  }
 
}
