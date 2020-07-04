import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of } from 'rxjs';
import { IUser } from '../shared/models/user.interface';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { IAdress } from '../shared/models/adress.interface';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl + 'account';

  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  loadCurrentUser(token: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    return this.http.get(this.baseUrl, { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  login(values: any) {
    return this.http.post(this.baseUrl + '/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(values: any) {
    return this.http.post(this.baseUrl + '/register', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkIfEmailExists(email: string) {
    return this.http.get(this.baseUrl + '/emailexists?email=' + email);
  }

  getUserAdress() {
    return this.http.get<IAdress>(this.baseUrl + '/adress');
  }

  updateUserAdress(adress: IAdress) {
    return this.http.put<IAdress>(this.baseUrl + '/adress', adress);
  }
}
