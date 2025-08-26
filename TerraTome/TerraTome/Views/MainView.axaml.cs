using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using TerraTome.Domain;
using TerraTome.Domain.Dtos;
using TerraTome.ViewModels;

namespace TerraTome.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private static FilePickerFileType TerraTomeFileType => new("TerraTome Project") { Patterns = ["*.tt"] };

    private async void Create_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        var topLevel = GetTopLevel();
        if (topLevel is null) return;

        var file = await ShowSaveFileDialog(topLevel, "Save Project");
        if (file == null) return;

        vm.SetProject(new TerraTomeProjectDto(), file.Path);
        await SaveProjectAsync(file, vm);
    }

    private async Task<TerraTomeProjectDto?> DeserializeProjectDtoAsync(IStorageFile file)
    {
        await using var stream = await file.OpenReadAsync();
        return await JsonSerializer.DeserializeAsync<TerraTomeProjectDto>(stream);
    }

    private TopLevel? GetTopLevel() => TopLevel.GetTopLevel(this);

    private async void Load_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        var topLevel = GetTopLevel();
        if (topLevel is null) return;

        var files = await ShowOpenFileDialog(topLevel, "Load Project");
        if (files.Count < 1) return;

        var projectDto = await DeserializeProjectDtoAsync(files[0]);
        vm.SetProject(projectDto!, files[0].Path);
    }

    private async void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        var topLevel = GetTopLevel();
        if (topLevel is null) return;

        var file = await topLevel.StorageProvider.TryGetFileFromPathAsync(filePath: vm.TerraTomeProjectSettings!.GetFilePath());
        if (file == null) throw new FileNotFoundException();
        await SaveProjectAsync(file, vm);
    }

    private async void SaveAs_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        var topLevel = GetTopLevel();
        if (topLevel is null) return;

        var file = await ShowSaveFileDialog(topLevel, "Save Project Copy");
        if (file == null) return;
        vm.TerraTomeProjectSettings!.SetFilePath(file.Path);
        await SaveProjectAsync(file, vm);
    }

    /// <summary>
    /// This needs to take in the top level project and each view should have its own BoundedContext/AggregateRoot
    /// </summary>
    /// <param name="file"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    private async Task<TerraTomeProjectDto> SaveProjectAsync(IStorageFile file, MainViewModel vm)
    {
        await using var stream = await file.OpenWriteAsync();

        var dto = GetDto(vm);
        await JsonSerializer.SerializeAsync(stream, GetDto(vm));
        return dto;
    }

    private async Task<IReadOnlyList<IStorageFile>> ShowOpenFileDialog(TopLevel topLevel, string title)
    {
        return await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            FileTypeFilter = [TerraTomeFileType],
        });
    }

    private async Task<IStorageFile?> ShowSaveFileDialog(TopLevel topLevel, string title)
    {
        return await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            FileTypeChoices = [TerraTomeFileType],
        });
    }

    /// <summary>
    /// Method that gets the view models and converts them to a DTO to save
    /// </summary>
    /// <param name="mainViewModel"></param>
    /// <returns></returns>
    private static TerraTomeProjectDto GetDto(MainViewModel mainViewModel)
    {
        if (!mainViewModel.ViewModels.Any()) 
        {
            return TerraTomeBasics.TryCreate().Value.ToDto();
        }

        var worldViewModel = mainViewModel.ViewModels.First(x => x is WorldViewModel) as WorldViewModel;
        var timelineViewModel = mainViewModel.ViewModels.First(x => x is TimelineViewModel) as TimelineViewModel;

        var dto = mainViewModel.TerraTomeProjectSettings!.ToDto();
        dto = worldViewModel!.Project.ToDto(dto);

        return dto;
    }

    private void UserControl_KeyUp_1(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        // CTRL + S
        if (e.Key == Avalonia.Input.Key.S && (e.KeyModifiers & Avalonia.Input.KeyModifiers.Control) != 0)
        {
            Save_Click(sender, e);
        }

        // CTRL + O
        else if (e.Key == Avalonia.Input.Key.O && (e.KeyModifiers & Avalonia.Input.KeyModifiers.Control) != 0)
        {
            Load_OnClick(sender, e);
        }

        // CTRL + N
        else if (e.Key == Avalonia.Input.Key.N && (e.KeyModifiers & Avalonia.Input.KeyModifiers.Control) != 0)
        {
            Create_OnClick(sender, e);
        }
    }
}