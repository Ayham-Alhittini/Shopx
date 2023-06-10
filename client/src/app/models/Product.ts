import { ProductPhoto } from "./ProductPhoto";
import { ProductReview } from "./productReview";
import { Accessories } from "./Products Specifcation/Accessories";
import { ComputerAndLaptop } from "./Products Specifcation/computer-and-laptop";
import { Mobile } from "./Products Specifcation/Mobile";
import { MonitorProduct } from "./Products Specifcation/MonitorProduct";
import { Pet } from "./Products Specifcation/Pet";
import { Vehicle } from "./Products Specifcation/Vehicle";

export interface Product {
    id: number;
    productName: string;
    category: string;
    subCategory: string;
    link: string;
    price: number;
    onWishlist: boolean;
    onCart: boolean;
    quantity: number;
    sellerId: string;
    sellerName: string;
    productDescription: string;
    productPhotos: ProductPhoto[];
    state: string;
    created: string;
    computerAndLaptop: ComputerAndLaptop;
    vehicle: Vehicle;
    pet: Pet;
    mobile: Mobile;
    accessories: Accessories;
    monitorProduct: MonitorProduct;
    specification: string;
    productReview: ProductReview;
    reported: boolean;
    discountRate: number;
    priceAfterDiscount: number;
}