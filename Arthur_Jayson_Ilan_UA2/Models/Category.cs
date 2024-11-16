using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    
    public class Category : INotifyPropertyChanged
    {
        // Champs privés
        private int _categoryID;
        private string _categoryName = string.Empty;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int CategoryID
        {
            get => _categoryID;
            set
            {
                if (_categoryID != value)
                {
                    _categoryID = value;
                    OnPropertyChanged(nameof(CategoryID));
                }
            }
        }

        [Required]
        [MaxLength(100)]
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (_categoryName != value)
                {
                    _categoryName = value;
                    OnPropertyChanged(nameof(CategoryName));
                }
            }
        }

        // Relations
        public ICollection<Book>? Books { get; }

        // Constructeurs
        public Category() { }

        public Category(int categoryID, string categoryName)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
        }


        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
