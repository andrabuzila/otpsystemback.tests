using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using otpsystemback.Services;
using otpsystemback.Repositories;
using otpsystemback.Data;
using otpsystemback.Models;
using otpsystemback.Models.ModelToEntity;

namespace otpsystemback.Tests;

public class UserServiceTests
{
    protected ApplicationDbContext DbContext { get; set; }
    private JwtSettings jwtSettings;
    private UserRepository userRepository;
    private RegisterUserModelToEntity registerUserModelToEntity;
    public void Setup()
    {
        this.jwtSettings = new JwtSettings();
        this.userRepository = new UserRepository(DbContext);
        this.registerUserModelToEntity = new RegisterUserModelToEntity();
    }

    [Fact]
    public void GeneratePass_ReturnsValidPass()
    {
        //Arrange
        Setup();
        var target = new UserService(registerUserModelToEntity, userRepository, jwtSettings);

        //Act
        var result = target.GeneratePass();

        //Assert
        Assert.NotNull(result);

    }

    [Fact]
    public void GeneratePass_ReturnsNotEmptyPass()
    {
        //Arrange
        Setup();
        var target = new UserService(registerUserModelToEntity, userRepository, jwtSettings);

        //Act
        var result = target.GeneratePass();

        //Assert
        Assert.NotEmpty(result);

    }

    [Fact]
    public void GenerateToken_ReturnsNonEmptyToken()
    {
        //Arrange
        Setup();
        var target = new UserService(this.registerUserModelToEntity, this.userRepository, this.jwtSettings);

        //Act
        var result = target.GenerateToken("testPassword1-");

        //Assert
        Assert.NotEmpty(result);

    }

    [Fact]
    public void GenerateToken_ReturnsEmptyToken()
    {
        //Arrange
        Setup();
        var target = new UserService(registerUserModelToEntity, userRepository, jwtSettings);

        //Act
        var result = target.GenerateToken(null);

        //Assert
        Assert.Empty(result);

    }
}
