namespace ScoBro.Foundation.Modeling;

public static class EnumBaseMappingExtensions {
    /// <summary>
    /// Converts an EnumTypeWithDescription to an EnumWithDescriptionDto.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="domainEnum">The domain enum to convert.</param>
    /// <returns>The converted enum with description DTO.</returns>
    public static EnumWithDescriptionDto ToDto<T>(this EnumTypeWithDescription<T> domainEnum) where T : EnumTypeWithDescription<T> =>
        new(domainEnum.Id, domainEnum.Description);
}
