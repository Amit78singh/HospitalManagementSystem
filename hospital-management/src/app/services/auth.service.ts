import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, tap,catchError, throwError } from "rxjs";

import { JwtHelperService } from "@auth0/angular-jwt";



@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7064/api/Auth';
  message:string='';


  constructor(
    private http: HttpClient,
    private jwtHelper:JwtHelperService
  ) {}

  //set a message (to auto clear after 3 sec)
  setMessage(msg:string):void{
    this.message=msg;
    setTimeout(()=> this.message='',3000);
  }

  register(email: string, password: string, role: string): Observable<any> {
  return this.http.post(`${this.apiUrl}/register`, {
    Email: email,
    PasswordHash: password,
    Role: role
  });
}


  login(email: string, password: string): Observable<any> {
  return this.http.post<{ token: string }>('https://localhost:7064/api/Auth/login', {
    Email: email,
    PasswordHash: password
  }).pipe(
    tap(res => { localStorage.setItem('token', res.token);
    this.setMessage(' Login Successful!');
  }), 
  catchError(error=>{
    this.setMessage('Login failed . Please Check Your Email and Password');
    return throwError(()=>error);

  })
  );
}


  logout(): void {
    
    localStorage.removeItem('token');
     this.setMessage('🚪 Logged out successfully!'); 

     setTimeout(()=>{
     window.location.href = '/login';
     },800);
     
  }

  isLoggedIn(): boolean {
    const token =this.getToken();
    return !! token && !this.jwtHelper.isTokenExpired(token);

  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

 getUserRole(): string | null {
  const token = this.getToken();
  if (!token) return null;

  const decoded = this.jwtHelper.decodeToken(token);

  // Handle standard and Microsoft-specific claim formats
  return decoded?.role || decoded?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
}


  isAdmin(): boolean{
    return this.getUserRole()==='Admin'
  }
}
