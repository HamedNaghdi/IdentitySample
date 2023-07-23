using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;

var builder = WebApplication.CreateBuilder(args);

// Add Authentications
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
//        options => builder.Configuration.Bind("JwtSettings", options))
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//        options => builder.Configuration.Bind("CookieSettings", options));
builder.Services.AddAuthentication(defaultScheme: CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme,
    configureOptions: config =>
    {
        config.Cookie.Name = "demo";
        config.ExpireTimeSpan = TimeSpan.FromHours(8);
        config.SlidingExpiration = true;

        config.LoginPath = "/Account/Login";
        config.AccessDeniedPath = "/AccessDenied";
    })
    .AddCookie("temp")
    .AddGoogle("Google", config =>
    {
        config.ClientId = "898444421934-j3819skn5fuutcovg37d8phs6saosmov.apps.googleusercontent.com";
        config.ClientSecret = "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";

        //config.CallbackPath = "/signin-google"; // default
        //config.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme // default
        config.SignInScheme = "temp";

        config.Events = new OAuthEvents
        {
            //OnCreatingTicket = 
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CheckUserClaims", policy =>
    {
        policy.RequireAuthenticatedUser();
        //policy.RequireRole("Admin");
        //policy.RequireClaim("sub", "124");
    });
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
