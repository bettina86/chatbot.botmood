
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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.IO;
using System.Web.Configuration;

namespace BotLib
{
    public class AttachedAudioProcessor
    {
        public static async Task ReceiveAttachedAudio(Microsoft.Bot.Connector.Activity activity)
        {
            switch (activity.ChannelId)
            {
                case "facebook":
                case "messenger":
                    await ReceiveFacebookMessengerAudio(activity);
                    break;

                case "skype":
                    // não testado ainda
                    await ReceiveSkypeAudio(activity);
                    break;
            }
        }

        private static async Task ReceiveFacebookMessengerAudio(Microsoft.Bot.Connector.Activity activity)
        {
            var ConvertedFileNameAndPath = "";

            var audioAttachment = activity.Attachments?.FirstOrDefault(a => a.ContentType.Equals("audio/aac") ||
                                                                            a.ContentType.Equals("video/mp4") ||
                                                                            a.ContentType.Equals("video/mp3") ||
                                                                            a.ContentType.Equals("audio/wav") ||
                                                                            a.ContentType.Equals("audio") ||
                                                                            a.ContentType.Equals("video") ||
                                                                            a.ContentType.Equals("application/octet-stream"));
            if (audioAttachment != null)
            {
                BotUtilities.Trace("Possui audio. ContentType is " + audioAttachment.ContentType);
                BotUtilities.Trace("Root Directory " + BotUtilities.GetRootDirectory());

                try
                {
                    var voiceMessage = new ReceiveIncomingVoiceMessage(activity);
                    voiceMessage.DownloadFile();
                    var originalAudioFileName = voiceMessage.GetLocalPathAndFileName();
                    BotUtilities.Trace("originalAudioFile " + originalAudioFileName);

                    var wavFolder = Path.Combine(BotUtilities.GetRootDirectory(), WebConfigurationManager.AppSettings["WAVFilesFolder"]);
                    BotUtilities.Trace("wavFolder  " + wavFolder);

                    var audioConverter = new AudioFormatConverterMediaToolkit(originalAudioFileName, wavFolder, voiceMessage.ContentType);
                    audioConverter.ConvertAudioFile();
                    ConvertedFileNameAndPath = audioConverter.ConvertedFileNameAndPath;
                }
                catch (Exception ex)
                {
                    BotUtilities.Trace("Erro ao fazer download do audio: " + ex.ToString());
                }

                try
                {
                    BotUtilities.Trace("Convertendoi áudio WAV para texto");
                    var speech = new SpeechToTextService();
                    if (!string.IsNullOrEmpty(ConvertedFileNameAndPath))
                    {
                        speech.ConvertSpeechToText(ConvertedFileNameAndPath, DictationType.Long);
                        var txt = speech.ResultAsText();
                        activity.Text = txt;
                        await BotUtilities.Say(activity, "Você disse: " + txt);
                    }
                }
                catch (Exception ex)
                {
                    BotUtilities.Trace("Erro ao converter audio para texto: " + ex.ToString());
                }
            }
            else
            {
                BotUtilities.Trace("No Attachment content");
            }
        }


        //- não testado!
        private static async Task ReceiveSkypeAudio(Activity activity)
        {
            BotUtilities.Trace("ReceiveSkypeAudio");
            if (activity.Attachments != null && activity.Attachments.Any())
            {
                BotUtilities.Trace("Possui arquivo anexado");
                var attachment = activity.Attachments.First();
                using (HttpClient httpClient = new HttpClient())
                {
                    // Skype & MS Teams attachment URLs are secured by a JwtToken, so we need to pass the token from our bot.
                    if ((activity.ChannelId.Equals("skype", StringComparison.InvariantCultureIgnoreCase) || activity.ChannelId.Equals("msteams", StringComparison.InvariantCultureIgnoreCase))
                        && new Uri(attachment.ContentUrl).Host.EndsWith("skype.com"))
                    {
                        var token = await new MicrosoftAppCredentials().GetTokenAsync();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    var responseMessage = await httpClient.GetAsync(attachment.ContentUrl);
                    var contentLenghtBytes = responseMessage.Content.Headers.ContentLength;

                    await BotUtilities.Say(activity, $"Attachment of {attachment.ContentType} type and size of {contentLenghtBytes} bytes received.");
                }
            }
        }

        //- não testado!
        private static async Task<Stream> GetAudioStream(Activity activity, Attachment audioAttachment)
        {
            ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));

            using (var httpClient = new HttpClient())
            {
                // The Skype attachment URLs are secured by JwtToken,
                // you should set the JwtToken of your bot as the authorization header for the GET request your bot initiates to fetch the image.
                // https://github.com/Microsoft/BotBuilder/issues/662
                var uri = new Uri(audioAttachment.ContentUrl);
                if (uri.Host.EndsWith("skype.com") && uri.Scheme == "https")
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(connector));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                }

                return await httpClient.GetStreamAsync(uri);
            }
        }

        // Gets the JwT token of the bot. 
        private static async Task<string> GetTokenAsync(ConnectorClient connector)
        {
            var credentials = connector.Credentials as MicrosoftAppCredentials;
            if (credentials != null)
            {
                return await credentials.GetTokenAsync();
            }

            return null;
        }


    }
}




