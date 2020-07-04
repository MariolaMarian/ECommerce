import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PaginationHeaderComponent } from './components/pagination-header/pagination-header.component';
import { PaginationPagerComponent } from './components/pagination-pager/pagination-pager.component';
import { OrderTotalsComponent } from './order-totals/order-totals.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from './components/text-input/text-input.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { StepperComponent } from './components/stepper/stepper.component';
import { RouterModule } from '@angular/router';
import { BasketSummaryComponent } from './components/basket-summary/basket-summary.component';

@NgModule({
  declarations: [
    PaginationHeaderComponent,
    PaginationPagerComponent,
    OrderTotalsComponent,
    TextInputComponent,
    StepperComponent,
    BasketSummaryComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    ReactiveFormsModule,
    CdkStepperModule,
    RouterModule,
  ],
  exports: [
    PaginationModule,
    PaginationHeaderComponent,
    PaginationPagerComponent,
    TextInputComponent,
    CarouselModule,
    BsDropdownModule,
    OrderTotalsComponent,
    ReactiveFormsModule,
    CdkStepperModule,
    StepperComponent,
    BasketSummaryComponent
  ],
})
export class SharedModule {}
