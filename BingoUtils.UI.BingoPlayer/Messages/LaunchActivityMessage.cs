using BingoUtils.Domain.Enums;

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
