using BingoUtils.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.UI.BingoPlayer.Messages
{
    public class LaunchFinishedDistributionMessage
    {
        public Cartela[] Cartelas { get; private set; }
        public int MaxSemelhanca { get; private set; }

        public LaunchFinishedDistributionMessage(Cartela[] cartelas, int maxSemelhanca)
        {
            Cartelas = cartelas;
            MaxSemelhanca = maxSemelhanca;
        }
    }
}