using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using otpsystemback.Repositories;
using otpsystemback.Data;
using otpsystemback.Data.Entities;
using otpsystemback.Validators;
using otpsystemback.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace otpsystemback.Tests;
public class UserValidatorTests
{
    private UserRepository userRepository;
    protected ApplicationDbContext DbContext { get; set; }
    public void Setup()
    {
        this.userRepository = new UserRepository(DbContext);
    }

    [Fact]
    public void ValidateOTP_ValidPass_ReturnsOk()
    {
        //arrange
        var userValidator = new UserValidator(userRepository);
        var validPassword = "ValidPassword123!";

        //act
        var result = userValidator.ValidateOTP(validPassword);

        //assert
        Assert.IsType<OkResult>(result);
    }

    [Theory]
    [InlineData("abc", "Password length not ok")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Password length not ok")]
    public void ValidateOTP_InvalidLength_ReturnsBadRequest(string password, string expectedErrorMessage)
    {
        // Arrange
        var userValidator = new UserValidator(userRepository); 

        // Act
        var result = userValidator.ValidateOTP(password);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Equal(expectedErrorMessage, badRequestResult.Value);
    }

    [Theory]
    [InlineData("password", "Password does not contain Upper Cases")]
    [InlineData("PASSWORD", "Password does not contain Lower Cases")]
    [InlineData("Password123", "Password does not contain Punctuations")]
    [InlineData("Password!@#", "Password does not contain Numbers")]
    public void ValidateOTP_InvalidCharacters_ReturnsBadRequest(string password, string expectedErrorMessage)
    {
        // Arrange
        var userValidator = new UserValidator(userRepository); 

        // Act
        var result = userValidator.ValidateOTP(password);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Equal(expectedErrorMessage, badRequestResult.Value);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("test123@gmail.com")]
    [InlineData("test.new@company.com")]
    public void ValidateUserEmail_ValidEmail_ReturnsEmptyString(string email)
    {
        // Arrange
        var userValidator = new UserValidator(userRepository); 

        // Act
        var result = userValidator.ValidateUserEmail(email);

        // Assert
        Assert.Equal("", result);
    }

    [Theory]
    [InlineData("invalidemail")]
    [InlineData("invalidemail.com")]
    [InlineData("invalidemail@")]
    public void ValidateUserEmail_InvalidEmail_ReturnsErrorMessage(string email)
    {
        // Arrange
        var userValidator = new UserValidator(userRepository);

        // Act
        var result = userValidator.ValidateUserEmail(email);

        // Assert
        Assert.Equal("Email is wrong", result);
    }

    [Fact]
    public void CheckIfEmailExist_EmailExists_ReturnsTrue()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.Get())
                          .Returns(new List<User> { new User { Email = "test@example.com" } }.AsQueryable());
        var userValidator = new UserValidator(mockUserRepository.Object); 

        // Act
        var result = userValidator.CheckIfEmailExist("test@example.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckIfEmailExist_EmailDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.Get())
                          .Returns(new List<User>().AsQueryable());
        var userValidator = new UserValidator(mockUserRepository.Object); 

        // Act
        var result = userValidator.CheckIfEmailExist("nonexistent@example.com");

        // Assert
        Assert.False(result);
    }
}

