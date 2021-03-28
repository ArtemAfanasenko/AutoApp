using AutoApp.Contexts;
using AutoApp.Helpers;
using AutoApp.Models;
using AutoApp.Services;
using AutoApp.Interfaces;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.IO;
using AutoApp.Converters;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Windows;
using System.Threading.Tasks;

namespace AutoApp.ViewModels
{
    public class CarViewModel : BaseViewModel
    {
        public ObservableCollection<FuelModel> Fuels { get; set; } = new ObservableCollection<FuelModel>();
        public ObservableCollection<TypeCarModel> TypesCar { get; set; } = new ObservableCollection<TypeCarModel>();
        public ObservableCollection<BrandModel> Brands { get; set; } = new ObservableCollection<BrandModel>();
        public ObservableCollection<CarModel> Cars { get; set; } = new ObservableCollection<CarModel>();
        public ObservableCollection<string> BrandList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> FuelList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> TypeCarList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableList { get; set; } = new ObservableCollection<string>() {"Доступен", "Не доступен"};
        ApplicationContext db;
        ImageConverter image;
        BaseConverter baseConverter;

        public CarViewModel()
        {
            image = new ImageConverter();
            baseConverter = new BaseConverter();
            VisibleCarAR = true;
            VisibleCarSC = false;
            VisibleBrandAR = true;
            VisibleBrandSC = false;
            db = SimpleIoc.Default.GetInstance<ApplicationContext>();
            Fuels = new ObservableCollection<FuelModel>(db.Fuel.ToList());
            TypesCar = new ObservableCollection<TypeCarModel>(db.TypeCar.ToList());
            Brands = new ObservableCollection<BrandModel>();
            Cars = new ObservableCollection<CarModel>();

            var turpe = baseConverter.GetBrands();
            Brands = new ObservableCollection<BrandModel>(turpe.Item1);
            BrandList = new ObservableCollection<string>(turpe.Item2);
            FuelList = new ObservableCollection<string>(baseConverter.GetFuels());
            TypeCarList = new ObservableCollection<string>(baseConverter.GetTypesCar());
            Cars = new ObservableCollection<CarModel>(baseConverter.GetCars());

            AddCarCommand = new RelayCommand(async obj =>
            {
                await OnAddCar();
            });

            RemoveCarCommand = new RelayCommand(async obj =>
            {
                await OnRemoveCar();
            },
            (obj) => SelectedCar != null);

            FindCarCommand = new RelayCommand(obj =>
            {
                OnFindCar();
            });

            EditCarCommand = new RelayCommand(obj =>
            {
                OnEditCar();
            });

            SaveCarCommand = new RelayCommand(async obj =>
            {
                await OnSaveCar();
            });

            CancelCarCommand = new RelayCommand(obj =>
            {
                VisibleCarAR = true;
                VisibleCarSC = false;
                ClearCarParameters();
            });


            OpenCommand = new RelayCommand(obj =>
            {
                IOpenFile openFile = new OpenFile();
                PathImage = openFile.OpenFileDialogs();
                Logo = image.PathImageToString(PathImage);
                ImageBrand = image.StringToImage(Logo);
            });


            AddBrandCommand = new RelayCommand(async obj =>
            {
                await OnAddBrand();
            });

            RemoveBrandCommand = new RelayCommand(async obj =>
            {
                await OnRemoveBrand();
            },
            (obj) => SelectedBrand != null);

            FindBrandCommand = new RelayCommand(obj =>
            {
                OnFindBrand();
            });

            EditBrandCommand = new RelayCommand(obj =>
            {
                OnEditBrand();
            });

            SaveBrandCommand = new RelayCommand(async obj =>
            {
                await OnSaveBrand();
            });

            CancelBrandCommand = new RelayCommand(obj =>
            {
                VisibleBrandAR = true;
                VisibleBrandSC = false;
                ClearBrandParameters();
            });
        }

        public RelayCommand AddBrandCommand { get; private set; }
        public RelayCommand RemoveBrandCommand { get; private set; }
        public RelayCommand OpenCommand { get; private set; }
        public RelayCommand AddCarCommand { get; private set; }
        public RelayCommand RemoveCarCommand { get; private set; }
        public RelayCommand FindCarCommand { get; private set; }
        public RelayCommand EditCarCommand { get; private set; }
        public RelayCommand SaveCarCommand { get; private set; }
        public RelayCommand CancelCarCommand { get; private set; }
        public RelayCommand EditBrandCommand { get; private set; }
        public RelayCommand SaveBrandCommand { get; private set; }
        public RelayCommand CancelBrandCommand { get; private set; }
        public RelayCommand FindBrandCommand { get; private set; }

