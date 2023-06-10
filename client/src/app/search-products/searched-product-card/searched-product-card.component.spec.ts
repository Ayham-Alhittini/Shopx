import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchedProductCardComponent } from './searched-product-card.component';

describe('SearchedProductCardComponent', () => {
  let component: SearchedProductCardComponent;
  let fixture: ComponentFixture<SearchedProductCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchedProductCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchedProductCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
