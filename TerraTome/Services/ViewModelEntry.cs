using System;
using TerraTome.ViewModels;

namespace TerraTome.Services
{
    public record ViewModelEntry(string DisplayName, Func<AggregateViewModel> ViewModel)
    {
        public override string ToString()
        {
            return DisplayName;
        }
    }
}