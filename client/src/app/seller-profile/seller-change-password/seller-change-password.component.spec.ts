import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerChangePasswordComponent } from './seller-change-password.component';

describe('SellerChangePasswordComponent', () => {
  let component: SellerChangePasswordComponent;
  let fixture: ComponentFixture<SellerChangePasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerChangePasswordComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerChangePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
