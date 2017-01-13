using BingoUtils.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.Helpers.Event_Args
{
    public class StartNewGameEventArgs : EventArgs
    {
        public IEnumerable<Question> Questions;
        public string Disciplina { get; private set; }
        public string Assunto { get; private set; }

        public StartNewGameEventArgs(IEnumerable<Question> questions, string disciplina, string assunto)
        {
            Questions = questions;
            Disciplina = disciplina;
            Assunto = assunto;
        }
    }
}