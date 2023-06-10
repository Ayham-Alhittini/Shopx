import { Component, Input, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Review } from 'src/app/models/review';
import { ReviewService } from 'src/app/services/review/review.service';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.scss']
})
export class ReviewComponent implements OnInit {
  
  @Input() review: Review;
  ratingValue: string[] = ['', '', '', '', ''];
  initialVote = 0;
  voteRating;

  constructor(
    private reviewService: ReviewService,
    private _snackbar: MatSnackBar
  ) {}
  
  ngOnInit(): void {
    const rating = this.review.ratingValue;
    for(let i = 0; i < rating; i++) {
      this.ratingValue[i] = 'checked';
    }

    this.voteRating = this.review.voteRating;
    this.initialVote = this.review.initial;
  }

  vote(vote: number) {
    this.reviewService.postProductReviewRating(vote, this.review.id).subscribe({
      next: () => {
        if(this.initialVote === 0) {
          this.voteRating += vote;
          this.initialVote = vote; return;
        }
        if(vote !== this.initialVote) {
          this.voteRating += vote * 2;
          this.initialVote = vote; return;
        }
        if(vote === this.initialVote) {
          this.voteRating += vote * -1;
          this.initialVote = 0; return;
        }
      },
      error: () => {
        this.openSnackbar("something went wrong, please try again", "ok");
      }
    })
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }


}
