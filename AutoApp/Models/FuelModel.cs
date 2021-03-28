using AutoApp.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoApp.Models
{
    [Table("Fuel")]
    public class FuelModel : BaseViewModel
    {
        public FuelModel(int id, string type)
        {
            IdFuel = id;
            TypeFuel = type;
        }

        public FuelModel( string type)
        {
            TypeFuel = type;
        }

        public FuelModel() { 
        }


        private int _idFuel;
        [Key,Column("Id_Fuel")]
        public int IdFuel
        {
            get { return _idFuel; }
            set
            {
                if (_idFuel == value) return;
                _idFuel = value;
                OnPropertyChanged(nameof(IdFuel));
            }
        }

        private string _typeFuel;
        [Column("Type_Fuel"), Index(IsUnique = true)]
        public string TypeFuel
        {
            get { return _typeFuel; }
            set
            {
                if (_typeFuel == value) return;
                _typeFuel = value;
                OnPropertyChanged(nameof(TypeFuel));
            }
        }

    }
}
