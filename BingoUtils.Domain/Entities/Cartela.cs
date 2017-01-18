using System.Collections.Generic;
using System.Linq;

namespace BingoUtils.Domain.Entities
{
    public class Cartela
    {
        public int Id { get; private set; }
        public int QuestionsCount { get; private set; }
        public bool IsFull
        {
            get
            {
                return Questions.Count == QuestionsCount;
            }
        }

        private SortedSet<int> Questions;

        public Cartela(int Id, int QuestionsCount)
        {
            this.Id = Id;
            this.QuestionsCount = QuestionsCount;
            Questions = new SortedSet<int>();
        }

        public bool AddQuestion(int id)
        {
            return Questions.Add(id);
        }

        public bool ContainsQuestion(int id)
        {
            return Questions.Contains(id);
        }

        public bool RemoveQuestion(int id)
        {
            return Questions.Remove(id);
        }

        public double GetSemelhanca(Cartela c)
        {
            if(QuestionsCount != c.QuestionsCount)
            {
                return -1;
            }

            int iguais = 0;

            for(int i = 0; i < QuestionsCount; i++)
            {
                if(c.Questions.Contains(Questions.ElementAt(i)))
                {
                    iguais++;
                }
            }

            return (((double) iguais) / QuestionsCount) * 100;
        }

        public string GetIds(string separator)
        {
            return string.Join(separator, Questions);
        }
    }
}
