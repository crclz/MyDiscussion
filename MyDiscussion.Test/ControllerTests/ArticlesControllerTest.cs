using Autofac;
using Microsoft.AspNetCore.Mvc;
using MyDiscussion.Controllers;
using MyDiscussion.Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

		[Fact]
		void CreateArticle_returns_ok_when_ok_with_id_and_saves_article()
		{
			// Arrange
			var container = ContainerCreator.CreateContainer();
			var scope = container.BeginLifetimeScope();

			var controller = scope.Resolve<ArticlesController>();
			var loginStatus = scope.Resolve<FakeLoginStatus>();
			loginStatus.RealUserId = Guid.NewGuid();

			// Act
			var model = new ArticlesController.CreateArticleModel
			{
				Title = "title23213",
				Text = "asdasdasdasdasd"
			};
			var result = controller.CreateArticle(model);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var articleId = Assert.IsType<Guid>(okResult.Value);

			var article = controller.Context.Articles.Where(p => p.Id == articleId).FirstOrDefault();
			Assert.NotNull(article);
			Assert.Equal(articleId, article.Id);
			Assert.Equal(model.Title, article.Title);
			Assert.Equal(model.Text, article.Text);
			Assert.Equal(loginStatus.UserId, article.UserId);
		}
	}
}
