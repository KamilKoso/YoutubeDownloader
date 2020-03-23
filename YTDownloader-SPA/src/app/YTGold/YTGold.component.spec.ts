/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { YTGoldComponent } from './YTGold.component';

describe('YTGoldComponent', () => {
  let component: YTGoldComponent;
  let fixture: ComponentFixture<YTGoldComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YTGoldComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YTGoldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
