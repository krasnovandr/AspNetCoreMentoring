import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { MenuComponent } from "./menu/menu.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import {
  MatMenuModule,
  MatButtonModule,
  MatIconModule,
  MatCardModule,
  MatToolbarModule,
  MatTableModule,
  MatSidenavModule,
  MatPaginator
  // MatCellDef,
  // MatHeaderCellDef,
  // MatColumnDef
} from "@angular/material";
import { ProductsComponent } from "./products/products.component";
import { HomeComponent } from "./home/home.component";
import { HttpClientModule } from "@angular/common/http";
import { CategoriesComponent } from './categories/categories.component';

@NgModule({
  declarations: [AppComponent, MenuComponent, ProductsComponent, HomeComponent, CategoriesComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatToolbarModule,
    MatTableModule,
    HttpClientModule,
    MatSidenavModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
