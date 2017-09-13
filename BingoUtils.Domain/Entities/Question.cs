namespace BingoUtils.Domain.Entities
{
    public class Question
    {
        public string Title { get; private set; }
        public string Answer { get; private set; }
        public string TitleImagePath { get; private set; }
        public string AnswerImagePath { get; private set; }

        public Question(string title, string answer, string titleImagePath, string answerImagePath)
        {
            Title = title;
            Answer = answer;
            TitleImagePath = titleImagePath;
            AnswerImagePath = answerImagePath;
        }
    }
}