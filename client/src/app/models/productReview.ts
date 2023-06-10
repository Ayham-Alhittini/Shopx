import { Review } from "./review";

export interface ProductReview {
    productRate: number,
    numberOfReviews: number,
    fiveStarPercentage: number,
    fiveStarCount: number,
    fourStarPercentage: number,
    fourStarCount: number,
    threeStarPercentage: number,
    threeStarCount: number,
    twoStarPercentage: number,
    twoStarCount: number,
    oneStarPercentage: number,
    oneStarCount: number,
    productReviews: Review[],
}


