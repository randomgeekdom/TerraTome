namespace TerraTome.ViewModels;

public abstract class AggregateViewModel : ViewModelBase
{
    public abstract string DisplayName { get; }

    public override string ToString()
    {
        return DisplayName;
    }
}