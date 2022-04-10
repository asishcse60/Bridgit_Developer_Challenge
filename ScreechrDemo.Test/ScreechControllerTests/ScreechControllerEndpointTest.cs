using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using ScreechrDemo.Api.Controllers;
using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Business.Core.Utils;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Utils;
using Xunit;

namespace ScreechrDemo.Api.Test.ScreechControllerTests
{
    public class ScreechControllerEndpointTest
    {
        private readonly Mock<IScreechService> _repositoryMock;
        private readonly Mock<ILogger<ScreechController>> _loggerMock;
        private readonly string _urlConnection;

        public ScreechControllerEndpointTest()
        {
            _repositoryMock = new Mock<IScreechService>();
            _loggerMock = new Mock<ILogger<ScreechController>>();
            _urlConnection = "http://localhost:8000";

        }

        [Fact]
        public async Task Test_PostScreechEndpointAsync()
        {
            //Assert
            var item = GetScreechItem();
            var sResult = new ScreechWrapperResult
            {
                Result = Result.SUCCESS,
                Screech = new Databases.Dtos.Screechr.Screech()
                {
                    UserProfileId = item.CreatorId,
                    Content = item.Content

                }
            };
            _repositoryMock.Setup(repo => repo.AddScreechAsync(It.IsAny<ScreechModel>())).ReturnsAsync(sResult);
            string callUri = _urlConnection + $"/api/v1/Screech/add";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(callUri).Respond("application/json", $"/screech/{item.CreatorId}");

            var httpClient = new HttpClient(mockHttp);
            httpClient.BaseAddress = new Uri(_urlConnection);
            var httpRequest = GetHttpMessageRequest(HttpMethod.Post, callUri, item);
            //Act
            var response = await httpClient.SendAsync(httpRequest);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //var json = await response.Content.ReadAsStringAsync();

        }

        [Fact]
        public async Task Test_GetScreechEndpointAsync()
        {
            //Assert
            var item = GetScreechItem();
            var sResult = new ScreechWrapperResult
            {
                Result = Result.SUCCESS,
                Screech = new Databases.Dtos.Screechr.Screech()
                {
                    UserProfileId = item.CreatorId,
                    Content = item.Content

                }
            };
            _repositoryMock.Setup(repo => repo.GetScreechrByIdAsync(It.IsAny<ulong>())).ReturnsAsync(sResult);
            string callUri = _urlConnection + $"/api/v1/Screech/get";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(callUri).Respond("application/json", "{'UserProfileId':'123456789','Content':'This is a screechr test content'}");

            var httpClient = new HttpClient(mockHttp);
            httpClient.BaseAddress = new Uri(_urlConnection);
            var httpRequest = GetHttpMessageRequest(HttpMethod.Get, callUri, item);
            //Act
            var response = await httpClient.SendAsync(httpRequest);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //var json = await response.Content.ReadAsStringAsync();

        }
        [Fact]
        public async Task Test_UpdateScreechEndpointAsync()
        {
            //Assert
            var item = GetScreechItem();
            var updateModel = new UpdateScreechModel()
            {
                Text = "Hello, Updated!"
            };
            var sResult = new ScreechWrapperResult
            {
                Result = Result.SUCCESS,
               
            };
            _repositoryMock.Setup(repo => repo.UpdateScreechrContentByIdAsync(item.CreatorId, updateModel)).ReturnsAsync(sResult);
            string callUri = _urlConnection + $"/api/v1/Screech/update-text";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(callUri).Respond("application/json", "");

            var httpClient = new HttpClient(mockHttp);
            httpClient.BaseAddress = new Uri(_urlConnection);
            var httpRequest = GetHttpMessageRequest(HttpMethod.Patch, callUri, item);
            //Act
            var response = await httpClient.SendAsync(httpRequest);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //var json = await response.Content.ReadAsStringAsync();

        }
        private HttpRequestMessage GetHttpMessageRequest(HttpMethod httpMethod, string callUri, object obj)
        {
            var request = new HttpRequestMessage();
            request.Method = httpMethod;
            request.Content = JsonContent.Create(obj);
            request.RequestUri = new Uri(callUri);
            return request;
        }
        private ScreechModel GetScreechItem()
        {
            return new ScreechModel()
            {
                Content = "This is a screechr test content",
                CreatorId = 123456789
            };
        }
    }
}
