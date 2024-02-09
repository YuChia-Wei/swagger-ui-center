using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using SwaggerUI.Center.Components.Implements;
using SwaggerUI.Center.Components.Interfaces;
using SwaggerUI.Center.Components.OptionModels;
using SwaggerUI.Center.Infrastructure.ConfigureOptions;
using SwaggerUI.Center.Infrastructure.Middleware;
using SwaggerUI.Center.Infrastructure.ServiceCollectionExtension;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

var jsonPath = Path.Combine("Configuration", "swagger-endpoints.json");
var resolveLinkTarget = File.ResolveLinkTarget(jsonPath, true);
var swaggerDefaultSetPath = resolveLinkTarget?.FullName ?? jsonPath;
builder.Configuration.AddJsonFile(swaggerDefaultSetPath, true, true);

builder.Services.Configure<ApiEndpointsSettingOption>(builder.Configuration.GetSection("ApiEndpointSetting"));

builder.Services.AddLogging();

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

// 註冊 Service
builder.Services.AddScoped<IOpenApiDocumentEndpointService, OpenApiDocumentEndpointService>();
builder.Services.AddScoped<IOpenApiDocumentService, OpenApiDocumentService>();

// 註冊 Repository
builder.Services.AddScoped<IOpenApiDocumentEndpointRepository, OpenApiDocumentEndpointRepository>();
builder.Services.AddScoped<IOpenApiDocumentRepository, OpenApiDocumentRepository>();
builder.Services.AddScoped<IConfigureOptions<SwaggerUIOptions>, SwaggerUiOptionsConfigure>();
builder.Services.AddScoped<DynamicSwaggerUiEndpointMiddleware>();

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

app.UseHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseSwagger();

//使用動態的 Swagger UI Endpoint List (Swagger UI Option)
app.UseDynamicSwaggerUiEndpoint();

app.MapDefaultControllerRoute();

app.MapControllers();

app.Run();