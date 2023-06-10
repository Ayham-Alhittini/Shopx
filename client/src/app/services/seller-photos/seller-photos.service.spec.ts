import { TestBed } from '@angular/core/testing';

import { SellerPhotosService } from './seller-photos.service';

describe('SellerPhotosService', () => {
  let service: SellerPhotosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SellerPhotosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
