using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BotMood
{
    public class DataStorage
    {

        public static T Read<T>(IDialogContext context) where T : new()
        {
             var data = BotStateHelper<T>.GetUserData(context);

            if (data == null)
            {
                data = new T();
            }
            return data;
        }

        public static T Read<T>(IActivity activity) where T : new()
        {
            T data = default(T);

            data = BotStateHelper<T>.GetUserData(activity);

            if (data == null)
            {
                data = new T();
            }
            return data;
        }

        public static void Save<T>(IDialogContext context, T data)
        {
            BotStateHelper<T>.SetUserData(context, data);
        }

    }

}