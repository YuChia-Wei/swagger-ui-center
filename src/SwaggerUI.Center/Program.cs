using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using SwaggerUI.Center.Authentication;
using SwaggerUI.Center.Authorization;
using SwaggerUI.Center.Components.Domain;
using SwaggerUI.Center.Components.Implements;
using SwaggerUI.Center.Components.Interfaces;
using SwaggerUI.Center.Configuration;
using SwaggerUI.Center.Middleware;
using SwaggerUI.Center.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
{
    // logging fields can via. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging/?view=aspnetcore-7.0#loggingfields
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                            HttpLoggingFields.ResponsePropertiesAndHeaders;

    // 加上這個可以將指定的 http header 從 log 中加入/移除
    // options.RequestHeaders.Add("SomeRequestHeader");
    // options.RequestHeaders.Remove("Cookie");
    // options.ResponseHeaders.Add("SomeResponseHeader");
});

builder.Configuration.AddOpenApiEndpointConfigurationJsons();
builder.Configuration.AddAuthenticationConfigurationJsons();

builder.Services.Configure<WebApiEndpoints>(builder.Configuration.GetSection("WebApiEndpoints"));

builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    //使用外部權限控管 api 來驗證使用者可不可以使用此站台的認政策略
    options.AddPolicy("permissionValidation", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequirePermissionValidation();
    });

    //指定特定使用者才能用的認證策略
    options.AddPolicy("specifyUser", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireUserName("userName");
    });

    //指定包含特定身分的使用者才能使用的認證策略
    options.AddPolicy("specifyRole", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("admin");
    });

    //定義全站的認證策略，除了有指定允許匿名外的資源都必須經過身份認證
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddSingleton<IPermissionValidator, PermissionValidator>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionValidationHandler>();

// 處理中文轉碼
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin,
                                                 UnicodeRanges.CjkUnifiedIdeographs));

// API Url Path 使用小寫
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services
       .AddControllers()
       .AddJsonOptions(options =>
       {
           // ViewModel 與 Parameter 顯示為小駝峰命名
           options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
       });

builder.Services.AddOpenApiDocGenerate();

builder.Services.AddHttpClient();

builder.Services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);

// add Component
builder.Services.AddScoped<IWebApiEndpointRepository, WebApiEndpointRepository>();
builder.Services.AddScoped<IOpenApiDocumentRepository, OpenApiDocumentRepository>();

builder.Services.AddScoped<IConfigureOptions<SwaggerUIOptions>, SwaggerUiOptionsConfigure>();
builder.Services.AddScoped<DynamicSwaggerUiMiddleware>();

// 開啟 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto |
                               ForwardedHeaders.XForwardedHost;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();
var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

app.UseHealthChecks("/health");

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //開發模式下才提供本站的 open api json
    app.UseSwagger();

    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseDynamicSwaggerUi();

app.MapDefaultControllerRoute();

app.MapControllers();

app.Run();