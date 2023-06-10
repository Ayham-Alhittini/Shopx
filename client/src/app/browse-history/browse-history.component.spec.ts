import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrowseHistoryComponent } from './browse-history.component';

describe('BrowseHistoryComponent', () => {
  let component: BrowseHistoryComponent;
  let fixture: ComponentFixture<BrowseHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BrowseHistoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BrowseHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
