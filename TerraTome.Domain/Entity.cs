using TerraTome.Domain.Dtos;

namespace TerraTome.Domain;

public abstract class Entity<TDto>
	where TDto: EntityDto
{
	public Guid Id { get; protected set; } = Guid.NewGuid();

	public override bool Equals(object? obj)
	{
		if (obj is not Entity<TDto> other)
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		if (this.GetType() != other.GetType())
		{
			return false;
		}

		return Id == other.Id;
	}

	public static bool operator ==(Entity<TDto>? a, Entity<TDto>? b)
	{
		if (a is null && b is null)
		{
			return true;
		}

		if (a is null || b is null)
		{
			return false;
		}

		return a.Equals(b);
	}

	public static bool operator !=(Entity<TDto>? a, Entity<TDto>? b)
	{
		return !(a == b);
	}

	public override int GetHashCode()
	{
		return (GetType().ToString() + Id).GetHashCode();
	}
	
	public abstract TDto ToDto();
	public abstract void FromDto(TDto dto);
}