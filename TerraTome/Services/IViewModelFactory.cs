using System;
using System.Collections.Generic;
using TerraTome.ViewModels;

namespace TerraTome.Services
{
    public interface IViewModelFactory
    {
        IEnumerable<ViewModelEntry> Views { get; }
    }
}