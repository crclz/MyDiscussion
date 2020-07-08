using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDiscussion.Core;
using MyDiscussion.Data;
using MyDiscussion.Data.Models;

namespace MyDiscussion.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArticlesController : ControllerBase
	{
		public DiscussionContext Context { get; }
		public ILoginStatus LoginStatus { get; }

		public ArticlesController(DiscussionContext context, ILoginStatus loginStatus)
		{
			Context = context;
			LoginStatus = loginStatus;
		}

		[HttpPost]
		public ActionResult CreateArticle([FromBody] CreateArticleModel model)
		{
			if (!ApiUtils.IsModelValid(model))
				return BadRequest("模型无效");

			if (!LoginStatus.IsLoggedIn)
			{
				return Unauthorized();
			}

			// 创建用户对象
			var article = new Article
			{
				Id = Guid.NewGuid(),
				Title = model.Title,
				Text = model.Text,
				ThumbCount = 0,
				UserId = LoginStatus.UserId //
			};

			// 保存用户对象
			Context.Articles.Add(article);
			Context.SaveChanges();

			return Ok(article.Id);
		}

		public class CreateArticleModel
		{
			[Required]
			[MinLength(3)]
			[MaxLength(255)]
			public string Title { get; set; }

			[Required]
			[MinLength(6)]
			[MaxLength(4096)]
			public string Text { get; set; }

		}
	}
}
