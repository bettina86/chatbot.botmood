/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using System.Web.Configuration;
using BotLib;
using Microsoft.Bot.Builder.Dialogs;

namespace BotMood
{

    [Serializable]
    public static class StringMessages
    {
        public static string Versao { get; internal set; } = "Marcos Tito de Pardo Marques - https://www.linkedin.com/in/mrctito/ - BETA v.1.1";

        public static string Sobre { get; internal set; } = "O BotMood é um ChatBot que tenta detectar sentimentos!";

        public static string[] OpcoesAjuda { get; internal set; } =
            new string[]
            {
                "O BotMood é um ChatBot que tenta detectar sentimentos!",
                "Para testar basta escrever ou falar qualquer coisa e ele tentará descobrir o seu humor. "+
                "Dica: Procure escrever algo que faça sentido e se aproxime de uma opinião!"
            };
        public static string Ajuda() { return string.Join(" ", OpcoesAjuda); }

    }

    [Serializable]
    public static class CardMessages
    {

        public static Activity GetAboutCards(IDialogContext context)
        {
            return GetAboutCards((Activity)context.Activity);
        }

        public static Activity GetHelpCards(IDialogContext context)
        {
            return GetHelpCards((Activity)context.Activity);
        }

        public static Activity GetAboutCards(Activity activity)
        {
            var cards = CardUtilities.CreateCards();

            var image = new List<string>();
            image.Add(@"https://botmood.azurewebsites.net/images/iconNeutro.png");

            var card = CardUtilities.CreateHeroCard("BotMood", "Como você está se sentindo?", "", image);
            cards.Add(card.ToAttachment());

            var msg1 = activity.CreateReply();
            msg1.Attachments = cards;
            return msg1;
        }

        public static Activity GetHelpCards(Activity activity)
        {
            var msg3 = activity.CreateReply();
            msg3.Text = StringMessages.OpcoesAjuda[0] + " " + StringMessages.OpcoesAjuda[1];
            return msg3;
        }

    }

}