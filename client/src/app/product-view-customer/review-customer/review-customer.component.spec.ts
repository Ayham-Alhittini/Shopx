import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewCustomerComponent } from './review-customer.component';

describe('ReviewCustomerComponent', () => {
  let component: ReviewCustomerComponent;
  let fixture: ComponentFixture<ReviewCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewCustomerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
