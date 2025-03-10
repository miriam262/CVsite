using CVsite.CachedService;
using Service;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.Configure<GitHubIntegrationsOptions>(builder.Configuration.GetSection(nameof(GitHubIntegrationsOptions)));
//builder.Services.Configure<GitHubIntegrationsOptions>(builder.Configuration.GetSection("GitHubIntegrationsOptions"));
builder.Services.AddGitHubIntegration(option => builder.Configuration.GetSection("GitHubIntegrationsOptions").Bind(option));
var v = builder.Configuration.GetSection("GitHubIntegrationsOptions");
var token = builder.Configuration["GitHubIntegrationsOptions:Token"];
//var options = builder.Configuration.GetSection(nameof(GitHubIntegrationsOptions)).Get<GitHubIntegrationsOptions>();
//Console.WriteLine($"🔍 Token: {options?.Token}, UserName: {options?.UserName}");
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IGitHubService, GithubService>();
builder.Services.Decorate<IGitHubService,CachedGitHubService>();
//builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();