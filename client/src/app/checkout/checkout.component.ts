import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OrderService } from '../services/order/order.service';
import { InitProductService } from '../services/init/init-product.service';
import { CartService } from '../services/customer-product/cart.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private _snackbar: MatSnackBar,
    private orderService: OrderService,
    private initService: InitProductService,
    private cartService: CartService
  ) {}
  
  address: FormGroup;
  card: FormGroup;
  deliveryOption: FormGroup;
  selected: number = 0;
  isCheckedout: boolean = false;
  disable: boolean = false;

  cityOptions: string[] = [];
  deliveryOptions: string[] = ['Quick', 'Medium', 'Slow', 'Free'];

  ngOnInit(): void {
    this.initService.getCities().subscribe((res) => {
      this.cityOptions = res;
      
    });

    
    
    this.address = this.fb.group({
      city: [this.cityOptions, { validators: [Validators.required] }],
      postCode: ['', { validators: [Validators.required, Validators.pattern(".{5,5}")]}],
      address1: ['', { validators: [Validators.required] }],
      address2: [''],
    });
    
    this.card = this.fb.group({
      cardNumber: ['', { validators: [Validators.required] }],
      expirationYear: ['', { validators: [Validators.required] }],
      expirationMonth: ['', { validators: [Validators.required] }],
      cvc: ['', { validators: [Validators.required, Validators.pattern(".{3,3}")] }],
    });

    this.deliveryOption = this.fb.group({
      deliveryOption: ['', { validators: [Validators.required] }]
    });

  }

  onIndexChange(index: number) {
    this.selected = index;
  }

  checkout() {

    
    this.disable = true;
    
    const model = {
      address: {
        postCode: this.address.get('postCode').value,
        city: this.address.get('city').value,
        address1: this.address.get('address1').value,
        address2: this.address.get('address2').value,
      },
      card: {
        cardNumber: this.card.get('cardNumber').value,
        expirationYear: this.card.get('expirationYear').value + '',
        expirationMonth: this.card.get('expirationMonth').value + '',
        cvc: this.card.get('cvc').value + '',
      },
      deliveryOption: this.deliveryOption.get('deliveryOption').value
    }

    //// call API and inside set isCheckedout
    this.orderService.checkout(model).subscribe(() => {
      this.isCheckedout = true
      this.cartService.cartCount = 0;
    });
    
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
}

