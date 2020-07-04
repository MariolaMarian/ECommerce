import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/order.interface';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss'],
})
export class OrderDetailedComponent implements OnInit {
  order: IOrder;

  constructor(
    private route: ActivatedRoute,
    private breadcrumb: BreadcrumbService,
    private ordersService: OrdersService
  ) {
    this.breadcrumb.set('@OrderDetailed', '');
  }

  ngOnInit(): void {
    this.ordersService
      .getOrderDetailed(+this.route.snapshot.paramMap.get('id'))
      .subscribe(
        (order: IOrder) => {
          this.order = order;
          this.breadcrumb.set(
            '@OrderDetailed',
            `Order #${order.id} - ${order.status}`
          );
        },
        (error) => {
          console.log(error);
        }
      );
  }
}
