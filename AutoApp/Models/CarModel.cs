using AutoApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoApp.Models
{
    [Table("Car")]
    public class CarModel : BaseViewModel
    {
        public CarModel(string id, string brandId, string name, int fuelId, decimal cost, DateTime dateRelease, int typeCarId, byte capacity, string available, string brandCar, string fuelCar, string typeCar)
        {
            Id = id;
            BrandId = brandId;
            Name = name;
            FuelId = fuelId;
            Cost = cost;
            DateRelease = dateRelease;
            TypeCarId = typeCarId;
            Capacity = capacity;
            Available = available;
            BrandCar = brandCar;
            FuelCar = fuelCar;
            TypeCar = typeCar;
        }

        public CarModel()
        {

        }

        private string _id;
        [Key, Column("Id_Car")]
        public string Id
        {
            get { return _id; }
            set
            {
                if (Id == value) return;
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _brandId;
        [Column("Id_Brand")]
        public string BrandId
        {
            get { return _brandId; }
            set
            {
                if (_brandId == value) return;
                _brandId = value;
                OnPropertyChanged(nameof(BrandId));
            }
        }


        private string _name;
        [Column("Name"), Index(IsUnique = true)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int _fuelId;
        [Column("Id_Fuel")]
        public int FuelId
        {
            get { return _fuelId; }
            set
            {
                if (_fuelId == value) return;
                _fuelId = value;
                OnPropertyChanged(nameof(FuelId));
            }
        }

        private decimal _cost;
        [Column("Cost")]
        public decimal Cost
        {
            get { return _cost; }
            set
            {
                if (_cost == value) return;
                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }

        private DateTime _dateRelease;
        [Column("Date_Release")]
        public DateTime DateRelease
        {
            get { return _dateRelease; }
            set
            {
                if (_dateRelease == value) return;
                _dateRelease = value;
                OnPropertyChanged(nameof(DateRelease));
            }
        }

        private int _typeCarId;
        [Column("Id_Type_Car")]
        public int TypeCarId
        {
            get { return _typeCarId; }
            set
            {
                if (_typeCarId == value) return;
                _typeCarId = value;
                OnPropertyChanged(nameof(TypeCarId));
            }
        }

        private byte _сapacity;
        [Column("Сapacity")]
        public byte Capacity
        {
            get { return _сapacity; }
            set
            {
                if (_сapacity == value) return;
                _сapacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }

        private string _available;
        [Column("Available")]
        public string Available
        {
            get { return _available; }
            set
            {
                if (_available == value) return;
                _available = value;
                OnPropertyChanged(nameof(Available));
            }
        }

        private string _brandCar;
        [NotMapped]
        public string BrandCar
        {
            get { return _brandCar; }
            set
            {
                if (_brandCar == value) return;
                _brandCar = value;
                OnPropertyChanged(nameof(BrandCar));
            }
        }

        private string _fuelCar;
        [NotMapped]
        public string FuelCar
        {
            get { return _fuelCar; }
            set
            {
                if (_fuelCar == value) return;
                _fuelCar = value;
                OnPropertyChanged(nameof(FuelCar));
            }
        }

        private string _typeCar;
        [NotMapped]
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
