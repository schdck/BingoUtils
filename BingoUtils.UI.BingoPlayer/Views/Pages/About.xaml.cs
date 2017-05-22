using BingoUtils.UI.Shared.Languages;
using System.Deployment.Application;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BingoUtils.UI.BingoPlayer.Views.Pages
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            string version = null;

            try
            {
                version = string.Format("v{0}", ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
            }
            catch (InvalidDeploymentException)
            {
                version = LanguageLocator.Instance.CurrentLanguage.OTHER_UNKNOWN_VERSION;
            }

            InitializeComponent();

            Version.Inlines.Add(new Run(version));
        }
    }
}
