using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Windows.Input;
using TerraTome.Domain.Dtos;

namespace TerraTome.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private bool _isVisible = true;
    [ObservableProperty] private ICommand _tabCloseCommand;

    public ViewModelBase()
    {
        TabCloseCommand = new RelayCommand(TabClose);
    }

    public event EventHandler? TabCloseRequested;

    public bool IsDirty { get; set; } = false;
    public abstract string Name { get; }

    public abstract void MapToDto(TerraTomeProjectDto project);

    private void TabClose()
    {
        this.IsVisible = false;
        TabCloseRequested?.Invoke(this, EventArgs.Empty);
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