import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { HttpService } from 'src/app/shared/services/http.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpService, private router: Router) {}

  getAllUser() {
    return this.http
      .get(environment.api, {
        url: 'User/GetAllUser',
        version: '1.0',
      })
      .pipe(
        map((data) => {
          return data;
        })
      );
  }
}
