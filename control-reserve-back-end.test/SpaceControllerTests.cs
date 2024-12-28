using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using control_reserve_back_end.Controllers;
using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Dto.Responses;
using control_reserve_back_end.Interfaces;
using control_reserve_back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace control_reserve_back_end.test
{
    public class SpaceControllerTests
    {
        private readonly Mock<ISpaceService> _mockService;
        private readonly SpaceController _controller;

        public SpaceControllerTests()
        {
            _mockService = new Mock<ISpaceService>();
            _controller = new SpaceController(_mockService.Object);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WhenSpaceIsCreated()
        {
            // Arrange
            var spaceRequest = new CreateSpaceRequest { Name = "Space 1", Description = "Description 1" };
            var space = new Space { Id = 1, Name = "Space 1", Description = "Description 1" };
            _mockService.Setup(service => service.CreateSpaceAsync(spaceRequest)).ReturnsAsync(space);

            // Act
            var result = await _controller.Create(spaceRequest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Space>(actionResult.Value);
            Assert.Equal(space.Id, returnValue.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenSpaceIsDeleted()
        {
            // Arrange
            var spaceId = 1;
            _mockService.Setup(service => service.DeleteSpaceAsync(spaceId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(spaceId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithListOfSpaces()
        {
            // Arrange
            var spaces = new List<SpacesResponse> { new SpacesResponse { Id = 1, Name = "Space 1", Description = "Description 1" } };
            _mockService.Setup(service => service.GetAllSpacesAsync()).ReturnsAsync(spaces);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<SpacesResponse>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Update_ReturnsOkObjectResult_WhenSpaceIsUpdated()
        {
            // Arrange
            var spaceRequest = new UpdateSpaceRequest { Id = 1, Name = "Updated Space", Description = "Updated Description" };
            var space = new Space { Id = 1, Name = "Updated Space", Description = "Updated Description" };
            _mockService.Setup(service => service.UpdateSpaceAsync(spaceRequest)).ReturnsAsync(space);

            // Act
            var result = await _controller.Update(spaceRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Space>(okResult.Value);
            Assert.Equal(space.Id, returnValue.Id);
        }
    }
}
