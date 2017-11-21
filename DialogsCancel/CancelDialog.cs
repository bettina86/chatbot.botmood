using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotMood
{
    [Serializable]
    public class CancelDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context);
        }

        public async Task MessageReceivedAsync(IDialogContext context) //, IAwaitable<IMessageActivity> result)
        {
            var msg = "OK, operação cancelada.";
            await context.PostAsync(msg);
            context.Done(true);
        }

    }
}