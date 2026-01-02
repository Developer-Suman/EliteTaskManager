using Web.CustomMiddleware.GlobalErrorHandling;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Web.Configs
{
    public static class ApplicationConfiguration
    {
        public static void Configure(WebApplication app)
        {
            //app.UseSerilogRequestLogging();
            app.Use((context, next) =>
            {
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                return next();
            });

            #region RedirectSwagger
            //Redirect request from the root Url to swagger UI
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger/index.html");
                    return;
                }

                await next();

            });
            #endregion

            #region HttpRequestPipeLine For Swagger
            // Enable Swagger
            if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "swagger"; // URL path to access Swagger (swagger/index.html)
                });
            }


            #endregion

            app.UseSession();

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            //app.UseSerilogRequestLogging();
           
            app.UseCors("AllowAllOrigins"); //UseCors must be placed after UseRouting an before UseAuthorization
            //This is to ensure the cors headers are included in the response for both authorized and unauthorized calls

            app.UseAuthentication();

            app.UseAuthorization();

            


            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();
        }
    }
}
