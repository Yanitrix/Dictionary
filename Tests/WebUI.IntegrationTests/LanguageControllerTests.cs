using Domain.Dto;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace WebUI.IntegrationTests
{
    public class LanguageControllerTests : ControllerTestBase
    {

        [Fact]
        public async Task PostLanguage_ReturnsCreatedResponseWithCreatedLanguage()
        {
            var response = await client.PostAsJsonAsync("api/language", new CreateLanguageCommand
            {
                Name = "polish"
            });

            //var body = await response.Content.ReadFromJsonAsync<GetLanguage>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("/api/language/polish", response.Headers.Location.ToString());
            //Assert.Equal("polish", body.Name);
        }
    }
}
