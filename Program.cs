
using Knowledge_Graph_Analysis_BackEnd.IRepositories;
using Knowledge_Graph_Analysis_BackEnd.Models;
using Knowledge_Graph_Analysis_BackEnd.Repositories;
using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<KnowledgeGraphContext>(options =>
                    options.UseMySql(builder.Configuration["KnowledgeGraph:MySQLConnectionString"], 
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql")));
//add neo4j driver.
builder.Services.AddSingleton(GraphDatabase.Driver(builder.Configuration["KnowledgeGraph:Neo4jConnectionSettings:Server"],
    AuthTokens.Basic(builder.Configuration["KnowledgeGraph:Neo4jConnectionSettings:UserName"],
    builder.Configuration["KnowledgeGraph:Neo4jConnectionSettings:Password"])));


// add Repository DI.
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IPaperRepository, PaperRepository>();

//Cors

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(op => {
    op.AddPolicy(MyAllowSpecificOrigins, set => {
        set.SetIsOriginAllowed(origin => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
