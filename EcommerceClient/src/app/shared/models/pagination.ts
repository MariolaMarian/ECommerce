import { IPagination } from './pagination.interface';
import { IProduct } from './product.interface';

export class Pagination implements IPagination
{
    pageIndex = 1;
    pageSize = 6;
    count: number;
    data: any[] = [];
}