using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TerraTome.Domain.Dtos;
using TerraTome.Events;
using TerraTome.Services;

namespace TerraTome.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private bool _isVisible = true;
    [ObservableProperty] private ICommand _tabCloseCommand;

    public ViewModelBase()
    {
        TabCloseCommand = new AsyncRelayCommand(TabCloseAsync);
    }

    public event EventHandler? TabCloseRequested;

    public bool IsDirty { get; set; } = false;
    public abstract string Name { get; }

    public abstract void MapToDto(TerraTomeProjectDto project);

    private async Task TabCloseAsync()
    {
        if (!IsDirty)
        {
            this.IsVisible = false;
            TabCloseRequested?.Invoke(this, new TabCloseEventArgs { IsSaving = false });
            return;
        }

        var result = await ApplicationService.ShowMessage("Save?", "Contents on this tab have changed.  Would you like to save?", ButtonEnum.YesNoCancel);

        if (result == ButtonResult.Cancel)
        {
            return;
        }

        this.IsVisible = false;
        TabCloseRequested?.Invoke(this, new TabCloseEventArgs { IsSaving = result == ButtonResult.Yes });
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(IsDirty))
        {
            this.IsDirty = true;
        }
        base.OnPropertyChanged(e);
    }
}