import { BreakpointObserver } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../services/product/product.service';
import { Observable, map } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../models/Product';
import { InitProductService } from '../services/init/init-product.service';
import { SellerPhotosService } from '../services/seller-photos/seller-photos.service';
import { MatDialog } from '@angular/material/dialog';
import { EditComponent } from '../edit/edit.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent {
  
  stepperOrientation: Observable<'horizontal' | 'vertical'>;

  editProductForm: FormGroup;
  editProductSpecForm: FormGroup;

  productId: number;
  productUrl: string;
  product;

  model: any = {};

  constructor(
    private breakpointObserver: BreakpointObserver,
    private fb: FormBuilder,
    private productService: ProductService,
    private initService: InitProductService,
    private route: ActivatedRoute,
    private router: Router,
    private sellerPhotosService: SellerPhotosService,
    private dialog: MatDialog,
    private _snackbar: MatSnackBar
  ) {
    this.stepperOrientation = breakpointObserver
      .observe('(min-width: 800px)')
      .pipe(map(({matches}) => (matches ? 'horizontal' : 'vertical')));
  }

  ngOnInit(): void {
    
    this.productId = parseInt(this.route.snapshot.paramMap.get('id'));
    
    this.productService.getProductById(this.productId).subscribe({
      next: (product) => {
        this.product = product;
        this.getEditForm(product);

        this.photoList = product.productPhotos;

        this.initService.getCategories().subscribe({
          next: (res) => { 
            this.productUrl = res.find(category => category.label === this.product.category).link;

            this.initService.getModel(this.productUrl).subscribe((formModel) => {
              this.setFormControlValues(product, formModel); // update form model values
            });
          }
        });
      }
    });

    this.editProductForm = this.fb.group({
      productName: [ '', { validators: [ Validators.required, ] }],
      price: [ '', { validators: [ Validators.required, Validators.min(0) ] }],
      quantity: [ '', { validators: [ Validators.required, , Validators.min(0) ] }],
      productDescription: [ '', { validators: [ Validators.required ] }],
      subCategory: new FormControl({ value: '', disabled: true }),
      category: new FormControl({ value: '', disabled: true }),
    })

    this.editProductSpecForm = this.fb.group({});

  }

  setFormControlValues(product: Product, model) {

    for(const field of Object.keys(model)) {
      model[field].value = product[product.specification][field];
    }

    this.checkIfPets(model);
    this.model = { ...model };

  }

  checkIfPets(formModel) {
    if(formModel["petName"]) {
      formModel["petName"].value = this.product["subCategory"];
      formModel["petName"].rules = [ ...formModel["petName"].rules, "disabled" ];
    }
  }

  setForm(form: FormGroup) {
    this.editProductSpecForm = form;
  }

  submit() {
    const id = this.productId;
    const product = { ...this.editProductForm.getRawValue(), ...this.editProductSpecForm.getRawValue(), type: this.product.subCategory };

    //// API call
    this.productService.editProduct(this.productUrl, product, id).subscribe({
      next: () => {
        this.openSnackbar("product edited successfully", 'ok');
        ///// routing on success 
        this.router.navigateByUrl("/seller-products");
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackbar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackbar("an error occured, please try again", "ok");
        }
      }
    });

  }

  getEditForm(product) {
    this.editProductForm = this.fb.group({
      productName: [ product.productName, { validators: [ Validators.required, ] }],
      price: [ product.price, { validators: [ Validators.required, Validators.min(0) ] }],
      quantity: [ product.quantity, { validators: [ Validators.required, , Validators.min(0) ] }],
      productDescription: [ product.productDescription, { validators: [ Validators.required ] }],
      subCategory: [ product.subCategory ],
      category: [ product.category ],
      type: [ product.type ]
    })
  }

  //////////////// edit photos;

  photoList: { url: string, id: number}[] = [];

  upload(result) {
    const id = this.productId;
    const file = result.file;
  
    if(this.photoList.length < 5 && result) {
      this.photoList.push({ url: result.url, id: -1 })

      const formData = new FormData();
      formData.append('file', file);

      this.sellerPhotosService.uploadProductPhoto(id, formData).subscribe({
        next: (photo) => { /// { url: "", id: ""}
          const photoIndex = this.photoList.findIndex(photo => photo.id === -1);
          if(photoIndex !== -1)
            this.photoList.splice(photoIndex, 1, { url: result.url, id: photo.id });
        }
      });
    }
  }

  deleteImage(id: number) {
    const index = this.photoList.findIndex(file => file.id === id);
    this.photoList.splice(index, 1);

    if(id === -1){
      this.openSnackbar("photo was deleted", 'ok');
      return;
    }

    this.sellerPhotosService.deleteProductPhoto(this.productId, id).subscribe({
      next: () => this.openSnackbar("photo was deleted", 'ok')
    });
  }

  dialogTitle = "Add an Image";
  dialogDescreption = "This image will be one of the images that are avalible for customers to see when they view this product.";
  dialogUrl = "";

  openDialog() {
    const dialogRef = this.dialog.open(EditComponent, {
      disableClose: true,
      data: {
        title: this.dialogTitle,
        description: this.dialogDescreption,
        initialUrl: this.dialogUrl
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.upload(result);
    });
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
 
}
