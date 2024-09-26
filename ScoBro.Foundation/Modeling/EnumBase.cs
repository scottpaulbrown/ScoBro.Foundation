using System.Reflection;

namespace ScoBro.Foundation;

public record EnumBase {
#pragma warning disable CS8618
    protected EnumBase() { }
#pragma warning restore CS8618

    protected EnumBase(string id) {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        Id = id;
    }

    public string Id { get; private set; }
}

public record EnumBase<T> : EnumBase where T : EnumBase<T> {
    protected EnumBase() { }

    protected EnumBase(string id) : base(id) { }

    public static IEnumerable<T> GetAll() {
        var fields = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);
        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public static T Parse(string id) =>
        GetAll().First(a => a.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

    public static bool TryParse(string id, out T? obj) {
        obj = GetAll().FirstOrDefault(a => a.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        return obj != null;
    }

    public override string ToString() => Id;
}

public record EnumTypeWithDescription<T> : EnumBase<T> where T : EnumBase<T> {
#pragma warning disable CS8618
    protected EnumTypeWithDescription() { }
#pragma warning restore CS8618

    protected EnumTypeWithDescription(string id, string description) : base(id) {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
        Description = description;
    }

    public string Description { get; private set; }
}
