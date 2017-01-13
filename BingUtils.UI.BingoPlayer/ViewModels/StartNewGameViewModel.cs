using BingoUtils.Domain.Entities;
using BingoUtils.Helpers.Event_Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.UI.BingoPlayer.ViewModels
{
    public class StartNewGameViewModel
    {
        public EventHandler<StartNewGameEventArgs> NemGameStarted { get; private set; }

        public SimpleDelegateCommand StartNewgameCommand { get; private set; }
        public SimpleDelegateCommand DroppedFileCommand { get; private set; }

        public StartNewGameViewModel(EventHandler<StartNewGameEventArgs> nemGameStarted)
        {
            NemGameStarted = nemGameStarted;

            StartNewgameCommand = new SimpleDelegateCommand((x) =>
            {
                NemGameStarted(this, (StartNewGameEventArgs)x);
            });

            DroppedFileCommand = new SimpleDelegateCommand((x) =>
            {
                NemGameStarted(this, (StartNewGameEventArgs)x);
            });
        }
    }
}