using System;
using System.Diagnostics;
using System.Reflection;

namespace CreativeMinds.CQSLight.Instrumentation {

	internal static class CQSLightInstrumentation {
		internal static readonly AssemblyName AssemblyName = typeof(CommandDispatcher).Assembly.GetName();

		/// <summary>
		/// The activity source name.
		/// </summary>
		internal static readonly String ActivitySourceName = AssemblyName.Name;

		/// <summary>
		/// The version.
		/// </summary>
		internal static readonly Version Version = AssemblyName.Version;

		/// <summary>
		/// The activity source.
		/// </summary>
		internal static readonly ActivitySource ActivitySource = new(ActivitySourceName, Version.ToString());

		/// <summary>
		/// The default display name delegate.
		/// </summary>
		//internal static readonly Func<BackgroundJob, string> DefaultDisplayNameFunc = backgroundJob => $"JOB {backgroundJob.Job.Type.Name}.{backgroundJob.Job.Method.Name}";
	}
}
