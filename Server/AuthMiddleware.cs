namespace Server;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

	public AuthMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var request = context.Request;

		if (request.Path.StartsWithSegments("/chat")
			&& request.Query.TryGetValue("access_token", out var accessToken))
		{
			request.Headers.Add("Authorization", $"Bearer {accessToken}");
		}

		await _next(context);
	}
}