        #region Car
        private bool _visibleCardAR;
        public bool VisibleCarAR
        {
            get { return _visibleCardAR; }
            set
            {
                if (_visibleCardAR == value) return;
                _visibleCardAR = value;
                OnPropertyChanged(nameof(VisibleCarAR));
            }
        }

        private bool _visibleCarSC;
        public bool VisibleCarSC
        {
            get { return _visibleCarSC; }
            set
            {
                if (_visibleCarSC == value) return;
                _visibleCarSC = value;
                OnPropertyChanged(nameof(VisibleCarSC));
            }
        }

        private string _findCarText;
        public string FindCarText
        {
            get { return _findCarText; }
            set
            {
                if (_findCarText == value) return;
                _findCarText = value;
                OnPropertyChanged(nameof(FindCarText));
            }
        }

        private string _findBrandText;
        public string FindBrandText
        {
            get { return _findBrandText; }
            set
            {
                if (_findBrandText == value) return;
                _findBrandText = value;
                OnPropertyChanged(nameof(FindBrandText));
            }
        }

        private string _id;
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

        private string _selectedBrandCar;
        public string SelectedBrandCar
        {
            get { return _selectedBrandCar; }
            set
            {
                if (_selectedBrandCar == value) return;
                _selectedBrandCar = value;
                OnPropertyChanged(nameof(SelectedBrandCar));
            }
        }


        private string _nameCar;
        public string NameCar
        {
            get { return _nameCar; }
            set
            {
                if (_nameCar == value) return;
                _nameCar = value;
                OnPropertyChanged(nameof(NameCar));
            }
        }

        private string _selectedFuel;
        public string SelectedFuel
        {
            get { return _selectedFuel; }
            set
            {
                if (_selectedFuel == value) return;
                _selectedFuel = value;
                OnPropertyChanged(nameof(SelectedFuel));
            }
        }

        private decimal _costCar;
        public decimal CostCar
        {
            get { return _costCar; }
            set
            {
                if (_costCar == value) return;
                _costCar = value;
                OnPropertyChanged(nameof(CostCar));
            }
        }

        private DateTime _dateRelease;
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

        private string _selectedTypeCar;
        public string SelectedTypeCar
        {
            get { return _selectedTypeCar; }
            set
            {
                if (_selectedTypeCar == value) return;
                _selectedTypeCar = value;
                OnPropertyChanged(nameof(SelectedTypeCar));
            }
        }

        private byte _сapacityCar;
        public byte CapacityCar
        {
            get { return _сapacityCar; }
            set
            {
                if (_сapacityCar == value) return;
                _сapacityCar = value;
                OnPropertyChanged(nameof(CapacityCar));
            }
        }

        private string _selectedAvailable;
        public string SelectedAvailable
        {
            get { return _selectedAvailable; }
            set
            {
                if (_selectedAvailable == value) return;
                _selectedAvailable = value;
                OnPropertyChanged(nameof(SelectedAvailable));
            }
        }

        private CarModel _selectedCar;
        public CarModel SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                if (_selectedCar == value) return;
                _selectedCar = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Brand

        private bool _visibleBrandAR;
        public bool VisibleBrandAR
        {
            get { return _visibleBrandAR; }
            set
            {
                if (_visibleBrandAR == value) return;
                _visibleBrandAR = value;
                OnPropertyChanged(nameof(VisibleBrandAR));
            }
        }

        private bool _visibleBrandSC;
        public bool VisibleBrandSC
        {
            get { return _visibleBrandSC; }
            set
            {
                if (_visibleBrandSC == value) return;
                _visibleBrandSC = value;
                OnPropertyChanged(nameof(VisibleBrandSC));
            }
        }

        private BitmapImage _imageBrand;
        public BitmapImage ImageBrand
        {
            get { return _imageBrand; }
            set
            {
                if (_imageBrand == value) return;
                _imageBrand = value;
                OnPropertyChanged(nameof(ImageBrand));
            }
        }

