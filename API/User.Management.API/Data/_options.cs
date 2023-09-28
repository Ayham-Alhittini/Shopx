using Shopx.API.DTOs;
using Shopx.API.DTOs.Initialization;
using Shopx.API.DTOs.Initialization.Options;

namespace Shopx.API.Data
{
    public static class _options
    {
        public static List<Category> GetCategoires()
        {
            List<Category> categories = new List<Category>();

            Category ComputerAndLaptops = new Category
            {
                label = Categories.ComputersAndLaptops,
                link = "computers&laptops",
                logo = "https://opensooqui2.os-cdn.com/api/apiV/web/categories/ComputerAndLaptops.webp",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        label = "Laptop",
                        link = "laptop",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Laptop.png"
                    },
                    new SubCategory
                    {
                        label = "Computer",
                        link = "computer",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Computers.png"
                    }
                }
            };

            Category Mointors = new Category
            {
                label = SubCategories.Monitors,
                link = "monitors",
                logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Monitors.png",
                SubCategories = new List<SubCategory> 
                { 
                    new SubCategory
                    {
                        label = SubCategories.Monitors,
                        link = "monitors",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Monitors.png",
                    }
                }
            };

            Category Accessories = new Category
            {
                label = "Accessories",
                link = "accessories",
                logo = "https://cdn2.vectorstock.com/i/1000x1000/34/86/fashion-accessories-word-concepts-banner-vector-28883486.jpg",
                SubCategories = new List<SubCategory> 
                {
                    new SubCategory
                    {
                        label = Categories.ComputersAndLaptops,
                        link = "computers&laptops",
                        logo = "https://opensooqui2.os-cdn.com/api/apiV/web/categories/ComputerAndLaptops.webp",
                    },

                    new SubCategory
                    {
                        label = Categories.MobilesAndTablets,
                        logo = "https://opensooqui2.os-cdn.com/api/apiV/web/categories/MobileTablet.webp",
                        link = "mobile&tablets",
                    }
                }
            };


