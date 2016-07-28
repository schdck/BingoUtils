using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.Domain.Entities
{
    public class Question : IComparable<Question>
    {
        public int Id { get; set; }
        public string QuestionTitle { get; set; }
        public string Answer { get; set; }

        public Question(int Id)
        {
            this.Id = Id;
        }

        public int CompareTo(Question other)
        {
            return Id.CompareTo(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}