using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ScreechrDemo.Api.Controllers;
using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Business.Core.Utils;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Dtos.User;
using ScreechrDemo.Databases.Utils;
using Xunit;

namespace ScreechrDemo.Api.Test.UserControllerTests
{
    public class UserControllerTest
    {
        //don't test multiple logic in one test
        private readonly Mock<IUserProfileService> _userProfileService;
        private readonly Mock<ILogger<UserProfileController>> _loggerMock;
        private readonly UserProfileController _userprofileController;

        public UserControllerTest()
        {
            _userProfileService = new Mock<IUserProfileService>();
            _loggerMock = new Mock<ILogger<UserProfileController>>();
            _userprofileController = new UserProfileController(_userProfileService.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddUserAsync()
        {
            //Arrange
            var item = GetUser();
            var sResult = new UserResultWrapper()
            {
                ResultStatus = Result.SUCCESS,
                UserProfile = new UserProfile()
                {
                    Id = item.Id
                }
            };
            _userProfileService.Setup(repo => repo.AddUserAsync(item))
                .ReturnsAsync(sResult);
            //Act
            var result = await _userprofileController.AddUser(item);
            var actionResult = result as CreatedResult;

            Xunit.Assert.NotNull(actionResult);

            //Assert
            Xunit.Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);//optional

        }


        [Fact]
        public async Task GetUserProfileAsync_OK()
        {
            //Arrange
            var item = GetUser();
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.SUCCESS,
            };
            _userProfileService.Setup(repo => repo.GetUserProfileByKeyAsync(item.Id))
                .ReturnsAsync(sResult);
            //Act
            var result = await _userprofileController.GetUserProfile(item.Id);
            var actionResult = result as OkObjectResult;

            Xunit.Assert.NotNull(actionResult);

            //Assert
            Xunit.Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);//optional

        }
        [Fact]
        public async Task GetUserProfileAsync_NotFound()
        {
            //Arrange
            var item = GetUser();
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.PROFILE_NOT_FOUND,
            };
            _userProfileService.Setup(repo => repo.GetUserProfileByKeyAsync(item.Id))
                .ReturnsAsync(sResult);
            //Act
            var result = await _userprofileController.GetUserProfile(item.Id);
            var actionResult = result as BadRequestResult;

            Xunit.Assert.NotNull(actionResult);

            //Assert
            Xunit.Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);//optional

        }

        [Fact]
        public async Task UpdateProfileAsync_OK()
        {
            //Arrange
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

            _userProfileService.Setup(repo => repo.UpdateProfileAsync(updateModel, item.Id))
                .ReturnsAsync(sResult);
            //Act
            var result = await _userprofileController.UpdateProfile(updateModel, item.Id);


            var actionResult = result as NoContentResult;

            Xunit.Assert.NotNull(actionResult);

            //Assert
            Xunit.Assert.Equal((int)HttpStatusCode.NoContent, actionResult.StatusCode);//optional

        }
        [Fact]
        public async Task UpdateProfileAsync_NotFound()
        {
            var item = GetUser();
            var updateModel = new UpdateProfileModel()
            {
                FirstName = "Asish1",
                LastName = "Pal1"
            };
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.PROFILE_NOT_FOUND
            };

            _userProfileService.Setup(repo => repo.UpdateProfileAsync(updateModel, item.Id))
                .ReturnsAsync(sResult);
            //Act
            var result = await _userprofileController.UpdateProfile(updateModel, item.Id);


            var actionResult = result as BadRequestResult;

            Xunit.Assert.NotNull(actionResult);

            //Assert
            Xunit.Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);//optional

        }
        [Fact]
        public async Task UpdateProfilePictureAsync_OK()
        {
            //Arrange
            var item = GetUser();
            var updateModel = new UpdateProfileImageModel()
            {
               ProfileImageUri = "/asish/img/a.png"
            };
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.SUCCESS
            };

            _userProfileService.Setup(repo => repo.UpdateProfilePictureAsync(item.Id, updateModel))
                .ReturnsAsync(sResult);
            //Act
            var result = await _userprofileController.UpdateProfilePicture(updateModel, item.Id);


            var actionResult = result as NoContentResult;

            Xunit.Assert.NotNull(actionResult);

            //Assert
            Xunit.Assert.Equal((int)HttpStatusCode.NoContent, actionResult.StatusCode);//optional

        }
        [Fact]
        public async Task UpdateProfilePictureAsync_NotFound()
        {
            //Arrange
            var item = GetUser();
            var updateModel = new UpdateProfileImageModel()
            {
                ProfileImageUri = "/asish/img/a.png"
            };
            var sResult = new UserResultWrapper
            {
                ResultStatus = Result.PROFILE_NOT_FOUND
            };

            _userProfileService.Setup(repo => repo.UpdateProfilePictureAsync(item.Id, updateModel))
                .ReturnsAsync(sResult);
            //Act
            var result = await _userprofileController.UpdateProfilePicture(updateModel, item.Id);


            var actionResult = result as BadRequestResult;

            Xunit.Assert.NotNull(actionResult);

            //Assert
            Xunit.Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);//optional

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
