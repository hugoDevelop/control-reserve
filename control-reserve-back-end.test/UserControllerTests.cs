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
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WhenUserIsCreated()
        {
            // Arrange
            var userRequest = new CreateUserRequest { Name = "User 1", Email = "user1@example.com" };
            var user = new User { Id = 1, Name = "User 1", Email = "user1@example.com" };
            _mockService.Setup(service => service.CreateUserAsync(userRequest)).ReturnsAsync(user);

            // Act
            var result = await _controller.Create(userRequest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<User>(actionResult.Value);
            Assert.Equal(user.Id, returnValue.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenUserIsDeleted()
        {
            // Arrange
            var userId = 1;
            _mockService.Setup(service => service.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UsersResponse> { new UsersResponse { Id = 1, Name = "User 1", Email = "user1@example.com" } };
            _mockService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UsersResponse>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Update_ReturnsOkObjectResult_WhenUserIsUpdated()
        {
            // Arrange
            var userRequest = new UpdateUserRequest { Id = 1, Name = "Updated User", Email = "updateduser@example.com" };
            var user = new User { Id = 1, Name = "Updated User", Email = "updateduser@example.com" };
            _mockService.Setup(service => service.UpdateUserAsync(userRequest)).ReturnsAsync(user);

            // Act
            var result = await _controller.Update(userRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user.Id, returnValue.Id);
        }
    }
}
