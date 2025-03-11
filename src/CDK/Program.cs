using Amazon.CDK;

namespace CDK
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new DdTraceStack(app, "DdTrace");

            app.Synth();
        }
    }
}
