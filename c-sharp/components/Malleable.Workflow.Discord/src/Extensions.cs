using Akka.Actor;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Discord.Models;

namespace Kadense.Malleable.Workflow.Discord
{
    public static class Extensions
    {
        public static MalleableWorkflowCoordinatorFactory AddDiscord(this MalleableWorkflowCoordinatorFactory factory, string namePrefix = "Discord", string urlPrefix = "/api/discord")
        {
            factory.WorkflowContext.AddDiscord();
            return factory;
        }


        public static MalleableWorkflowContext AddDiscord(this MalleableWorkflowContext ctx)
        {
            ctx.WithAssembly(GetDiscordAssembly());
            return ctx;
        }

        public static MalleableAssemblyFactory AddDiscord(this MalleableAssemblyFactory factory)
        {
            factory
                .WithType<DiscordInteraction>()
                .WithType<DiscordInteractionResponse>();
            return factory;
        }
        

        public static MalleableWorkflowApiFactory AddDiscordApi(this MalleableWorkflowFactory factory, string name = "Discord", string urlPrefix = "/api/discord")
        {
            return factory.AddApi(name)
                .SetUnderlyingType<DiscordInteraction>(); 
        }
        
        private static MalleableAssembly GetDiscordAssembly()
        {
            var factory = new MalleableAssemblyFactory()
                .AddDiscord();

            return factory.GetAssemblies().First().Value;
        }
    }
}