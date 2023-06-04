import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {Order} from "./order.model";
import {environment} from "../../../environments/environment.development";

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    Authorization: 'my-auth-token'
  })
};

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) { }

  getOrders(): Observable<Order[]> {
      return this.http.get<Order[]>(environment.orderApiUrl + '/Orders');
  }

  addOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(environment.orderApiUrl + '/Orders', order);
  }

  addOrderCache(order: Order): Observable<Order> {
    return this.http.post<Order>(environment.cacheApiUrl + '/cache/Order', order);
  }

  getOrder(id: number): Observable<Order> {
    return this.http.get<Order>(environment.orderApiUrl + '/Orders/' + id);
  }
}
