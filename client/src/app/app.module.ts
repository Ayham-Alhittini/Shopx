import { ClassProvider, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { NavComponent } from './nav/nav.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { HomeComponent } from './home/home.component';
import { SignupComponent } from './signup/signup.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { SearchBarComponent } from './nav/search-bar/search-bar/search-bar.component';
import { MatMenuModule } from '@angular/material/menu';
import { ProductSectionComponent } from './home/product-section/product-section/product-section.component';
import { ProductImageComponent } from './home/product-image/product-image/product-image.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { FloatingSearchBarComponent } from './nav/search-bar/floating-search-bar/floating-search-bar.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ResendConfirmComponent } from './resend-confirm/resend-confirm.component';
import { MatCardModule } from '@angular/material/card';
import { SellerSignupComponent } from './seller-signup/seller-signup.component';
import { SellerProfileComponent } from './seller-profile/seller-profile.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProfileComponent } from './profile/profile.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { EmailConfirmedComponent } from './email-confirmed/email-confirmed.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './profile/change-password/change-password.component';
import { SellerChangePasswordComponent } from './seller-profile/seller-change-password/seller-change-password.component';
import { EditComponent } from './edit/edit.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ProductsComponent } from './products/products.component';
import { ProductCardComponent } from './products/product-card/product-card.component';
import { JwtInterceptor } from './interceptor/jwt.interceptor';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AddProductComponent } from './add-product/add-product.component';
import { MatStepperModule } from '@angular/material/stepper';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SafePipe } from './pipes/safe.pipe';
import { ChooseSubcategoryComponent } from './add-product/choose-subcategory/choose-subcategory.component';
import { AddFormComponent } from './add-product/add-form/add-form.component';
import { DynamicFormComponent } from './dynamic-form/dynamic-form.component';
import { DynamicFieldComponent } from './dynamic-form/dynamic-field/dynamic-field.component';
import { DynamicCheckboxComponent } from './dynamic-form/dynamic-field/dynamic-checkbox/dynamic-checkbox.component';
import { DynamicRadioComponent } from './dynamic-form/dynamic-field/dynamic-radio/dynamic-radio.component';
import { DynamicSelectComponent } from './dynamic-form/dynamic-field/dynamic-select/dynamic-select.component';
import { DynamicInputComponent } from './dynamic-form/dynamic-field/dynamic-input/dynamic-input.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductViewComponent } from './product-view/product-view.component';
import { MatRippleModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { ReviewComponent } from './product-view/review/review.component';
import { SellerEditProfileComponent } from './seller-profile/seller-edit-profile/seller-edit-profile.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ReviewCustomerComponent } from './product-view-customer/review-customer/review-customer.component';
import { ProductViewCustomerComponent } from './product-view-customer/product-view-customer.component';
import { EditPhoneNumberComponent } from './profile/edit-phone-number/edit-phone-number.component';
import { SearchProductsComponent } from './search-products/search-products.component';
import { FilterComponent } from './search-products/filter/filter.component';
import { SearchedProductCardComponent } from './search-products/searched-product-card/searched-product-card.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { FavoriteProductComponent } from './favorites/favorite-product/favorite-product.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { CartProductComponent } from './shopping-cart/cart-product/cart-product.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { MatTabsModule } from '@angular/material/tabs';
import { OrdersComponent } from './orders/orders.component';
import { MatTableModule } from '@angular/material/table';
import { OrderDetailComponent } from './orders/order-detail/order-detail.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NgxStarRatingModule } from 'ngx-star-rating';
import { NotificationViewComponent } from './notification-view/notification-view.component';
import { SellerRequestComponent } from './admin/shop-requests/seller-request/seller-request.component';
import { ShopRequestsComponent } from './admin/shop-requests/shop-requests.component';
import { ReportsComponent } from './admin/reports/reports.component';
import { ManageProductsComponent } from './admin/manage-products/manage-products.component';
import { ManageUsersComponent } from './admin/manage-users/manage-users.component';
import { ChangePasswordDialogComponent } from './admin/change-password-dialog/change-password-dialog.component';
import { ReportDialogComponent } from './product-view-customer/report-dialog/report-dialog.component';
import { BrowseHistoryComponent } from './browse-history/browse-history.component';
import { HistoryProductComponent } from './browse-history/history-product/history-product.component';
import { DiscountDialogComponent } from './product-view/discount-dialog/discount-dialog.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ProductStaticsComponent } from './product-statics/product-statics.component';


const JWT_INTERCEPTOR_PROVIDER: ClassProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: JwtInterceptor,
  multi: true
};

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavComponent,
    HomeComponent,
    SignupComponent,
    SearchBarComponent,
    ProductSectionComponent,
    ProductImageComponent,
    FloatingSearchBarComponent,
    ResendConfirmComponent,
    SellerSignupComponent,
    SellerProfileComponent,
    ProfileComponent,
    DashboardComponent,
    ResetPasswordComponent,
    EmailConfirmedComponent,
    ForgotPasswordComponent,
    ChangePasswordComponent,
    SellerChangePasswordComponent,
    EditComponent,
    ProductsComponent,
    ProductCardComponent,
    AddProductComponent,
    SafePipe,
    ChooseSubcategoryComponent,
    AddFormComponent,
    DynamicFormComponent,
    DynamicFieldComponent,
    DynamicCheckboxComponent,
    DynamicRadioComponent,
    DynamicSelectComponent,
    DynamicInputComponent,
    ProductEditComponent,
    ProductViewComponent,
    ReviewComponent,
    SellerEditProfileComponent,
    ReviewCustomerComponent,
    ProductViewCustomerComponent,
    EditPhoneNumberComponent,
    SearchProductsComponent,
    FilterComponent,
    SearchedProductCardComponent,
    FavoritesComponent,
    FavoriteProductComponent,
    ShoppingCartComponent,
    CartProductComponent,
    CheckoutComponent,
    OrdersComponent,
    OrderDetailComponent,
    NotificationViewComponent,
    SellerRequestComponent,
    ShopRequestsComponent,
    ReportsComponent,
    ManageProductsComponent,
    ManageUsersComponent,
    ChangePasswordDialogComponent,
    ReportDialogComponent,
    BrowseHistoryComponent,
    HistoryProductComponent,
    DiscountDialogComponent,
    NotFoundComponent,
    ProductStaticsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    FormsModule,
    MatSidenavModule,
    MatButtonModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule,
    MatAutocompleteModule,
    MatMenuModule,
    OverlayModule,
    HttpClientModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatExpansionModule,
    MatTooltipModule,
    MatDialogModule,
    MatPaginatorModule,
    MatStepperModule,
    MatRadioModule,
    MatSelectModule,
    MatCheckboxModule,
    MatRippleModule,
    MatDividerModule,
    MatSnackBarModule,
    MatTabsModule,
    MatTableModule,
    MatProgressBarModule,
    NgxStarRatingModule
  ],
  providers: [ JWT_INTERCEPTOR_PROVIDER ],
  bootstrap: [AppComponent]
})
export class AppModule { }
