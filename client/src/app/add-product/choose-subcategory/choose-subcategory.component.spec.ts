import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseSubcategoryComponent } from './choose-subcategory.component';

describe('ChooseSubcategoryComponent', () => {
  let component: ChooseSubcategoryComponent;
  let fixture: ComponentFixture<ChooseSubcategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseSubcategoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChooseSubcategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
