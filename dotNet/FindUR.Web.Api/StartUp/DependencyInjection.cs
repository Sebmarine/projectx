using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sabio.Data;
using Sabio.Models.Domain.Products;
using Sabio.Services;
using Sabio.Services.HorseProfiles;
using Sabio.Services.Interfaces;
using Sabio.Services.Interfaces.HorseProfiles;
using Sabio.Web.Api.StartUp.DependencyInjection;
using Sabio.Web.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabio.Web.StartUp
{
    public class DependencyInjection
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is IConfigurationRoot)
            {
                services.AddSingleton<IConfigurationRoot>(configuration as IConfigurationRoot);   // IConfigurationRoot
            }

            services.AddSingleton<IConfiguration>(configuration);   // IConfiguration explicitly

            string connString = configuration.GetConnectionString("Default");
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
            // The are a number of differe Add* methods you can use. Please verify which one you
            // should be using services.AddScoped<IMyDependency, MyDependency>();

            // services.AddTransient<IOperationTransient, Operation>();

            // services.AddScoped<IOperationScoped, Operation>();

            // services.AddSingleton<IOperationSingleton, Operation>();

            services.AddSingleton<IAppointmentService, AppointmentService>();

            services.AddSingleton<IAuthenticationService<int>, WebAuthenticationService>();

            services.AddSingleton<Sabio.Data.Providers.IDataProvider, SqlDataProvider>(delegate (IServiceProvider provider)
            {
                return new SqlDataProvider(connString);
            }
            );

            services.AddSingleton<IAdminService, AdminService>();
            services.AddSingleton<IBlogService, BlogService>();
            services.AddSingleton<ICharitableFundService, CharitableFundService>();
            services.AddSingleton<ICommentsService, CommentsService>();
            services.AddSingleton<IDiagnosticService, DiagnosticService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IExternalLinksService, ExternalLinksService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IFAQService, FAQService>();
            services.AddSingleton<IGAService, GAService>();
            services.AddSingleton<IHorseService, HorseService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IIdentityProvider<int>, WebAuthenticationService>();
            services.AddSingleton<IInventoryService, InventoryService>();
            services.AddSingleton<ILicensesService, LicensesService>();
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<ILookUpService, LookUpService>();
            services.AddSingleton<ILikeService, LikeService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<INewsletterSubService, NewsletterSubService>();
            services.AddSingleton<INewsfeedService, NewsfeedService>();
            services.AddSingleton<IPracticeService, PracticeService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IResourceService, ResourceService>();
            services.AddSingleton<IScheduleService, ScheduleService>();
            services.AddSingleton<IServiceProvidedService, ServiceProvidedService>();
            services.AddSingleton<IServiceProvidedTypeService, ServiceProvidedTypeService>();
            services.AddSingleton<IShareStoryService, ShareStoryService>();
            services.AddSingleton<IShoppingCartService, ShoppingCartService>();
            services.AddSingleton<ISiteReferenceService, SiteReferenceService>();
            services.AddSingleton<IStripeAccountLinkService, StripeAccountLinkService>();
            services.AddSingleton<ISubscriptionService, SubscriptionTierService>();
            services.AddSingleton<ISurveyService, SurveyService>();
            services.AddSingleton<ISurveyInstanceService, SurveyInstanceService>();
            services.AddSingleton<ISurveyQuestionService, SurveyQuestionService>();
            services.AddSingleton<ISurveyQuestionAnswerOptionService, SurveyQuestionAnswerOptionService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IVendorService, VendorService>();
            services.AddSingleton<IVetProfileService, VetProfileService>();
            services.AddSingleton<IVideoChatService, VideoChatService>();
            services.AddSingleton<ISurveyAnswersService, SurveyAnswersService>();
            services.AddSingleton<ISurveyInstanceService, SurveyInstanceService>();

            GetAllEntities().ForEach(tt =>
            {
                IConfigureDependencyInjection idi = Activator.CreateInstance(tt) as IConfigureDependencyInjection;

                //This will not error by way of being null. BUT if the code within the method does
                // then we would rather have the error loadly on startup then worry about debuging the issues as it runs
                idi.ConfigureServices(services, configuration);
            });
        }

        public static List<Type> GetAllEntities()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IConfigureDependencyInjection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .ToList();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

    }
}