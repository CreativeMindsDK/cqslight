using OpenTelemetry.Trace;
using System;
using System.Reflection;

namespace CreativeMinds.CQSLight.Instrumentation {

	public class CQSLightInstrumentation {
		private static readonly AssemblyName AssemblyName = typeof(CommandDispatcher).Assembly.GetName();
		public static readonly String ActivitySourceName = AssemblyName.Name;
		public static readonly Version Version = AssemblyName.Version;

		private static Tracer tracer = TracerProvider.Default.GetTracer(ActivitySourceName, Version.ToString());

		public Tracer Tracer { get { return CQSLightInstrumentation.tracer; } }
	}
}
