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

builder.Services.AddGitHubIntegration(option => builder.Configuration.GetSection(nameof(GitHubIntegrationsOptions)).Bind(option));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IGitHubService, GithubService>();
builder.Services.Decorate<IGitHubService,CachedGitHubService>();

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