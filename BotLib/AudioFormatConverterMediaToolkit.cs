/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

namespace BotLib
{
    public class AudioFormatConverterMediaToolkit
    {
        public string SourceFileNameAndPath { get; private set; }

        public string ConvertedFileNameAndPath { get; private set; }

        public string TargetPath { get; private set; }

        public string ContentType { get; private set; }

        public AudioFormatConverterMediaToolkit(string sourceFileNameAndPath, string targetPath, string contentType)
        {
            if (string.IsNullOrWhiteSpace(sourceFileNameAndPath))
            {
                throw new Exception("Nome do arquivo vazio.");
            }
            else if (string.IsNullOrWhiteSpace(targetPath))
            {
                throw new Exception("Diretório destino vazio.");
            }
            else
            {
                this.ContentType = contentType;
                this.SourceFileNameAndPath = sourceFileNameAndPath;
                this.TargetPath = targetPath;
                this.ConvertedFileNameAndPath = Path.Combine(TargetPath, Path.GetFileNameWithoutExtension(this.SourceFileNameAndPath) + ".wav");

                //create folder if it's not there
                if (!Directory.Exists(TargetPath))
                {
                    Directory.CreateDirectory(TargetPath);
                }
            }
        }


        public void ConvertAudioFile()
        {
            switch (ContentType)
            {
                case "video/mp4":
                    ConvertMP4toWAV();
                    break;

                case "audio/aac":
                    ConvertAACtoWAV();
                    break;

                default:
                    JustMoveFile();
                    break;
            }
        }

        public void ConvertMP4toWAV()
        {
            var inputFile = new MediaFile { Filename = SourceFileNameAndPath };
            var outputFile = new MediaFile { Filename = ConvertedFileNameAndPath };

            var ffmpegPath = GetFFMPEGBinaryPath();
            using (var engine = new Engine(ffmpegPath))
            {
                engine.Convert(inputFile, outputFile);
            }
        }

        public void ConvertAACtoWAV()
        {
            var inputFile = new MediaFile { Filename = SourceFileNameAndPath };
            var outputFile = new MediaFile { Filename = ConvertedFileNameAndPath };

            var ffmpegPath = GetFFMPEGBinaryPath();
            using (var engine = new Engine(ffmpegPath))
            {
                engine.Convert(inputFile, outputFile);
            }
        }

        public void JustMoveFile()
        {
            BotUtilities.Trace("Just moving file to destination folder " + this.ContentType);
            File.Move(SourceFileNameAndPath, ConvertedFileNameAndPath);
        }

        public string GetFFMPEGBinaryPath()
        {
            var aux1 = BotUtilities.GetRootDirectory();
            aux1 = Path.Combine(aux1, "bin", "ffmpeg.exe");
            if(!File.Exists(aux1))
            {
                BotUtilities.Trace(aux1 + " não encontrado!!!");
            }
            return aux1;
        }
    }
}
