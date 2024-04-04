
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;


namespace ZadanieRekrutacyjne.IntegrationTests
{
    public class TagsControllerTests
    {
        [Fact]
        public void GetAll_WithQueryParameteres_ReturnsOkResult()
        {
            //arange

            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();



            //act




            //assert


        }


    }
}
