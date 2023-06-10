export interface User {
    id: string;
    email: string;
    username: string;//customer => email
    token: string;
    photoUrl: string;
    knownAs: string;
    accountState: string;
    accountType: string;
    backgroundPhotoUrl: string
    lastActive:Date;
    created: Date;
}
