import { IAdress } from './adress.interface';
import { IOrderItem } from './order-item.interface';

export interface IOrder {
  id: number;
  buyerEmail: string;
  orderDate: Date;
  shipToAdress: IAdress;
  deliveryMethod: string;
  shippingPrice: number;
  orderItems: IOrderItem[];
  subtotal: number;
  status: string;
  total: number;
}
