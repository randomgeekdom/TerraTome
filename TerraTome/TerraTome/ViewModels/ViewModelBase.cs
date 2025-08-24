using CommunityToolkit.Mvvm.ComponentModel;

namespace TerraTome.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
	public abstract string Name { get; }

	[ObservableProperty]
	private bool _isVisible = true;
}