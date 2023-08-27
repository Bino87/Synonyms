namespace Synonym_Test.DataTransferObjects;

public sealed class GetSynonymsResponseDto
{
    public string? Value { get; set; }
    public int Closeness { get; set; }
}