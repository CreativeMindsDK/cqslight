using System;

namespace CreativeMinds.CQSLight.Decoraters {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ValidatorAttribute : Attribute {
		public readonly Type Validator;

		public ValidatorAttribute(Type validator) {
			this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

	}
}
