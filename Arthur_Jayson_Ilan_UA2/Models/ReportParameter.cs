using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class ReportParameter : INotifyPropertyChanged
    {

        // Champs privés
        private int _parameterID;
        private int? _reportID;
        private string _parameterName = string.Empty;
        private string _parameterValue = string.Empty;

        // Propriétés publiques avec notifications de changement

        [Key]
        public int ParameterID
        {
            get => _parameterID;
            set
            {
                if (_parameterID != value)
                {
                    _parameterID = value;
                    OnPropertyChanged(nameof(ParameterID));
                }
            }
        }

        [ForeignKey("Report")]
        public int? ReportID
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

        public Report? Report { get; }

        [Required]
        [MaxLength(100)]
        public string ParameterName
        {
            get => _parameterName;
            set
            {
                if (_parameterName != value)
                {
                    _parameterName = value;
                    OnPropertyChanged(nameof(ParameterName));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string ParameterValue
        {
            get => _parameterValue;
            set
            {
                if (_parameterValue != value)
                {
                    _parameterValue = value;
                    OnPropertyChanged(nameof(ParameterValue));
                }
            }
        }

        // Constructeurs
        public ReportParameter() { }

        public ReportParameter(int parameterID, int? reportID, string parameterName, string parameterValue)
        {
            ParameterID = parameterID;
            ReportID = reportID;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        // Implémentation de INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
