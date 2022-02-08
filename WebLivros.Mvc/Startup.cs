using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLivros.Mvc
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
            services.AddControllersWithViews();

            //Adicionamos um IHttpClientFactory para a injeção de dependência e definimos a Uri base que será utilizada
            //A Uri pode ser consultada e alterada no appsettings.json
            services.AddHttpClient("livros", c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("WebApiLivros"));
            });

            //Adicionamos um AuthenticationBuilder para contruir o cookie de autenticação e passamos um nome para ele
            services.AddAuthentication("CookieLivros")
                .AddCookie("CookieLivros", opcoes => 
                {
                    //Aqui defino as configurações base do cookie

                    //Nome do cookie
                    opcoes.Cookie.Name = "CookieLivros";
                    
                    //O caminho para a tela de Login
                    opcoes.LoginPath = "/Conta/Login";

                    //O caminho para a tela de acesso negado
                    opcoes.AccessDeniedPath = "/Conta/AcessoNegado";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
