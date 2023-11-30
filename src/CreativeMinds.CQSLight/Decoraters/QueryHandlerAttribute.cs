using System;

namespace CreativeMinds.CQSLight.Decoraters {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class QueryHandlerAttribute : Attribute {
		public readonly Type Handler;

		public QueryHandlerAttribute(Type handler) {
			this.Handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}
	}
}
