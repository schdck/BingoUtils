using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace BingoUtils.UI.QuestionsSorter.UI_Helpers
{

    public static class MetroUtils
    {
        public static Task HideAllVisibleDialogs(this MetroWindow parent)
        {
            return Task.Run(async () =>
            {
                await parent.Dispatcher.Invoke(async () =>
                {
                    BaseMetroDialog dialogBeingShow = await parent.GetCurrentDialogAsync<BaseMetroDialog>();

                    while (dialogBeingShow != null)
                    {
                        await parent.HideMetroDialogAsync(dialogBeingShow);
                        dialogBeingShow = await parent.GetCurrentDialogAsync<BaseMetroDialog>();
                    }
                });
            });
        }

        public static Task<MessageDialogResult> ShowMessageAsync(this MetroWindow window, string title, string message, int timeout, MessageDialogResult defaultResult = MessageDialogResult.Affirmative, string displayFormat = "{0} ({1})", MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            Task<MessageDialogResult> task = new Task<MessageDialogResult>(() => ShowMessage(window, title, message, timeout, defaultResult, displayFormat, style, settings));
            task.Start();
            return task;
        }

        private static MessageDialogResult ShowMessage(MetroWindow window, string title, string message, int timeout, MessageDialogResult defaultResult = MessageDialogResult.Affirmative, string displayFormat = "{0} ({1})", MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            window.HideAllVisibleDialogs();

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Task<MessageDialogResult> dialogTask = null;
            MessageDialog dialog = null;
            bool timeouted = false;
            string defaultMessage = GetDefaultMessage(defaultResult, settings);

            ChangeDefaultMessage(defaultResult, string.Format(displayFormat, defaultMessage, timeout), ref settings);

            window.Dispatcher.Invoke(() =>
            {
                dialogTask = window.ShowMessageAsync(title, message, style, settings);
                dialogTask.ContinueWith((s) => { autoResetEvent.Set(); });
            });

            System.Timers.Timer timer = new System.Timers.Timer(1000) { AutoReset = false };

            BackgroundWorker worker = new BackgroundWorker();

            timer.Elapsed += async (s, e) =>
            {
                if (--timeout <= 0)
                {
                    timeouted = true;

                    if (dialog != null && dialog.IsVisible)
                    {
                        await System.Windows.Application.Current.Dispatcher.Invoke(async () =>
                        {
                            await window.HideMetroDialogAsync(dialog);
                        });
                    }
                    autoResetEvent.Set();
                }
                else
                {
                    if (dialog.IsVisible)
                    {
                        window.Dispatcher.Invoke(() => ChangeDefaultMessage(defaultResult, string.Format(displayFormat, defaultMessage, timeout), dialog));
                        timer.Start();
                    }
                }
            };

            worker.DoWork += (s, e) =>
            {
                while (dialog == null)
                {
                    window.Dispatcher.Invoke(async () =>
                    {
                        dialog = await window.GetCurrentDialogAsync<MessageDialog>();
                    });

                    if (dialog == null)
                    {
                        /*
                            * There where some cases where the dialog hasn't been shown yet, so it's best to
                            * hold the thread a little, so it gives a little time for the dialog to be shown
                            * before it checks again.
                            */
                        Thread.Sleep(250);
                    }
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                timer.Start();
            };

            worker.RunWorkerAsync();

            autoResetEvent.WaitOne();

            return timeouted ? defaultResult : dialogTask.Result;
        }

        private static string GetDefaultMessage(MessageDialogResult defaultResult, MetroDialogSettings settings)
        {
            settings = settings ?? new MetroDialogSettings();

            switch (defaultResult)
            {
                case MessageDialogResult.Affirmative:
                    return settings.AffirmativeButtonText;

                case MessageDialogResult.Negative:
                    return settings.NegativeButtonText;

                case MessageDialogResult.FirstAuxiliary:
                    return settings.FirstAuxiliaryButtonText;

                case MessageDialogResult.SecondAuxiliary:
                    return settings.SecondAuxiliaryButtonText;
            }

            return string.Empty;
        }

        private static void ChangeDefaultMessage(MessageDialogResult defaultResult, string message, ref MetroDialogSettings settings)
        {
            settings = settings ?? new MetroDialogSettings();

            switch (defaultResult)
            {
                case MessageDialogResult.Affirmative:
                    settings.AffirmativeButtonText = message;
                    break;
                case MessageDialogResult.Negative:
                    settings.NegativeButtonText = message;
                    break;
                case MessageDialogResult.FirstAuxiliary:
                    settings.FirstAuxiliaryButtonText = message;
                    break;
                case MessageDialogResult.SecondAuxiliary:
                    settings.SecondAuxiliaryButtonText = message;
                    break;
            }
        }

        private static void ChangeDefaultMessage(MessageDialogResult defaultResult, string message, MessageDialog dialog)
        {
            switch (defaultResult)
            {
                case MessageDialogResult.Affirmative:
                    dialog.AffirmativeButtonText = message;
                    break;
                case MessageDialogResult.Negative:
                    dialog.NegativeButtonText = message;
                    break;
                case MessageDialogResult.FirstAuxiliary:
                    dialog.FirstAuxiliaryButtonText = message;
                    break;
                case MessageDialogResult.SecondAuxiliary:
                    dialog.SecondAuxiliaryButtonText = message;
                    break;
            }
        }
    }
}
