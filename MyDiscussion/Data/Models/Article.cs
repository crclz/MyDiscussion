using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiscussion.Data.Models
{
	public class Article
	{

		[Key]
		public Guid Id { get; set; }

		public Guid UserId { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public int ThumbCount { get; set; }


	}
}
