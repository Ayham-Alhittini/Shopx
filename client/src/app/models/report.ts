export interface Report {
    id: number;
    customerId: string;
    knownAs: string;
    backgroundUrl: string;
    productId: number;
    reportReason: string;
    reportDetails: string | null;
    sendDate: Date;
    watchDate: Date | null;
}