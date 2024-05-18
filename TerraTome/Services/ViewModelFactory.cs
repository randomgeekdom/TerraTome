using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraTome.ViewModels;

namespace TerraTome.Services
{
    public class ViewModelFactory : IViewModelFactory
    {
        // Make viewmodels singleton

        public IEnumerable<ViewModelEntry> Views { get; } =
        [
            new ViewModelEntry("Events", () => new EventsViewModel()),
            new ViewModelEntry("Locales", () => new LocalesViewModel()),
        ];
    }
}