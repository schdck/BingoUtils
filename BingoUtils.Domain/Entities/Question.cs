namespace BingoUtils.Domain.Entities
{
    public class Question
    {
        public string Title { get; private set; }
        public string Answer { get; private set; }

        public Question(string title, string answer)
        {
            Title = title;
            Answer = answer;
        }
    }
}