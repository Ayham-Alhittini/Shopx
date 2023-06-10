import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { ResendConfirmComponent } from './resend-confirm/resend-confirm.component';
import { SellerSignupComponent } from './seller-signup/seller-signup.component';
import { SellerProfileComponent } from './seller-profile/seller-profile.component';
import { ProfileComponent } from './profile/profile.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { EmailConfirmedComponent } from './email-confirmed/email-confirmed.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ProductsComponent }  from './products/products.component';
import { AddProductComponent } from './add-product/add-product.component';
import { ChooseSubcategoryComponent } from './add-product/choose-subcategory/choose-subcategory.component';
import { AddFormComponent } from './add-product/add-form/add-form.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductViewComponent } from './product-view/product-view.component';
import { AccountTypeGuard } from './guards/account-type/account-type.guard';
import { ProductViewCustomerComponent } from './product-view-customer/product-view-customer.component';
import { SearchProductsComponent } from './search-products/search-products.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { OrdersComponent } from './orders/orders.component';
import { OrderDetailComponent } from './orders/order-detail/order-detail.component';
import { NotificationViewComponent } from './notification-view/notification-view.component';
import { ShopRequestsComponent } from './admin/shop-requests/shop-requests.component';
import { ManageUsersComponent } from './admin/manage-users/manage-users.component';
import { ManageProductsComponent } from './admin/manage-products/manage-products.component';
import { ReportsComponent } from './admin/reports/reports.component';
import { BrowseHistoryComponent } from './browse-history/browse-history.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ProductStaticsComponent } from './product-statics/product-statics.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'resend-confirm', component: ResendConfirmComponent },
  { path: 'email-confirmed', component: EmailConfirmedComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'home', component: HomeComponent},
  { path: 'home/product/:id', component: ProductViewCustomerComponent},
  { path: 'home/:category', component: SearchProductsComponent},
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Customer" }},
  { path: 'favorites', component: FavoritesComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Customer" }},
  { path: 'browse-history', component: BrowseHistoryComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Customer" }},
  { path: 'shopping-cart', component: ShoppingCartComponent},
  { path: 'checkout', component: CheckoutComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Customer" }},
  { path: 'notifications/:id', component: NotificationViewComponent, canActivate: [AuthGuard]},
  { path: 'orders', component: OrdersComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Customer" }},
  { path: 'orders/:id', component: OrderDetailComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Customer" }},
  { path: 'seller-signup', component: SellerSignupComponent },
  { path: 'seller-profile', component: SellerProfileComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  { path: 'seller-products', component: ProductsComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  { path: 'seller-products/:id', component: ProductViewComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  { path: 'add-product', component: AddProductComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  { path: 'add-product/:category', component: ChooseSubcategoryComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  { path: 'add-product/:category/:subcategory', component: AddFormComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  { path: 'edit/:id', component: ProductEditComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  { path: 'products-statics', component: ProductStaticsComponent, canActivate: [AuthGuard, AccountTypeGuard], data: { accountType: "Seller" } },
  {path : 'admin', 
    runGuardsAndResolvers : 'always',
    canActivate : [AuthGuard, AccountTypeGuard],
    data: { accountType: "Admin" },
    children : [
      {path : '', component : ShopRequestsComponent, pathMatch: 'full'},
      {path : 'shops-requests', component : ShopRequestsComponent},
      {path : 'manage-users', component : ManageUsersComponent},
      {path : 'manage-products', component : ManageProductsComponent},
      {path : 'reports/:productId', component : ReportsComponent},
    ]
  },
  { path: 'not-found', component: NotFoundComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path : '**', redirectTo : 'not-found'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
