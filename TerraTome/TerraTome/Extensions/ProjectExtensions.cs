using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerraTome.Constants;
using TerraTome.Domain.Dtos;
using TerraTome.ViewModels;

namespace TerraTome.Extensions
{
    public static class ProjectExtensions
    {
        public static IEnumerable<ViewModelBase> GetViewModelsByTabs(this TerraTomeProjectDto project, IEnumerable<string> tabNames)
        {
            var viewModels = new List<ViewModelBase>();

            if (tabNames.Contains(TabNames.Basics))
            {
                viewModels.Add(new WorldViewModel(project));
            }

            if (tabNames.Contains(TabNames.Timeline))
            {
                viewModels.Add(new TimelineViewModel());
            }

            return viewModels;
        }
    }
}