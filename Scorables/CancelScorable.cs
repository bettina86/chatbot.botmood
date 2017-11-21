    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Builder.Dialogs;

namespace BotMood
{

    #pragma warning disable 1998

    public class CancelScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogStack stack;

        public CancelScorable(IDialogStack stack)
        {
            SetField.NotNull(out this.stack, nameof(stack), stack);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals("cancelar", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var activity = item as Activity;

            var client = new ConnectorClient(new Uri(activity.ServiceUrl));
            var clearMsg = activity.CreateReply();
            clearMsg.Text = "OK, operação cancelada.";
            await client.Conversations.SendToConversationAsync(clearMsg);

            this.stack.Reset();
        }

        private async Task AfterCancel(IDialogContext context, IAwaitable<object> result)
        {
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}