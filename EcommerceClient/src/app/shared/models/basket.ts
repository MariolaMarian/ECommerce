import { IBasket } from './basket.interface';
import { IBasketItem } from './basket-item.interface';
import { v4 as uuidv4 } from 'uuid';

export class Basket implements IBasket {
  id = uuidv4();
  items: IBasketItem[] = [];
}
