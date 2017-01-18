using System;

namespace BingoUtils.Helpers
{
    using System.Speech.Synthesis;

    namespace BingoUtils.Helpers
    {
        public static class AudioPlayer
        {
            private static SpeechSynthesizer _Synthesizer = new SpeechSynthesizer() { Rate = 3, Volume = 100 };
            private static Prompt CurrentSpeak;

            public static void PlaySpeech(string speech, Action whenFinished = null)
            {
                CurrentSpeak = _Synthesizer.SpeakAsync(speech);  
            }

            public static void StopSpeach()
            {
                if(CurrentSpeak == null)
                {
                    return;
                }

                _Synthesizer.SpeakAsyncCancel(CurrentSpeak);
                CurrentSpeak = null;
            }

            public static void AddSpeakCompletedHandler(Action handler)
            {
                var eventHandler = new EventHandler<SpeakCompletedEventArgs>((s, e) => handler());

                _Synthesizer.SpeakCompleted += eventHandler;
            }
        }
    }

}
