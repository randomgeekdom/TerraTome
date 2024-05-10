using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TerraTome.Domain;
using TerraTome.ViewModels;

namespace TerraTome.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public MainViewModel ViewModel => DataContext as MainViewModel ?? throw new InvalidOperationException("DataContext is not MainViewModel");

    public async Task CheckSaveOnCloseAsync()
    {
        var project = this.ViewModel.Project;
        if (project != null)
        {
            var box = MessageBoxManager
            .GetMessageBoxStandard("Caption", "Would you like to Save?",
                ButtonEnum.YesNo);

            var result = await box.ShowAsync();

            switch (result)
            {
                case ButtonResult.Yes:
                    await this.SaveAsync();
                    this.ViewModel.CloseProject();
                    break;

                case ButtonResult.No:
                    this.ViewModel.CloseProject();
                    break;

                default:
                    throw new InvalidOperationException("Unknown button result");
            }
        }
    }

    private async void CloseClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await CheckSaveOnCloseAsync();
    }

    private void NewClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.ViewModel.LoadProject(Project.TryCreate().Value);
    }

    private async void OpenClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open TerraTome File",
            AllowMultiple = false,
            FileTypeFilter = [
                new("TerraTome Files")
                {
                    Patterns = ["*.tome"]
                }
            ]
        });

        if (files.Count >= 1)
        {
            // Open reading stream from the first file.
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);

            var vm = ViewModel;
            if (vm != null)
            {
                vm.LoadProject(await JsonSerializer.DeserializeAsync<Project>(stream));
            }
        }
    }

    private async Task SaveAsync()
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save TerraTome File",
            DefaultExtension = "tome",
            FileTypeChoices = [
                new("TerraTome Files")
                {
                    Patterns = ["*.tome"]
                }
            ]
        });

        if (file == null)
        {
            return;
        }

        var project = ViewModel.Project;
        await file.OpenWriteAsync().ContinueWith(async task =>
        {
            await using var stream = await task;
            await JsonSerializer.SerializeAsync(stream, project);
        });
    }

    private async void SaveClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ViewModel.Project == null)
        {
            return;
        }

        await SaveAsync();
    }
}