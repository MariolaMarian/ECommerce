import { IBasketItem } from './basket-item.interface';

export interface IBasket {
  id: string;
  items: IBasketItem[];
}
