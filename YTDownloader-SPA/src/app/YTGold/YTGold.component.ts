import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import { AuthService } from '../services/Auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-YTGold',
  templateUrl: './YTGold.component.html',
  styleUrls: ['./YTGold.component.css']
})
export class YTGoldComponent implements OnInit {

  constructor(public activeModal: NgbActiveModal, private toastr: ToastrService, private authService: AuthService) {}

  ngOnInit() {
  }

  // Account levels
  // 0 - none
  // 1 - standard
  // 2 - gold
changeToGold() {
 this.authService.changeAccountLevel(2).subscribe(response => {
   this.toastr.success('Success !');
 }, (error: HttpErrorResponse) => {
    this.toastr.error(error.error);
    console.log(error);
 });
}

changeToStandard() {
  this.authService.changeAccountLevel(1).subscribe(response => {
    this.toastr.success('Success !');
  }, (error: HttpErrorResponse) => {
     this.toastr.error(error.error);
     console.log(error);
  });
}

loggedIn() {
  return this.authService.loggedIn();
}
}
