using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public enum ReportType
    {
        [Display(Name = "User Activity")]
        UserActivity,
        [Display(Name = "Usage Stats")]
        UsageStats,
        [Display(Name = "Error Log")]
        ErrorLog,
        [Display(Name = "Custom")]
        Custom
    }

    [Table("report")]
    public class Report : INotifyPropertyChanged
    {

        // Champs privés
        private int _reportID;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private int? _generatedBy;
        private DateTime _generatedAt;
        private string _reportPath = string.Empty;
        private ReportType _reportType;


        // Propriétés publiques avec notifications de changement

        [Key]
        public int ReportID
        {
            get => _reportID;
            set
            {
                if (_reportID != value)
                {
                    _reportID = value;
                    OnPropertyChanged(nameof(ReportID));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        [ForeignKey("GeneratedByUser")]
        public int? GeneratedBy
        {
            get => _generatedBy;
            set
            {
                if (_generatedBy != value)
                {
                    _generatedBy = value;
                    OnPropertyChanged(nameof(GeneratedBy));
                }
            }
        }

        public User? GeneratedByUser { get; }

        [Required]
        public DateTime GeneratedAt
        {
            get => _generatedAt;
            set
            {
                if (_generatedAt != value)
                {
                    _generatedAt = value;
                    OnPropertyChanged(nameof(GeneratedAt));
                }
            }
        }

        public string ReportPath
        {
            get => _reportPath;
            set
            {
                if (_reportPath != value)
                {
                    _reportPath = value;
                    OnPropertyChanged(nameof(ReportPath));
                }
            }
        }

        [Required]
        public ReportType ReportType
        {
            get => _reportType;
            set
            {
                if (_reportType != value)
                {
                    _reportType = value;
                    OnPropertyChanged(nameof(ReportType));
                }
            }
        }

        // Relations
        public ICollection<ReportParameter>? ReportParameters { get; }

        // Constructeurs
        public Report() { }

        public Report(int reportID, string title, string description, int? generatedBy, ReportType reportType, DateTime? generatedAt = null)
        {
            ReportID = reportID;
            Title = title;
            Description = description;
            GeneratedBy = generatedBy;
            ReportType = reportType;
            GeneratedAt = generatedAt ?? DateTime.Now;
        }

        // Méthodes pour générer et sauvegarder le rapport
        public void GenerateReport()
        {
            // Implémentez la logique de génération de rapport ici
            // Par exemple, créer un fichier PDF, Excel, etc., et définir ReportPath
            ReportPath = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
