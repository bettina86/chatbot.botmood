/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace BotLib
{
    public class BotUtilities
    {
        public static void SetLocale(string locale)
        {
            var culture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static async Task BotIsTyping(Activity activity)
        {
            var message = activity.CreateReply();
            ConnectorClient connector = new ConnectorClient(new System.Uri(message.ServiceUrl));
            message.Type = ActivityTypes.Typing;
            message.ServiceUrl = null;
            await connector.Conversations.SendToConversationAsync(message);
        }

        public static async Task BotIsTyping(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Type = ActivityTypes.Typing;
            message.ServiceUrl = null;
            await context.PostAsync(message);
        }

        public static string GetRootDirectory()
        {
            var aux = HostingEnvironment.ApplicationPhysicalPath;
            return aux;
        }

        public static async Task Say(IActivity activity, string text)
        {
            await Say(activity as Activity, text);
        }

        public static async Task Say(Activity activity, string text)
        {
            ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
            Activity reply1 = activity.CreateReply();
            reply1.Text = text;
            await connector.Conversations.SendToConversationAsync(reply1);
        }

        public static async Task Say(IDialogContext context, string text)
        {
            var reply1 = context.MakeMessage();
            reply1.Text = text;
            await context.PostAsync(reply1);
        }

        public static void Trace(string msg)
        {
            string dt = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            System.Diagnostics.Trace.WriteLine(dt+"> "+msg);
        }

    }

}