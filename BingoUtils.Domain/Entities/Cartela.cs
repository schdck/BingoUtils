using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.Domain.Entities
{
    public class Cartela
    {
        public int QuestionsCount { get; private set; }

        private HashSet<Question> Questions;

        public Cartela(int QuestionsCount)
        {
            this.QuestionsCount = QuestionsCount;

            Questions = new HashSet<Question>();
        }

        public bool AddQuestion(Question q)
        {
            return Questions.Add(q);
        }

        public void RemoveQuestion(Question q)
        {
            Questions.Remove(q);
        }

        public void RemoveQuestion(int id)
        {
            RemoveQuestion(Questions.SingleOrDefault((x) => x.Id == id));
        }
    }
}
