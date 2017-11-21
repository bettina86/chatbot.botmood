/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using NAudio.Wave;
using System.IO;

namespace BotLib
{
    public class AudioFormatConverterNAudio
    {
        /*
        public static string ConvertToWav(string nonWavFile, string targetPath)
        {
            var wavFile = Path.Combine(targetPath, Path.GetFileNameWithoutExtension(nonWavFile) + ".wav");

            //create folder if it's not there
            if (!Directory.Exists(targetPath))
            {
                BotUtilities.Trace("Creating Directory " + targetPath);
                Directory.CreateDirectory(targetPath);
            }

            if (nonWavFile.ToLower().EndsWith("wav"))
            {
                BotUtilities.Trace($"Just moving file from {nonWavFile} to {wavFile}");
                File.Move(nonWavFile, wavFile);
                return wavFile;
            }

            if (nonWavFile.ToLower().EndsWith("aac"))
            {
                ConvertFromAAC(nonWavFile, wavFile);
                return wavFile;
            }

            if (nonWavFile.ToLower().EndsWith("m4a"))
            {
                ConvertFromM4A(nonWavFile, wavFile);
                return wavFile;
            }

            if (nonWavFile.ToLower().EndsWith("mp4"))
            {
                ConvertFromMP4(nonWavFile, wavFile);
                return wavFile;
            }

            if (nonWavFile.ToLower().EndsWith(".mp3"))
            {
                ConvertFromMP3(nonWavFile, wavFile);
                return wavFile;
            }

            return nonWavFile;
        }

        private static void ConvertFromMP3(string nonWavFile, string wavFile)
        {
            BotUtilities.Trace($"Converting from MP3 {nonWavFile} to {wavFile}");

            using (Mp3FileReader reader = new Mp3FileReader(nonWavFile))
            {
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    WaveFileWriter.CreateWaveFile(wavFile, pcmStream);
                }
            }
        }

        private static void ConvertFromM4A(string nonWavFile, string wavFile)
        {
            BotUtilities.Trace($"Converting from M4A {nonWavFile} to {wavFile}");

            // create media foundation reader to read the AAC encoded file
            using (MediaFoundationReader reader = new MediaFoundationReader(nonWavFile))
            {
                // resample the file to PCM with same sample rate, channels and bits per sample
                using (ResamplerDmoStream resampledReader = new ResamplerDmoStream(reader, new WaveFormat(reader.WaveFormat.SampleRate, reader.WaveFormat.BitsPerSample, reader.WaveFormat.Channels)))
                {
                    // create WAVe file
                    using (WaveFileWriter waveWriter = new WaveFileWriter(wavFile, resampledReader.WaveFormat))
                    {
                        // copy samples
                        resampledReader.CopyTo(waveWriter);
                    }
                }
            }
        }

        private static void ConvertFromMP4(string nonWavFile, string wavFile)
        {
            BotUtilities.Trace($"Converting from MP4 {nonWavFile} to {wavFile}");

            // create media foundation reader to read the AAC encoded file
            using (MediaFoundationReader reader = new MediaFoundationReader(nonWavFile))
            {
                // resample the file to PCM with same sample rate, channels and bits per sample
                using (ResamplerDmoStream resampledReader = new ResamplerDmoStream(reader, new WaveFormat(reader.WaveFormat.SampleRate, reader.WaveFormat.BitsPerSample, reader.WaveFormat.Channels)))
                {
                    // create WAVe file
                    using (WaveFileWriter waveWriter = new WaveFileWriter(wavFile, resampledReader.WaveFormat))
                    {
                        // copy samples
                        resampledReader.CopyTo(waveWriter);
                    }
                }
            }
        }

        private static void ConvertFromAAC(string nonWavFile, string wavFile)
        {
            BotUtilities.Trace($"Converting from AAC {nonWavFile} to {wavFile}");

            // create media foundation reader to read the AAC encoded file
            using (MediaFoundationReader reader = new MediaFoundationReader(nonWavFile))
            {
                // resample the file to PCM with same sample rate, channels and bits per sample
                using (ResamplerDmoStream resampledReader = new ResamplerDmoStream(reader, new WaveFormat(reader.WaveFormat.SampleRate, reader.WaveFormat.BitsPerSample, reader.WaveFormat.Channels)))
                {
                    // create WAVe file
                    using (WaveFileWriter waveWriter = new WaveFileWriter(wavFile, resampledReader.WaveFormat))
                    {
                        // copy samples
                        resampledReader.CopyTo(waveWriter);
                    }
                }
            }
        }

        public static bool RemoveConvertedFromDisk(string wavFile)
        {
            try
            {
                BotUtilities.Trace($"Deleting file {wavFile}");
                File.Delete(wavFile);
                return true;
            }
            catch
            {
                return false;
            }
        }
        */
    }

}
