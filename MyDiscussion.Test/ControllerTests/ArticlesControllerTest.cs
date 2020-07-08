using Autofac;
using Microsoft.AspNetCore.Mvc;
using MyDiscussion.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyDiscussion.Test.ControllerTests
{
	public class ArticlesControllerTest
	{
		[Fact]
		void CreateArticle_returns_400_when_model_invalid()
		{
			// Arrange
			var container = ContainerCreator.CreateContainer();
			var scope = container.BeginLifetimeScope();

			var controller = scope.Resolve<ArticlesController>();

			// Act
			var result = controller.CreateArticle(new ArticlesController.CreateArticleModel
			{
				Title = "",
				Text = ""
			});

			// Assert
			var badResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("模型无效", badResult.Value);
		}

		[Fact]
		void CreateArticle_returns_unauthorized_when_not_login()
		{
			// Arrange
			var container = ContainerCreator.CreateContainer();
			var scope = container.BeginLifetimeScope();

			var controller = scope.Resolve<ArticlesController>();

			// Act
			var result = controller.CreateArticle(new ArticlesController.CreateArticleModel
			{
				Title = "title23213",
				Text = "asdasdasdasdasd"
			});

			// Assert
			Assert.IsType<UnauthorizedResult>(result);
		}
	}
}
