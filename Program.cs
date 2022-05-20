
using Knowledge_Graph_Analysis_BackEnd.Models;
using Knowledge_Graph_Analysis_BackEnd.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KnowledgeGraphContext>(options =>
                    options.UseMySql(builder.Configuration["KnowledgeGraph:MySQLConnectionString"], 
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql")));

// add Repository DI.
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
