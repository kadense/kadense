using Akka.Actor;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Discord.Models;

namespace Kadense.Malleable.Workflow.Discord
{
    public static class Extensions
    {
        public static MalleableWorkflowCoordinatorFactory AddDiscord(this MalleableWorkflowCoordinatorFactory factory, DiscordCommandProvider provider, string discordCommandName = "DiscordCommand", string discordCommandProviderName = "DiscordCommandProvider")
        {
            factory.WithWorkflowProvider(discordCommandProviderName, provider);
            factory.WorkflowContext.AddDiscord();
            factory.AddAction(discordCommandName, typeof(DiscordCommandProcessor<,>));
            return factory;
        }


        public static MalleableWorkflowContext AddDiscord(this MalleableWorkflowContext ctx)
        {
            return ctx.WithAssembly(GetDiscordAssembly());
        }

        public static MalleableAssemblyFactory AddDiscord(this MalleableAssemblyFactory factory)
        {
            return factory
                .WithType<DiscordInteraction>()
                .WithType<DiscordInteractionResponse>()
                .WithType<DiscordRequestAndResponse>();
        }
        

        public static MalleableWorkflowApiFactory AddDiscordApi(this MalleableWorkflowFactory factory, string name = "Discord")
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