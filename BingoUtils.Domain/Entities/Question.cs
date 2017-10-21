namespace BingoUtils.Domain.Entities
{
    public class Question
    {
        /// <summary>
        /// Represents the question title
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Represents the question answer
        /// </summary>
        public string Answer { get; private set; }
        /// <summary>
        /// Represents the path to the image that should be displayed with the title
        /// </summary>
        public string TitleImagePath { get; private set; }
        /// <summary>
        /// Represents the path to the image that should be displayed with the answer
        /// </summary>
        public string AnswerImagePath { get; private set; }
        /// <summary>
        /// Indicates wheter the image that is displayed with the title is necessary to understand the question.
        /// So far, the only difference this makes is that if it's not, the image is not displayed on "Previous Question"
        /// </summary>
        public bool IsTitleImageImportant { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Question class.
        /// </summary>
        /// <param name="title">The question title</param>
        /// <param name="answer">The question answer</param>
        /// <param name="titleImagePath">The path to the image of the title</param>
        /// <param name="isTitleImageImportant">The path to the image of the answer</param>
        /// <param name="answerImagePath">A value indicating wheter the image of the title is necessary to understand the question</param>
        public Question(string title, string answer, string titleImagePath, bool isTitleImageImportant, string answerImagePath)
        {
            Title = title;
            Answer = answer;
            TitleImagePath = titleImagePath;
            IsTitleImageImportant = isTitleImageImportant;
            AnswerImagePath = answerImagePath;
        }
    }
}