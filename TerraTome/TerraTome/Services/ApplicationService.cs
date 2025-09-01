using Ardalis.Result;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TerraTome.Domain.Dtos;
using TerraTome.ViewModels;

namespace TerraTome.Services
{
    /// <summary>
    /// A class that handles application-wide services, such as showing message boxes.
    /// </summary>
    public static class ApplicationService
    {
        /// <summary>
        /// This needs to take in the top level project and each view should have its own BoundedContext/AggregateRoot
        /// </summary>
        /// <param name="file"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static async Task<TerraTomeProjectDto> SaveProjectAsync(IStorageFile file, MainViewModel vm)
        {
            await using var stream = await file.OpenWriteAsync();

            var dto = GetDto(vm);
            await JsonSerializer.SerializeAsync(stream, GetDto(vm));
            return dto;
        }

        public static async Task<ButtonResult> ShowMessage(string header, string message, ButtonEnum buttons)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(header, message, buttons);

            var result = default(ButtonResult);
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
            {
                result = await messageBox.ShowAsPopupAsync(desktopApp!.MainWindow!);
            }
            else if (Application.Current!.ApplicationLifetime is ISingleViewApplicationLifetime singleApp)
            {
                result = await messageBox.ShowAsync();
            }

            return result;
        }

        public static TopLevel GetTopLevel()
        {
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
            {
                return TopLevel.GetTopLevel(desktopApp!.MainWindow!)!;
            }
            else if (Application.Current!.ApplicationLifetime is ISingleViewApplicationLifetime singleApp)
            {
                return TopLevel.GetTopLevel(singleApp!.MainView!)!;
            }
            else
            {
                throw new InvalidOperationException("No valid application lifetime found.");
            }
        }


        /// <summary>
        /// Method that gets the view models and converts them to a DTO to save
        /// </summary>
        /// <param name="mainViewModel"></param>
        /// <returns></returns>
        private static TerraTomeProjectDto GetDto(MainViewModel mainViewModel)
        {
            var dto = new TerraTomeProjectDto { Id = mainViewModel.Project?.Id ?? Guid.NewGuid() };

            mainViewModel.MapToDto(dto);

            return dto;
        }

        public static async Task<IStorageFile?> ShowSaveFileDialog(TopLevel topLevel, string title)
        {
            return await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = title,
                FileTypeChoices = [TerraTomeFileType],
            });
        }


        public static FilePickerFileType TerraTomeFileType => new("TerraTome Project") { Patterns = ["*.tt"] };


        public static async Task<IReadOnlyList<IStorageFile>> ShowOpenFileDialog(TopLevel topLevel, string title)
        {
            return await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = [ApplicationService.TerraTomeFileType],
            });
        }


        public static async Task<TerraTomeProjectDto?> DeserializeProjectDtoAsync(IStorageFile file)
        {
            await using var stream = await file.OpenReadAsync();
            return await JsonSerializer.DeserializeAsync<TerraTomeProjectDto>(stream);
        }
    }
}