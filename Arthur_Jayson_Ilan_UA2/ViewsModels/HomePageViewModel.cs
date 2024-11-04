using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Models;
using System.Windows.Controls;
using Arthur_Jayson_Ilan_UA2.Services;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Arthur_Jayson_Ilan_UA2.Commands;
using Arthur_Jayson_Ilan_UA2.Views;
using System.Windows;

namespace Arthur_Jayson_Ilan_UA2.ViewsModels
{
    public class HomePageViewModel : INotifyPropertyChanged, INavigable
    {
        private User _currentUser = new User();
        private string _profileImagePath = string.Empty;
        private string _selectedTabHeader = string.Empty;
        private UserControl _selectedTabContent = new UserControl();
        private bool _isMenuExpanded = true;

        public ObservableCollection<TabItemModel> Tabs { get; set; } = new ObservableCollection<TabItemModel>();

        public string ProfileImagePath
        {
            get => _profileImagePath;
            set { _profileImagePath = value; OnPropertyChanged(); }
        }

        public string SelectedTabHeader
        {
            get => _selectedTabHeader;
            set { _selectedTabHeader = value; OnPropertyChanged(); }
        }

        public UserControl SelectedTabContent
        {
            get => _selectedTabContent;
            set { _selectedTabContent = value; OnPropertyChanged(); }
        }

        public bool IsMenuExpanded
        {
            get => _isMenuExpanded;
            set { _isMenuExpanded = value; OnPropertyChanged(); }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        public ICommand LogoutCommand { get; }
        public ICommand AddNewMemberCommand { get; }
        public ICommand ToggleMenuCommand { get; }
        public ICommand CogCommand { get; }
        public ICommand BellCommand { get; }
        public ICommand SwitchTabCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ProfileCommand { get; }

        public HomePageViewModel()
        {
            // Initialisation des commandes
            LogoutCommand = new RelayCommand(ExecuteLogout);
            AddNewMemberCommand = new RelayCommand(ExecuteAddNewMember);
            ToggleMenuCommand = new RelayCommand(ExecuteToggleMenu);
            CogCommand = new RelayCommand(ExecuteCogCommand);
            BellCommand = new RelayCommand(ExecuteBellCommand);
            SwitchTabCommand = new RelayCommand<TabItemModel?>(execute: ExecuteSwitchTab);
            MinimizeCommand = new RelayCommand<Window>(ExecuteMinimize);
            CloseCommand = new RelayCommand<Window>(ExecuteClose);
            ProfileCommand = new RelayCommand(ExecuteProfile);
        }

        private void ExecuteMinimize(Window? window)
        {
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void ExecuteClose(Window? window)
        {
            window?.Close();
        }

        // Implémentation de la méthode ExecuteProfile
        private void ExecuteProfile(object? parameter)
        {
            // Trouver l'onglet "Profil" dans la collection Tabs
            var profileTab = Tabs.FirstOrDefault(t => t.Header.Equals("Profil", StringComparison.OrdinalIgnoreCase));
            if (profileTab != null)
            {
                // Exécuter la même logique que pour SwitchTabCommand
                SwitchTabCommand.Execute(profileTab);
            }
            else
            {
                // Si l'onglet "Profil" n'existe pas, créer et ajouter un nouvel onglet
                var newProfileTab = new TabItemModel
                {
                    Header = "Profil",
                    IconKind = "Account",
                    IsSelected = true
                };
                Tabs.Add(newProfileTab);
                SwitchTabCommand.Execute(newProfileTab);
            }
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is User user)
            {
                CurrentUser = user;
                SetProfileImagePath(user.Role);
                LoadTabsBasedOnRole(user.Role);

                // Charger par défaut le premier onglet
                if (Tabs.Count > 0)
                {
                    SwitchTabCommand.Execute(Tabs[0]);
                }
            }
            else
            {
                // Si le paramètre est incorrect, revenir à la fenêtre de connexion
                NavigationService.Instance.OpenWindow<MainWindow>();
            }

        }

        private void SetProfileImagePath(UserRole role)
        {
            ProfileImagePath = role switch
            {
                UserRole.Administrator => "/Assets/Images/adminAccount.png",
                UserRole.Client => "/Assets/Images/clientAccount.png",
                UserRole.Librarian => "/Assets/Images/librarianAccount.png",
                UserRole.SuperAdmin => "/Assets/Images/superAdminAccount.png",
                _ => "/Assets/Images/clientAccount.png",
            };
        }

        private void LoadTabsBasedOnRole(UserRole role)
        {
            Tabs.Clear();

            // Définir les onglets disponibles pour chaque rôle
            var tabsForAll = new[]
            {
                new TabItemModel
                {
                    Header = "Dashboard",
                    IconKind = "ViewDashboard"
                },
                new TabItemModel
                {
                    Header = "Profil",
                    IconKind = "Account"
                },
                new TabItemModel
                {
                    Header = "Messages",
                    IconKind = "Message"
                }
            };

            switch (role)
            {
                case UserRole.Administrator:
                case UserRole.SuperAdmin:
                    Tabs.AddRange(tabsForAll);
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Gestion des Utilisateurs",
                        IconKind = "AccountMultiple"                        
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Gestion des Livres",
                        IconKind = "BookOpenPageVariant"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Prêts et Retours",
                        IconKind = "BookCheck"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Réservations",
                        IconKind = "BookClock"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Rapports",
                        IconKind = "ChartBox"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Support",
                        IconKind = "HelpCircle"
                    });
                    break;

                case UserRole.Librarian:
                    Tabs.AddRange(tabsForAll);
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Gestion des Livres",
                        IconKind = "BookOpenPageVariant"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Prêts et Retours",
                        IconKind = "BookCheck"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Rapports",
                        IconKind = "ChartBox"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Support",
                        IconKind = "HelpCircle"
                    });
                    break;

