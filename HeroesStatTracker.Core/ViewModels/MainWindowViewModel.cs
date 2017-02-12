using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMatchSummaryFlyoutService, IMainTabsService
    {
        private bool _matchSummaryIsOpen;
        private int _selectedMainTab;
        private string _matchSummaryHeader;

        private IDatabaseService Database;
        private IUserProfileService UserProfile;

        public MainWindowViewModel(IDatabaseService database, IUserProfileService userProfile)
        {
            MatchSummaryIsOpen = false;
            MatchSummaryHeader = "Match Summary";

            Database = database;
            UserProfile = userProfile;

            SimpleIoc.Default.Register<IMatchSummaryFlyoutService>(() => this);
            SimpleIoc.Default.Register<IMainTabsService>(() => this);
            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));
        }

        public IDatabaseService GetDatabaseService { get { return Database; } }

        public string AppVersion { get { return AssemblyVersions.HeroesStatTrackerVersion().ToString(); } }

        public bool MatchSummaryIsOpen
        {
            get { return _matchSummaryIsOpen; }
            set
            {
                _matchSummaryIsOpen = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenWhatsNewWindowCommand => new RelayCommand(OpenWhatsNewWindow);
        public RelayCommand OpenProfileCommand => new RelayCommand(OpenProfile);

        public IWhatsNewWindowService WhatsNewWindow
        {
            get { return ServiceLocator.Current.GetInstance<IWhatsNewWindowService>(); }
        }

        public IProfileWindowService ProfileWindow
        {
            get { return ServiceLocator.Current.GetInstance<IProfileWindowService>(); }
        }

        public string UserBattleTag
        {
            get { return UserProfile.BattleTagName; }
            set
            {
                RaisePropertyChanged();
            }
        }

        public int SelectedMainTab
        {
            get { return _selectedMainTab; }
            set
            {
                _selectedMainTab = value;
                RaisePropertyChanged();
            }
        }

        public string MatchSummaryHeader
        {
            get { return _matchSummaryHeader; }
            set
            {
                _matchSummaryHeader = value;
                RaisePropertyChanged();
            }
        }

        public void ToggleMatchSummaryFlyout()
        {
            MatchSummaryIsOpen = !MatchSummaryIsOpen;
        }

        public void CloseMatchSummaryFlyout()
        {
            MatchSummaryIsOpen = false;
        }

        public void OpenMatchSummaryFlyout()
        {
            MatchSummaryIsOpen = true;
        }

        public void SetMatchSummaryHeader(string headerTitle)
        {
            MatchSummaryHeader = headerTitle;
        }

        public void SwitchToTab(MainTabs selectedMainTab)
        {
            SelectedMainTab = (int)selectedMainTab;
        }

        private void OpenWhatsNewWindow()
        {
            WhatsNewWindow.CreateWhatsNewWindow();
        }

        private void OpenProfile()
        {
            ProfileWindow.CreateProfileWindow();
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.UpdateUserBattleTag)
                UserBattleTag = UserProfile.BattleTagName;
        }
    }
}