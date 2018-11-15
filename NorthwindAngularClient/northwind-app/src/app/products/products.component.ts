import { Component, OnInit, ViewChild } from "@angular/core";
import { ProductsService } from "../products.service";
import { ProductReadListDto } from "../models/ProductReadListDto";
import { MatPaginator } from "@angular/material/paginator";

@Component({
  selector: "app-products",
  templateUrl: "./products.component.html",
  styleUrls: ["./products.component.css"]
})
export class ProductsComponent implements OnInit {
  public products: ProductReadListDto[] = [];

  displayedColumns: string[] = [
    "productId",
    "productName",
    "quantityPerUnit",
    "unitPrice",
    "unitsInStock",
    "unitsOnOrder",
    "reorderLevel",
    "discontinued",
    "categoryName",
    "supplierName"
  ];

  constructor(private productsService: ProductsService) {}

  ngOnInit() {
    this.productsService.getList().subscribe(v => {
      this.products = v;
    });
  }
}