                case UserRole.Client:
                    Tabs.AddRange(tabsForAll);
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Prêts et Retours",
                        IconKind = "BookCheck"
                    });
                    Tabs.Add(new TabItemModel
                    {
                        Header = "Réservations",
                        IconKind = "BookClock"
                    });
                    break;

                default:
                    Tabs.AddRange(tabsForAll);
                    break;
            }
        }

        private void ExecuteSwitchTab(TabItemModel? tab)
        {
            if (tab == null) return;

            foreach (var t in Tabs)
            {
                t.IsSelected = false;
            }

            tab.IsSelected = true;
            SelectedTabHeader = tab.Header;

            // Lazy Loading : Charger le UserControl seulement lorsqu'il est sélectionné
            //SelectedTabContent = tab.Header.ToLower() switch
            //{
            //    "dashboard" => NavigationService.Instance.NavigateTo(new DashboardView()),
            //    "profil" => new ProfileView(),// À implémenter
            //    "gestion des utilisateurs" => new UserManagementView(),// À implémenter
            //    "gestion des livres" => new BookManagementView(),// À implémenter
            //    "prêts et retours" => new LoanReturnView(),// À implémenter
            //    "réservations" => new ReservationsView(),// À implémenter
            //    "rapports" => new ReportsView(),// À implémenter
            //    "support" => new SupportView(),// À implémenter
            //    "messages" => new MessagesView(),// À implémenter
            //    _ => new DashboardView(),
            //};

            switch (tab.Header.ToLower())
            {
                case "dashboard":
                    NavigationService.Instance.NavigateTo(new DashboardView());
                    break;
                case "profil":
                    NavigationService.Instance.NavigateTo(new ProfileView());
                    break;
                case "gestion des utilisateurs":
                    NavigationService.Instance.NavigateTo(new UserManagementView());
                    break;
                case "gestion des livres":
                    NavigationService.Instance.NavigateTo(new BookManagementView());
                    break;
                case "prêts et retours":
                    NavigationService.Instance.NavigateTo(new LoanReturnView());
                    break;
                case "réservations":
                    NavigationService.Instance.NavigateTo(new ReservationsView());
                    break;
                case "rapports":
                    NavigationService.Instance.NavigateTo(new ReportsView());
                    break;
                case "support":
                    NavigationService.Instance.NavigateTo(new SupportView());
                    break;
                case "messages":
                    NavigationService.Instance.NavigateTo(new MessagesView());
                    break;
                default:
                    NavigationService.Instance.NavigateTo(new DashboardView());
                    break;
            }
        }

        private void ExecuteLogout(object? parameter)
        {
            // Logique de déconnexion si nécessaire
            // Naviguer vers la fenêtre de connexion
            NavigationService.Instance.OpenWindow<MainWindow>();
        }

        private void ExecuteAddNewMember(object? parameter)
        {
            // Même si le bouton ne sert pas à ajouter des membres, il peut naviguer vers une autre vue
            // Exemple : Naviguer vers une vue d'ajout d'entité
            NavigationService.Instance.NavigateTo(new AddEntityView()); // À implémenter
        }

        private void ExecuteToggleMenu(object? parameter)
        {
            IsMenuExpanded = !IsMenuExpanded;
        }

        private void ExecuteCogCommand(object? parameter)
        {
            // Mettre à jour le titre et le contenu pour les Notifications
            SelectedTabHeader = "Paramètres";

            // Désélectionner tous les onglets
            foreach (var t in Tabs)
            {
                t.IsSelected = false;
            }

            // Naviguer vers les paramètres via un UserControl ou une fenêtre
            NavigationService.Instance.NavigateTo(new SettingsView()); // À implémenter
        }

        private void ExecuteBellCommand(object? parameter)
        {
            // Mettre à jour le titre et le contenu pour les Notifications
            SelectedTabHeader = "Notifications";

            // Désélectionner tous les onglets
            foreach (var t in Tabs)
            {
                t.IsSelected = false;
            }

            // Naviguer vers les messages ou notifications via un UserControl
            NavigationService.Instance.NavigateTo(new NotificationsView()); // À implémenter
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
