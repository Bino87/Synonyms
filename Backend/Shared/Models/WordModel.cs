namespace Shared.Models;

public sealed class WordModel
{
    public Guid Id { get; init; } 
    public string Value { get; set; } = null!;
}