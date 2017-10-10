using BingoUtils.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.Helpers
{
    public static class GameHelper
    {
        public static readonly string GamesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Bingo", "Jogos");
        private static readonly string ExtractedFilesDirectory = Path.Combine(Path.GetTempPath(), "BingoTemp");

        public static List<Question> LoadGame(string path, string temporaryFolder)
        {
            List<Question> list = new List<Question>();

            if (!File.Exists(path))
            {
                throw (new Exception("Erro ao abrir arquivo"));
            }

            string currentGameExtractedFilesDirectory = Path.Combine(ExtractedFilesDirectory, temporaryFolder);

            if (Directory.Exists(currentGameExtractedFilesDirectory))
            {
                Directory.Delete(currentGameExtractedFilesDirectory, true);
            }

            ZipFile.ExtractToDirectory(path, currentGameExtractedFilesDirectory);

            using (StreamReader reader = new StreamReader(Path.Combine(currentGameExtractedFilesDirectory, "Game.csv"), Encoding.GetEncoding("WINDOWS-1252")))
            {
                try
                {
                    string line = reader.ReadLine(); // Pular a linha de cabeçalho

                    while ((line = reader.ReadLine()) != null && !string.IsNullOrEmpty(line))
                    {
                        string[] values = line.Split(';');

                        string questionTitle = values[0],
                               questionAnswer = values[1],
                               titleImagePath = null,
                               answerImagePath = null;

                        if (!string.IsNullOrEmpty(values[2]))
                        {
                            titleImagePath = Path.Combine(currentGameExtractedFilesDirectory, "img", values[2]);
                        }

                        if (!string.IsNullOrEmpty(values[4]))
                        {
                            answerImagePath = Path.Combine(currentGameExtractedFilesDirectory, "img", values[4]);
                        }

                        list.Add(new Question(questionTitle, questionAnswer, titleImagePath, values[3] == "true", answerImagePath));
                    }
                }
                catch (Exception e)
                {
                    throw (new Exception("Erro ao abrir arquivo", e));
                }
            }

            return list;
        }

        public static List<Question> LoadGame(string subject, string topic, string temporaryFolder)
        {
            string path = Path.Combine(GamesDirectory, subject, string.Format("{0}.zip", topic));

            return LoadGame(path, temporaryFolder);
        }

        public static IEnumerable<string> GetAvaliableSubjects()
        {
            var folders = new List<string>();

            foreach (string dir in Directory.GetDirectories(GamesDirectory))
            {
                folders.Add(new DirectoryInfo(dir).Name);
            }

            return folders;
        }

        public static IEnumerable<string> GetAvaliableTopicsForSubject(string subject)
        {
            IEnumerable<string> files;

            try
            {
                files = Directory.GetFiles(
                            Path.Combine(GamesDirectory, subject))
                                .Where((x) => (Path.GetExtension(x) == ".zip"));
            }
            catch
            {
                return null;
            }


            List<string> fileNamesWithoutExtension = new List<string>();

            foreach (string s in files)
            {
                fileNamesWithoutExtension.Add(Path.GetFileNameWithoutExtension(s));
            }

            return fileNamesWithoutExtension;
        }

    }
}
