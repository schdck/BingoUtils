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

            /// <summary>
            /// Speaks an speech async
            /// </summary>
            /// <param name="speech">The speech to be speaked</param>
            public static void PlaySpeech(string speech)
            {
                CurrentSpeak = _Synthesizer.SpeakAsync(speech);  
            }

            /// <summary>
            /// Stops the current speaking
            /// </summary>
            public static void StopSpeach()
            {
                if(CurrentSpeak == null)
                {
                    return;
                }

                _Synthesizer.SpeakAsyncCancel(CurrentSpeak);

                CurrentSpeak = null;
            }

            /// <summary>
            /// Adds an EventHandler (from the action) to the SpeakCompleted event
            /// </summary>
            /// <param name="handler">The action to be converted to EventHandler</param>
            public static void AddSpeakCompletedHandler(Action handler)
            {
                var eventHandler = new EventHandler<SpeakCompletedEventArgs>((s, e) => handler());

                _Synthesizer.SpeakCompleted += eventHandler;
            }
        }
    }

}
