using CreativeMinds.CQSLight.Instrumentation;
using CreativeMinds.CQSLight.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace CreativeMinds.CQSLight {

	public static class ServiceCollectionExtensions {

		public static IServiceCollection AddCQS(this IServiceCollection services, params Assembly[] assemblies) {
			services.AddSingleton<CQSLightInstrumentation>();

			services.AddScoped<ICommandDispatcher, CommandDispatcher>();
			services.AddScoped<IQueryDispatcher, QueryDispatcher>();

			services.AddSingleton<DefaultQueryValidationFailuresHandler>();

			foreach (Assembly assembly in assemblies) {
				foreach (Type type in assembly.GetTypes()) {
					foreach (Type interfac in type.GetInterfaces()) {
						if (interfac.IsGenericType == true && interfac.Name == "ICommandHandler`1" && interfac.Namespace == "CreativeMinds.CQSLight.Abstract") {
							services.AddScoped(type);
						}
						else if (interfac.IsGenericType == true && interfac.Name == "IQueryHandler`2" && interfac.Namespace == "CreativeMinds.CQSLight.Abstract") {
							services.AddScoped(type);
						}
						else if (interfac.IsGenericType == true && interfac.Name == "IAuthoriser`1" && interfac.Namespace == "CreativeMinds.CQSLight.Abstract") {
							services.AddScoped(type);
						}
						else if (interfac.IsGenericType == true && interfac.Name == "IValidator`1" && interfac.Namespace == "CreativeMinds.CQSLight.Abstract") {
							services.AddScoped(type);
						}
						else if (interfac.IsGenericType == true && interfac.Name == "IQueryValidationFailuresHandler`2" && interfac.Namespace == "CreativeMinds.CQSLight.Validation") {
							services.AddScoped(type);
						}
					}
				}
			}

			return services;
		}
	}
}
