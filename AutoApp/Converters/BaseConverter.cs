using AutoApp.Contexts;
using AutoApp.Models;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoApp.Converters
{
    public class BaseConverter
    {
        ApplicationContext db;
        ImageConverter image;
        public List<BrandModel> Brands { get; set; } = new List<BrandModel>();
        public List<CarModel> Cars { get; set; } = new List<CarModel>();
        public List<string> BrandList { get; set; } = new List<string>();
        public List<string> FuelList { get; set; } = new List<string>();
        public List<string> TypeCarList { get; set; } = new List<string>();
        public BaseConverter()
        {
            db = SimpleIoc.Default.GetInstance<ApplicationContext>();
            image = new ImageConverter();
        }

        public string GetBrandName(string idBrandCar) => (from Brand in db.Brand.Where(x => x.IdBrand == idBrandCar)
                                                          select Brand.Name).ToList().FirstOrDefault();

        public string GetFuelName(int idFuelCar) => (from Fuel in db.Fuel.Where(x => x.IdFuel == idFuelCar)
                                                     select Fuel.TypeFuel).ToList().FirstOrDefault();

        public string GetTypeCarName(int idTypeCar) => (from TypeCar in db.TypeCar.Where(x => x.IdTypeCar == idTypeCar)
                                                        select TypeCar.TypeCar).ToList().FirstOrDefault();

        public Tuple<List<BrandModel>, List<string>> GetBrands() //Чтобы дважды не проходиться по тому же списку, использую кортеж
        {
            Brands = new List<BrandModel>();
            BrandList = new List<string>();
            foreach (var item in db.Brand.ToList())
            {
                Brands.Add(new BrandModel(item.IdBrand, item.Name, item.Description, item.Logo, image.StringToImage(item.Logo)));
                BrandList.Add(item.Name);
            }
            return Tuple.Create(Brands, BrandList);
        }

        public List<string> GetFuels()
        {
            FuelList = new List<string>();
            foreach (var item in db.Fuel.ToList())
            {
                FuelList.Add(item.TypeFuel);
            }
            return FuelList;
        }

        public List<string> GetTypesCar()
        {
            TypeCarList = new List<string>();
            foreach (var item in db.TypeCar.ToList())
            {
                TypeCarList.Add(item.TypeCar);
            }
            return TypeCarList;
        }

        public List<CarModel> GetCars()
        {
            Cars = new List<CarModel>();
            foreach (var item in db.Car.ToList())
            {
                var brandName = GetBrandName(item.BrandId);
                var fuelName = GetFuelName(item.FuelId);
                var typeCarName = GetTypeCarName(item.TypeCarId);
                Cars.Add(
                    new CarModel(item.Id, item.BrandId, item.Name, item.FuelId,
                        item.Cost, item.DateRelease, item.TypeCarId,
                        item.Capacity, item.Available, brandName,
                        fuelName, typeCarName));
            }
            return Cars;
        }

        public Tuple<string, int, int, string, string, string> GetParametersCar(string selectedBrand, string selectedFuel, string selectedTypeCar)
        {
            var brand = (from Brand in db.Brand.Where(x => x.Name == selectedBrand.ToString())
                         select Brand.IdBrand).ToList().FirstOrDefault();
            var fuel = (from Fuel in db.Fuel.Where(x => x.TypeFuel == selectedFuel.ToString())
                        select Fuel.IdFuel).ToList().FirstOrDefault();
            var typeCar = (from TypeCar in db.TypeCar.Where(x => x.TypeCar == selectedTypeCar.ToString())
                           select TypeCar.IdTypeCar).ToList().FirstOrDefault();
            var brandName = GetBrandName(brand);
            var fuelName = GetFuelName(fuel);
            var typeCarName = GetTypeCarName(typeCar);
            return Tuple.Create(brand, fuel, typeCar, brandName, fuelName, typeCarName);
        }
    }
}
