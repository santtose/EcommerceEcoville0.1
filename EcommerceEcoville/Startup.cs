using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using EcommerceEcoville.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace EcommerceEcoville
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Configurando a injeção de dependência
            services.AddScoped<ProdutoDAO>();
            services.AddScoped<CategoriaDAO>();
            services.AddScoped<UsuarioDAO>();
            services.AddScoped<ItemVendaDAO>();
            services.AddScoped<UtilsSession>();
            services.AddHttpContextAccessor();

            services.AddDbContext<Context>
                (options => options.UseSqlServer
                (Configuration.GetConnectionString
                ("EcommerceConnection")));

            //Configuração da sessão deve ser colocada antes do services.AddMvc
            services.AddSession();
            services.AddDistributedMemoryCache();

            //Configurar o Identity na aplicação
            services.AddIdentity<UsuarioLogado, IdentityRole>().
                AddEntityFrameworkStores<Context>().
                AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Usuario/Login";
                options.AccessDeniedPath = "/Usuario/AcessoNegado";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Produto}/{action=Index}/{id?}");
            });
        }
    }
}
