using CreativeMinds.CQSLight;
using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Decoraters;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddOpenTelemetry()
	.AddConsole();

String serviceName = "CreativeMinds.CQSLight";
String serviceVersion = "0.5.0.0";

builder.Services.AddOpenTelemetry()
	.WithMetrics(c => {
		c.AddConsoleExporter();
	})
	.WithTracing(e => {
		//e.AddAspNetCoreInstrumentation();
		e.AddSource(serviceName);
		e.ConfigureResource(resource =>
		resource.AddService(
		  serviceName: serviceName,
		  serviceVersion: serviceVersion));
		e.AddConsoleExporter();
	});


//using var tracerProvider = Sdk.CreateTracerProviderBuilder()
//	.AddSource(serviceName)
//	.ConfigureResource(resource =>
//		resource.AddService(
//		  serviceName: serviceName,
//		  serviceVersion: serviceVersion))
//	.AddConsoleExporter()
//	.Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCQS(typeof(Program).Assembly);
//builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
//builder.Services.AddScoped<AddToBasketCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/helloworld", async ([FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken) => {

	await commandDispatcher.DispatchAsync<AddToBasketCommand>(new AddToBasketCommand { }, cancellationToken);

	return "HelloWorld";
})
.WithOpenApi();

app.MapGet("/gutentagwelt", async ([FromServices] IQueryDispatcher queryDispatcher, CancellationToken cancellationToken) => {

	return await queryDispatcher.DispatchAsync<DummyQuery, String>(new DummyQuery { }, cancellationToken);
})
.WithOpenApi();

app.Run();



[CommandHandler(typeof(AddToBasketCommandHandler))]
public record AddToBasketCommand : ICommand { }


public class AddToBasketCommandHandler : ICommandHandler<AddToBasketCommand> {

	public Task HandleAsync(AddToBasketCommand command, CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}
}

//[QueryHandler(typeof(DummyQueryHandler))]
public record DummyQuery : IQuery<String> { }

public class DummyQueryHandler : IQueryHandler<DummyQuery, String> {

	public async Task<String> HandleAsync(DummyQuery query, CancellationToken cancellationToken) {
		return "Guten tag welt is German for Hello World";
	}
}