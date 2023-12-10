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

# CreativeMinds CQSLight Dependency Injection

Unlike CreativeMinds.CQS, CreativeMinds.CQSLight uses the build-in dependency injection of .NET, and does not need a 3rd party library.

All you need to do, is add the handlers, validators and authorisers using the ```AddCQS``` method with the assemblies where the CQS code is.

```
builder.Services.AddCQS(typeof(Program).Assembly);
```

