import { Component, OnInit, ViewContainerRef  } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {FormGroup, FormControl, Validators} from '@angular/forms';
import {AuthService} from '../services/Auth/auth.service';
import {HttpErrorResponse} from '@angular/common/http';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css']
})
export class RegisterFormComponent implements OnInit {

  constructor(public activeModal: NgbActiveModal, private auth: AuthService, private toastr: ToastrService) { }


registerModel: FormGroup;
correctRegisterModel: any = {};
serverError: string = null;

  ngOnInit() {
    this.registerModel = new FormGroup({
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15),
      ]),
      email: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$')
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(320),
      ]),
    });
  }

  register() {
      if (this.registerModel.valid) {
        this.correctRegisterModel = this.registerModel.getRawValue();
        this.auth.register(this.correctRegisterModel).subscribe(() => {
            this.toastr.success("Registration completed successfully")
        }, (error: HttpErrorResponse) => {
            this.toastr.error(error.error);
        });

       }
      }
    }
