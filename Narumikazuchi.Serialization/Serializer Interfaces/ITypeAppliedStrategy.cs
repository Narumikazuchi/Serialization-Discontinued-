namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a strategy for either serialization or deserialization, which can be apllied to a specific type.
/// </summary>
public interface ITypeAppliedStrategy
{
    /// <summary>
    /// Checks whether this strategy can be applied to the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type that will be checked.</param>
    /// <returns><see langword="true"/> if this strategy can be applied to the specified <paramref name="type"/>; otherwise, <see langword="false"/>.</returns>
    public Boolean CanBeAppliedTo(Type type);
}