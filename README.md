# Datadog .NET Lambda Tracing Bug Reproduction

This repository demonstrates a bug with the Datadog .NET Lambda tracing layer where the `DD_TRACE_ENABLED=false`
environment variable is not respected in versions 17-19 of the `dd-trace-dotnet` layer.

## Bug Description

When using the Datadog .NET Lambda layer (`dd-trace-dotnet`):

- Version 16: Setting `DD_TRACE_ENABLED=false` correctly disables tracing
- Versions 17-19: Setting `DD_TRACE_ENABLED=false` does not disable tracing as expected

## Prerequisites

- AWS CLI configured with appropriate credentials
- AWS CDK CLI installed
- .NET 8 SDK installed
- A Datadog API key

## Project Structure

- `src/CDK/`: Contains the AWS CDK infrastructure code
- `src/EchoFunction/`: Contains a simple Lambda function that echoes its input
- The Lambda function is configured with:
    - Datadog Extension Layer (version 72)
    - dd-trace-dotnet Layer (version 17)
    - Environment variable `DD_TRACE_ENABLED=false`

## Steps to Reproduce

1. Clone this repository

2. Build and package the Lambda function:

```bash
./publish.sh
```

3. Deploy the stack with your Datadog API key (and optionally specify your Datadog site):

```bash
# Deploy with default site (datadoghq.com)
cdk deploy --parameters DdApiKey=<your-dd-api-key>

# Or specify a custom site
cdk deploy --parameters DdApiKey=<your-dd-api-key> --parameters DdSite=<your-dd-site>
```

4. Once deployed, invoke the DdTrace-EchoFunction Lambda function with any JSON payload

5. Check Datadog APM:
    - Despite `DD_TRACE_ENABLED=false`, traces will appear in Datadog APM
    - This behavior differs from version 16 where traces would be correctly disabled

## Expected Behavior

Setting `DD_TRACE_ENABLED=false` should prevent traces from being sent to Datadog APM.

## Actual Behavior

Traces continue to be sent to Datadog APM despite the environment variable being set to false.

## Additional Notes

- Debug logs are enabled (`DD_LOG_LEVEL=debug`) to help with troubleshooting
- The issue has been observed across versions 17-19 of the dd-trace-dotnet layer
- Version 16 of the layer exhibits the expected behavior

## Cleanup

To remove the deployed resources:

```bash
cdk destroy
```