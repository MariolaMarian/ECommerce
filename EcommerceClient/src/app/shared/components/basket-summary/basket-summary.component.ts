import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Observable } from 'rxjs';
import { IBasket } from '../../models/basket.interface';
import { IBasketItem } from '../../models/basket-item.interface';
import { IOrderItem } from '../../models/order-item.interface';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {

  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();

  @Input() isModificable = true;
  @Input() items: IBasketItem[] | IOrderItem[] = [];

  ngOnInit(): void {
  }

  decrementItemQuantity(item: IBasketItem)
  {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: IBasketItem)
  {
    this.increment.emit(item);
  }

  removeBasketItem(item: IBasketItem)
  {
    this.remove.emit(item);
  }

}
