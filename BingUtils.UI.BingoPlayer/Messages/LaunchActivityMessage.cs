using BingoUtils.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.UI.BingoPlayer.Messages
{
    public class LaunchActivityMessage
    {
        public Activity Activity { get; private set; }

        public LaunchActivityMessage(Activity activity)
        {
            Activity = activity;
        }
    }
}
