using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ZadanieRekrutacyjne.Controllers;
using ZadanieRekrutacyjne.Model;
using ZadanieRekrutacyjne.Services;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;

namespace ZadanieRekrutacyjne.Tests
{
    public class TagControllerTests
    {
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly Mock<TagContext> _tagContextMock;
        private readonly Mock<ITagApiConfiguration> _tagApiConfigurationMock;
        private readonly Mock<ILogger<TagController>> _loggerMock;

        public TagControllerTests()
        {
            _httpClientMock = new Mock<HttpClient>();
            _tagContextMock = new Mock<TagContext>();
            _tagApiConfigurationMock = new Mock<ITagApiConfiguration>();
            _loggerMock = new Mock<ILogger<TagController>>();
        }
        [Fact]
        public void Can_Add_Tag()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TagContext>()
                .UseInMemoryDatabase(databaseName: "Can_Add_Tag_Database")
                .Options;

            // Act
            using (var context = new TagContext(options))
            {
                context.Tags.Add(new Tag { Name = "Test Tag", Count = 10, Percentage = 5.0 });
                context.SaveChanges();
            }

            // Assert
            using (var context = new TagContext(options))
            {
                var tag = context.Tags.Find(1);
                Assert.NotNull(tag);
                Assert.Equal("Test Tag", tag.Name);
            }
        }

        [Fact]
        public async Task GetTags_Returns_Sorted_TagList_By_Percentage()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TagContext>()
                .UseInMemoryDatabase(databaseName: "GetTags_Returns_Sorted_TagList_By_Percentage")
                .Options;
            using (var dbContext = new TagContext(options))
            {
                dbContext.Tags.AddRange(new List<Tag>
        {
            new Tag { Name = "Tag1", Count = 10, Percentage = 5.0 },
            new Tag { Name = "Tag2", Count = 20, Percentage = 3.0 },
            new Tag { Name = "Tag3", Count = 30, Percentage = 7.0 }
        });
                dbContext.SaveChanges();

                var controller = new TagController(_httpClientMock.Object, dbContext, _tagApiConfigurationMock.Object, _loggerMock.Object);

                // Act
                var result = await controller.GetTags(sortBy: "percentage");

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var model = okResult.Value as dynamic;
                var tags = model.Tags as List<Tag>;

                // Sort expected and actual tags lists by percentage
                var expectedSortedByPercentage = dbContext.Tags.OrderBy(tag => tag.Percentage).ToList();
                var actualSortedByPercentage = tags.OrderBy(tag => tag.Percentage).ToList();

                Assert.Equal(expectedSortedByPercentage, actualSortedByPercentage, new TagComparer());
            }
        }

        public class TagComparer : IEqualityComparer<Tag>
        {
            public bool Equals(Tag x, Tag y)
            {
                return x.Id == y.Id && x.Name == y.Name && x.Percentage == y.Percentage;
            }

            public int GetHashCode(Tag obj)
            {
                return obj.Id.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.Percentage.GetHashCode();
            }
        }


        [Fact]
        public async Task GetTags_Returns_OkObjectResult_With_TagList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TagContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new TagContext(options))
            {
                context.Tags.Add(new Tag { Name = "Test Tag 1", Count = 10, Percentage = 5.0 });
                context.Tags.Add(new Tag { Name = "Test Tag 2", Count = 20, Percentage = 10.0 });
                await context.SaveChangesAsync();
            }

            var controller = new TagController(null, new TagContext(options), null, NullLogger<TagController>.Instance);

            // Act
            var result = await controller.GetTags();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<object>(okResult.Value);

            dynamic response = model;
            var totalRecords = (int)response.TotalRecords;
            var totalPages = (int)response.TotalPages;
            var currentPage = (int)response.CurrentPage;
            var tags = (List<Tag>)response.Tags; 

            Assert.Equal(2, totalRecords);
            Assert.Equal(1, totalPages);
            Assert.Equal(1, currentPage);
            Assert.Equal(2, tags.Count); 
            Assert.Equal("Test Tag 1", tags[0].Name);
            Assert.Equal(10, tags[0].Count);
            Assert.Equal(5.0, tags[0].Percentage);
            Assert.Equal("Test Tag 2", tags[1].Name);
            Assert.Equal(20, tags[1].Count);
            Assert.Equal(10.0, tags[1].Percentage);
        }




    }
}
