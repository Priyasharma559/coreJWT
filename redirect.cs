🧱 Option 1 — Middleware returns JSON when AJAX detected

In your middleware or auth handler, detect if the request is AJAX or expects JSON.
If yes, return a 401 or 440 with redirect URL in JSON.

Example middleware logic:

public async Task InvokeAsync(HttpContext context)
{
    var tokenExpired = /* your token validation logic */;

    if (tokenExpired)
    {
        var isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        if (isAjax)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                redirectUrl = "https://login.example.com"
            });
            return;
        }
        else
        {
            context.Response.Redirect("https://login.example.com");
            return;
        }
    }

    await _next(context);
}
---------------------------------

🧱 Option 2 — Custom HTTP header (alternative)

If you don’t want to modify status codes, your middleware can return:

context.Response.Headers["X-Redirect-To"] = "https://login.example.com";
context.Response.StatusCode = 403;


Then in JavaScript:

error: function (xhr) {
    const redirectUrl = xhr.getResponseHeader("X-Redirect-To");
    if (redirectUrl) {
        window.location.href = redirectUrl;
    }
}
