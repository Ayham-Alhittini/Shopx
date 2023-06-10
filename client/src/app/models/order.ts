export interface Order {
    id: number;
    state: string;
    dateAdded: string;
    numberOfProducts: number;
    sales: [];
    customerKnownAs: string;
    customerId: string;
    total: number;
    addressId: number;
    address: string;
}