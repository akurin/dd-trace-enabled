using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace CDK
{
    public class DdTraceStack : Stack
    {
        internal DdTraceStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var ddExtensionLayerArn = $"arn:aws:lambda:{Aws.REGION}:464622532012:layer:Datadog-Extension:72";

            // Setting DD_TRACE_ENABLED=false disables tracing when using dd-trace-dotnet extension version 16.
            // However, this setting appears to be ineffective for versions 17-19. As of this writing, version 19 
            // is the latest release.
            var ddTraceDotnetLayerArn = $"arn:aws:lambda:{Aws.REGION}:464622532012:layer:dd-trace-dotnet:17";

            var ddApiKeyParam = new CfnParameter(this, "DdApiKey", new CfnParameterProps
            {
                Description = "Datadog API key",
                Type = "String",
                MinLength = 1
            });

            var ddSiteParam = new CfnParameter(this, "DdSite", new CfnParameterProps
            {
                Description = "Datadog site",
                Type = "String",
                Default = "app.datadoghq.com"
            });

            var environment = new Dictionary<string, string>
            {
                ["DD_API_KEY"] = ddApiKeyParam.ValueAsString,
                ["DD_SITE"] = ddSiteParam.ValueAsString,
                ["DD_TRACE_ENABLED"] = "false",
                ["AWS_LAMBDA_EXEC_WRAPPER"] = "/opt/datadog_wrapper",
                ["DD_LOG_LEVEL"] = "debug"
            };

            new Function(
                this,
                "EchoFunction",
                new FunctionProps
                {
                    FunctionName = $"{Aws.STACK_NAME}-EchoFunction",
                    Runtime = Runtime.DOTNET_8,
                    Handler = "EchoFunction::" +
                              "EchoFunction.Function::" +
                              "FunctionHandler",
                    Code = Code.FromAsset("EchoFunction.zip"),
                    Layers =
                    [
                        LayerVersion.FromLayerVersionArn(this, "DatadogExtension", ddExtensionLayerArn),
                        LayerVersion.FromLayerVersionArn(this, "DDTraceDotnet", ddTraceDotnetLayerArn)
                    ],
                    Environment = environment,
                    Timeout = Duration.Seconds(10)
                });
        }
    }
}