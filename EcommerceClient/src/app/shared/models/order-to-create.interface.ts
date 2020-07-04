import { IAdress } from './adress.interface';

export interface IOrderToCreate {
  basketId: string;
  deliveryMethodId: number;
  shipToAdress: IAdress;
}
