﻿using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;
using System.Windows.Input;

namespace HeroesParserData.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _selectedMainTab;
        private int _selectedStatisticsTab;
        private bool _isExtendedAboutTextVisible;
        private string _extendedAboutText;

        public string AppVersion
        {
            get
            {
                return HPDVersion.GetVersionAsString();
            }
        }

        public int SelectedMainTab
        {
            get { return _selectedMainTab; }
            set
            {
                _selectedMainTab = value;
                if (value == 0)
                    Messenger.Default.Send(new HomeTabMessage() { Trigger = Trigger.Update });

                if (value == 1)
                    Messenger.Default.Send(new MatchSummaryMessage() { MatchSummary = MatchSummary.LastMatch });

                if (value == 4) // stats tab
                {
                    SelectedStatisticsTab = SelectedStatisticsTab;
                }
                RaisePropertyChangedEvent(nameof(SelectedMainTab));
            }
        }

        public int SelectedStatisticsTab
        {
            get { return _selectedStatisticsTab; }
            set
            {
                _selectedStatisticsTab = value;
                if (value == 0)
                    Messenger.Default.Send(new StatisticsTabMessage { StatisticsTab = StatisticsTab.Overview });
                else if (value == 1)
                    Messenger.Default.Send(new StatisticsTabMessage { StatisticsTab = StatisticsTab.Heroes });
                else if (value == 2)
                    Messenger.Default.Send(new StatisticsTabMessage { StatisticsTab = StatisticsTab.GameModes });
                RaisePropertyChangedEvent(nameof(SelectedStatisticsTab));
            }
        }

        public bool IsExtendedAboutTextVisible
        {
            get { return _isExtendedAboutTextVisible; }
            set
            {
                _isExtendedAboutTextVisible = value;
                RaisePropertyChangedEvent(nameof(IsExtendedAboutTextVisible));
            }
        }

        public string ExtendedAboutText
        {
            get { return _extendedAboutText; }
            set
            {
                _extendedAboutText = value;
                RaisePropertyChangedEvent(nameof(ExtendedAboutText));
            }
        }

        public MainWindowViewModel() 
            : base()
        {
            Messenger.Default.Register<AboutUpdateMessage>(this, (action) => ReceiveMessage(action));

            IsExtendedAboutTextVisible = false;
        }

        private void ReceiveMessage(AboutUpdateMessage action)
        {
            ExtendedAboutText = action.Message;
            IsExtendedAboutTextVisible = action.IsVisible;
        }
    }
}
