namespace BotMood
{
    using Autofac;
    using BotLib;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using System;
    using System.Web.Http;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BotUtilities.Trace("Application_Start");
            BotUtilities.SetLocale("pt-BR");

            this.Error += WebApiApplication_Error;

            this.RegisterBotModules();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private void WebApiApplication_Error(object sender, EventArgs e)
        {
            BotUtilities.Trace("WebApiApplication_Error: "+e.ToString());
        }

        private void RegisterBotModules()
        {
            Conversation.UpdateContainer(builder =>
            {
                builder.RegisterModule(new ReflectionSurrogateModule());
                builder.RegisterModule<GlobalMessageHandlersBotModule>();
            });
        }

        protected void Application_BeginRequest()
        {
            System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        }

    }
}
