namespace Services.Request
{
    public record Response(
    int error,
    String message,
    object? data
);
}

