using Synonyms_Test.Core.Implementation;
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Tests;

public class ResponseErrorFactoryTests
{
  
    private IResponseErrorHandler _service;

    [SetUp]
    public void Setup()
    {
        _service = new ResponseErrorHandler();
    }

    [TestCase]
    public void SetError_AllowOnlyDistinctCodes()
    {
        //Arrange
        _service.SetErrorCode(1);
        _service.SetErrorCode(1);
        _service.SetErrorCode(2);
        _service.SetErrorCode(2);

        //Act
        int[] res = _service.GetErrorCodes();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual(2, res.Length);
        Assert.That(1, Is.EqualTo(res.First()));
        Assert.That(2, Is.EqualTo(res.Last()));
    }
}