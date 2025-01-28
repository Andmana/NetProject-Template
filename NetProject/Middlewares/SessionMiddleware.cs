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
            var userId = context.Session.GetInt32("UserId");
            var requestPath = context.Request.Path.Value;

            // Log request path and userId for debugging purposes
            Console.WriteLine($"Request Path: {requestPath}, UserId: {userId}");

            if (context.Session.GetInt32("UserId") == null &&
                !(context.Request.Path.Value.Contains("/") || context.Request.Path.Value.Contains("/Auth")))
            {
                context.Response.Redirect("/");
                return;
            }
            // User Null goes to path
            // exclude equal "", "/"
            // exclude start with /Auth, /Home
            // redirect to home/privacy
            //if (userId == null &&
            //    requestPath != "/" &&  // Exclude root (home) path
            //    !string.IsNullOrEmpty(requestPath) &&  // Exclude empty path
            //    !requestPath.StartsWith("/Auth") &&   // Exclude Auth paths
            //    !requestPath.StartsWith("/Home"))     // Exclude Home paths
            //{
            //    // Redirect to "/Home/Privacy" if the user is not logged in
            //    context.Response.Redirect("/Home/Privacy");
            //    return;
            //}

            if (userId != null && requestPath.Equals("/Auth/Login"))
            {
                context.Response.Redirect("/");
                return;
            }



            await _next(context);
        }
    }
}
