import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import {AuthService} from '../services/Auth/auth.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RegisterFormComponent } from '../register-form/register-form.component';
import {HttpErrorResponse} from '@angular/common/http';
import {ToastrService} from 'ngx-toastr';
import { YTGoldComponent } from '../YTGold/YTGold.component';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(public authService: AuthService, private modalService: NgbModal, private toastr: ToastrService ) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.toastr.success('Welcome back !');
    }, (error: HttpErrorResponse) => {
        this.toastr.error(error.error);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.toastr.info('You\'ve been logged out');
  }

  openRegisterModal() {
    const registerModalRef = this.modalService.open(RegisterFormComponent, {size: 'lg'});
  }

  openBuyYTGoldModal() {
    const buyYTGoldModalRef = this.modalService.open(YTGoldComponent, {size: 'lg'});
  }

}
