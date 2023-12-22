using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Piranha;

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

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            App.Permissions["App"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Necessário Autenticação",
                Name = "ReadSecuredPosts"
            });

            app.Use(async (ctx, next) =>
            {
                // Verificar se o usuário está autenticado e possui a reivindicação "ReadSecuredPosts"
                //if (ctx.User.Identity.IsAuthenticated && !ctx.User.HasClaim("ReadSecuredPosts", "ReadSecuredPosts"))
                //{
                //    // Se o usuário não tiver a permissão necessária, definir o status 403 (Forbidden)
                //    ctx.Response.StatusCode = 403;
                //    return;
                //}

                if (ctx.Response.StatusCode == 401 && !ctx.User.HasClaim("ReadSecuredPosts", "ReadSecuredPosts"))
                {
                    ctx.Response.Redirect("/login");
                    return;
                }

                await next();
            });
         
            app.UsePiranha(options =>
            {
                options.UseManager(); 
                options.UseTinyMCE();
                options.UseIdentity();
            });
        }
    }
}