import { Product } from "./Product"

export interface orderDetail {
    id: number,
    state: string,
    dateAdded: string,
    numberOfProducts: number,
    sales: {
        id: number,
        customerId: string,
        customerUsername: string,
        sellerId: string,
        sellerName: string,
        productId: number,
        productName: string,
        price: number,
        quantity: number,
        product: Product,
        total: number,
        orderId: number
      }[],
    customerKnownAs: string,
    customerId: string,
    total: number,
    addressId: number,
    address: {
      city: string,
      postCode: number,
      address1: string,
      address2: string
    }
}