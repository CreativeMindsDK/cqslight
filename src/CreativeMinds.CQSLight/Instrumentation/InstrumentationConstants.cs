using System;

namespace CreativeMinds.CQSLight.Instrumentation {

	internal static class InstrumentationConstants {
		//public const String JobIdTag = "job.id";
		//public const String JobCreatedAtTag = "job.createdat";
		public const String Type = "cqslight.type";

		public const String CommandDispatchActivityName = "CommandDispatch";
		public const String QueryDispatchActivityName = "QueryDispatch";

		public const String ActivityKey = "cqslight_activity_key";
		public const String ActivityContextKey = "cqslight_activity_context";
	}
}
