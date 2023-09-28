using Shopx.API.Data;
using Shopx.API.DTOs.Initialization.Options;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs.Create_Products
{
    public class CreatePetDto: CreateProductDto
    {
        private PetOptoins initial = _options.GetPets();
        private string _petName;
        private string _petType;

        [Required]
        public string PetName
        {
            get { return _petName; }
            set 
            {
                if (!initial.PetName.Contains(value))
                    throw new ArgumentException("Pet Name not exist");
                _petName = value;
            }
        }

        public string PetType
        {
            get { return _petType; }
            set 
            {
                ///check if this pet have a types
                var petTypes = initial.PetType.Where(p => p.name == _petName).Select(p => p.list).FirstOrDefault();
                if (petTypes != null)
                {
                    if (!petTypes.Contains(value))
                        throw new ArgumentException("Pet Type not exist");
                    _petType = value;
                }
            }
        }
    }
}
