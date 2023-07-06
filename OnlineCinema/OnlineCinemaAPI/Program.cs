using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using OnlineCinemaAPI.Sequrity;
using OnlineCinemaBusnesLogic.Logics;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Storage;
using OnlineCinemaStorageDatabase.DiskFileSystem;
using OnlineCinemaStorageDatabase.Implements;
using OnlineCinemaStorageDatabase.Models;
using OnlineCinemaStorageDatabase.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddNLog("nlog.config");

// Add services to the container.
    #region «ависимости
    builder.Services.AddTransient<IFilmStorage, FilmStorage>();
    builder.Services.AddTransient<IEpisodeStorage, EpisodeStorage>();
    builder.Services.AddTransient<ISeasonStorage, SeasonStorage>();
    builder.Services.AddTransient<ISeriesStorage, SeriesStorage>();
    builder.Services.AddTransient<ITagStorage, TagStorage>();
    builder.Services.AddTransient<IUserStorage, UserStorage>();
    builder.Services.AddTransient<IFileConverter, FileConverter>();

    builder.Services.AddTransient<IFilmLogic, FilmLogic>();
    builder.Services.AddTransient<IEpisodeLogic, EpisodeLogic>();
    builder.Services.AddTransient<ISeasonLogic, SeasonLogic>();
    builder.Services.AddTransient<ISeriesLogic, SeriesLogic>();
    builder.Services.AddTransient<IUserLogic, UserLogic>();
    builder.Services.AddTransient<IServiceLogic, ServiceLogic>();

    builder.Services.AddSingleton<JWTUser>();
#endregion «ависимости
//

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироватьс€ издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представл€юща€ издател€
                            ValidIssuer = AuthOptions.ISSUER,

                            // будет ли валидироватьс€ потребитель токена
                            ValidateAudience = true,
                            // установка потребител€ токена
                            ValidAudience = AuthOptions.AUDIENCE,
                            // будет ли валидироватьс€ врем€ существовани€
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // валидаци€ ключа безопасности
                            ValidateIssuerSigningKey = true,
                        };
                    });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineCinemaRestApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
});


//¬ подобном случа проблемм с серелизацией интерфейсов более нет. » кортежи он сам(по умолчанию!!!!! “ак, оказываетс€, можно) серелизует
builder.Services.AddControllers().AddNewtonsoftJson();


builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod())
);

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
