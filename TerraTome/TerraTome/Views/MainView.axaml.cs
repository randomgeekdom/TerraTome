using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using TerraTome.Domain;
using TerraTome.Domain.Dtos;
using TerraTome.Services;
using TerraTome.ViewModels;

namespace TerraTome.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void UserControl_KeyUp_1(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        var vm = DataContext as MainViewModel;
        // CTRL + S
        if (e.Key == Avalonia.Input.Key.S && (e.KeyModifiers & Avalonia.Input.KeyModifiers.Control) != 0)
        {
            vm?.SaveCommand.Execute(null);
        }

        // CTRL + O
        else if (e.Key == Avalonia.Input.Key.O && (e.KeyModifiers & Avalonia.Input.KeyModifiers.Control) != 0)
        {
            vm?.LoadCommand.Execute(null);
        }

        // CTRL + N
        else if (e.Key == Avalonia.Input.Key.N && (e.KeyModifiers & Avalonia.Input.KeyModifiers.Control) != 0)
        {
            vm?.CreateCommand.Execute(null);
        }
    }
}