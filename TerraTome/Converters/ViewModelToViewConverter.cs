using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraTome.Services;
using TerraTome.ViewModels;

namespace TerraTome.Converters
{
    public class ViewModelToViewConverter : IValueConverter
    {
        public object? Convert(object? val, Type targetType, object? parameter, CultureInfo culture)
        {
            if (val is not ViewModelEntry vm)
            {
                return null;
            }

            var value = vm.ViewModel();

            var viewModelType = value.GetType();
            var viewTypeName = viewModelType.FullName!.Replace("ViewModels", "Views").Replace("ViewModel", "View");
            var viewType = Type.GetType(viewTypeName);

            if (viewType is null)
            {
                throw new InvalidOperationException($"Could not find view type for {viewModelType}");
            }

            var view = Activator.CreateInstance(viewType);

            return view;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}