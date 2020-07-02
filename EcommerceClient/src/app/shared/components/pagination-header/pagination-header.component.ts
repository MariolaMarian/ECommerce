import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-pagination-header',
  templateUrl: './pagination-header.component.html',
  styleUrls: ['./pagination-header.component.scss'],
})
export class PaginationHeaderComponent implements OnInit {
  @Input() totalCount: number;
  @Input() pageNumber: number;
  @Input() pageSize: number;

  constructor() {}

  ngOnInit(): void {}
}
