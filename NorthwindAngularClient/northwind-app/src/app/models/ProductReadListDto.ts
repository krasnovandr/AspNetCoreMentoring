export class ProductReadListDto {
  ProductId: number;
  ProductName: string;
  QuantityPerUnit: string;
  UnitPrice: number;
  UnitsInStock: number;
  UnitsOnOrder: number;
  ReorderLevel: number;
  Discontinued: boolean;
  CategoryName: string;
  SupplierName: string;
}
