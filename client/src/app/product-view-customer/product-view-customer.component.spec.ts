import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductViewCustomerComponent } from './product-view-customer.component';

describe('ProductViewCustomerComponent', () => {
  let component: ProductViewCustomerComponent;
  let fixture: ComponentFixture<ProductViewCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductViewCustomerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductViewCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
