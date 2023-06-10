import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoryProductComponent } from './history-product.component';

describe('HistoryProductComponent', () => {
  let component: HistoryProductComponent;
  let fixture: ComponentFixture<HistoryProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistoryProductComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistoryProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
