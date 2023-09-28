import { Component, OnInit } from '@angular/core';
import { HandsetService } from '../services/handset/handset.service';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../services/product/product.service';
import { ProductPhoto } from '../models/ProductPhoto';
import { InitProductService } from '../services/init/init-product.service';
import { Category } from '../models/categories';
import { WishlistService } from '../services/customer-product/wishlist.service';
import { CartService } from '../services/customer-product/cart.service';
import { ProductReview } from '../models/productReview';
import { Review } from '../models/review';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReviewService } from '../services/review/review.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../services/auth/auth.service';
import { ReportService } from '../services/customer-product/report.service';
import { ProductReport } from '../models/productReport';
import { MatDialog } from '@angular/material/dialog';
import { ReportDialogComponent } from './report-dialog/report-dialog.component';
import { GuestService } from '../services/guest.service';
import { Product } from '../models/Product';

@Component({
  selector: 'app-product-view-customer',
  templateUrl: './product-view-customer.component.html',
  styleUrls: ['./product-view-customer.component.scss']
})
export class ProductViewCustomerComponent implements OnInit {

  isHandset$ = this.handsetObserver.getIsHandset();

  loadingImages: boolean = true;
  loading: boolean = true;

  product: Product;
  productId: number;
  photoIndex = 0;
  photos: ProductPhoto[] = [];
  subCategory;

  dataSource: { label: string, value: string }[] = [];

  productReview: ProductReview;
  reviews: Review[] = [];

  ratingValue: string[] = ['', '', '', '', ''];

  onWishlist: boolean;
  onCart: boolean;

  postForm: FormGroup;
  hasReview: boolean = false;
  customerReview : Review[] = [];

  reported: boolean;

  constructor(
    private handsetObserver: HandsetService,
    private route: ActivatedRoute,
    private productService: ProductService,
    private initService: InitProductService,
    private wishlistService: WishlistService,
    private cartService: CartService,
    private fb: FormBuilder,
    private reviewService: ReviewService,
    private _snackbar: MatSnackBar,
    public authService: AuthService,
    public dialog: MatDialog,
    private guestService: GuestService
  ) {}

  ngOnInit(): void {
    this.postForm = this.fb.group({
      postContent: ['', { validators: Validators.required }],
      rating: ['', { validators: Validators.required }]
    })

    this.productId = parseInt(this.route.snapshot.paramMap.get('id'));
    
    this.productService.getProductById(this.productId).subscribe({
      next: (product) => {
        this.product = product;
        console.log(product);
        this.reported = product.reported;
        this.loading = false;
        this.photos = product.productPhotos;
        this.loadingImages = false;
        this.productReview = product.productReview;
        if(this.productReview?.productReviews.length > 0) {
          this.reviews = this.productReview.productReviews;
          const id = this.authService.loadedUser.id;
          this.customerReview[0] = this.reviews.find((review) => review.customer.id = id);
          if(this.customerReview[0]) {
            this.postForm.get('postContent').setValue(this.customerReview[0].reviewContent);
            this.postForm.get('rating').setValue(this.customerReview[0].ratingValue);
            this.hasReview = true;
          }
        }
        const rating = this.productReview?.productRate;
        for(let i = 0; i < rating; i++) {
          this.ratingValue[i] = 'checked';
        }

        if (this.authService.loadedUser)
          this.onCart = this.product.onCart;
        else 
          this.onCart = this.guestService.onCart(this.productId);


        this.onWishlist = this.product.onWishlist;

        const link = product.link
        this.initService.getModel(link).subscribe({
          next: (model: Category[]) => {
            for(let spec of Object.keys(model)) {
              this.dataSource.push({ label: model[spec].label, value: product[product.specification][spec]})
            }
          }
        });

      },
      error: () => this.openSnackbar('somethin went wrong, please refresh', 'ok')
    });
  }

