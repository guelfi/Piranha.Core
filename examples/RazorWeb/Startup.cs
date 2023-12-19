namespace RazorWeb
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add custom policies
            services.AddAuthorization(o =>
            {
                // Read secured posts
                o.AddPolicy("ReadSecuredPosts", policy =>
                {
                    policy.RequireClaim("ReadSecuredPosts", "ReadSecuredPosts");
                });
            });
        }


    }
}