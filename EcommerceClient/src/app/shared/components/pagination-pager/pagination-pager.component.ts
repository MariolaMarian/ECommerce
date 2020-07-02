import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-pagination-pager',
  templateUrl: './pagination-pager.component.html',
  styleUrls: ['./pagination-pager.component.scss'],
})
export class PaginationPagerComponent implements OnInit {
  @Input() pageSize: number;
  @Input() totalCount: number;
  @Output() pageChanged = new EventEmitter<number>();

  constructor() {}

  ngOnInit(): void {}

  onPageChanged(event: any){
    this.pageChanged.emit(event.page);
  }
}
