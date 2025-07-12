import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";                      

@Injectable({
    providedIn:'root'
})
export class AdminService{
    private apiUrl= 'https://localhost:7064/api/Admin';
 constructor(private http:HttpClient){}
 getDashboardStats(): Observable<any>{
    return this.http.get(`${this.apiUrl}/stats`); 


 }

}