
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace ZadanieRekrutacyjne.IntegrationTests
{
 
    public class TagsControllerTests
    {
        [Theory]
        [InlineData("currentPage=1&sortBy=name&sortOrder=asc")]
        [InlineData("currentPage=1&sortBy=percentage&sortOrder=asc")]
        [InlineData("currentPage=1&sortBy=name&sortOrder=desc")]
        [InlineData("currentPage=3&sortBy=percentage&sortOrder=desc")]

        public async Task GetList_WithQueryParameteres_ReturnsOkResult(string QueryParams)
        {
            //arange

            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            //act

            var response = await client.GetAsync("/api/tags/list?"+ QueryParams);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
              
        [Fact]
        public async Task GetItem_ById_ReturnsOkResult()
        {
            //arange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            //act
            var response = await client.GetAsync("/api/tags/1");
            //asset
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK); 

        }
    }
}
