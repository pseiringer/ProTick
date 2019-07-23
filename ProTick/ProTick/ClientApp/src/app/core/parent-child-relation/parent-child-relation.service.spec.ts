import { TestBed } from '@angular/core/testing';

import { ParentChildRelationService } from './parent-child-relation.service';

describe('ParentChildRelationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ParentChildRelationService = TestBed.get(ParentChildRelationService);
    expect(service).toBeTruthy();
  });
});
