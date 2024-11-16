using System.Collections.ObjectModel;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels.HomePageViewModels
{
    public class UsageStatisticViewModel
    {
        public ObservableCollection<Statistic> UsageStatistics { get; set; }

        public UsageStatisticViewModel()
        {
            // Sample Data for Statistics
            UsageStatistics = new ObservableCollection<Statistic>
            {
                new Statistic { StatName = "Utilisateurs Actifs", StatValue = 120 },
                new Statistic { StatName = "Sessions", StatValue = 350 },
                new Statistic { StatName = "Durée Moyenne", StatValue = 25 },
                new Statistic { StatName = "Feedbacks", StatValue = 40 },
                new Statistic { StatName = "Erreurs", StatValue = 5 }
            };
        }
    }

    public class Statistic
    {
        public string StatName { get; set; } = string.Empty;
        public double StatValue { get; set; }
    }
}
