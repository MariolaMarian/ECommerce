import { IPagination } from './pagination.interface';
import { IProduct } from './product.interface';

export class Pagination implements IPagination
{
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IProduct[] = [];
    
}