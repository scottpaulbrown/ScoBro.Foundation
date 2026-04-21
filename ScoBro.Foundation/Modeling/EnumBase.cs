using System.Reflection;

namespace ScoBro.Foundation;

/// <summary>
/// A data transfer object representing an <see cref="EnumTypeWithDescription{T}"/> value with its id and human-readable description.
/// </summary>
public record EnumWithDescriptionDto(string Id, string Description);

/// <summary>
/// Non-generic base record for string-keyed enumeration types.
/// </summary>
public record EnumBase {
#pragma warning disable CS8618
    protected EnumBase() { }
#pragma warning restore CS8618

    /// <param name="id">The string identifier for this enumeration value. Must not be null or empty.</param>
    protected EnumBase(string id) {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        Id = id;
    }

    /// <summary>The unique string identifier for this enumeration value.</summary>
    public string Id { get; private set; }
}

/// <summary>
/// Base record for strongly-typed string enumerations. Derive from this type and declare
/// public static readonly fields of type <typeparamref name="T"/> to define the enumeration members.
/// </summary>
/// <typeparam name="T">The concrete enumeration type.</typeparam>
public record EnumBase<T> : EnumBase where T : EnumBase<T> {
    protected EnumBase() { }

    /// <param name="id">The string identifier for this enumeration value. Must not be null or empty.</param>
    protected EnumBase(string id) : base(id) { }

    /// <summary>Returns all declared members of <typeparamref name="T"/> by reflecting over its public static fields.</summary>
    /// <returns>An enumerable of all <typeparamref name="T"/> instances.</returns>
    public static IEnumerable<T> GetAll() {
        var fields = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);
        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    /// <summary>Returns the member whose <see cref="EnumBase.Id"/> matches <paramref name="id"/> (case-insensitive).</summary>
    /// <param name="id">The id to look up.</param>
    /// <returns>The matching <typeparamref name="T"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no match is found.</exception>
    public static T Parse(string id) =>
        GetAll().First(a => a.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

    /// <summary>Attempts to find the member whose <see cref="EnumBase.Id"/> matches <paramref name="id"/> (case-insensitive).</summary>
    /// <param name="id">The id to look up.</param>
    /// <param name="obj">The matching instance, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if a match was found; otherwise <see langword="false"/>.</returns>
    public static bool TryParse(string id, out T? obj) {
        obj = GetAll().FirstOrDefault(a => a.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        return obj != null;
    }

    /// <inheritdoc/>
    public override string ToString() => Id;
}

/// <summary>
/// Extends <see cref="EnumBase{T}"/> with a human-readable <see cref="Description"/> for each member.
/// </summary>
/// <typeparam name="T">The concrete enumeration type.</typeparam>
public record EnumTypeWithDescription<T> : EnumBase<T> where T : EnumBase<T> {
#pragma warning disable CS8618
    protected EnumTypeWithDescription() { }
#pragma warning restore CS8618

    /// <param name="id">The string identifier for this enumeration value. Must not be null or empty.</param>
    /// <param name="description">The human-readable description for this enumeration value. Must not be null or empty.</param>
    protected EnumTypeWithDescription(string id, string description) : base(id) {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
        Description = description;
    }

    /// <summary>A human-readable description of this enumeration value.</summary>
    public string Description { get; private set; }
}
