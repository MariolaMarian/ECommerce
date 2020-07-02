import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/product.interface';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private breadcrumb: BreadcrumbService
  ) {}

  ngOnInit(): void {
    this.breadcrumb.set('@productDetails', '');
    this.loadProduct();
  }

  loadProduct() {
    this.shopService
      .getProduct(+this.activatedRoute.snapshot.paramMap.get('id'))
      .subscribe(
        (resp) => {
          this.product = resp;
          this.breadcrumb.set('@productDetails', this.product.name);
        },
        (error) => {
          console.log(error);
        }
      );
  }
}
