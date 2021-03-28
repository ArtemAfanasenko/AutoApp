using AutoApp.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoApp.Models
{
    [Table("TypeCar")]
    public class TypeCarModel : BaseViewModel
    {
        public TypeCarModel(int id, string type)
        {
            IdTypeCar = id;
            TypeCar = type;
        }

        public TypeCarModel(string type)
        {
            TypeCar = type;
        }

        public TypeCarModel()
        {

        }

        private int _idTypeCar;
        [Key, Column("Id_Type_Car")]
        public int IdTypeCar
        {
            get { return _idTypeCar; }
            set
            {
                if (_idTypeCar == value) return;
                _idTypeCar = value;
                OnPropertyChanged(nameof(IdTypeCar));
            }
        }

        private string _typeCar;
        [Column("Type"), Index(IsUnique = true)]
        public string TypeCar
        {
            get { return _typeCar; }
            set
            {
                if (_typeCar == value) return;
                _typeCar = value;
                OnPropertyChanged(nameof(TypeCar));
            }
        }
    }
}