            Category Vehicles = new Category
            {
                label = Categories.Vehicles,
                link = "vehicles",
                logo = "https://opensooqui2.os-cdn.com/api/apiV/web/categories/Autos.webp",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        label =  "Bus-MiniVan",
                        link = "bus-minivan"
                    },
                    new SubCategory
                    {
                        label =  "Convertible",
                        link = "convertible"
                    },
                    new SubCategory
                    {
                        label =  "Coupe",
                        link = "coupe"
                    },
                    new SubCategory
                    {
                        label =  "Hatch-Back",
                        link = "hatch-back"
                    },
                    new SubCategory
                    {
                        label =  "Pick-Up",
                        link = "pick-up"
                    },
                    new SubCategory
                    {
                        label =  "SUV",
                        link = "suv"
                    },
                    new SubCategory
                    {
                        label =  "Sedan",
                        link = "sedan"
                    },
                    new SubCategory
                    {
                        label =  "Truck",
                        link = "truck"
                    },
                }
            };

            Category Pets = new Category
            {
                label =  Categories.Pets,
                link = "pets",
                logo = "https://opensooqui2.os-cdn.com/api/apiV/web/categories/Animals.webp",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        label =  "Cat",
                        link = "cat",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Cats.png"
                    },
                    new SubCategory
                    {
                        label =  "Dog",
                        link = "dog",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Dogs.png"
                    },
                    new SubCategory
                    {
                        label =  "Parrot",
                        link = "parrot",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Parrots.png"
                    },
                    new SubCategory
                    {
                        label =  "Animal Food",
                        link = "animal-food",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/AnimalFood.png"
                    },
                    new SubCategory
                    {
                        label =  "Pets Grooming",
                        link = "pets-grooming",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/PetsGrooming.png"
                    },
                }
            };

            Category MobileAndTablets = new Category
            {
                label = Categories.MobilesAndTablets,
                logo = "https://opensooqui2.os-cdn.com/api/apiV/web/categories/MobileTablet.webp",
                link = "mobile&tablets",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        label = "Mobile",
                        link = "mobile",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/MobilePhones.png"
                    },
                    new SubCategory
                    {
                        label = "Tablet",
                        link = "tablet",
                        logo = "https://opensooqui2.os-cdn.com/api/common_lnm/Tablets.png"
                    }
                }
            };


            categories.Add(ComputerAndLaptops);
            categories.Add(Vehicles);
            categories.Add(Pets);
            categories.Add(Accessories);
            categories.Add(MobileAndTablets);
            categories.Add(Mointors);

            return categories;
        }
        private static Category GetCategory(string categoryName)
        {
            var categories = GetCategoires();
            var category = categories.FirstOrDefault(c => c.label == categoryName);
            if (category == null)
                throw new ArgumentException("Category not exist");

            return category;
        }
        public static SubCategory GetSubCategory(string categoryName, string subcategoryName)
        {
            var category = GetCategory(categoryName);
            foreach(var sub in  category.SubCategories)
            {
                if (sub.label == subcategoryName) return sub;
            }

            throw new ArgumentException("SubCategory not exist");
        }
        public static ComputerAndLaptopOptions GetLaptopAndComputer()
        {
            var computerAndLaptop = new ComputerAndLaptopOptions
            {
                Brand = new List<string>
                {
                    "Apple",
                    "Lenovo",
                    "Acer",
                    "Asus",
                    "Dell",
                    "HP",
                    "MSI",
                },
                OperatingSystem = new List<DependentOption>(),
                ScreenSize = new List<string>
                {
                    "12 inch",
                    "13 inch",
                    "13.3 inch",
                    "14 inch",
                    "15 inch",
                    "15.6 inch",
                    "16 inch",
                    "17 inch",
                    "Above 17"
                },
            };
            var lst1 = new List<string> { "Windows", "Linux"};
            var lst2 = new List<string> { "MacOS"};

            foreach (var brand in computerAndLaptop.Brand)
            {
                var option = new DependentOption();
                option.name = brand;
                if (brand == "Apple")
                    option.list = lst2;
                else
                    option.list = lst1;

                computerAndLaptop.OperatingSystem.Add(option);
            }
            


            return computerAndLaptop;
        }
        public static VehicleOptions GetVehicles()
        {
            VehicleOptions vehicle = new VehicleOptions
            {
                CarMake = new List<string>
                {
                    "Mercedes Benz",
                    "BMW",
                    "Audi",
                    "Land Rover",
                    "Lamborghini"
                },
                Model = new List<DependentOption>
                {
                    new DependentOption
                    {
                        name = "Mercedes Benz",
                        list = new List<string>
                        {
                            "A Class",
                            "B Class",
                            "C Class",
                            "CLA Class",
                            "E Class",
                            "G Class",
                            "S Class",
                            "Other"
                        }
                    },
                    new DependentOption
                    {
                        name = "BMW",
                        list = new List<string>
                        {
                            "3 Series",
                            "4 Series",
                            "5 Series",
                            "6 Series",
                            "I Series",
                            "M Series",
                            "Other"
                        }
                    },
                    new DependentOption
                    {
                        name = "Audi",
                        list = new List<string>
                        {
                            "A Model",
                            "Q Model",
                            "R Model",
                            "Other Model"
                        }
                    },
                    new DependentOption
                    {
                        name = "Land Rover",
                        list = new List<string>
                        {
                            "Defender",
                            "Discovery",
                            "Discovery Sport",
                            "Range Rover Vogue",
                            "Other"
                        }
                    },
                    new DependentOption
                    {
                        name = "Lamborghini",
                        list = new List<string>
                        {
                            "Aventador",
                            "Huracan",
                            "Other"
                        }
                    },
                },
                Transmission = new List<string>
                {
                    "Automatic",
                    "Manual"
                },
                Fuel = new List<string>
                {
                    "Diesel",
                    "Electric",
                    "Gasoline",
                    "Hybrid",
                    "Plug-in - Hybrid",
                },
                Color = new List<string>
                {
                    "Black",
                    "White",
                    "Gold",
                    "Gray",
                    "Blue",
                    "Red",
                    "Yellow"
                },
                Condition = new List<string>
                {
                    "Used",
                    "New"
                },
                Kilometers = new List<string>
                {
                    "0",
                    "1 - 999",
                    "1,000 - 9,999",
                    "10,000 - 19,999",
                    "20,000 - 29,999",
                    "30,000 - 39,999",
                    "40,000 - 49,999",
                    "50,000 - 59,999",
                    "60,000 - 69,999",
                    "70,000 - 79,999",
                    "80,000 - 89,999",
                    "90,000 - 99,999",
                    "+100,000"
                },
                Paint = new List<string>
                {
                    "Original Paint",
                    "Partially repainted",
                    "Total repaint",
                    "Other"
                },
            };
            return vehicle;
        }
        public static PetOptoins GetPets()
        {
            PetOptoins pet = new PetOptoins
            {
                PetName = new List<string>
                {
                    "Cat",
                    "Dog",
                    "Parrot",
                    "Animal Food",
                    "Pets Grooming"
                },
                PetType = new List<DependentOption>
                {
                    new DependentOption
                    {
                        name = "Cat",
                        list = new List<string>
                        {
                            "Himalayan",
                            "Scottish",
                            "Siamese",
                            "Other",
                        }
                    },
                    new DependentOption
                    {
                        name = "Dog",
                        list = new List<string>
                        {
                            "German Shepherd",
                            "Husky",
                            "Pitbull",
                            "Chow Chow",
                            "Other",
                        }
                    }
                }
            };

            return pet;
        }
        public static MobileOptions GetMobile()
        {
            MobileOptions mobile = new MobileOptions
            {
                Brand = new List<string>
                {
                    "Apple",
                    "Samsung"
                },
                Model = new List<DependentOption>
                {
                    new DependentOption
                    {
                        name = "Apple",
                        list = new List<string>
                        {
                            "iPhone 8",
                            "iPhone 8 Plus",
                            "iPhone 11",
                            "iPhone 11 Pro",
                            "iPhone 11 Pro Max",
                            "iPhone 12",
                            "iPhone 12 Pro",
                            "iPhone 12 Pro Max",
                            "iPhone 13",
                            "iPhone 13 Pro",
                            "iPhone 13 Pro Max",
                            "iPhone 14",
                            "iPhone 14 Pro",
                            "iPhone 14 Pro Max",
                        }
                    },
                    new DependentOption
                    {
                        name = "Samsung",
                        list = new List<string>
                        {
                            "Galaxy S21 Ultra",
                            "Galaxy Note 9",
                            "Galaxy A90"
                        }
                    }
                },
                StorageSize = new List<string>
                {
                    "4 GB",
                    "8 GB",
                    "16 GB",
                    "32 GB",
                    "64 GB",
                    "128 GB",
                    "256 GB",
                    "512 GB",
                    "1 TB",
                    "2 TB",
                    "Other"
                },
                Color = new List<string>
                {
                    "Black",
                    "White",
                    "Gold",
                    "Gray",
                    "Blue",
                    "Red",
                    "Yellow"
                },
                ScreenSize = new List<string>
                {
                    "5",
                    "5.5",
                    "6",
                    "6.9",
                    "7",
                    "7.7",
                    "8",
                    "8.7",
                    "10",
                    "10.4",
                    "12",
                    "Other",
                }
            };
            return mobile;
        }
        public static MonitorOptions GetMointer()
        {
            MonitorOptions monitor = new MonitorOptions
            {
                Brand = new List<string>
                {
                    "Apple",
                    "Lenovo",
                    "Acer",
                    "Asus",
                    "Dell",
                    "HP",
                    "MSI",
                },
                ScreenSize = new List<string>
                {
                    "20 inch",
                    "21 inch",
                    "22 inch",
                    "23 inch",
                    "24 inch",
                    "25 inch",
                    "26 inch",
                    "27 inch",
                    "28 inch",
                    "29 inch",
                    "+30 inch",
                },
            };
            return monitor;
        }
        public static AccessorieOptions GetAccessorie()
        {
            var result = new AccessorieOptions
            {
                Category = new List<string>
                {
                    Categories.ComputersAndLaptops,
                    Categories.MobilesAndTablets
                },
                AccessoriesType = new List<DependentOption>
                {
                    new DependentOption
                    {
                        name = Categories.ComputersAndLaptops,
                        list = new List<string>
                        {
                            "Audio & Headsets",
                            "Keyboard",
                            "Laptop Bag",
                            "Mouse",
                            "Printer",
                            "Webcam",
                            "Components",
                            "Other"
                        }
                    },
                    new DependentOption
                    {
                        name = Categories.MobilesAndTablets,
                        list = new List<string>
                        {
                            "Charger",
                            "Cover",
                            "Spare Parts",
                            "Headphone",
                            "Smart Watch",
                            "Other"
                        }
                    }
                }
            };

            return result;
        }
        public static List<string> ReportReasons()
        {
            var result = new List<string>();

            result.Add("Inappropriate Ad");
            result.Add("Wrong category Ad");
            result.Add("Other");

            return result;
        }
        public static List<string> Cities()
        {
            var result = new List<string>
            {
                "Amman",
                "Irbid",
                "Zarqa",
                "Salt",
                "Madaba",
                "Aqaba",
                "Mafraq",
                "Jerash",
                "Al Karak",
                "Ajloun",
                "Ma'an",
                "Ramtha",
                "Tafila",
                "Dead Sea"
            };

            return result;
        }
    }
}