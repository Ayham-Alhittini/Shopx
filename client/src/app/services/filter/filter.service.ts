import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Product } from 'src/app/models/Product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  baseUrl = environment.apiBase + 'user/';

  constructor(private http: HttpClient) { }

  genericFilter(category='', searchContent='', minPrice='', maxPrice='', pageNumber='', pageSize='') {
    let query = '';

    if(category) {
      query +=  'category=' + encodeURIComponent(category) + '&';
    }
    query += this.getQuery(searchContent, minPrice, maxPrice, pageNumber, pageSize);

    return this.http.get<Product[]>(this.baseUrl + 'generic-filter?' +  query, { observe: 'response'})
      .pipe(map(res => ({ 
        products: res.body, 
        pagination: this.readPaginationHeader(res.headers.get('pagination')) 
      })));
  }

  filterGategory(categoryLink, model={}, searchContent='', minPrice='', maxPrice='', pageNumber='', pageSize='') {
    let query = '';

    query += this.getQuery(searchContent, minPrice, maxPrice, pageNumber, pageSize);
    query += this.getQueryFromModel(model);

    return this.http.get<Product[]>(this.baseUrl + categoryLink + '-filter?' + query, { observe: 'response'})
      .pipe(map(res => ({ 
        products: res.body, 
        pagination: this.readPaginationHeader(res.headers.get('pagination')) 
      })));
  }

  private getQueryFromModel(model) {
    let query = '';

    if(Object.keys(model).length === 0)
      return query;

    for(let key of Object.keys(model)) {
      if(model[key] !== '') {
        query += key + '=' + model[key] + '&';
      }
    }

    return query;
  }

  private getQuery(searchContent, minPrice, maxPrice, pageNumber, pageSize) {
    let query = '';

    if(searchContent) {
      query +=  'SearchContent=' + encodeURIComponent(searchContent) + '&';
    }
    if(minPrice) {
      query +=  'MinPrice=' + encodeURIComponent(minPrice) + '&';
    }
    if(maxPrice) {
      query +=  'MaxPrice=' + encodeURIComponent(maxPrice) + '&';
    }
    if(pageNumber) {
      query +=  'PageNumber=' + encodeURIComponent(pageNumber) + '&';
    }
    if(pageSize) {
      query +=  'PageSize=' + encodeURIComponent(pageSize) + '&';
    }

    return query;
  }

  private readPaginationHeader(pagenationString: string) {
    let currentPage: number;
    let itemsPerPage: number;
    let totalItems: number;
    let totalPages: number;

    var str = pagenationString.split(':')[1].split(',')[0];
    currentPage = parseInt(str);
    str = pagenationString.split(':')[2].split(',')[0];
    itemsPerPage = parseInt(str);
    str = pagenationString.split(':')[3].split(',')[0];
    totalItems = parseInt(str);
    str = pagenationString.split(':')[4].split('}')[0];
    totalPages = parseInt(str);

    const pagination = { currentPage: currentPage, itemsPerPage: itemsPerPage, totalItems: totalItems, totalPages: totalPages };
    
    return pagination;
  }
}
