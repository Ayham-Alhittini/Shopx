import { Component, OnInit } from '@angular/core';
import { HandsetService } from '../services/handset/handset.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/product/product.service';
import { ProductPhoto } from '../models/ProductPhoto';
import { InitProductService } from '../services/init/init-product.service';
import { Category } from '../models/categories';
import { ProductReview } from '../models/productReview';
import { Review } from '../models/review';
import { User } from '../models/user';
import { AuthService } from '../services/auth/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { DiscountDialogComponent } from './discount-dialog/discount-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product-view',
  templateUrl: './product-view.component.html',
  styleUrls: ['./product-view.component.scss']
})
export class ProductViewComponent implements OnInit {

  currentUser: User;
  isHandset$ = this.handsetObserver.getIsHandset();

  loadingImages: boolean = true;
  loading: boolean = true;

  product;
  productId: number;
  photoIndex = 0;
  photos: ProductPhoto[] = [];
  subCategory;

  dataSource: { label: string, value: string }[] = [];
  displayedColumns: string[] = [];
  productReview: ProductReview;
  reviews: Review[] = [];

  ratingValue: string[] = ['', '', '', '', ''];

  constructor(
    private handsetObserver: HandsetService,
    private route: ActivatedRoute,
    private productService: ProductService,
    private initService: InitProductService,
    private router: Router,
    private authService: AuthService,
    public dialog: MatDialog,
    private _snackbar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.loadedUser;

    this.productId = parseInt(this.route.snapshot.paramMap.get('id'));
    
    this.productService.getProductById(this.productId).subscribe({
      next: (product) => {
        this.product = product;
        this.loading = false;
        this.photos = product.productPhotos;
        this.loadingImages = false;
        this.productReview = product.productReview;
        if(this.productReview?.productReviews.length > 0) {
          this.reviews = this.productReview.productReviews;
        }
        const rating = this.productReview?.productRate;
        for(let i = 0; i < rating; i++) {
          this.ratingValue[i] = 'checked';
        }

        const link = product.link
        this.initService.getModel(link).subscribe({
          next: (model: Category[]) => {
            for(let spec of Object.keys(model)) {
              this.displayedColumns.push(spec);
              this.dataSource.push({ label: model[spec].label, value: product[product.specification][spec]})
            }
          }
        });

      }
    });
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

  deleteProduct() {
    const id = this.productId;
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.openSnackbar("product with id: " + id + " was deleted successfully", "ok");
        this.router.navigateByUrl("/seller-products")
      }
    });
  }

  editMode() :boolean {
    return this.currentUser.id === this.product.sellerId;
  }

  openDiscountDialog() {
    const dialogRef = this.dialog.open(DiscountDialogComponent, { data: { id: this.product.id }});

    dialogRef.afterClosed().subscribe((rate) => {
      if(rate) {
        this.product = { 
          ...this.product, 
          priceAfterDiscount: this.product.price - (this.product.price * rate)/100,
        }
      }
    });
  }

  openSnackbar(message: string, action: string) {

  }

}
