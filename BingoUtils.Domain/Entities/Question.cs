using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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