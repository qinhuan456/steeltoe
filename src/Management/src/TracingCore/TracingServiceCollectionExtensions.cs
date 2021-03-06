﻿// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace.Configuration;
using Steeltoe.Common;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.OpenTelemetry.Trace;
using Steeltoe.Management.OpenTelemetry.Trace.Exporter.Zipkin;
using Steeltoe.Management.Tracing.Observer;
using System;

namespace Steeltoe.Management.Tracing
{
    public static class TracingServiceCollectionExtensions
    {
        public static void AddDistributedTracing(this IServiceCollection services, IConfiguration config, Action<TracerBuilder> configureTracer = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var appInstanceInfo = services.BuildServiceProvider().GetService<IApplicationInstanceInfo>();

            services.TryAddSingleton<IDiagnosticsManager, DiagnosticsManager>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, TracingService>());

            services.TryAddSingleton<ITracingOptions>((p) =>
            {
                return new TracingOptions(appInstanceInfo, config);
            });

            services.TryAddSingleton<ITraceExporterOptions>((p) =>
            {
                return new TraceExporterOptions(appInstanceInfo, config);
            });

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticObserver, AspNetCoreHostingObserver>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticObserver, AspNetCoreMvcActionObserver>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticObserver, AspNetCoreMvcViewObserver>());

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticObserver, HttpClientDesktopObserver>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticObserver, HttpClientCoreObserver>());

            services.TryAddSingleton<ITracing>((p) => { return new OpenTelemetryTracing(p.GetService<ITracingOptions>(), configureTracer); });
            services.TryAddSingleton<IDynamicMessageProcessor, TracingLogProcessor>();
        }

        public static void UseZipkinWithTraceOptions(this TracerBuilder builder, IServiceCollection services)
        {
            var options = services.BuildServiceProvider().GetService<ITraceExporterOptions>();
            builder.UseZipkin(zipkinOptions =>
            {
                zipkinOptions.Endpoint = new Uri(options.Endpoint);
                zipkinOptions.ServiceName = options.ServiceName;
                zipkinOptions.TimeoutSeconds = new TimeSpan(0, 0, options.TimeoutSeconds);
                zipkinOptions.UseShortTraceIds = options.UseShortTraceIds;
            });
        }
    }
}
