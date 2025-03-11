using System.Text.Json.Nodes;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EchoFunction;

public class Function
{
    public JsonObject FunctionHandler(JsonObject input, ILambdaContext context) => input;
}