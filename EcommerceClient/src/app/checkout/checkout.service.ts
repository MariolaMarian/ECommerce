import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { IDeliveryMethod } from '../shared/models/delivery-method.interface';
import { map } from 'rxjs/operators';
import { IOrderToCreate } from '../shared/models/order-to-create.interface';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  baseUrl = environment.apiUrl + 'orders';

  constructor(private http: HttpClient) {}

  createOrder(order: IOrderToCreate) {
    return this.http.post(this.baseUrl, order);
  }

  getDeliveryMethods() {
    return this.http.get(this.baseUrl + '/deliveryMethods').pipe(
      map((dm: IDeliveryMethod[]) => {
        return dm.sort((a, b) => b.price - a.price);
      })
    );
  }
}
