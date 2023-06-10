import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStaticsComponent } from './product-statics.component';

describe('ProductStaticsComponent', () => {
  let component: ProductStaticsComponent;
  let fixture: ComponentFixture<ProductStaticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductStaticsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductStaticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
