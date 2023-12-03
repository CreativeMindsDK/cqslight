using System;
using System.Diagnostics;
using System.Reflection;

namespace CreativeMinds.CQSLight.Instrumentation {

	public class CQSLightInstrumentation {
		private static readonly AssemblyName AssemblyName = typeof(CommandDispatcher).Assembly.GetName();
		public static readonly String ActivitySourceName = AssemblyName.Name;
		public static readonly Version Version = AssemblyName.Version;

		public CQSLightInstrumentation() {
			this.ActivitySource = new(ActivitySourceName, Version.ToString());
		}

		public ActivitySource ActivitySource { get; }
	}
}
