import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PaginationHeaderComponent } from './components/pagination-header/pagination-header.component';
import { PaginationPagerComponent } from './components/pagination-pager/pagination-pager.component';

@NgModule({
  declarations: [PaginationHeaderComponent, PaginationPagerComponent],
  imports: [
    CommonModule,
    PaginationModule.forRoot()
  ],
  exports: [PaginationModule, PaginationHeaderComponent, PaginationPagerComponent]
})
export class SharedModule { }
