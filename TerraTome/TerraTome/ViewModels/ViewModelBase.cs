using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TerraTome.Domain.Dtos;
using TerraTome.Events;

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
        var messageBox = MessageBoxManager.GetMessageBoxStandard("Save?", "Contents on this tab have changed.  Would you like to save?", MsBox.Avalonia.Enums.ButtonEnum.YesNoCancel);

        var application = Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var result = await messageBox.ShowAsPopupAsync(application!.MainWindow!);

        if(result == MsBox.Avalonia.Enums.ButtonResult.Cancel)
        {
            return;
        }

        this.IsVisible = false;
        TabCloseRequested?.Invoke(this, new TabCloseEventArgs { IsSaving = result == MsBox.Avalonia.Enums.ButtonResult.Yes });
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