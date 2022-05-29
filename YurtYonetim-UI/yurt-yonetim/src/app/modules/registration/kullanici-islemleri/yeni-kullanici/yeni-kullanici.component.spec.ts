/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { YeniKullaniciComponent } from './yeni-kullanici.component';

describe('YeniKullaniciComponent', () => {
  let component: YeniKullaniciComponent;
  let fixture: ComponentFixture<YeniKullaniciComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YeniKullaniciComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YeniKullaniciComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
