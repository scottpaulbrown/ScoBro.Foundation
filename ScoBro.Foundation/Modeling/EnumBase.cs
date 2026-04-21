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

    /// <inheritdoc/>
    public override string ToString() => Id;
}

/// <summary>
/// Base record for strongly-typed string enumerations. Derive from this type and declare
/// public static readonly fields of type <typeparamref name="T"/> to define the enumeration members.
/// </summary>
/// <typeparam name="T">The concrete enumeration type.</typeparam>
public record EnumBase<T> : EnumBase where T : EnumBase<T> {
    private static IReadOnlyList<T>? _all;

    protected EnumBase() { }

    /// <param name="id">The string identifier for this enumeration value. Must not be null or empty.</param>
    protected EnumBase(string id) : base(id) { }

    /// <summary>Returns all declared members of <typeparamref name="T"/> by reflecting over its public static fields. Result is cached after the first call.</summary>
    /// <returns>All <typeparamref name="T"/> instances.</returns>
    public static IEnumerable<T> GetAll() =>
        _all ??= [.. typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null)).Cast<T>()];

    /// <summary>Returns the member whose <see cref="EnumBase.Id"/> matches <paramref name="id"/> (case-insensitive).</summary>
    /// <param name="id">The id to look up.</param>
    /// <returns>The matching <typeparamref name="T"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when no match is found.</exception>
    public static T Parse(string id) =>
        TryParse(id, out var obj) ? obj! : throw new ArgumentException($"'{id}' is not a valid {typeof(T).Name}.", nameof(id));

    /// <summary>Attempts to find the member whose <see cref="EnumBase.Id"/> matches <paramref name="id"/> (case-insensitive).</summary>
    /// <param name="id">The id to look up.</param>
    /// <param name="obj">The matching instance, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if a match was found; otherwise <see langword="false"/>.</returns>
    public static bool TryParse(string id, out T? obj) {
        obj = GetAll().FirstOrDefault(a => a.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        return obj != null;
    }

    /// <summary>Determines whether <paramref name="id"/> corresponds to a declared member (case-insensitive).</summary>
    /// <param name="id">The id to check.</param>
    /// <returns><see langword="true"/> if a match exists; otherwise <see langword="false"/>.</returns>
    public static bool IsDefined(string id) => TryParse(id, out _);

    /// <summary>Implicitly converts a <typeparamref name="T"/> value to its string identifier.</summary>
    public static implicit operator string(EnumBase<T> value) => value.Id;
}

/// <summary>
/// Extends <see cref="EnumBase{T}"/> with a human-readable <see cref="Description"/> for each member.
/// </summary>
/// <typeparam name="T">The concrete enumeration type.</typeparam>
public record EnumTypeWithDescription<T> : EnumBase<T> where T : EnumTypeWithDescription<T> {
#pragma warning disable CS8618
    protected EnumTypeWithDescription() { }
#pragma warning restore CS8618

    /// <param name="id">The string identifier for this enumeration value. Must not be null or empty.</param>
    /// <param name="description">The human-readable description for this enumeration value. Must not be null or empty.</param>
    protected EnumTypeWithDescription(string id, string description) : base(id) {
        if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
        Description = description;
    }

    /// <summary>A human-readable description of this enumeration value.</summary>
    public string Description { get; private set; }

    /// <summary>Returns the member whose <see cref="Description"/> matches <paramref name="description"/> (case-insensitive).</summary>
    /// <param name="description">The description to look up.</param>
    /// <returns>The matching <typeparamref name="T"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when no match is found.</exception>
    public static T ParseByDescription(string description) =>
        TryParseByDescription(description, out var obj) ? obj! : throw new ArgumentException($"'{description}' does not match any {typeof(T).Name} description.", nameof(description));

    /// <summary>Attempts to find the member whose <see cref="Description"/> matches <paramref name="description"/> (case-insensitive).</summary>
    /// <param name="description">The description to look up.</param>
    /// <param name="obj">The matching instance, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if a match was found; otherwise <see langword="false"/>.</returns>
    public static bool TryParseByDescription(string description, out T? obj) {
        obj = GetAll().FirstOrDefault(a => a.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase));
        return obj != null;
    }
}
