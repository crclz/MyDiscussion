using Autofac;
using Microsoft.EntityFrameworkCore;
using MyDiscussion.Controllers;
using MyDiscussion.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDiscussion.Test
{
	class ContainerCreator
	{
		public static ContainerBuilder CreateBuilder()
		{
			var builder = new ContainerBuilder();

			builder.Register(
				c => new DbContextOptionsBuilder().UseInMemoryDatabase("dbtest").Options
				)
				.InstancePerLifetimeScope();

			builder.RegisterType<DiscussionContext>().InstancePerLifetimeScope();


			// Controllers
			builder.RegisterType<UsersController>().InstancePerLifetimeScope();
			builder.RegisterType<ArticlesController>().InstancePerLifetimeScope();


			return builder;
		}

		public static IContainer CreateContainer()
		{
			return CreateBuilder().Build();
		}
	}
}
