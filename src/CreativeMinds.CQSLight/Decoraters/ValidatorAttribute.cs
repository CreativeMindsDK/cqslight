using System;

namespace CreativeMinds.CQSLight.Decoraters {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ValidatorAttribute : Attribute {
		public readonly Type Validator;
		public readonly Int32 Priority;

		public ValidatorAttribute(Type validator, Int32 priority = 0) {
			this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
			this.Priority = priority;
		}
	}
}
