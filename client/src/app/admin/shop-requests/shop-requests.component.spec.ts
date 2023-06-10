import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopRequestsComponent } from './shop-requests.component';

describe('ShopRequestsComponent', () => {
  let component: ShopRequestsComponent;
  let fixture: ComponentFixture<ShopRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShopRequestsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShopRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
