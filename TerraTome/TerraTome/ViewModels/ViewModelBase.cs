using CommunityToolkit.Mvvm.ComponentModel;

namespace TerraTome.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
	public abstract string Name { get; }
}