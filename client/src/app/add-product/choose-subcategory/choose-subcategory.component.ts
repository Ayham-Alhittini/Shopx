import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Category } from 'src/app/models/categories';
import { InitProductService } from 'src/app/services/init/init-product.service';

@Component({
  selector: 'app-choose-subcategory',
  templateUrl: './choose-subcategory.component.html',
  styleUrls: ['./choose-subcategory.component.scss']
})
export class ChooseSubcategoryComponent implements OnInit {

  linkValue: string;
  subCategories: any[];

  constructor(
    private route: ActivatedRoute,
    private initService: InitProductService
  )  {}

  ngOnInit(): void {
    this.linkValue = this.route.snapshot.paramMap.get('category');

    this.initService.getCategories().subscribe({
      next: (res) => { 
        var categories: Category[] = res;
        this.subCategories = categories.find(category => category.link === this.linkValue).subCategories;
      }
    });

  }

}
