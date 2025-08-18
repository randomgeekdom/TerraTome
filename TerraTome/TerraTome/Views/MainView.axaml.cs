using System;
using System.Collections.Generic;
using System.IO;
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
        var project = TerraTomeProject.TryCreate(file.Path).Value;
        await SerializeProjectAsync(file, project);
        vm.SetProject(project);
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
        var entity = TerraTomeProject.TryCreate(files[0].Path).Value;
        if (projectDto != null) entity.FromDto(projectDto);
        vm.SetProject(entity);
    }

    private async void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        var topLevel = GetTopLevel();
        if (topLevel is null) return;

        var file = await topLevel.StorageProvider.TryGetFileFromPathAsync(filePath: vm.TerraTomeProject!.GetFilePath());
        if (file == null) throw new FileNotFoundException();
        await SerializeProjectAsync(file, vm.TerraTomeProject!);
    }

    private async void SaveAs_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        var topLevel = GetTopLevel();
        if (topLevel is null) return;

        var file = await ShowSaveFileDialog(topLevel, "Save Project Copy");
        if (file == null) return;
        var project = vm.TerraTomeProject!;
        project.SetFilePath(file.Path);
        await SerializeProjectAsync(file, project);
        vm.SetProject(project);
    }

    private async Task SerializeProjectAsync(IStorageFile file, TerraTomeProject project)
    {
        await using var stream = await file.OpenWriteAsync();
        await JsonSerializer.SerializeAsync(stream, project.ToDto());
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
}