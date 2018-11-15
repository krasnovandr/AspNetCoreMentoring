import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";
import { CategoryReadListDto } from "./models/CategoryReadListDto";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class CategoriesService {
  public baseUrl = environment.apiEndpoints.apiUrl;
  public categoriesUrl = "api/categories";
  constructor(private http: HttpClient) {}

  public getList(): Observable<CategoryReadListDto[]> {
    return this.http.get(`${this.baseUrl}/${this.categoriesUrl}`).pipe(
      map((response: CategoryReadListDto[]) => {
        return response;
      })
    );
  }
}
