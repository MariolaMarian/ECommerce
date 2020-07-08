import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { IBasket } from '../shared/models/basket.interface';
import { map } from 'rxjs/operators';
import { IProduct } from '../shared/models/product.interface';
import { IBasketItem } from '../shared/models/basket-item.interface';
import { Basket } from '../shared/models/basket';
import { IBasketTotals } from '../shared/models/basket-totals.interface';
import { IDeliveryMethod } from '../shared/models/delivery-method.interface';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = environment.apiUrl;
  shipping = 0;
  private basketSource = new BehaviorSubject<IBasket>(null);
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);

  basket$ = this.basketSource.asObservable();
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient, private toastr: ToastrService) {}

  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id).pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
        this.shipping = basket.shippingPrice;
        this.calculateTotals();
      })
    );
  }

  setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe(
      (resp: IBasket) => {
        this.basketSource.next(resp);
        this.calculateTotals();
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(
      item,
      quantity
    );
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.AddOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex((x) => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex((x) => x.id === item.id);
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
    } else {
      this.removeItemFromBasket(item);
    }

    this.setBasket(basket);
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some((x) => x.id === item.id)) {
      basket.items = basket.items.filter((i) => i.id !== item.id);
    }
    if (basket.items.length > 0) {
      this.setBasket(basket);
    } else {
      this.deleteBasket(basket);
    }
  }

  deleteBasketLocal() {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(
      () => {
        this.basketSource.next(null);
        this.basketTotalSource.next(null);
        localStorage.removeItem('basket_id');
      },
      (error) => {
        console.log(error);
      }
    );
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = this.shipping;
    const subtotal = basket.items.reduce((a, b) => a + b.price * b.quantity, 0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({ shipping, total, subtotal });
  }

  private AddOrUpdateItem(
    items: IBasketItem[],
    itemToAdd: IBasketItem,
    quantity: number
  ): IBasketItem[] {
    const index = items.findIndex((i) => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
      this.toastr.success('Item added to cart');
    } else {
      items[index].quantity += quantity;
      this.toastr.success('Item count increased')
    }

    return items;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(
    item: IProduct,
    quantity: number
  ): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType,
    };
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod) {
    this.shipping = deliveryMethod.price;
    const basket = this.getCurrentBasketValue();
    basket.deliveryMethodId = deliveryMethod.id;
    basket.shippingPrice = deliveryMethod.price;
    this.calculateTotals();
    this.setBasket(basket);
  }

  createPaymentIntent() {
    return this.http
      .post(this.baseUrl + 'payments/' + this.getCurrentBasketValue().id, {})
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
        })
      );
  }
}
