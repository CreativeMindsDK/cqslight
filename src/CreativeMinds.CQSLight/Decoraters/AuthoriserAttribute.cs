using System;

namespace CreativeMinds.CQSLight.Decoraters {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class AuthoriserAttribute : Attribute {
		public readonly Type Authorisor;

		public AuthoriserAttribute(Type authorisor) {
			this.Authorisor = authorisor ?? throw new ArgumentNullException(nameof(authorisor));
		}
	}
}
