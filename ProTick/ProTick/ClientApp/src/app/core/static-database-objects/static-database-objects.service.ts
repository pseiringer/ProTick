import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StaticDatabaseObjectsService {

  constructor() { }

  getStates() {
    return {
      Open: 1,
      InProgress: 2,
      Finished: 3
    };
  };
  getRoles() {
    return {
      Admin: "Admin",
      Employee: "Employee"
    };
  };
}
