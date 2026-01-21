
using ScoBro.Foundation.Results;

namespace ScoBro.Foundation;

/// <summary>
/// A result class that can be used to return a result from a method.
/// </summary>
public record class SimpleResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleResult"/> class.
    /// </summary>
    /// <param name="wasSuccessful">Whether the result is successful.</param>
    /// <param name="errors">The errors that occurred.</param>
    /// <param name="failureType">The type of failure that occurred.</param>
    public SimpleResult(bool wasSuccessful, IEnumerable<string>? errors = null, FailureType? failureType = null)
    {
        WasSuccessful = wasSuccessful;
        Errors = errors ?? [];
        FailureType = failureType;
    }

    /// <summary>
    /// Gets a value indicating whether the result was successful.
    /// </summary>
    public bool WasSuccessful { get; }

    /// <summary>
    /// Gets the collection of error messages that occurred.
    /// </summary>
    public IEnumerable<string> Errors { get; }

    /// <summary>
    /// The type of failure that occurred.
    /// </summary>
    public FailureType? FailureType { get; }

    /// <summary>
    /// Gets a delimited string of the errors.
    /// </summary>
    /// <param name="delimiter">The delimiter to use between errors. Defaults to ", ".</param>
    /// <returns>A delimited string of the errors.</returns>
    public string GetDelimitedErrors(string delimiter = ", ") => string.Join(delimiter, Errors);

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to return.</param>
    /// <returns>A successful <see cref="SimpleResult{T}"/> with the specified value.</returns>
    public static SimpleResult<T> Ok<T>(T value) => new(value, true);

    /// <summary>
    /// Creates a failed result with a single error message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with the specified error message.</returns>
    public static SimpleResult<T> Fail<T>(string message) => new(default, false, [message]);

    /// <summary>
    /// Creates a failed result with multiple error messages.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="errors">The collection of error messages.</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with the specified error messages.</returns>
    public static SimpleResult<T> Fail<T>(IEnumerable<string> errors) => new(default, false, errors);

    /// <summary>
    /// Creates a failed result with a NotFound failure type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message. Defaults to "The requested resource was not found".</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with NotFound failure type.</returns>
    public static SimpleResult<T> FailNotFound<T>(string message = "The requested resource was not found") => new(default, false, [message], FailureType.NotFound);

    /// <summary>
    /// Creates a failed result with a UserError failure type and a single error message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with UserError failure type.</returns>
    public static SimpleResult<T> FailWIthUserError<T>(string message) => new(default, false, [message], FailureType.UserError);

    /// <summary>
    /// Creates a failed result with a UserError failure type and multiple error messages.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="errors">The collection of error messages.</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with UserError failure type.</returns>
    public static SimpleResult<T> FailWIthUserError<T>(List<string> errors) => new(default, false, errors, FailureType.UserError);

    /// <summary>
    /// Creates a failed result with a ValidationError failure type and a single error message.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with ValidationError failure type.</returns>
    public static SimpleResult<T> FailWIthValidationErrors<T>(string message) => new(default, false, [message], FailureType.ValidationError);

    /// <summary>
    /// Creates a failed result with a ValidationError failure type and multiple error messages.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="errors">The collection of error messages.</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with ValidationError failure type.</returns>
    public static SimpleResult<T> FailWIthValidationErrors<T>(List<string> errors) => new(default, false, errors, FailureType.ValidationError);

    /// <summary>
    /// Creates a failed result with a SystemError failure type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with SystemError failure type.</returns>
    public static SimpleResult<T> FailWIthSystemError<T>(string message) => new(default, false, [message], FailureType.SystemError);

    /// <summary>
    /// Creates a failed result with a Forbidden failure type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message. Defaults to "User is not authorized for this action".</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with Forbidden failure type.</returns>
    public static SimpleResult<T> FailWithForbidden<T>(string message = "User is not authorized for this action") => new(default, false, [message], FailureType.Forbidden);

    /// <summary>
    /// Creates a failed result with an UnAuthorized failure type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="message">The error message. Defaults to "User is not authorized".</param>
    /// <returns>A failed <see cref="SimpleResult{T}"/> with UnAuthorized failure type.</returns>
    public static SimpleResult<T> FailWithUnAuthorized<T>(string message = "User is not authorized") => new(default, false, [message], FailureType.UnAuthorized);

    /// <summary>
    /// Creates a successful result without a value.
    /// </summary>
    /// <returns>A successful <see cref="SimpleResult"/>.</returns>
    public static SimpleResult Ok() => new(true);

    /// <summary>
    /// Creates a failed result with a single error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult"/> with the specified error message.</returns>
    public static SimpleResult Fail(string message) => new(false, [message]);

    /// <summary>
    /// Creates a failed result with optional error messages.
    /// </summary>
    /// <param name="errors">The collection of error messages, or null.</param>
    /// <returns>A failed <see cref="SimpleResult"/> with the specified error messages.</returns>
    public static SimpleResult Fail(IEnumerable<string>? errors = null) => new(false, errors);

    /// <summary>
    /// Creates a failed result with a UserError failure type and a single error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult"/> with UserError failure type.</returns>
    public static SimpleResult FailWIthUserError(string message) => new(false, [message], FailureType.UserError);

    /// <summary>
    /// Creates a failed result with a UserError failure type and multiple error messages.
    /// </summary>
    /// <param name="errors">The collection of error messages.</param>
    /// <returns>A failed <see cref="SimpleResult"/> with UserError failure type.</returns>
    public static SimpleResult FailWIthUserError(List<string> errors) => new(false, errors, FailureType.UserError);

    /// <summary>
    /// Creates a failed result with a UserError failure type and a single error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult"/> with UserError failure type.</returns>
    public static SimpleResult FailWIthValidationErrors(string message) => new(false, [message], FailureType.UserError);

    /// <summary>
    /// Creates a failed result with a UserError failure type and multiple error messages.
    /// </summary>
    /// <param name="errors">The collection of error messages.</param>
    /// <returns>A failed <see cref="SimpleResult"/> with UserError failure type.</returns>
    public static SimpleResult FailWIthValidationErrors(List<string> errors) => new(false, errors, FailureType.UserError);

    /// <summary>
    /// Creates a failed result with a SystemError failure type.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A failed <see cref="SimpleResult"/> with SystemError failure type.</returns>
    public static SimpleResult FailWIthSystemError(string message) => new(false, [message], FailureType.SystemError);

    /// <summary>
    /// Creates a failed result with a NotFound failure type.
    /// </summary>
    /// <param name="message">The error message. Defaults to "The requested resource was not found".</param>
    /// <returns>A failed <see cref="SimpleResult"/> with NotFound failure type.</returns>
    public static SimpleResult FailNotFound(string message = "The requested resource was not found") => new(false, [message], FailureType.NotFound);

    /// <summary>
    /// Creates a failed result with a Forbidden failure type.
    /// </summary>
    /// <param name="message">The error message. Defaults to "User is not authorized for this action".</param>
    /// <returns>A failed <see cref="SimpleResult"/> with Forbidden failure type.</returns>
    public static SimpleResult FailWithForbidden(string message = "User is not authorized for this action") => new(false, [message], FailureType.Forbidden);

    /// <summary>
    /// Creates a failed result with an UnAuthorized failure type.
    /// </summary>
    /// <param name="message">The error message. Defaults to "User is not authorized".</param>
    /// <returns>A failed <see cref="SimpleResult"/> with UnAuthorized failure type.</returns>
    public static SimpleResult FailWithUnAuthorized(string message = "User is not authorized") => new(false, [message], FailureType.UnAuthorized);

    /// <summary>
    /// Converts this non-generic result to a generic result of a different type, preserving the success status, errors, and failure type.
    /// </summary>
    /// <typeparam name="TNew">The new value type for the result.</typeparam>
    /// <returns>A new <see cref="SimpleResult{TNew}"/> with the same success status, errors, and failure type as this result.</returns>
    public SimpleResult<TNew> Switch<TNew>(FailureType? failureType = null) => new(
        value: default,
        wasSuccessful: WasSuccessful,
        errors: Errors,
        failureType: failureType ?? FailureType
    );
}

/// <summary>
/// A generic result class that can be used to return a result with a value from a method.
/// </summary>
/// <typeparam name="T">The type of the value contained in the result.</typeparam>
public record class SimpleResult<T> : SimpleResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleResult{T}"/> class.
    /// </summary>
    /// <param name="value">The value to return.</param>
    /// <param name="wasSuccessful">Whether the result is successful.</param>
    /// <param name="errors">The errors that occurred.</param>
    /// <param name="failureType">The type of failure that occurred.</param>
    /// <exception cref="ArgumentException">Thrown when the result is successful but the value is null.</exception>
    public SimpleResult(
        T? value,
        bool wasSuccessful,
        IEnumerable<string>? errors = null,
        FailureType? failureType = null) : base(wasSuccessful, errors, failureType)
    {

        if (wasSuccessful && value is null)
            throw new ArgumentException("Value must be set when the result is successful.", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Gets the value contained in the result.
    /// </summary>
    public T? Value { get; }
}
