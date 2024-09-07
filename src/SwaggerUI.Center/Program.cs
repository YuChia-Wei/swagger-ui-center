using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using SwaggerUI.Center.Authentication;
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
builder.Services.AddAuthorization();

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

// 註冊 Repository
builder.Services.AddScoped<IWebApiEndpointRepository, WebApiEndpointRepository>();
builder.Services.AddScoped<IOpenApiDocumentRepository, OpenApiDocumentRepository>();

builder.Services.AddScoped<IConfigureOptions<SwaggerUIOptions>, SwaggerUiOptionsConfigure>();
builder.Services.AddScoped<DynamicSwaggerUiMiddleware>();
builder.Services.AddScoped<SwaggerUiAuthorizationMiddleware>();

builder.Services.AddScoped<ISwaggerUiPermissionValidator, SwaggerUiPermissionValidator>();

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

app.UseSwaggerUiAuthorization();

app.UseDynamicSwaggerUi();

app.MapDefaultControllerRoute();

app.MapControllers();

app.Run();