  updateReviews(id) {
    this.productService.getProductById(id).subscribe({
      next: (product) => {
        this.productReview = { ...product.productReview };
        if(this.productReview?.productReviews.length > 0) {
          this.reviews = this.productReview.productReviews;
          const id = this.authService.loadedUser.id;
          const varReview = this.reviews.find((review) => review.customer.id = id)
          this.customerReview = [varReview];
          
          if(this.customerReview) {
            this.postForm.get('postContent').setValue(this.customerReview[0].reviewContent);
            this.postForm.get('rating').setValue(this.customerReview[0].ratingValue);
            this.hasReview = true;
          }
        }
        else {
          this.reviews = [];
          this.customerReview = [];
        }
        this.ratingValue = ['', '', '', '', ''];
        const rating = this.productReview?.productRate;
        for(let i = 0; i < rating; i++) {
          this.ratingValue[i] = 'checked';
        }
      }
    });
    this.postForm.setValue({
      postContent: '',
      rating: 0
    })
    this.postForm.markAsUntouched(); 
    this.postForm.markAsPristine();
  }

  deletePost() {
    this.reviewService.deleteProductReview(this.productId).subscribe({
      next: () => {
        this.openSnackbar('post deleted succesfully', 'ok');
        this.hasReview = false;
        this.updateReviews(this.productId);
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackbar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackbar("an error occured while deleting post, please try again", "ok");
        }
      } 
    })
  }

  scrollBy(images: HTMLDivElement, value: number) {
    images.scrollBy({
      left: images.offsetWidth * value,
      behavior: 'smooth'
    });
  }

  scroll(images: HTMLDivElement, value: number) {
    images.scroll({
      left: images.offsetWidth * value,
      behavior: 'smooth'
    });
  }

  toggleFavorites() {

    if(!this.onWishlist) {
      this.wishlistService.addToWishlist(this.product.id).subscribe(() => this.onWishlist = !this.onWishlist);
    } else {
      this.wishlistService.removeFromWishlist(this.product.id).subscribe(() => this.onWishlist = !this.onWishlist);
    }
  }

  toggleShoppingCart() {

    if (this.authService.loadedUser === null) {///guest mode
      if (!this.onCart) {
        this.product.onCart = true;
        this.guestService.addToCart({product: this.product, quantity: 1});
      } else {
        this.product.onCart = false;
        this.guestService.removeFromCart(this.productId);
      }
      this.onCart = !this.onCart;

    } else {
      if(!this.onCart) {
        this.cartService.addToCart({ productId: this.product.id, quantity: 1 }).subscribe((res) => {
          this.onCart = !this.onCart;
          this.cartService.cartCount++;
        });
      } else {
        this.cartService.removeFromCart(this.productId).subscribe({
          next: res => {
            this.onCart = !this.onCart;
            this.cartService.cartCount -= res['deletedQuantity'];
          }
        });
      }
    }
  }

  postProductReview() {
    const post = {
      productId: this.productId,
      ratingValue: this.postForm.get('rating').value,
      reviewContent: this.postForm.get('postContent').value,
    }
    this.reviewService.postProductReview(post).subscribe({
      next: () => {
        this.openSnackbar('review posted successfully', 'ok');
        this.updateReviews(this.productId);
        this.hasReview = true;
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackbar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackbar("you can only review the product that you bought", "ok");
        }
      }
    })
  }

  editPost() {
    const post = {
      productId: this.productId,
      ratingValue: this.postForm.get('rating').value,
      reviewContent: this.postForm.get('postContent').value,
    }
    this.reviewService.editProductReview(post).subscribe({
      next: () => {
        this.openSnackbar('review edited successfully', 'ok');
        this.updateReviews(this.productId);
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackbar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackbar("an error occured while posting, please try again", "ok");
        }
      }
    })
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }

  openReportDialog() {
    const dialogRef = this.dialog.open(ReportDialogComponent, { data: { id: this.product.id }})

    dialogRef.afterClosed().subscribe(res => res ? this.reported = true : this.reported = this.reported);
  }

}
