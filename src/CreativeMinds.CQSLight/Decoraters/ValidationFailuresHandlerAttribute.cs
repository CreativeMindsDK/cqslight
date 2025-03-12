using System;

namespace CreativeMinds.CQSLight.Decoraters {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class ValidationFailuresHandlerAttribute : Attribute {
		public readonly Type ValidationFailuresHandler;

		public ValidationFailuresHandlerAttribute(Type validationFailuresHandler) {
			this.ValidationFailuresHandler = validationFailuresHandler ?? throw new ArgumentNullException(nameof(validationFailuresHandler));
		}
	}
}
