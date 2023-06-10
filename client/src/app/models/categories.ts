export interface Category {
    label: string,
    link: string,
    logo: string,
    subCategories: [
        {
            label: string,
            link: string,
            logo: string
        }
    ]
}