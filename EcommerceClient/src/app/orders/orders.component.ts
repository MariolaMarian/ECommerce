import { Component, OnInit } from '@angular/core';
import { IOrder } from '../shared/models/order.interface';
import { OrdersService } from './orders.service';
import { Pagination } from '../shared/models/pagination';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {
  orders: IOrder[];
  totalCount: number;
  paginationParams: Pagination;
  useCache = true;

  constructor(private ordersService: OrdersService, private activatedRoute: ActivatedRoute) {
    this.useCache = this.activatedRoute.snapshot.queryParams.useCache || true;
    this.paginationParams = ordersService.pagination;
  }

  ngOnInit(): void {
    this.getOrders();
  }

  private getOrders() {
    this.ordersService.getOrdersForUser().subscribe(
      (resp) => {
        this.orders = resp.data;
        this.totalCount = resp.count;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  onPageChanged(event: number) {
    const pagination = this.ordersService.pagination;
    if (pagination.pageIndex !== event) {
      pagination.pageIndex = event;
      this.ordersService.pagination = pagination;
      this.paginationParams = pagination;
      this.getOrders();
    }
  }
}
