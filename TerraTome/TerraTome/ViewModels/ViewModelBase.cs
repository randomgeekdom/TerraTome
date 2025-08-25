using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace TerraTome.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private bool _isVisible = true;
    [ObservableProperty] private ICommand _tabCloseCommand;

    public event EventHandler? TabCloseRequested;

    public ViewModelBase()
    {
        TabCloseCommand = new RelayCommand(TabClose);
    }

    public abstract string Name { get; }

    private void TabClose()
    {
        this.IsVisible = false;
        TabCloseRequested?.Invoke(this, EventArgs.Empty);
    }
}