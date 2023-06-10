import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerRequestComponent } from './seller-request.component';

describe('SellerRequestComponent', () => {
  let component: SellerRequestComponent;
  let fixture: ComponentFixture<SellerRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerRequestComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
