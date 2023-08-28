using Synonyms_Test.Core.Implementation;
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Tests;

public class SettingTest
{
    private ISettings _service;

    [SetUp]
    public void Setup()
    {
        _service = new Settings();
    }

    [TestCase]
    public void GetSearchDepthLimit_ShouldReturn3()
    {
        //Act
        var res = _service.GetSearchDepthLimit();

        //Assert
        Assert.That(res, Is.EqualTo(3));
    }
}