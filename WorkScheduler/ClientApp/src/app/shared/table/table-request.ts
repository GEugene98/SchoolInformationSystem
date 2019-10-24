import { SortDirection } from "./sort-direction";

export class TableRequest<FilterType> {
  pageSize: number;
  pageNumber: number;

  sortProperty: string;
  sortDirection: SortDirection;

  filter: FilterType;
}
