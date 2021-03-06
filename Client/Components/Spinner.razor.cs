﻿using Microsoft.AspNetCore.Components;
using System;

namespace FeatureDBPortal.Client.Components
{
    public class SpinnerService
    {
        public event Action OnShow;
        public event Action OnHide;

        public void Show()
        {
            OnShow?.Invoke();
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }
    }

    public class SpinnerDataModel : ComponentBase
    {
        [Inject]
        SpinnerService SpinnerService { get; set; }
        [Parameter]
        public bool IsVisible { get; set; }

        protected override void OnInitialized()
        {
            SpinnerService.OnShow += Show;
            SpinnerService.OnHide += Hide;
        }

        public void Show()
        {
            IsVisible = true;
            StateHasChanged();
        }

        public void Hide()
        {
            IsVisible = false;
            StateHasChanged();
        }
    }
}