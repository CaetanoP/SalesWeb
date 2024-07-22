using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SalesWebMVc.Services.Exceptions;
using System;

namespace SalesWebMVc.Filter
{
	public class CustomExceptionFilter : IAsyncExceptionFilter
	{
		public async Task OnExceptionAsync(ExceptionContext context)
		{

			switch (context.Exception)
			{
				case NotFoundException e:
					context.Result = new NotFoundObjectResult(e.Message);
					break;
				case IntegrityException e:
					context.Result = new BadRequestObjectResult(e.Message);
					break;
				case DbConcurrencyException e:
					context.Result = new BadRequestObjectResult(e.Message);
					break;
				case BadRequestException e:
					context.Result = new BadRequestObjectResult(e.Message);
					break;
				case Exception e:
					context.Result = new BadRequestObjectResult(e.Message);
					break;
				default:
					context.Result = new BadRequestObjectResult(context.Exception.Message);
					break;
			}
			context.ExceptionHandled = true; 
			await Task.CompletedTask;
		}
	}

}
