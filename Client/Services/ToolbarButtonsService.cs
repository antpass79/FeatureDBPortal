using FeatureDBPortal.Client.Models;
using System;
using System.Collections.ObjectModel;

namespace FeatureDBPortal.Client.Services
{
    public class ToolbarButtonsService
    {
        public Action<ButtonAction> FireAction { get; set; }
        public ObservableCollection<ButtonAction> Actions { get; } = new ObservableCollection<ButtonAction>();
    }
}
