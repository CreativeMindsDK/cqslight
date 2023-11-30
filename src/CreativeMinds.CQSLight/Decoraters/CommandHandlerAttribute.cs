using System;

namespace CreativeMinds.CQSLight.Decoraters {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class CommandHandlerAttribute : Attribute {
		public readonly Type Handler;

		public CommandHandlerAttribute(Type handler) {
			this.Handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}
	}
}
