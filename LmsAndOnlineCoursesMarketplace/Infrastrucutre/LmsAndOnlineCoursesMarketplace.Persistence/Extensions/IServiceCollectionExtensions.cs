// using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace LmsAndOnlineCoursesMarketplace.Persistence.Extensions;
//
// public class IServiceCollectionExtensions
// {
//     public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
//     {
//         services.AddDbContext(configuration);
//         services.AddRepositories();
//     }
//     
//     public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
//     {
//         var connectionString = configuration.GetConnectionString("DefaultConnection");
//         
//         services.AddDbContext<ApplicationDbContext>(options =>
//             options.UseNpgsql(connectionString,
//                 builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
//     }
//     
//     private static void AddRepositories(this IServiceCollection services)
//     {
//        
//     }
// }