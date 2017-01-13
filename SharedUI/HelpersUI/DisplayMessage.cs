using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.UI.SharedUI.HelpersUI
{
    public static class DisplayMessage
    {
        public static void DisplayError(string message, string caption="Atenção!")
        {
            Console.WriteLine(message);
        }
    }
}
