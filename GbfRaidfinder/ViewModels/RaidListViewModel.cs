using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GbfRaidfinder.Common;
using GbfRaidfinder.Data;
using GbfRaidfinder.Factorys;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using PropertyChanged;

namespace GbfRaidfinder.ViewModels {
    public class RaidListViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IControllerFactory _controllerFactory;

        public RaidListViewModel(IControllerFactory controllerFactory) {
            _controllerFactory = controllerFactory;
            AddCommand = new ActionCommand(r => Add((RaidListItem) r));
            SetupRaids();
        }

        public ObservableCollection<RaidListItem> RaidListItems { get; set; }

        public ICommand AddCommand { get; }

        private void SetupRaids() {
            var raidfollows = _controllerFactory.GetRaidsController.Follows.Select(f => f.English);
            var raids = _controllerFactory.GetRaidlistController.RaidBossListItems;
            foreach (var raidListItem in raids) {
                if (raidfollows.Contains(raidListItem.English)) {
                    raidListItem.Following = true;
                }
            }
            RaidListItems = raids;
        }

        private void Add(RaidListItem raidBoss) {
            try {
                if (_controllerFactory.GetRaidsController.Follows.Select(f => f.English).ToList()
                    .Contains(raidBoss.English)) {
                    _controllerFactory.GetRaidsController.Follows.Remove(
                        f => f.Japanese == raidBoss.Japanese || f.English == raidBoss.English);
                    _controllerFactory.GetRaidlistController.RaidBossListItems.First(
                        f => f.Japanese == raidBoss.Japanese || f.English == raidBoss.English).Following = false;
                    _controllerFactory.GetRaidsController.Save();
                    return;
                }
                _controllerFactory.GetRaidsController.Follows.Add(new FollowModel(raidBoss.Japanese, raidBoss.English,
                    raidBoss.Image, _controllerFactory.GetBlacklistController));
                _controllerFactory.GetRaidsController.Save();
                raidBoss.Following = true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}