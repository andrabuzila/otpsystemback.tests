namespace otpsystemback.Tests;
using otpsystemback.Services;
using otpsystemback.Repositories;
using otpsystemback.Data;
using otpsystemback.Models;
using otpsystemback.Models.ModelToEntity;
public class UnitTest1
{
    protected ApplicationDbContext DbContext { get; set; }

    [Fact]
    public void Test1()
    {
        //Arrange
        var registerUserModelToEntity = new RegisterUserModelToEntity();
        var userRepository = new UserRepository(DbContext);
        var jwtSettings = new JwtSettings();

        var target = new UserService(registerUserModelToEntity, userRepository, jwtSettings);

        //Act
        var result = target.GeneratePass();

        //Assert
        Assert.NotNull(result);
    }
}