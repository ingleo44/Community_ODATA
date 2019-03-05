using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Adventureworks.Core.MiddleWare;
using Adventureworks.Core.Supervisor.Classes;
using Adventureworks.Core.Supervisor.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Swashbuckle.AspNetCore.Swagger;
using static Adventureworks.Model.PhoneNumberTypeWithEnum;


namespace Adventureworks
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("AdventureWorksDB");
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonSupervisor,PersonSupervisor>();
            // Add framework services.
            services.AddOData();
            services.AddODataQueryFilter();

            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddDbContext<AdventureWorks2017Context>(options =>
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging()
            );


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMvc(b =>
                b.MapODataServiceRoute("odata", "odata", GetEdmModel(app.ApplicationServices)
                ));
        }


        private static IEdmModel GetEdmModel(IServiceProvider serviceProvider)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder(serviceProvider);

            builder.EntitySet<Address>("Address")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<AddressType>("AddressType")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<BusinessEntity>("BusinessEntity")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();
            builder.EntitySet<BusinessEntityAddress>("BusinessEntityAddress")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<BusinessEntityContact>("BusinessEntityContact")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<ContactType>("ContactType")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<CountryRegion>("CountryRegion")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<EmailAddress>("EmailAddress")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<Password>("Password")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<Person>("Person")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();
            builder.EntitySet<PersonPhone>("PersonPhone")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<PhoneNumberType>("PhoneNumberType")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<StateProvince>("StateProvince")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            builder.EntitySet<EntityWithEnum>("EntityWithEnum")
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            EntitySetConfiguration<ContactType> contactType = builder.EntitySet<ContactType>("ContactType");
            var actionY = contactType.EntityType.Action("ChangePersonStatus");
            actionY.Parameter<string>("Level");
            actionY.Returns<bool>();

            var changePersonStatusAction = contactType.EntityType.Collection.Action("ChangePersonStatus");
            changePersonStatusAction.Parameter<string>("Level");
            changePersonStatusAction.Returns<bool>();

            EntitySetConfiguration<Person> persons = builder.EntitySet<Person>("Person");
            FunctionConfiguration myFirstFunction = persons.EntityType.Collection.Function("MyFirstFunction");
            myFirstFunction.ReturnsCollectionFromEntitySet<Person>("Person");

            EntitySetConfiguration<EntityWithEnum> entitiesWithEnum = builder.EntitySet<EntityWithEnum>("EntityWithEnum");
            FunctionConfiguration functionEntitiesWithEnum = entitiesWithEnum.EntityType.Collection.Function("PersonSearchPerPhoneType");
            functionEntitiesWithEnum.Parameter<PhoneNumberTypeEnum>("PhoneNumberTypeEnum");
            functionEntitiesWithEnum.ReturnsCollectionFromEntitySet<EntityWithEnum>("EntityWithEnum");

            return builder.GetEdmModel();
        }
    }
}
