using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class JsonDeserializationExceptionMiddleware
{
	private readonly RequestDelegate _next;

	public JsonDeserializationExceptionMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (JsonException ex)
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.ContentType = "application/json";

			var errorResponse = new
			{
				Message = "Erro na desserialização do JSON",
				Details = ex.Message
			};

			await context.Response.WriteAsJsonAsync(errorResponse);
		}
		catch(Exception e)
		{
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.ContentType = "application/json";

			var errorResponse = new
			{
				Message = "Erro interno no servidor",
				Details = e.Message
			};

			await context.Response.WriteAsJsonAsync(errorResponse);
		}
	}
}
