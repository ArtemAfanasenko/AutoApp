using AutoApp.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;

namespace AutoApp.Models
{
    [Table("Brand")]
    public class BrandModel : BaseViewModel
    {
        public BrandModel(string id, string name, string description, string logo, BitmapImage logoImage)
        {
            IdBrand = id;
            Name = name;
            Description = description;
            Logo = logo;
            LogoImage = logoImage;
        }

        public BrandModel()
        {

        }

        private string _idBrand;
        [Key, Column("Id_Brand")]
        public string IdBrand
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

        private string _description;
        [Column("Description")]
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
        [Column("Logo")]
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

        private BitmapImage _logoImage;
        [NotMapped]
        public BitmapImage LogoImage
        {
            get { return _logoImage; }
            set
            {
                if (_logoImage == value) return;
                _logoImage = value;
                OnPropertyChanged(nameof(LogoImage));
            }
        }
    }
}
