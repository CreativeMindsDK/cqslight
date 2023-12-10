# CreativeMinds CQSLight

A .NET library for simple command query separation, build for .NET 8+.

According to CQS, every method should either be a command that performs an action, or a query that returns data to the caller, but not both.

If you need to validate your commands or queries or check for authorisation, CreativeMinds CQSLight will handle that too, automatically. As per CQS, CreativeMinds CQSLight does not allow commands to return any data, so if any validation or permission check fail, an exception will be thrown.

Commands and queries will have their authorisers executed first, if any are present, then the validations, again if any are present, and finally the actual command or query will be executed.

###### Validation
```
[Validator(typeof(CreateForumCommandValidator))]
public record CreateForumCommand : ICommand {
	public string Name {get;set;}
}
```
By adding the ```ValidatorAttribute``` to your command classes, they will be validated be the given validators before being executed.

```
public class CreateForumValidator : IValidator<CreateForumCommand> {

	public Task<ValidationResult> Validate(CreateForumCommand command, CancellationToken cancellationToken) {
		// ......
	}
}
```
You can add any number of validators for the same command.

###### Authorisation

```
[Authoriser(typeof(CreateForumCommandAuthoriser))]
public record CreateForumCommand : ICommand {
	public string Name {get;set;}
}
```
By adding the ```AuthoriserAttribute``` to your command classes, they will be checked before being executed.

```
public class CreateForumAuthoriser : IAuthoriser<CreateForumCommand>
{
		protected Task<AuthorisationResult> AuthoriseAsync(CreateForumCommand command, CancellationToken cancellationToken) {
			// .....
		}
}
```
You can add one or more authorisers for each command.

###### Dispatching commands and queries

When you need to call the code of a command or a query, all you need to do is get a hold of a ```ICommandDispatcher``` or a ```IQueryDispatcher``` and dispatch the command/query.

```
public SaleController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) {
	this.commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
	this.queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
}
```

```
[HttpPut("{saleId}/{bidId}/accept")]
public async Task<IActionResult> AcceptBidAsync(String saleId, String bidId, CancellationToken cancellationToken) {
	// A command does not return anything, so unless an exception is throw, everything went according to the plan!
	await this.commandDispatcher.DispatchAsync<AcceptBidCommand>(new AcceptBidCommand { SaleId = saleId, BidId = bidId }, cancellationToken);
	return Ok();
}
```

```
[AllowAnonymous]
[HttpGet]
public async Task<IEnumerable<Dtos.SaleBaseForList>> GetSalesAsync(CancellationToken cancellationToken) {
	return await this.queryDispatcher.DispatchAsync<FindActiveSalesQuery, IEnumerable<Dtos.SaleBaseForList>>(new FindActiveSalesQuery { PageIndex = 0 }, cancellationToken);
}
```

# CreativeMinds CQSLight Dependency Injection

Unlike CreativeMinds.CQS, CreativeMinds.CQSLight uses the build-in dependency injection of .NET, and does not need a 3rd party library.

All you need to do, is add the handlers, validators and authorisers using the ```AddCQS``` method with the assemblies where the CQS code is.

```
builder.Services.AddCQS(typeof(Program).Assembly);
```

