import { Component, OnInit } from "@angular/core";
import { CategoryReadListDto } from "../models/CategoryReadListDto";
import { CategoriesService } from "../categories.service";

@Component({
  selector: "app-categories",
  templateUrl: "./categories.component.html",
  styleUrls: ["./categories.component.css"]
})
export class CategoriesComponent implements OnInit {
  public categories: CategoryReadListDto[] = [];
  displayedColumns: string[] = ["categoryId", "description", "categoryName"];

  constructor(private categoriesService: CategoriesService) {}

  ngOnInit() {
    this.categoriesService.getList().subscribe(v => {
      this.categories = v;
    });
  }
}
