/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using Microsoft.Bot.Connector;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Configuration;

namespace BotLib
{
    /// <summary>
    /// Represents an incoming voice message from facebook messenger, 
    /// where the voice data is in an MP4 file (the message contains a link to download it).
    /// </summary>
    public class ReceiveIncomingVoiceMessage
    {
        #region Properties
        /// <summary>
        /// URL of the MP4 file sent by user and stored on facebook's servers.
        /// </summary>
        public Uri OriginalAudioFileUrl { get; private set; }
        
        /// <summary>
        /// Local filename of the MP4 file after it has been downloaded from Facebook.
        /// </summary>
        private string OriginalAudioLocalFileName { get; set; }

        /// <summary>
        /// Path to the folder on local disk containing the downloaded voice messages from Facebook.
        /// This is configured in Web.config using the FacebookDownloadedVoiceMessagesFolder key.
        /// The path in the Web.config will be relative to the site's root folder.
        /// </summary>
        public string VoiceMessageFolder { get; private set; }

        /// <summary>
        /// Content-type of the attachment (for debugging - it's not always MP4).
        /// </summary>
        public string ContentType { get; private set; }
        #endregion

        #region Constructors

        public ReceiveIncomingVoiceMessage(IMessageActivity activity)
        {
            var audioAttachment = activity.Attachments?.FirstOrDefault(a => a.ContentType.Equals("video/mp4") || 
                                                                            a.ContentType.Contains("audio") || 
                                                                            a.ContentType.Contains("video") ||
                                                                            a.ContentType.Equals("audio/aac") ||
                                                                            a.ContentType.Equals("audio/wav") ||
                                                                            a.ContentType.Equals("application/octet-stream")
                                                                            );
            if (audioAttachment != null)
            {
                BotUtilities.Trace("Attachment content " + audioAttachment.ContentType);
                this.OriginalAudioFileUrl = new Uri(audioAttachment.ContentUrl);
                this.VoiceMessageFolder = GetVoiceMessagesFolderFromWebConfig();
                this.ContentType = audioAttachment.ContentType;
            }
            else
            {
                BotUtilities.Trace("No Attachment content");
            }
        }
        #endregion

        #region Public methods
        public string DownloadFile()
        {
            var filename = GetRandomFileName();
            var filenameWithPath = VoiceMessageFolder + @"\" + filename;
 
            //if folder doesn't exist, create it
            if (!Directory.Exists(VoiceMessageFolder))
            {
                Directory.CreateDirectory(VoiceMessageFolder);
            }

            using (var client = new WebClient())
            {
                client.DownloadFile(this.OriginalAudioFileUrl, filenameWithPath);
            }

            OriginalAudioLocalFileName = filename;

            return filename;
        }

        /// <summary>
        /// Removes the downloaded MP4 file from the local disk to clean up space.
        /// </summary>
        /// <returns>True if successfully removed, false otherwise.</returns>
        public bool RemoveFromDisk()
        {
            try
            {
                File.Delete(GetLocalPathAndFileName());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the full local path and filename to the downloaded MP4 voice message.
        /// </summary>
        /// <returns>E.g. D:\home\site\wwwroot\abc.mp4</returns>
        public string GetLocalPathAndFileName()
        {
            if (string.IsNullOrWhiteSpace(OriginalAudioLocalFileName))
            {
                throw new Exception("The voice message has not been downloaded yet.");
            }

            return VoiceMessageFolder + @"\" + OriginalAudioLocalFileName;
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Reads Web.config and returns the path to the folder which will store downloaded messages.
        /// The folder in the config must be relative to the site's root.
        /// </summary>
        /// <returns>Full path to the folder that will be used to store MP4 voice messages.</returns>
        private string GetVoiceMessagesFolderFromWebConfig()
        {
            return Path.Combine(BotUtilities.GetRootDirectory(), WebConfigurationManager.AppSettings["DownloadedVoiceMessagesFolder"]);
        }

        /// <summary>
        /// Generates a random filename using a new GUID.
        /// </summary>
        /// <returns>A random file name in the format "msg-GUID.mp4".</returns>
        private string GetRandomFileName()
        {
            string ext = "";
            switch (ContentType)
            {
                case "video/mp4":
                    ext = ".mp4";
                    break;

                case "video/mp3":
                    ext = ".mp3";
                    break;

                case "video/aac":
                    ext = ".aac";
                    break;

                case "video/wav":
                    ext = ".wav";
                    break;
            }

            return "msg-" + Guid.NewGuid() + ext;
        }
        #endregion
    }

}