        private string _pathImage;
        public string PathImage
        {
            get { return _pathImage; }
            set
            {
                if (_pathImage == value) return;
                _pathImage = value;
                OnPropertyChanged(nameof(PathImage));
            }
        }

        private int _idBrand;
        public int IdBrand
        {
            get { return _idBrand; }
            set
            {
                if (_idBrand == value) return;
                _idBrand = value;
                OnPropertyChanged(nameof(IdBrand));
            }
        }

        private string _name;
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

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _logo;
        public string Logo
        {
            get { return _logo; }
            set
            {
                if (_logo == value) return;
                _logo = value;
                OnPropertyChanged(nameof(Logo));
            } 
        }


        private BrandModel _selectedBrand;
        public BrandModel SelectedBrand
        {
            get { return _selectedBrand; }
            set
            {
                if (_selectedBrand == value) return;
                _selectedBrand = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Fuel
        private int _idFuel;
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
        #endregion

        #region TypeCar
        private int _idTypeCar;
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
        #endregion

        private void ClearCarParameters()
        {
            SelectedBrandCar = default;
            SelectedFuel = default;
            SelectedTypeCar = default;
            NameCar = default;
            CostCar = default;
            DateRelease = default;
            CapacityCar = default;
            SelectedAvailable = default;
        }

        private void ClearBrandParameters()
        {
            Name = default;
            Description = default;
            ImageBrand = default;
        }

        private async Task OnAddCar()
        {
            if (NameCar != "" && SelectedBrandCar != "" && SelectedFuel != "" && CostCar > 0 && DateRelease < DateTime.Now && SelectedTypeCar != "" && CapacityCar > 0 && SelectedAvailable != "")
            {
                var turple = baseConverter.GetParametersCar(SelectedBrandCar, SelectedFuel, SelectedTypeCar);
                var item = new CarModel(Guid.NewGuid().ToString(), turple.Item1, NameCar,
                    turple.Item2, Convert.ToInt32(CostCar), DateRelease,
                    turple.Item3, Convert.ToByte(CapacityCar), SelectedAvailable,
                    turple.Item4, turple.Item5, turple.Item6);
                db.Car.Add(item);
                await db.SaveChangesAsync();
                Cars.Add(item);
                ClearCarParameters();
            }
            else { MessageBox.Show("Неверные данные! Повторите попытку!"); }
        }

        private async Task OnRemoveCar()
        {
            db.Car.Remove(db.Car.FirstOrDefault(x => x.Id == SelectedCar.Id));
            await db.SaveChangesAsync();
            Cars.Remove(SelectedCar);
        }

        private async Task OnSaveCar()
        {
            if (NameCar != "" && SelectedBrandCar != "" && SelectedFuel != "" && CostCar > 0 && DateRelease < DateTime.Now && SelectedTypeCar != "" && CapacityCar > 0 && SelectedAvailable != "")
            {
                var itemDB = db.Car.FirstOrDefault(x => x.Id == SelectedCar.Id);
                var turple = baseConverter.GetParametersCar(SelectedBrandCar, SelectedFuel, SelectedTypeCar);
                itemDB.BrandId = turple.Item1;
                itemDB.Name = NameCar;
                itemDB.FuelId = turple.Item2;
                itemDB.Cost = CostCar;
                itemDB.DateRelease = DateRelease;
                itemDB.TypeCarId = turple.Item3;
                itemDB.Capacity = CapacityCar;
                itemDB.Available = SelectedAvailable;
                await db.SaveChangesAsync();
                var item = Cars.FirstOrDefault(x => x.Id == SelectedCar.Id);
                item.BrandId = turple.Item1;
                item.Name = NameCar;
                item.FuelId = turple.Item2;
                item.Cost = CostCar;
                item.DateRelease = DateRelease;
                item.TypeCarId = turple.Item3;
                item.Capacity = CapacityCar;
                item.Available = SelectedAvailable;
                item.BrandCar = turple.Item4;
                item.FuelCar = turple.Item5;
                item.TypeCar = turple.Item6;
                VisibleCarAR = true;
                VisibleCarSC = false;
                ClearCarParameters();
            }
            else { MessageBox.Show("Неверные данные! Повторите попытку!"); }
        }

        private void OnFindCar()
        {
            Cars.Clear();
            Cars = new ObservableCollection<CarModel>(baseConverter.GetCars());

            var count = Cars.Where(x => x.Name.Contains(_findCarText) || x.BrandCar.Contains(_findCarText) || x.FuelCar.Contains(_findCarText) ||
                x.Capacity.ToString().Contains(_findCarText) || x.DateRelease.ToString().Contains(_findCarText) || x.TypeCar.Contains(_findCarText) ||
                x.Available.Contains(_findCarText)).Count();
            if (count != 0)
            {
                Cars = new ObservableCollection<CarModel>(Cars.Where(x => x.Name.Contains(_findCarText) || x.BrandCar.Contains(_findCarText) || x.FuelCar.Contains(_findCarText) ||
                x.Capacity.ToString().Contains(_findCarText) || x.DateRelease.ToString().Contains(_findCarText) || x.TypeCar.Contains(_findCarText) ||
                x.Available.Contains(_findCarText)));
            }
            else
            {
                Cars.Clear();
            }
            OnPropertyChanged(nameof(Cars));
        }

        private void OnEditCar()
        {
            if (SelectedCar != null)
            {
                SelectedBrandCar = SelectedCar.BrandCar;
                NameCar = SelectedCar.Name;
                SelectedFuel = SelectedCar.FuelCar;
                CostCar = SelectedCar.Cost;
                DateRelease = SelectedCar.DateRelease;
                SelectedTypeCar = SelectedCar.TypeCar;
                CapacityCar = SelectedCar.Capacity;
                SelectedAvailable = SelectedCar.Available;
                VisibleCarAR = false;
                VisibleCarSC = true;
            }
            else { MessageBox.Show("Выберите элемент!"); }
        }

        private async Task OnAddBrand()
        {
            if (Name != "" && Description != "" && ImageBrand != null)
            {
                var item = new BrandModel(Guid.NewGuid().ToString(), Name, Description, Logo, image.StringToImage(Logo));
                db.Brand.Add(item);
                await db.SaveChangesAsync();
                Brands.Add(item);
                BrandList.Add(item.Name);
                ClearBrandParameters();
            }
            else { MessageBox.Show("Неверные данные! Повторите попытку!"); }
        }

        private async Task OnRemoveBrand()
        {
            db.Brand.Remove(db.Brand.FirstOrDefault(x => x.IdBrand == SelectedBrand.IdBrand));
            await db.SaveChangesAsync();
            BrandList.Remove(BrandList.FirstOrDefault(x => x == SelectedBrand.Name));
            Brands.Remove(SelectedBrand);
        }

        private void OnFindBrand()
        {
            Brands.Clear();
            Brands = new ObservableCollection<BrandModel>(baseConverter.GetBrands().Item1);

            var count = Brands.Where(x => x.Name.Contains(FindBrandText) || x.Description.Contains(FindBrandText)).Count();
            if (count != 0)
            {
                Brands = new ObservableCollection<BrandModel>(Brands.Where(x => x.Name.Contains(FindBrandText) || x.Description.Contains(FindBrandText)));
            }
            else
            {
                Brands.Clear();
            }
            OnPropertyChanged(nameof(Brands));
        }

        private void OnEditBrand()
        {
            if (SelectedBrand != null)
            {
                Name = SelectedBrand.Name;
                Description = SelectedBrand.Description;
                ImageBrand = image.StringToImage(SelectedBrand.Logo);
                VisibleBrandAR = false;
                VisibleBrandSC = true;
            }
            else { MessageBox.Show("Выберите элемент!"); }
        }

        private async Task OnSaveBrand()
        {
            if (Name != "" && Description != "" && ImageBrand != null)
            {
                var itemDB = db.Brand.FirstOrDefault(x => x.IdBrand == SelectedBrand.IdBrand);
                itemDB.Name = Name;
                itemDB.Description = Description;
                itemDB.Logo = image.ImageToString(ImageBrand);
                await db.SaveChangesAsync();
                var item = Brands.FirstOrDefault(x => x.IdBrand == SelectedBrand.IdBrand);
                item.Name = Name;
                item.Description = Description;
                item.Logo = image.ImageToString(ImageBrand);
                item.LogoImage = image.StringToImage(item.Logo);
                VisibleBrandAR = true;
                VisibleBrandSC = false;
                ClearBrandParameters();
            }
            else { MessageBox.Show("Неверные данные! Повторите попытку!"); }
        }

        
    }
}
