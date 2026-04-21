namespace ScoBro.Foundation;

/// <summary>
/// Mapping extensions for <see cref="EnumBase{T}"/>-derived types.
/// </summary>
public static class EnumBaseMappingExtensions {
    /// <summary>
    /// Projects an <see cref="EnumTypeWithDescription{T}"/> value to an <see cref="EnumWithDescriptionDto"/>.
    /// </summary>
    /// <typeparam name="T">The concrete enumeration type.</typeparam>
    /// <param name="domainEnum">The enumeration value to map.</param>
    /// <returns>An <see cref="EnumWithDescriptionDto"/> containing the id and description.</returns>
    public static EnumWithDescriptionDto ToDto<T>(this EnumTypeWithDescription<T> domainEnum) where T : EnumTypeWithDescription<T> =>
        new(domainEnum.Id, domainEnum.Description);
}
