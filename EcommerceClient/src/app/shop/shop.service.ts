import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination.interface';
import { IBrand } from '../shared/models/brand.interface';
import { IProductType } from '../shared/models/product-type.interface';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shop-params';
import { IProduct } from '../shared/models/product.interface';
import { environment } from 'src/environments/environment';
import { of } from 'rxjs';
import { Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IProductType[] = [];
  pagination = new Pagination();
  shopParams = new ShopParams();

  constructor(private http: HttpClient) {}

  getProducts(useCache: boolean) {
    if (useCache === false) {
      this.products = [];
    }

    if (this.products.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(
        this.products.length / this.shopParams.pageSize
      );

      if (this.shopParams.pageNumber <= pagesReceived) {
        this.pagination.data = this.products.slice(
          (this.shopParams.pageNumber - 1) * this.shopParams.pageSize,
          this.shopParams.pageNumber * this.shopParams.pageSize
        );
        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.shopParams.brandIdSelected !== 0) {
      params = params.append(
        'brandId',
        this.shopParams.brandIdSelected.toString()
      );
    }

    if (this.shopParams.productTypeIdSelected !== 0) {
      params = params.append(
        'typeId',
        this.shopParams?.productTypeIdSelected.toString()
      );
    }

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sortSelected);

    params = params.append('pageIndex', this.shopParams.pageNumber.toString());
    params = params.append('pageSize', this.shopParams.pageSize.toString());

    return this.http
      .get<IPagination>(this.baseUrl + 'products', {
        observe: 'response',
        params,
      })
      .pipe(
        map((resp) => {
          this.products = [...this.products, ...resp.body.data];
          this.pagination = resp.body;
          return this.pagination;
        })
      );
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  getProduct(id: number) {
    const product = this.products.find((p) => p.id === id);
    if (product) {
      return of(product);
    }
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    return this.http.get<IBrand[]>(this.baseUrl + 'brands').pipe(
      map((resp) => {
        this.brands = resp;
        return resp;
      })
    );
  }

  getProductTypes() {
    if (this.types.length > 0) {
      return of(this.types);
    }
    return this.http.get<IProductType[]>(this.baseUrl + 'producttypes').pipe(
      map((resp) => {
        this.types = resp;
        return resp;
      })
    );
  }
}
