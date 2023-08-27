using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Synonym_Test.Models;
using Synonyms_Test.Core.Interfaces;
using Synonyms_Test.DataAccess.Context;
using Synonyms_Test.DataAccess.Implementation;
using Synonyms_Test.DataAccess.Interfaces;
#pragma warning disable CS8618 //Disable non-nullable uninitialized warning since they are initialized in setup

namespace Synonyms_Test.DataAccess.Tests
{
    public class SynonymRepositoryTests
    {
        private class DbContextFactoryMock : IDbContextFactory<WordContext>
        {
            private readonly WordContext _context;

            public DbContextFactoryMock(WordContext context)
            {
                _context = context;
            }

            public WordContext CreateDbContext()
            {
                return _context;
            }
        }

        private class LoggerMock : ILogger<SynonymsRepository>
        {
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                throw new NotImplementedException();
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            {
                throw new NotImplementedException();
            }
        }
        private ISynonymsRepository _service;
        private IDbContextFactory<WordContext> _wordContextFactory;
        private ILogger<SynonymsRepository> _logger;
        private IResponseErrorHandler _responseErrorHandler;
        private WordContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WordContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new WordContext(options);

            _wordContextFactory = new DbContextFactoryMock(_context);
            _logger = new LoggerMock();
            _responseErrorHandler = Substitute.For<IResponseErrorHandler>();

            _service = new SynonymsRepository(_wordContextFactory, _logger, _responseErrorHandler);

            
        }

        //[TearDown]
        //public void Teardown()
        //{
        //    _context.Dispose();
        //}

        private void AddToContext(params Word[] word)
        {
            foreach (var w in word)
            {
                _context.Words.Add(w);
            }

            _context.SaveChanges();
        }

        #region HasWordAsync Tests

        [TestCase]
        public async Task HasWordAsync_ReturnsFalse_WhenContextHasNoWord()
        {
            //Arrange

            //Act
            var res  = await _service.HasWordAsync("word");
            //Assert

            Assert.That(res, Is.False);
        }

        [TestCase]
        public async Task HasWordAsync_ReturnsTrue_WhenContextHasWord()
        {
            //Arrange
            AddToContext(new Word() { Value = "word" });

            //Act
            var res = await _service.HasWordAsync("word");
            //Assert

           

            Assert.That(res, Is.True);
        }

        #endregion

        #region AddWordAsync Tests

        [TestCase]
        public async Task AddWordAsync_Adds()
        {
            //Arrange

            Assert.That(_context.Words.Any(x => x.Value == "word" ), Is.False);

            //Act
            await _service.AddWordAsync("word", null);

            //Assert
            Assert.That(_context.Words.Any(x=> x.Value == "word"), Is.True);
            
        }

        #endregion

        [TestCase]
        public async Task Generic_Test()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}