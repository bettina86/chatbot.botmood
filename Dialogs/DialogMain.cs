/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
namespace BotMood
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using System.Threading;
    using System.Collections.Generic;
    using BotLib;

    public class Medidor
    {
        public double FaixaAte { get; internal set; } = 0;
        public string Image { get; internal set; } = "";
    }

    public class Medidores
    {
        public Medidores()
        {
            Lista = new Medidor[4];
            Lista[0] = new Medidor { FaixaAte = 0.25, Image = "imagem1.png" };
            Lista[1] = new Medidor { FaixaAte = 0.50, Image = "imagem2.png" };
            Lista[2] = new Medidor { FaixaAte = 0.75, Image = "imagem3.png" };
            Lista[3] = new Medidor { FaixaAte = 1.00, Image = "imagem4.png" };
        }

        public Medidor[] Lista { get; internal set; }

        public Medidor CalculaMedidor(double value)
        {
            if (value <= Lista[0].FaixaAte)
            {
                return Lista[0];
            }
            else
            if (value <= Lista[1].FaixaAte)
            {
                return Lista[1];
            }
            else
            if (value <= Lista[2].FaixaAte)
            {
                return Lista[2];
            }
            else
            if (value <= Lista[3].FaixaAte)
            {
                return Lista[3];
            }
            return new Medidor();
        }
    }


    [Serializable]
    public class DialogMain : IDialog<object>
    {

        public DialogMain()
        {

        }

        public async Task StartAsync(IDialogContext context)
        {
            await BotUtilities.BotIsTyping(context);
            context.Wait(ReadText);
        }

        private async Task ReadText(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var txt = await result;
            await ProcessText(context, txt.Text.Trim().ToLower());
        }

        public async Task ProcessText(IDialogContext context, string query)
        {
            switch (query)
            {
                case "olá":
                case "ola":
                case "ajuda":
                case "oi":
                case "socorro":
                case "cancelar":
                case "começar":
                case "comecar":
                case "recomeçar":
                case "recomecar":
                    await Wellcome(context);
                    context.Wait(ReadText);
                    return;
            }


            var medidores = new Medidores();

            var textAnalysis = new TextAnalysisService();
            double sentiment = await textAnalysis.Sentiment(language: "pt", text: query);
            var medidor = medidores.CalculaMedidor(sentiment);
            var image = @"https://botmood.azurewebsites.net/images/" + medidor.Image;

            await context.PostAsync("O grau de emoção  é um valor entre 0 e 1, sendo que 0 significa totalmente negativo e 1 totalmente positivo.");

            await context.PostAsync("Naturalmente meu grau de precisão é menor quanto menor for a frase. A brincadeira fica mais legal " +
                "se você escrever uma opinião, tal qual faria em um site de opiniões como o TripAdvisor.");

            var img = new List<string>() { image };
            var card1 = CardUtilities.CreateHeroCard("", "Grau de emoção", sentiment.ToString("0.00"), img, null);
            var reply1 = context.MakeMessage();
            reply1.Attachments = CardUtilities.CreateCards();
            reply1.Attachments.Add(card1.ToAttachment());
            await context.PostAsync(reply1);

            var keyPhrases = await textAnalysis.KeyPhrases(language: "pt", text: query);
            if ((keyPhrases != null) && (keyPhrases.Count > 0))
            {
                var key = "";
                foreach (var s in keyPhrases)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (key.Length > 0) key = key + ", ";
                        key = key + s;
                    }
                }
                var card2 = CardUtilities.CreateHeroCard("", "Palavras chave", key, null, null);
                var reply2 = context.MakeMessage();
                reply2.Attachments = CardUtilities.CreateCards();
                reply2.Attachments.Add(card2.ToAttachment());
                await context.PostAsync(reply2);
            }

            //var detectLanguage = await textAnalysis.DetectLanguage(result.Query);

            context.Wait(ReadText);
        }


        private static async Task Wellcome(IDialogContext context)
        {
            var msg0 = context.MakeMessage();
            msg0.Text = StringMessages.Versao;
            await context.PostAsync(msg0);

            var msg1 = CardMessages.GetAboutCards(context);
            await context.PostAsync(msg1);

            var msg2 = CardMessages.GetHelpCards(context);
            await context.PostAsync(msg2);
        }
    }

}
