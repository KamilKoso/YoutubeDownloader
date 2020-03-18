import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import {AuthService} from '../services/Auth/auth.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RegisterFormComponent } from '../register-form/register-form.component';
import {HttpErrorResponse} from '@angular/common/http';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(private authService: AuthService, private modalService: NgbModal, private toastr: ToastrService ) { }

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
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    this.toastr.info('You\'ve been logged out');
  }

  open() {
    const modalRef = this.modalService.open(RegisterFormComponent, {size: 'lg'});
  }

}
