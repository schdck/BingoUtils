using System.Collections.Generic;
using System.Linq;

namespace BingoUtils.Domain.Entities
{
    public class Card
    {
        /// <summary>
        /// The SortedSet that contains the question IDs
        /// </summary>
        private SortedSet<int> Questions;

        /// <summary>
        /// The ID of the card
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// The maximum amount of questions to be added to the card
        /// </summary>
        public int QuestionsCount { get; private set; }
        /// <summary>
        /// Indicates wheter the card is full or not
        /// </summary>
        public bool IsFull
        {
            get
            {
                return Questions.Count == QuestionsCount;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Card class.
        /// </summary>
        /// <param name="id">The ID of the card</param>
        /// <param name="questionCount">The maximum amount of questions to be added in this card</param>
        public Card(int id, int questionCount)
        {
            Id = id;
            QuestionsCount = questionCount;
            Questions = new SortedSet<int>();
        }

        /// <summary>
        /// Adds an question to the card
        /// </summary>
        /// <param name="id">The question ID</param>
        /// <returns>True if ID is added to the card; otherwise, false</returns>
        public bool AddQuestion(int id)
        {
            return Questions.Add(id);
        }

        /// <summary>
        /// Checks wheter the card contains the question
        /// </summary>
        /// <param name="id">The ID of the question</param>
        /// <returns>True if the card contains the question; otherwise, false</returns>
        public bool ContainsQuestion(int id)
        {
            return Questions.Contains(id);
        }

        /// <summary>
        /// Removes an question to the card
        /// </summary>
        /// <param name="id">The question ID</param>
        /// <returns>True if ID is removed to the card; otherwise, false</returns>
        public bool RemoveQuestion(int id)
        {
            return Questions.Remove(id);
        }

        /// <summary>
        /// Compares a card to other and returns an percentage indicating how similar they are
        /// </summary>
        /// <param name="c">The card to be compared to</param>
        /// <returns>A porcentage indicating how similar both cards are</returns>
        public double GetSimilarity(Card c)
        {
            if(QuestionsCount != c.QuestionsCount)
            {
                return -1;
            }

            int amout_of_equals = 0;

            for(int i = 0; i < QuestionsCount; i++)
            {
                if(c.Questions.Contains(Questions.ElementAt(i)))
                {
                    amout_of_equals++;
                }
            }

            return (((double) amout_of_equals) / QuestionsCount) * 100;
        }

        /// <summary>
        /// Returns a string with all the question IDs added to this card
        /// </summary>
        /// <param name="separator">The separator of the elements</param>
        /// <returns>A string with all the question IDs added to this card</returns>
        public string GetIds(string separator)
        {
            return string.Join(separator, Questions);
        }

        /// <summary>
        /// Returns an IEnumerable<int> containing all the question IDs added to this card
        /// </summary>
        /// <returns>An IEnumerable<int> containing all the question IDs added to this card</returns>
        public IEnumerable<int> GetIds()
        {
            return Questions.AsEnumerable();
        }
    }
}
