using CreativeMinds.CQSLight.Abstract;
using CreativeMinds.CQSLight.Exceptions;
using CreativeMinds.CQSLight.Instrumentation;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CreativeMinds.CQSLight.Validation {

	public class DefaultQueryValidationFailuresHandler{
		protected readonly ILogger logger;
		protected readonly CQSLightInstrumentation instrumentation;

		protected TelemetrySpan activity;

		public DefaultQueryValidationFailuresHandler(ILogger<DefaultQueryValidationFailuresHandler> logger, CQSLightInstrumentation instrumentation) {
			this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
			this.instrumentation = instrumentation ?? throw new System.ArgumentNullException(nameof(instrumentation));
		}

		public void Handle(IEnumerable<ValidationResult> errors, CancellationToken cancellationToken) {
			this.activity = this.instrumentation.Tracer.StartActiveSpan(InstrumentationConstants.DefaultQueryValidationsFailureHandlerActivityName);

			if (errors.Any() == true) {
				this.activity?.SetStatus(Status.Error);
				this.logger.LogError($"One or more validators returned errors {errors.Count()}");
				throw new ValidationException(errors);
			}
		}
	}
}
