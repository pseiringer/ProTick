import { TestBed } from '@angular/core/testing';

import { StaticDatabaseObjectsService } from './static-database-objects.service';

describe('StaticDatabaseObjectsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StaticDatabaseObjectsService = TestBed.get(StaticDatabaseObjectsService);
    expect(service).toBeTruthy();
  });
});
