/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;

namespace BotLib
{
    public class CardUtilities
    {

        public static List<Attachment> CreateCards()
        {
            List<Attachment> cards = new List<Attachment>();
            return cards;
        }

        public static HeroCard GetResponseCard(string title, string subtitle, string text, string acao)
        {
            var card = CreateHeroCard(title, subtitle, text);
            if (!string.IsNullOrEmpty(acao))
            {
                AddCardAction(card, "Continuar", acao);
            }
            AddCardAction(card, "Voltar", "cancelar");
            return card;

        }


        public static List<Attachment> MakeResponseCard(string title, string subtitle, string text, string acao)
        {
            var cards = CardUtilities.CreateCards();
            cards.Add(GetResponseCard(title, subtitle, text, acao).ToAttachment());
            return cards;
        }


        public static HeroCard CreateHeroCard(string title, string subtitle, string text, List<string> imageList = null, Dictionary<string, string> actionList = null)
        {
            var card = new HeroCard();
            card.Title = title;
            card.Subtitle = subtitle;
            card.Text = text;
            card.Buttons = new List<CardAction>();
            card.Images = new List<CardImage>();

            if (imageList != null)
            {
                foreach(string img in imageList)
                {
                    var image = new CardImage()
                    {
                        Url = img
                    };
                    card.Images.Add(image);
                }
            }

            if (actionList != null)
            {
                foreach (var kv in actionList)
                {
                    var actionLabel = kv.Key;
                    var actionValue = kv.Value;
                    var action = new CardAction()
                    {
                        Title = actionLabel,
                        Type = ActionTypes.PostBack,
                        Value = actionValue
                    };
                    card.Buttons.Add(action);
                }
            }

            return card;
        }


        public static ThumbnailCard CreateThumbnailCard(string title, string subtitle, string text, List<string> imageList = null, Dictionary<string, string> actionList = null)
        {
            var card = new ThumbnailCard();
            card.Title = title;
            card.Subtitle = subtitle;
            card.Text = text;
            card.Buttons = new List<CardAction>();
            card.Images = new List<CardImage>();

            if (imageList != null)
            {
                foreach (string img in imageList)
                {
                    var image = new CardImage()
                    {
                        Url = img
                    };
                    card.Images.Add(image);
                }
            }

            if (actionList != null)
            {
                foreach (var kv in actionList)
                {
                    var actionLabel = kv.Key;
                    var actionValue = kv.Value;
                    var action = new CardAction()
                    {
                        Title = actionLabel,
                        Type = ActionTypes.PostBack,
                        Value = actionValue
                    };
                    card.Buttons.Add(action);
                }
            }

            return card;
        }


        public static HeroCard GetHeroConfirmCard(string title, string subtitle, string text)
        {
            var card = new HeroCard();
            card.Title = title;
            card.Subtitle = subtitle;
            card.Text = text;

            if (card.Buttons == null) card.Buttons = new List<CardAction>();

            var action = new CardAction()
            {
                Title = $"Sim",
                Type = ActionTypes.PostBack,  
                Value = $"Sim"
            };
            card.Buttons.Add(action);

            action = new CardAction()
            {
                Title = $"Não",
                Type = ActionTypes.PostBack,  
                Value = $"Não"
            };
            card.Buttons.Add(action);

            return card;
        }


        public static ThumbnailCard GetThumbnailConfirmCard(string title, string subtitle, string text)
        {
            var card = new ThumbnailCard();
            card.Title = title;
            card.Subtitle = subtitle;
            card.Text = text;

            if (card.Buttons == null) card.Buttons = new List<CardAction>();

            var action = new CardAction()
            {
                Title = $"Sim",
                Type = ActionTypes.PostBack,
                Value = $"Sim"
            };
            card.Buttons.Add(action);

            action = new CardAction()
            {
                Title = $"Não",
                Type = ActionTypes.PostBack,
                Value = $"Não"
            };
            card.Buttons.Add(action);

            return card;
        }


        public static void AddCardAction(HeroCard card, string title, string value, string actionType = ActionTypes.PostBack)
        {
            if (string.IsNullOrEmpty(actionType)) actionType = ActionTypes.PostBack;
            var cardAction = new CardAction();
            cardAction.Title = title;
            cardAction.Type = actionType;
            cardAction.Value = value;
            if (card.Buttons == null) card.Buttons = new List<CardAction>();
            card.Buttons.Add(cardAction);
        }

        public static void AddCardAction(ThumbnailCard card, string title, string value, string actionType = ActionTypes.PostBack)
        {
            if (string.IsNullOrEmpty(actionType)) actionType = ActionTypes.PostBack;
            var cardAction = new CardAction();
            cardAction.Title = title;
            cardAction.Type = actionType;
            cardAction.Value = value;
            if (card.Buttons == null) card.Buttons = new List<CardAction>();
            card.Buttons.Add(cardAction);
        }

        public static void AddCardImage(HeroCard card, string url)
        {
            var cardImage = new CardImage() { Url = url };
            if (card.Images == null) card.Images = new List<CardImage>();
            card.Images.Add(cardImage);
        }

        public static void AddCardImage(ThumbnailCard card, string url)
        {
            var cardImage = new CardImage() { Url = url };
            if (card.Images == null) card.Images = new List<CardImage>();
            card.Images.Add(cardImage);
        }

    }

}