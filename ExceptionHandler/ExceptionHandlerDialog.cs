/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BotMood
{

    [Serializable]
    public class ExceptionHandlerDialog<T> : IDialog<object>
    {
        private readonly IDialog<T> _dialog;
        private readonly bool _displayException;
        private readonly int _stackTraceLength;

        public ExceptionHandlerDialog(IDialog<T> dialog, bool displayException, int stackTraceLength = 500)
        {
            _dialog = dialog;
            _displayException = displayException;
            _stackTraceLength = stackTraceLength;
        }

        public async Task StartAsync(IDialogContext context)
        {
            try
            {
                context.Call(_dialog, ResumeAsync);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                if (_displayException)
                    await DisplayException(context, e).ConfigureAwait(false);
            }
        }

        private async Task ResumeAsync(IDialogContext context, IAwaitable<T> result)
        {
            try
            {
                context.Done(await result);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                if (_displayException)
                    await DisplayException(context, e).ConfigureAwait(false);
            }
        }

        private async Task DisplayException(IDialogContext context, Exception e)
        {
            var isEmulator = (context.Activity.ChannelId == "emulator");

            if (isEmulator)
            {
                await context.PostAsync(e.ToString()+" \n\n ").ConfigureAwait(false);
            }
            else
            {
                await context.PostAsync("Desculpe, ainda estou instável e aconteceu um erro. \n\n ").ConfigureAwait(false);
                await context.PostAsync("Experimente digitar o comando /deleteprofile com um espaço antes do caracter '/' \n\n ").ConfigureAwait(false);
                await context.PostAsync(e.Message).ConfigureAwait(false);
            }
            context.Done(true);
        }
    }

}