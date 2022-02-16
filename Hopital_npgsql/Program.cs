var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// utiliser les chaînes de connexion fournies dans appsettings.json
//string connectionString = builder.Configuration.GetSection("ConnectionStrings")["dbHopital"];
//string connectionString = builder.Configuration.GetConnectionString("dbHopital"); // raccourci de la ligne précédente
//Console.WriteLine($"chaîne de connexion : {connectionString}");
// renseigner le champ de la classe de connexion avec les infos du json
Hopital_npgsql.Services.ConnectService.m_connectString = builder.Configuration.GetConnectionString("dbHopital"); 



// CORS
string devCorsPolicy = "devCorsPolicy";
builder.Services.AddCors(options =>
{
	options.AddPolicy(devCorsPolicy, builder => {
		builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
	});
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseCors(devCorsPolicy); // CORS
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
