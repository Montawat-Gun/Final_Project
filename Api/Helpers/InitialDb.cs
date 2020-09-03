using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Helpers
{
    public static class InitialDb
    {
        public static void Init(IApplicationBuilder application)
        {
            using(var serviceScope = application.ApplicationServices.CreateScope()){
                DataContext context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.Migrate();
            }
        }
    }
}