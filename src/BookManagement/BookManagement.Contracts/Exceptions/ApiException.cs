namespace BookManagement.Contracts.Exceptions;

/// <summary>
///     The api exception class
/// </summary>
/// <seealso cref="Exception" />
public class ApiException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiException" /> class
    /// </summary>
    /// <param name="description"></param>
    /// <param name="statusCode">The status code</param>
    public ApiException(string description, int statusCode)
    {
        Description = description;
        StatusCode = statusCode;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiException" /> class
    /// </summary>
    public ApiException()
    {
    }

    /// <summary>
    ///     Gets the value of the status code
    /// </summary>
    public int StatusCode { get; } = 500;

    /// <summary>
    ///     Gets the value of the message
    /// </summary>
    public string Description { get; } = "Unhandled exception occurred";


    public override string ToString()
    {
        return Description;
    }
}