import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable, map } from 'rxjs';
import { ProductService } from 'src/app/services/product/product.service';
import { BreakpointObserver } from '@angular/cdk/layout';
import { Category } from 'src/app/models/categories';
import { InitProductService } from 'src/app/services/init/init-product.service';
import { MatDialog } from '@angular/material/dialog';
import { EditComponent } from 'src/app/edit/edit.component';
import { SellerPhotosService } from 'src/app/services/seller-photos/seller-photos.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-form',
  templateUrl: './add-form.component.html',
  styleUrls: ['./add-form.component.scss']
})
export class AddFormComponent implements OnInit {

  editable: boolean = true;
  stepperOrientation: Observable<"horizontal" | "vertical">;
  productLoaded = false;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private productService: ProductService,
    private initService: InitProductService,
    private dialog: MatDialog,
    private sellerPhotosService: SellerPhotosService,
    private _snackbar: MatSnackBar
  ) {
    this.stepperOrientation = breakpointObserver
      .observe('(min-width: 800px)')
      .pipe(map(({matches}) => (matches ? 'horizontal' : 'vertical')));
  }

  addProductForm: FormGroup;
  addProductSpecForm: FormGroup;

  categoryLink: string;
  category: Category;
  subCategoryLink: string;
  subCategory: any;

  model = {};
  productId: number;

  ngOnInit(): void {
    this.categoryLink = this.route.snapshot.paramMap.get('category');
    this.subCategoryLink = this.route.snapshot.paramMap.get('subcategory');

    this.initService.getCategories().subscribe({
      next: (res) => { 
        var categories: Category[] = res;
        this.category = categories.find(category => category.link === this.categoryLink);
        this.subCategory = this.category.subCategories.find(subCategory => subCategory.link === this.subCategoryLink);

        this.initService.getModel(this.categoryLink).subscribe((formModel) => {
          this.checkIfPets(formModel);
          this.model = { ...formModel }; 
        });
      }
    });

    this.addProductForm = this.fb.group({
      productName: ['', { validators: [ Validators.required, ] }],
      price: ['', { validators: [ Validators.required, Validators.min(0) ] }],
      quantity: ['', { validators: [ Validators.required, , Validators.min(0) ] }],
      productDescription: ['', { validators: [ Validators.required, ] }],
    })

    this.addProductSpecForm = this.fb.group({});

  }

  setForm(form: FormGroup) {
    this.addProductSpecForm = form;
    
  }

  submit() {
    this.editable = !this.editable; /// lock form after adding product

    const model = {
      type: this.subCategory?.label , ...this.addProductForm.getRawValue(), ...this.addProductSpecForm.getRawValue()
    }

    ///////// api call 
    this.productService.addProduct(this.categoryLink, model).subscribe({
      next: (product) => {
        this.productId = product.id;
        this.productLoaded = true;
        this.openSnackbar("product added successfully", "ok");
      },
      error: () => this.openSnackbar("something went wrong please try again", "ok")
    });
  }

  photoList: { url: string, id: number}[] = [];

  upload(result) {
    const id = this.productId;
    const file = result.file;
  
    if(this.photoList.length < 5 && result) {

      const formData = new FormData();
      formData.append('file', file);

      this.sellerPhotosService.uploadProductPhoto(id, formData).subscribe({
        next: (photo) => { /// { url: "", id: ""}
          this.photoList.push({ url: result.url, id: -1 });
          const photoIndex = this.photoList.findIndex(photo => photo.id === -1);
          if(photoIndex !== -1)
            this.photoList.splice(photoIndex, 1, { url: result.url, id: photo.id });

          this.openSnackbar("photo added successfully", "ok");
        }
      });
    }
  }

  deleteImage(id: number) {
    const index = this.photoList.findIndex(file => file.id === id);

    if(id === -1){
      this.photoList.splice(index, 1);
      this.openSnackbar("photo was deleted", "ok");
      return;
    }

    this.sellerPhotosService.deleteProductPhoto(this.productId, id).subscribe({
      next: () => { 
        this.photoList.splice(index, 1);
        this.openSnackbar("photo was deleted", "ok");
      },
      error: () => this.openSnackbar("something wrong happend will trying to delete the photo, please try again", "ok")
    });
  }

  dialogTitle = "Add an Image";
  dialogDescreption = "This image will be one of the images that are avalible for customers to see when they view this product.";
  dialogUrl = "";

  openDialog() {
    if(!this.productLoaded) {
      this.openSnackbar("product is still being saved, please wait.", "ok");
      return;
    }

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

  checkIfPets(formModel) {
    if(formModel["petName"]) {
      formModel["petName"].value = this.subCategory.label;
      formModel["petName"].rules = [ ...formModel["petName"].rules, "disabled" ];
    }
    if(formModel["category"]) {
      formModel["category"].value = this.subCategory.label;
      formModel["category"].rules = [ ...formModel["category"].rules, "disabled" ];
    }
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
 
}
