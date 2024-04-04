
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
        [Fact]
        public async Task GetList_WithQueryParameteres_ReturnsOkResult()
        {
            //arange

            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();



            //act

            var response = await client.GetAsync("/api/tags/list?currentPage=1&sortBy=name&sortOrder=asc");


            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }


    }
}
