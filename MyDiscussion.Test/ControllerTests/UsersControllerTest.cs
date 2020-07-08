using Autofac;
using Microsoft.AspNetCore.Mvc;
using MyDiscussion.Controllers;
using MyDiscussion.Data;
using MyDiscussion.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MyDiscussion.Test.ControllerTests
{
    public class UsersControllerTest
    {
        [Fact]
        void CreateUser_returns_400_when_model_invalid()
        {
            // Arrange
            var container = ContainerCreator.CreateContainer();
            var scope = container.BeginLifetimeScope();

            var controller = scope.Resolve<UsersController>();

            // Act
            var result = controller.CreateUser(new UsersController.CreateUserModel
            {
                Username = "c",
                Password = "a1d2d21qa",
                Nickname = "realchr"
            });

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ModelInvalid", badResult.Value);
        }

        [Fact]
        void CreateUser_returns_400_when_username_exist()
        {
            // Arrange
            var container = ContainerCreator.CreateContainer();
            var scope = container.BeginLifetimeScope();

            var controller = scope.Resolve<UsersController>();
            var context = scope.Resolve<DiscussionContext>();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "chr",
                Password = "aaaaa32aaa",
                Nickname = "cccchr"
            };

            context.Users.Add(user);
            context.SaveChanges();

            // Act
            var result = controller.CreateUser(new UsersController.CreateUserModel
            {
                Username = "chr",
                Password = "a1d2d21qa",
                Nickname = "realchr"
            });

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("UsernameExist", badResult.Value);
        }

        [Fact]
        void CreateUser_returns_ok_when_everything_ok()
        {
            // Arrange
            var container = ContainerCreator.CreateContainer();
            var scope = container.BeginLifetimeScope();

            var controller = scope.Resolve<UsersController>();
            var context = scope.Resolve<DiscussionContext>();

            // Act

            var model = new UsersController.CreateUserModel
            {
                Username = "chr",
                Password = "a1d2d21qa",
                Nickname = "realchr"
            };

            var result = controller.CreateUser(model);

            // Assert
            Assert.IsType<OkResult>(result);

            var userInDatabase = context.Users.Where(p => p.Username == model.Username).FirstOrDefault();
            Assert.NotNull(userInDatabase);
            Assert.Equal(model.Username, userInDatabase.Username);
            Assert.Equal(model.Password, userInDatabase.Password);
            Assert.Equal(model.Nickname, userInDatabase.Nickname);
        }
    }
}
