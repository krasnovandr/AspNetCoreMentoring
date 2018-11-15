import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { ProductReadListDto } from "./models/ProductReadListDto";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { PagerOptions } from "./models/PagerOptions";

@Injectable({
  providedIn: "root"
})
export class ProductsService {
  public baseUrl = environment.apiEndpoints.apiUrl;
  public productsUrl = "api/products";
  constructor(private http: HttpClient) {}

  public getList(): Observable<ProductReadListDto[]> {
    const params = this.buildParams(null);

    return this.http
      .get(`${this.baseUrl}/${this.productsUrl}`, { params: params })
      .pipe(
        map((response: ProductReadListDto[]) => {
          return response;
        })
      );
  }

  private buildParams(pagerOptions?: PagerOptions) {
    let params = new HttpParams();
    if (!pagerOptions) {
      pagerOptions = PagerOptions.getDefaultOptions();
    }

    params = params.append("pageNumber", pagerOptions.pageIndex.toString());
    params = params.append(
      "itemsPerPage",
      pagerOptions.itemsPerPage.toString()
    );

    return params;
  }
}
