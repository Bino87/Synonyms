using Synonyms_Test.Core.Implementation;
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Tests;

public class WordAdapterTests
{
    private IWordAdapter _service;

    [SetUp]
    public void Setup()
    {
        _service = new WordAdapter();
    }

    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase("    ", "")]
    [TestCase("  test  ", "test")]
    [TestCase("TEST", "test")]
    [TestCase("   TEST   ", "test")]
    public void Adapt_ShouldReturnExpectedValue(string? input, string? expectedOutput)
    {
        //Act
        var res = _service.Adapt(input);

        Assert.That(res, Is.EqualTo(expectedOutput));
    }
}