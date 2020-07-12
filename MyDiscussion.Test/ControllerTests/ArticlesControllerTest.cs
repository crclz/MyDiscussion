using Autofac;
using Microsoft.AspNetCore.Mvc;
using MyDiscussion.Controllers;
using MyDiscussion.Data.Models;
using MyDiscussion.Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyDiscussion.Test.ControllerTests
{
	public class ArticlesControllerTest
	{
		#region Create Article Tests
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

		#endregion


		#region RemoveArticle Test

		// 未登录
		[Fact]
		void RemoveArticle_returns_unauthorized_when_not_login()
		{
			// rArrange
			var container = ContainerCreator.CreateContainer();
			var scope = container.BeginLifetimeScope();

			var controller = scope.Resolve<ArticlesController>();

			// Act
			var result = controller.RemoveArticle(Guid.NewGuid());

			// Assert
			Assert.IsType<UnauthorizedResult>(result);
		}


		// 文章不存在
		[Fact]
		void RemoveArticle_returns_notfound_when_article_not_exist()
		{
			// Arrange
			var container = ContainerCreator.CreateContainer();
			var scope = container.BeginLifetimeScope();

			var controller = scope.Resolve<ArticlesController>();
			var loginStatus = scope.Resolve<FakeLoginStatus>();

			loginStatus.RealUserId = Guid.NewGuid();

			// Act
			var result = controller.RemoveArticle(Guid.NewGuid());

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}


		// 文章不是自己的
		[Fact]
		void RemoveArticle_returns_forbidden_when_article_not_belong_to_current_user()
		{
			// Arrange
			var container = ContainerCreator.CreateContainer();
			var scope = container.BeginLifetimeScope();

			var controller = scope.Resolve<ArticlesController>();
			var loginStatus = scope.Resolve<FakeLoginStatus>();

			// Set userId of login status
			loginStatus.RealUserId = Guid.NewGuid();

			// add an article which not belongs to current user
			var article = new Article
			{
				Id = Guid.NewGuid(),
				Text = "asdasdasdas",
				ThumbCount = 0,
				Title = "asdasdasdad",
				UserId = Guid.NewGuid()// a random
			};
			controller.Context.Articles.Add(article);
			controller.Context.SaveChanges();

			// Act
			var result = controller.RemoveArticle(article.Id);

			// Assert
			Assert.IsType<ForbidResult>(result);
		}

		// 所有条件ok
		[Fact]
		void RemoveArticle_returns_ok_and_change_db_when_all_condition_ok()
		{
			// Arrange
			var container = ContainerCreator.CreateContainer();
			var scope = container.BeginLifetimeScope();

			var controller = scope.Resolve<ArticlesController>();
			var loginStatus = scope.Resolve<FakeLoginStatus>();

			// Set userId of login status
			loginStatus.RealUserId = Guid.NewGuid();

			// add an article which not belongs to current user
			var article = new Article
			{
				Id = Guid.NewGuid(),
				Text = "asdasdasdas",
				ThumbCount = 0,
				Title = "asdasdasdad",
				UserId = loginStatus.UserId
			};
			controller.Context.Articles.Add(article);
			controller.Context.SaveChanges();

			// Act
			var result = controller.RemoveArticle(article.Id);

			// Assert
			Assert.IsType<OkResult>(result);

			// Check database
			var articleInDatabase = controller.Context.Articles.FirstOrDefault(p => p.Id == article.Id);
			Assert.Null(articleInDatabase);
		}

		#endregion
	}
}
