import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AccountService } from 'src/app/account/account.service';
import { ToastrService } from 'ngx-toastr';
import { IAdress } from 'src/app/shared/models/adress.interface';

@Component({
  selector: 'app-checkout-adress',
  templateUrl: './checkout-adress.component.html',
  styleUrls: ['./checkout-adress.component.scss'],
})
export class CheckoutAdressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  saveUserAdress() {
    this.accountService
      .updateUserAdress(this.checkoutForm.get('adressForm').value)
      .subscribe(
        (adress: IAdress) => {
          this.toastr.success('Adress saved');
          this.checkoutForm.get('adressForm').reset(adress);
        },
        (error) => {
          this.toastr.error(error.message);
        }
      );
  }
}
