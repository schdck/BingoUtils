using BingoUtils.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace BingoUtils.Helpers
{
    public static class GameHelper
    {
        public static readonly string GamesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Bingo", "Jogos");
        private static readonly string ExtractedFilesDirectory = Path.Combine(Path.GetTempPath(), "BingoTemp");

        /// <summary>
        /// Load a game from file
        /// </summary>
        /// <param name="path">The complete path to the game file</param>
        /// <param name="temporaryFolder">The name of the folder where the game should be extracted inside de app's temporary folder</param>
        /// <returns>A list containing the questions loaded from the file</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="DirectoryNotFoundException" />
        /// /// <exception cref="FileNotFoundException">Thrown when the game file is not found</exception>
        /// <exception cref="NotSupportedException" />
        public static List<Question> LoadGame(string path, string temporaryFolder)
        {
            List<Question> list = new List<Question>();

            if (!File.Exists(path))
            {
                throw (new FileNotFoundException("Game file not found.", path));
            }

            string currentGameExtractedFilesDirectory = Path.Combine(ExtractedFilesDirectory, temporaryFolder);

            if (Directory.Exists(currentGameExtractedFilesDirectory))
            {
                Directory.Delete(currentGameExtractedFilesDirectory, true);
            }

            ZipFile.ExtractToDirectory(path, currentGameExtractedFilesDirectory);

            using (StreamReader reader = new StreamReader(Path.Combine(currentGameExtractedFilesDirectory, "Game.csv"), Encoding.GetEncoding("WINDOWS-1252")))
            {
                string line = reader.ReadLine(); // Skip header line

                while ((line = reader.ReadLine()) != null && !string.IsNullOrEmpty(line))
                {
                    string[] values = line.Split(';');

                    if(values.Length != 5)
                    {
                        throw new ArgumentException("The file is not a valid game");
                    }

                    string questionTitle = values[0],
                           questionAnswer = values[1],
                           titleImagePath = null,
                           answerImagePath = null;

                    bool isTitleImageImportant = values[3] == "true";

                    if (!string.IsNullOrEmpty(values[2]))
                    {
                        titleImagePath = Path.Combine(currentGameExtractedFilesDirectory, "img", values[2]);
                    }

                    if (!string.IsNullOrEmpty(values[4]))
                    {
                        answerImagePath = Path.Combine(currentGameExtractedFilesDirectory, "img", values[4]);
                    }

                    list.Add(new Question(questionTitle, questionAnswer, titleImagePath, isTitleImageImportant, answerImagePath));
                }
            }

            return list;
        }

        /// <summary>
        /// Combine the subject and the topic into a standard path and load a game from it
        /// </summary>
        /// <param name="subject">The subject of the game</param>
        /// <param name="topic">The topic</param>
        /// <param name="temporaryFolder">The name of the folder where the game should be extracted inside de app's temporary folder</param>
        /// <returns>A list containing the questions loaded from the file</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="DirectoryNotFoundException" />
        /// /// <exception cref="FileNotFoundException">Thrown when the game file is not found</exception>
        /// <exception cref="NotSupportedException" />
        public static List<Question> LoadGame(string subject, string topic, string temporaryFolder)
        {
            string path = Path.Combine(GamesDirectory, subject, string.Format("{0}.zip", topic));

            return LoadGame(path, temporaryFolder);
        }

        /// <summary>
        /// Gets the avaliable subjects at the game's folder on Doccuments
        /// </summary>
        /// <returns>An IEnumerable<string> containing the avaliable subjects</returns>
        /// <exception cref="UnauthorizedAccessException" />
        /// <exception cref="ArgumentException" />
        /// <exception cref="System.Security.SecurityException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="PathTooLongException" />
        /// <exception cref="IOException" />
        /// <exception cref="DirectoryNotFoundException" />
        public static IEnumerable<string> GetAvaliableSubjects()
        {
            var folders = new List<string>();

            foreach (string dir in Directory.GetDirectories(GamesDirectory))
            {
                folders.Add(new DirectoryInfo(dir).Name);
            }

            return folders;
        }

        /// <summary>
        /// Gets the avaliable topics for a subject at the game's folder on Doccuments
        /// </summary>
        /// <param name="subject">The subject to be used to get the avaliable topics</param>
        /// <returns>An IEnumerable<string> containing the avaliable topics for a subject</returns>
        /// <exception cref="UnauthorizedAccessException" />
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="PathTooLongException" />
        /// <exception cref="IOException" />
        /// <exception cref="DirectoryNotFoundException" />
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
