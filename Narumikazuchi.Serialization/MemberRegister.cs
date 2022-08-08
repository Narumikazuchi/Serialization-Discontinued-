namespace Narumikazuchi.Serialization;

/// <summary>
/// Contains the meta data that is required for serialization.
/// </summary>
public sealed class MemberRegister : IEnumerable<KeyValuePair<String, Type>>
{
    /// <summary>
    /// Gets an enumerator for the contents of the <see cref="MemberRegister"/>.
    /// </summary>
    /// <returns>An enumerator for the contents of the <see cref="MemberRegister"/>.</returns>
    public Dictionary<String, Type>.Enumerator GetEnumerator() =>
        m_Members.GetEnumerator();

    /// <summary>
    /// Registers the member with <paramref name="name"/> and type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the member to register.</typeparam>
    /// <param name="name">The name of the member to register.</param>
    /// <returns>Itself to chain multiple <see cref="Register{T}(String)"/> calls.</returns>
    public MemberRegister Register<T>(String name) =>
        this.Register(name: name,
                      type: typeof(T));
    /// <summary>
    /// Registers the member with <paramref name="name"/> and type <paramref name="type"/>.
    /// </summary>
    /// <param name="name">The name of the member to register.</param>
    /// <param name="type">The type of the member to register.</param>
    /// <returns>Itself to chain multiple <see cref="Register(String, Type)"/> calls.</returns>
    public MemberRegister Register(String name,
                                   Type type)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(type);

        if (m_Members.ContainsKey(name))
        {
            throw new NotAllowed("Duplicate member names are not supported.");
        }
        else
        {
            m_Members.Add(key: name,
                          value: type);
            return this;
        }
    }

    IEnumerator<KeyValuePair<String, Type>> IEnumerable<KeyValuePair<String, Type>>.GetEnumerator() =>
        this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        this.GetEnumerator();

    private readonly Dictionary<String, Type> m_Members = new();
}