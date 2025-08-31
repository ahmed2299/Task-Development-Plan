using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace TDP.BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBLLServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var registeredServices = new List<string>();

            // Register AutoMapper
            services.AddAutoMapper(assembly);
            Console.WriteLine("✓ Registered: AutoMapper");

            // Register FluentValidation
            try
            {
                services.AddControllers()
                    .AddFluentValidation(fv =>
                    {
                        fv.RegisterValidatorsFromAssembly(assembly);
                        fv.DisableDataAnnotationsValidation = true;
                    });
                Console.WriteLine("✓ Registered: FluentValidation with ASP.NET Core integration");
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠ Warning: FluentValidation not available. Add package: FluentValidation.AspNetCore");
                Console.WriteLine($"  Error: {ex.Message}");
            }

            // Register all services in the assembly
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
                .ToList();

            Console.WriteLine($"Found {serviceTypes.Count} service classes:");
            foreach (var serviceType in serviceTypes)
            {
                Console.WriteLine($"  - {serviceType.Name}");
            }

            foreach (var serviceType in serviceTypes)
            {
                // Find the interface that this service implements
                var interfaces = serviceType.GetInterfaces();
                var serviceInterface = interfaces.FirstOrDefault(i => 
                    i.Name.StartsWith("I") && 
                    (i.Name.EndsWith("Service") || i.Name == "I" + serviceType.Name) &&
                    i.Namespace == serviceType.Namespace);

                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, serviceType);
                    registeredServices.Add($"{serviceInterface.Name} -> {serviceType.Name}");
                    Console.WriteLine($"✓ Registered: {serviceInterface.Name} -> {serviceType.Name}");
                }
                else
                {
                    Console.WriteLine($"⚠ Warning: No interface found for service {serviceType.Name}");
                    Console.WriteLine($"  Available interfaces: {string.Join(", ", interfaces.Select(i => i.Name))}");
                }
            }

            // Register all repositories in the assembly
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repo"))
                .ToList();

            Console.WriteLine($"\nFound {repositoryTypes.Count} repository classes:");
            foreach (var repoType in repositoryTypes)
            {
                Console.WriteLine($"  - {repoType.Name}");
            }

            foreach (var repoType in repositoryTypes)
            {
                // Find the interface that this repository implements
                var interfaces = repoType.GetInterfaces();
                var repoInterface = interfaces.FirstOrDefault(i => 
                    i.Name.StartsWith("I") && 
                    (i.Name.EndsWith("Repo") || i.Name == "I" + repoType.Name) &&
                    i.Namespace == repoType.Namespace);

                if (repoInterface != null)
                {
                    services.AddScoped(repoInterface, repoType);
                    registeredServices.Add($"{repoInterface.Name} -> {repoType.Name}");
                    Console.WriteLine($"✓ Registered: {repoInterface.Name} -> {repoType.Name}");
                }
                else
                {
                    Console.WriteLine($"⚠ Warning: No interface found for repository {repoType.Name}");
                    Console.WriteLine($"  Available interfaces: {string.Join(", ", interfaces.Select(i => i.Name))}");
                }
            }



            Console.WriteLine($"\nTotal services registered: {registeredServices.Count}");
            return services;
        }

        /// <summary>
        /// Register all services with custom lifetime
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="lifetime">Service lifetime (default: Scoped)</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddBLLServicesWithLifetime(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Register all services in the assembly
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
                .ToList();

            foreach (var serviceType in serviceTypes)
            {
                var interfaces = serviceType.GetInterfaces();
                var serviceInterface = interfaces.FirstOrDefault(i => 
                    i.Name.StartsWith("I") && 
                    (i.Name.EndsWith("Service") || i.Name == "I" + serviceType.Name) &&
                    i.Namespace == serviceType.Namespace);

                if (serviceInterface != null)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(serviceInterface, serviceType);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(serviceInterface, serviceType);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(serviceInterface, serviceType);
                            break;
                    }
                }
            }

            // Register all repositories in the assembly
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repo"))
                .ToList();

            foreach (var repoType in repositoryTypes)
            {
                var interfaces = repoType.GetInterfaces();
                var repoInterface = interfaces.FirstOrDefault(i => 
                    i.Name.StartsWith("I") && 
                    (i.Name.EndsWith("Repo") || i.Name == "I" + repoType.Name) &&
                    i.Namespace == repoType.Namespace);

                if (repoInterface != null)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(repoInterface, repoType);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(repoInterface, repoType);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(repoInterface, repoType);
                            break;
                    }
                }
            }

            return services;
        }
    }
}
