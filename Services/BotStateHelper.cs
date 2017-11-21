using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotMood
{
    [Serializable]
    public static class BotStateHelper<T>
    {
        
        public static async void SetUserData(Activity activity, T data)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                userData.SetProperty<T>(data.GetType().FullName, data);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetUserData(IDialogContext context, T data)
        {
            try
            {
                context.UserData.SetValue(data.GetType().FullName, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T GetUserData(IActivity activity)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
                var data = userData.GetProperty<T>(typeof(T).FullName);
                
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T GetUserData(IDialogContext context)
        {
            try
            {
                T result;
                context.UserData.TryGetValue(typeof(T).FullName, out result);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

    }
}