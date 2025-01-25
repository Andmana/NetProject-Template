namespace NetProject.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Session.GetInt32("UserId") == null &&
                !(context.Request.Path.Value.Contains("/") || context.Request.Path.Value.Contains("/Auth")))
            {
                context.Response.Redirect("/");
                return;
            }

            await _next(context);
        }
    }
}
