/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
namespace BotLib
{
    using System.IO;
    using System.Threading;
    using Microsoft.CognitiveServices.SpeechRecognition;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Text;

    public enum DictationType { Short, Long};

    public class SpeechToTextService
    {
        public List<RecognitionResult> Result { get; private set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }

        private AutoResetEvent waitHandle;

        private void SpeechClient_OnPartialResponseReceived(object sender, PartialSpeechResponseEventArgs e)
        {
            var s = e.PartialResult;
        }

        private void OnShortResponseReceived(object sender, SpeechResponseEventArgs e)
        {
            HasError = false;
            ErrorMessage = "";
            Result.Add(e.PhraseResponse);
            waitHandle.Set();
        }

        private void OnLongResponseReceived(object sender, SpeechResponseEventArgs e)
        {
            HasError = false;
            ErrorMessage = "";
            if (e.PhraseResponse.Results.Length > 0)
            {
                Result.Add(e.PhraseResponse);
            }
            if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.EndOfDictation ||
               e.PhraseResponse.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout)
            {
                waitHandle.Set();
            }
        }


        private void OnConversationError(object sender, SpeechErrorEventArgs e)
        {
            HasError = true;
            ErrorMessage = $" {e.SpeechErrorCode} {e.SpeechErrorText}";
            waitHandle.Set();
        }


        public void ConvertSpeechToText(string fileNameAndPath, DictationType dictationType)
        {
            waitHandle = new AutoResetEvent(false);
            SendWavAudio(fileNameAndPath, dictationType);
            waitHandle.WaitOne();
        }

        public string ResultAsText()
        {
            var sb = new StringBuilder();
            foreach (var r in this.Result)
            {
                if (r.Results.Length > 0)
                {
                    sb.AppendLine(r.Results[0].DisplayText);
                }
            }
            return sb.ToString();
        }

        private void SendWavAudio(string wavFileName, DictationType dictationType)
        {
            Result = new List<RecognitionResult>();

            var speechClient = CreateSpeechClient(dictationType);

            using (FileStream fileStream = new FileStream(wavFileName, FileMode.Open, FileAccess.Read))
            {
                // Note for wave files, we can just send data from the file right to the server.
                // In the case you are not an audio file in wave format, and instead you have just
                // raw data (for example audio coming over bluetooth), then before sending up any 
                // audio data, you must first send up an SpeechAudioFormat descriptor to describe 
                // the layout and format of your raw audio data via DataRecognitionClient's sendAudioFormat() method.
                int bytesRead = 0;
                byte[] buffer = new byte[1024];

                try
                {
                    do
                    {
                        // Get more Audio data to send into byte buffer.
                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                        // Send of audio data to service. 
                        speechClient.SendAudio(buffer, bytesRead);
                    }
                    while (bytesRead > 0);
                }
                finally
                {
                    // We are done sending audio.  Final recognition results will arrive in OnResponseReceived event call.
                    speechClient.EndAudio();
                }
            }
        }


        private DataRecognitionClient CreateSpeechClient(DictationType dictationType)
        {
            var key = ConfigurationManager.AppSettings["MicrosoftSpeechApiKey"];
            var speechClient = SpeechRecognitionServiceFactory.CreateDataClient(
                //SpeechRecognitionMode.ShortPhrase,
                SpeechRecognitionMode.LongDictation,
                "pt-BR",
                key
                );
            speechClient.AuthenticationUri = "";
            if (dictationType == DictationType.Short)
            {
                speechClient.OnResponseReceived += this.OnShortResponseReceived;
            }
            else
            {
                speechClient.OnResponseReceived += this.OnLongResponseReceived;
            }
            speechClient.OnConversationError += this.OnConversationError;
            speechClient.OnPartialResponseReceived += SpeechClient_OnPartialResponseReceived;
            return speechClient;
        }

    }
}