using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Authorisation;
using CreativeMinds.CQSLight.Decoraters;
using CreativeMinds.CQSLight.Exceptions;
using CreativeMinds.CQSLight.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQSLight {

	public abstract class DispatcherBase {
		protected readonly IServiceProvider serviceProvider;
		protected readonly ILogger logger;

		protected Activity? activity;

		protected DispatcherBase(IServiceProvider serviceProvider, ILogger logger) {
			this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Run through any authorisations on the given command/query
		/// </summary>
		/// <typeparam name="TMessage"></typeparam>
		/// <param name="message"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="AuthorisationException">If one or more authorisations returns one or more failures</exception>
		protected async Task CheckAuthorisationAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : IMessage {
			IEnumerable<AuthoriserAttribute> authoriserAttributes = message.GetType().GetCustomAttributes<CreativeMinds.CQSLight.Decoraters.AuthoriserAttribute>(true);
			this.logger.LogDebug($"Found {authoriserAttributes.Count()} authorisers for the type '{message.GetType()}'");

			List<AuthorisationResult> authorisationResults = [];
			foreach (AuthoriserAttribute authoriserAttribute in authoriserAttributes) {
				IAuthoriser<TMessage>? authoriserInstance = this.serviceProvider.GetService(authoriserAttribute.Authorisor) as IAuthoriser<TMessage>;
				if (authoriserInstance != null) {
					authorisationResults.Add(await authoriserInstance.AuthoriseAsync(message, cancellationToken));
				}
				else {
					this.logger.LogWarning($"Trying to a instance of type '{authoriserAttribute.Authorisor}' failed, or it wasn't an IAuthoriser<TCommand>");
				}
			}

			IEnumerable<AuthorisationResult> failures = authorisationResults.Where(r => r.Success == false);
			if (failures.Any() == true) {
				this.activity?.SetStatus(ActivityStatusCode.Error, "One or more authorisations has failures");
				this.logger.LogError($"One or more authorisers returned failures {failures.Count()}");
				throw new AuthorisationException(failures);
			}
		}

		/// <summary>
		/// Run through any validations on the given command/query
		/// </summary>
		/// <typeparam name="TMessage"></typeparam>
		/// <param name="message"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ValidationException">If one or more validations returns one or more errors</exception>
		protected async Task CheckValidationAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : IMessage {
			IEnumerable<ValidatorAttribute> validatorAttributes = message.GetType().GetCustomAttributes<CreativeMinds.CQSLight.Decoraters.ValidatorAttribute>(true);
			this.logger.LogDebug($"Found {validatorAttributes.Count()} validators for the type '{message.GetType()}'");

			List<ValidationResult> validationResults = [];
			foreach (ValidatorAttribute validatorAttribute in validatorAttributes) {
				IValidator<TMessage>? validatorInstance = this.serviceProvider.GetService(validatorAttribute.Validator) as IValidator<TMessage>;
				if (validatorInstance != null) {
					validationResults.Add(await validatorInstance.ValidateAsync(message, cancellationToken));
				}
				else {
					this.logger.LogWarning($"Trying to a instance of type '{validatorAttribute.Validator}' failed, or it wasn't an IAuthoriser<TCommand>");
				}
			}

			IEnumerable<ValidationResult> errors = validationResults.Where(r => r.Success == false);
			if (errors.Any() == true) {
				this.activity?.SetStatus(ActivityStatusCode.Error, "One or more validations has errors");
				this.logger.LogError($"One or more validators returned errors {errors.Count()}");
				throw new ValidationException(errors);
			}
		}

	}
}
