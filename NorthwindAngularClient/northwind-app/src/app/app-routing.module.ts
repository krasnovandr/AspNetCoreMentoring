import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ProductsComponent } from "./products/products.component";
import { HomeComponent } from "./home/home.component";
import { CategoriesComponent } from "./categories/categories.component";

export const routes: Routes = [
  { path: "products", component: ProductsComponent },
  { path: "categories", component: CategoriesComponent },
  { path: "", component: HomeComponent, pathMatch: "full" }

  // { path: 'notfound', component: PageNotFoundComponent },
  //  { path: '', redirectTo: '/courses', pathMatch: 'full' },
  // { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
