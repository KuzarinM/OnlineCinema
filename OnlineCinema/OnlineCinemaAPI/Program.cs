using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using OnlineCinemaBusnesLogic.Logics;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Storage;
using OnlineCinemaStorageDatabase.Implements;
using OnlineCinemaStorageDatabase.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddNLog("nlog.config");

// Add services to the container.
    #region Зависимости
    builder.Services.AddTransient<IFilmStorage, FilmStorage>();
    builder.Services.AddTransient<IEpisodeStorage, EpisodeStorage>();
    builder.Services.AddTransient<ISeasonStorage, SeasonStorage>();
    builder.Services.AddTransient<ISeriesStorage, SeriesStorage>();

    builder.Services.AddTransient<IFilmLogic, FilmLogic>();
    builder.Services.AddTransient<IEpisodeLogic, EpisodeLogic>();
    builder.Services.AddTransient<ISeasonLogic, SeasonLogic>();
    builder.Services.AddTransient<ISeriesLogic, SeriesLogic>();
    #endregion Зависимости
//

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineCinemaRestApi", Version = "v1" });
});

//В подобном случа проблемм с серелизацией интерфейсов более нет. И кортежи он сам(по умолчанию!!!!! Так, оказывается, можно) серелизует
builder.Services.AddControllers().AddNewtonsoftJson();

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
