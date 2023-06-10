import { ProductPhoto } from "./ProductPhoto";

export interface Review {
    id: number,
    ratingValue: number,
    reviewDate: string,
    reviewContent: string,
    initial: number,
    voteRating: number,
    customer: {
        id: string,
        knownAs: string,
        isOnline: boolean,
        created: string,
        backgroundPhoto: ProductPhoto,
    }
}