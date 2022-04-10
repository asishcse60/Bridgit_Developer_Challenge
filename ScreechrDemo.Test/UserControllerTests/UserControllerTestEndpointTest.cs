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
using ScreechrDemo.Databases.Dtos.User;
using ScreechrDemo.Databases.Utils;
using Xunit;

namespace ScreechrDemo.Api.Test.UserControllerTests
{
    public class UserControllerTestEndpointTest
    {
        private readonly Mock<IUserProfileService> _repositoryMock;
        private readonly Mock<ILogger<UserProfileController>> _loggerMock;
        private readonly string _urlConnection;

        public UserControllerTestEndpointTest()
        {
            _repositoryMock = new Mock<IUserProfileService>();
            _loggerMock = new Mock<ILogger<UserProfileController>>();
            _urlConnection = "http://localhost:8000";
        }

        [Fact]
        public async Task Test_PostUserEndpointAsync()
        {
            //Assert
            var item = GetUser();
            var sResult = new UserResultWrapper()
            {
                ResultStatus = Result.SUCCESS,
                UserProfile = new UserProfile()
                {
                    Id = item.Id
                }
            };
            _repositoryMock.Setup(repo => repo.AddUserAsync(item)).ReturnsAsync(sResult);
            string callUri = _urlConnection + $"/api/v1/UserProfile/add";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(callUri).Respond("application/json", $"/profile/{sResult.UserProfile.Id}");

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
        public async Task Test_GetUserEndpointAsync()
        {
            //Assert
            var item = GetUser();
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.SUCCESS,
            };
            _repositoryMock.Setup(repo => repo.GetUserProfileByKeyAsync(It.IsAny<ulong>())).ReturnsAsync(sResult);
            string callUri = _urlConnection + $"/api/v1/UserProfile/get";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(callUri).Respond("application/json", "{'Id':123456789,'UserName':'Asish Pal', 'FirstName':'Asish', 'LastName': 'Pal'}");

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
        public async Task Test_UpdateUserEndpointAsync()
        {
            //Assert
            var item = GetUser();
            var updateModel = new UpdateProfileModel()
            {
                FirstName = "Asish1",
                LastName = "Pal1"
            };
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.SUCCESS
            };
            _repositoryMock.Setup(repo => repo.UpdateProfileAsync(updateModel, item.Id)).ReturnsAsync(sResult);
            string callUri = _urlConnection + $"/api/v1/UserProfile/update";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(callUri).Respond("application/json", "");

            var httpClient = new HttpClient(mockHttp);
            httpClient.BaseAddress = new Uri(_urlConnection);
            var httpRequest = GetHttpMessageRequest(HttpMethod.Put, callUri, item);
            //Act
            var response = await httpClient.SendAsync(httpRequest);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //var json = await response.Content.ReadAsStringAsync();

        }
        [Fact]
        public async Task Test_UpdateUserPictureEndpointAsync()
        {
            //Assert
            var item = GetUser();
            var updateModel = new UpdateProfileImageModel()
            {
                ProfileImageUri = "/asish/img/a.png"
            };
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.SUCCESS
            };
            _repositoryMock.Setup(repo => repo.UpdateProfilePictureAsync(item.Id, updateModel)).ReturnsAsync(sResult);
            string callUri = _urlConnection + $"/api/v1/UserProfile/update-image";

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
        private UserModel GetUser()
        {
            return new UserModel()
            {
                Id = 123456789,
                UserName = "Asish Pal",
                FirstName = "Asish",
                LastName = "Pal",
                SecretToken = "YXNpc2g6c2VjcmV0MTIz"

            };
        }

    }
}
