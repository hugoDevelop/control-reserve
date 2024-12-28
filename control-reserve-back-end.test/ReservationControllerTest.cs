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
    public class ReservationsControllerTests
    {
        private readonly Mock<IReservationService> _mockService;
        private readonly ReservationsController _controller;

        public ReservationsControllerTests()
        {
            _mockService = new Mock<IReservationService>();
            _controller = new ReservationsController(_mockService.Object);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WhenReservationIsCreated()
        {
            // Arrange
            var reservationRequest = new ReservationRequest { Id = 1, UserId = 1, SpaceId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            var reservation = new Reservation { Id = 1, UserId = 1, SpaceId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            _mockService.Setup(service => service.CreateReservation(reservationRequest)).ReturnsAsync(reservation);

            // Act
            var result = await _controller.Create(reservationRequest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Reservation>(actionResult.Value);
            Assert.Equal(reservation.Id, returnValue.Id);
        }

        [Fact]
        public async Task Cancel_ReturnsNoContentResult_WhenReservationIsCancelled()
        {
            // Arrange
            var reservationId = 1;
            _mockService.Setup(service => service.CancelReservation(reservationId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Cancel(reservationId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithListOfReservations()
        {
            // Arrange
            var reservations = new List<ReservationResponse> { new ReservationResponse { Id = 1, User = "User 1", Space = "Space 1", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) } };
            _mockService.Setup(service => service.GetReservations(null, null, null, null)).ReturnsAsync(reservations);

            // Act
            var result = await _controller.Get(null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ReservationResponse>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetSpaces_ReturnsOkObjectResult_WithListOfSpaces()
        {
            // Arrange
            var spaces = new List<SpacesResponse> { new SpacesResponse { Id = 1, Name = "Space 1" } };
            _mockService.Setup(service => service.GetSpaces()).ReturnsAsync(spaces);

            // Act
            var result = await _controller.GetSpaces();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<SpacesResponse>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkObjectResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UsersResponse> { new UsersResponse { Id = 1, Name = "User 1" } };
            _mockService.Setup(service => service.GetUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UsersResponse>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task CreateReservation_ThrowsException_WhenUserHasOtherReservationAtSameTime()
        {
            // Arrange
            var reservationRequest = new ReservationRequest { UserId = 1, SpaceId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            _mockService.Setup(service => service.CreateReservation(reservationRequest)).ThrowsAsync(new Exception("El usuario ya tiene otra reserva en este horario."));

            // Act
            var result = await _controller.Create(reservationRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = badRequestResult.Value;
            var messageProperty = errorResponse?.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(errorResponse, null);
            Assert.Equal("El usuario ya tiene otra reserva en este horario.", messageValue);
        }

        [Fact]
        public async Task CreateReservation_ThrowsException_WhenReservationOverlaps()
        {
            // Arrange
            var reservationRequest = new ReservationRequest { UserId = 1, SpaceId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
            _mockService.Setup(service => service.CreateReservation(reservationRequest)).ThrowsAsync(new Exception("El espacio ya está reservado en este horario."));

            // Act
            var result = await _controller.Create(reservationRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = badRequestResult.Value;
            var messageProperty = errorResponse?.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(errorResponse, null);
            Assert.Equal("El espacio ya está reservado en este horario.", messageValue);
        }
    }
}
