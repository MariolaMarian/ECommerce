import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IOrder } from '../shared/models/order.interface';
import { Pagination } from '../shared/models/pagination';
import { IPagination } from '../shared/models/pagination.interface';
import { map } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  baseUrl = environment.apiUrl + 'orders';
  orders: IOrder[] = [];
  pagination = new Pagination();
  useCache = true;

  constructor(private http: HttpClient) {}

  getOrdersForUser() {
    if (this.useCache === false) {
      this.orders = [];
      this.pagination = new Pagination();
    }

    if (this.orders.length > 0 && this.useCache === true) {
      const pagesReceived = Math.ceil(
        this.orders.length / this.pagination.pageSize
      );

      if (this.pagination.pageIndex <= pagesReceived) {
        this.pagination.data = this.orders.slice(
          (this.pagination.pageIndex - 1) * this.pagination.pageSize,
          this.pagination.pageIndex * this.pagination.pageSize
        );

        return of(this.pagination);
      }
    }

    let params = new HttpParams();
    params = params.append('pageIndex', this.pagination.pageIndex.toString());
    params = params.append('pageSize', this.pagination.pageSize.toString());

    return this.http
      .get<IPagination>(this.baseUrl, {
        observe: 'response',
        params,
      })
      .pipe(
        map((resp) => {
          this.orders = [...this.orders, ...resp.body.data];
          this.pagination = resp.body;
          this.useCache = true;
          return this.pagination;
        })
      );
  }

  getOrderDetailed(id: number) {
    return this.http.get(this.baseUrl + '/' + id);
  }
}
