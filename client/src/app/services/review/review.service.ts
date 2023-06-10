import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReviewPost } from 'src/app/models/reviewPost';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  private baseUrl = environment.apiBase + 'review/';

  constructor(private http: HttpClient) { }

  postProductReview(post: ReviewPost) {
    return this.http.post<ReviewPost>(this.baseUrl + 'post-product-review', post);
  }

  postProductReviewRating(vote: number, reviewId: number) {
    return this.http.post<number>(this.baseUrl + 'post-product-review-vote/' + reviewId, vote);
  }
  
  editProductReview(post: ReviewPost) {
    return this.http.put(this.baseUrl + 'edit-product-review', post);
  }

  deleteProductReview(productId: number) {
    return this.http.delete(this.baseUrl + 'delete-product-review/' + productId);
  }
}
