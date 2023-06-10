import { TestBed } from '@angular/core/testing';

import { InitProductService } from './init-product.service';

describe('InitProductService', () => {
  let service: InitProductService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InitProductService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
