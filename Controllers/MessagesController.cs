/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
namespace BotMood
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Http;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using BotLib;

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Microsoft.Bot.Connector.Activity activity)
        {
            if (activity != null && activity.Type == ActivityTypes.Message)
            {
                BotUtilities.SetLocale("pt-BR");
                await BotUtilities.BotIsTyping(activity);
                await AttachedAudioProcessor.ReceiveAttachedAudio(activity);
                await Conversation.SendAsync(activity, () => new ExceptionHandlerDialog<object>(new DialogMain(), displayException: true));
            }
            else
            {
                await HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public static async Task HandleSystemMessage(Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.DeleteUserData:
                    // Implement user deletion here
                    // If we handle user deletion, return a real message
                    break;

                case ActivityTypes.ConversationUpdate:

                    IConversationUpdateActivity iConversationUpdated = activity as IConversationUpdateActivity;
                    if (iConversationUpdated != null)
                    {
                        ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                        foreach (var member in iConversationUpdated.MembersAdded ?? System.Array.Empty<ChannelAccount>())
                        {
                            if (member.Id == iConversationUpdated.Recipient.Id)
                            {
                                Activity msg0 = activity.CreateReply();
                                msg0.Text = StringMessages.Versao;
                                connector.Conversations.SendToConversation(msg0);

                                Activity msg1 = CardMessages.GetAboutCards(activity);
                                connector.Conversations.SendToConversation(msg1);

                                Activity msg2 = CardMessages.GetHelpCards(activity);
                                connector.Conversations.SendToConversation(msg2);
                            }
                        }
                    }
                    break;

                case ActivityTypes.ContactRelationUpdate:
                    // Handle add/remove from contact lists
                    break;

                case ActivityTypes.Typing:
                    // Handle knowing that the user is typing
                    break;

                case ActivityTypes.Ping:
                    break;
            }
        }

    }
}