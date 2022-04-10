using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ScreechrDemo.Api.Controllers;
using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Business.Core.Utils;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Utils;
using Xunit;
using Assert = Xunit.Assert;

namespace ScreechrDemo.Api.Test.ScreechControllerTests
{
    public class ScreechControllerTest
    {
        //don't test multiple logic in one test
        private readonly Mock<IScreechService> _screechService;
        private readonly Mock<ILogger<ScreechController>> _loggerMock;
        private readonly ScreechController _screechController;

        public ScreechControllerTest()
        {
            _screechService = new Mock<IScreechService>();
            _loggerMock = new Mock<ILogger<ScreechController>>();
            _screechController = new ScreechController(_screechService.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddScreechAsync()
        {
            //Arrange
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
            _screechService.Setup(repo => repo.AddScreechAsync(item))
                .ReturnsAsync(sResult);
            //Act
            var result = await _screechController.AddScreech(item);
            var actionResult = result as CreatedResult;

            Assert.NotNull(actionResult);

            //Assert
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);//optional

        }

        [Fact]
        public async Task GetScreechAsync_OK()
        {
            //Arrange
            var item = GetScreechItem();
            var sResult = new ScreechWrapperResult
            {
                Result = Result.SUCCESS,
            };
            _screechService.Setup(repo => repo.GetScreechrByIdAsync(item.CreatorId))
                .ReturnsAsync(sResult);
            //Act
            var result = await _screechController.GetScreech(item.CreatorId);
            var actionResult = result as OkObjectResult;

            Assert.NotNull(actionResult);

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);//optional

        }
        [Fact]
        public async Task GetScreechAsync_NotFound()
        {
            //Arrange
            var item = GetScreechItem();
            var sResult = new ScreechWrapperResult
            {
                Result = Result.Screech_NOT_FOUND,
            };

            _screechService.Setup(repo => repo.GetScreechrByIdAsync(item.CreatorId))
                .ReturnsAsync(sResult);
            //Act
            var result = await _screechController.GetScreech(item.CreatorId);
            

            var actionResult = result as BadRequestResult;

            Assert.NotNull(actionResult);

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);//optional

        }

        [Fact]
        public async Task UpdateScreechAsync_OK()
        {
            //Arrange
            var item = GetScreechItem();
            var updateModel = new UpdateScreechModel()
            {
                Text = "Hello, Updated!"
            };
            var sResult = new ScreechWrapperResult
            {
                Result = Result.SUCCESS
            };

            _screechService.Setup(repo => repo.UpdateScreechrContentByIdAsync(item.CreatorId, updateModel))
                .ReturnsAsync(sResult);
            //Act
            var result = await _screechController.UpdateScreech(updateModel, item.CreatorId);


            var actionResult = result as NoContentResult;

            Assert.NotNull(actionResult);

            //Assert
            Assert.Equal((int)HttpStatusCode.NoContent, actionResult.StatusCode);//optional

        }
        [Fact]
        public async Task UpdateScreechAsync_NotFound()
        {
            //Arrange
            var item = GetScreechItem();
            var updateModel = new UpdateScreechModel();
            var sResult = new ScreechWrapperResult
            {
                Result = Result.Screech_NOT_FOUND
            };

            _screechService.Setup(repo => repo.UpdateScreechrContentByIdAsync(item.CreatorId, updateModel))
                .ReturnsAsync(sResult);
            //Act
            var result = await _screechController.UpdateScreech(updateModel, item.CreatorId);


            var actionResult = result as BadRequestResult;

            Assert.NotNull(actionResult);

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);//optional

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
