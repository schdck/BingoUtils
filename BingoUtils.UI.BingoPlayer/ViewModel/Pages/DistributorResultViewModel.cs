using BingoUtils.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class DistributorResultViewModel : BaseViewModel
    {
        public Cartela[] Cartelas { get; private set; }
        public int MaxSemelhanca { get; private set; }

        public ObservableCollection<object> QuestionList { get; private set; }

        public DistributorResultViewModel(Cartela[] cartelas, int maxSemelhanca)
        {
            Cartelas = cartelas;
            MaxSemelhanca = maxSemelhanca;
            QuestionList = new ObservableCollection<object>();

            int[] maxSemelhancas = new int[Cartelas.Length];
            int[] maxSemelhancasWith = new int[Cartelas.Length];

            for(int i = 0; i < Cartelas.Length; i++)
            {
                for(int j = 0; j < Cartelas.Length; j++)
                {
                    if (i != j)
                    {
                        int temp = (int)cartelas[i].GetSemelhanca(cartelas[j]);

                        if (temp > maxSemelhancas[i])
                        {
                            maxSemelhancas[i] = temp;
                            maxSemelhancasWith[i] = j + 1;
                        }
                    }
                }
            }

            for(int i = 0; i < Cartelas.Length; i++)
            {
                QuestionList.Add(new { Id = i + 1, MaxSemelhanca = maxSemelhancas[i], MaxSemelhancaCom = maxSemelhancasWith[i], Questions = cartelas[i].GetIds(", ") });
            }
        }
    }
}
