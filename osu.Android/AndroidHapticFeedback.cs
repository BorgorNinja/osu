// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Android.Content;
using Android.OS;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;

namespace osu.Android
{
    /// <summary>
    /// Android haptic feedback implementation for osu! hit objects.
    /// Triggers vibration patterns based on judgement type.
    /// </summary>
    public class AndroidHapticFeedback : IHapticFeedbackProvider
    {
        private readonly Vibrator? vibrator;
        private readonly bool canUseVibrationEffect;

        public AndroidHapticFeedback(Context context)
        {
            vibrator = context.GetSystemService(Context.VibratorService) as Vibrator;
            canUseVibrationEffect = Build.VERSION.SdkInt >= BuildVersionCodes.O;
        }

        public void TriggerHitFeedback(JudgementResult result)
        {
            if (vibrator?.HasVibrator != true)
                return;

            // Scale feedback intensity based on judgement quality.
            int durationMs = result.Type switch
            {
                HitResult.Perfect or HitResult.Great => 20,
                HitResult.Good or HitResult.Ok => 30,
                HitResult.Meh => 40,
                _ => 0
            };

            if (durationMs == 0)
                return;

            vibrate(durationMs, amplitude: 80);
        }

        public void TriggerMissFeedback()
        {
            if (vibrator?.HasVibrator != true)
                return;

            // Two short pulses for miss.
            if (canUseVibrationEffect)
            {
                var waveform = VibrationEffect.CreateWaveform(
                    new long[] { 0, 40, 40, 40 },
                    new int[] { 0, 120, 0, 120 },
                    -1);
                vibrator.Vibrate(waveform);
            }
            else
            {
#pragma warning disable CA1422
                vibrator.Vibrate(new long[] { 0, 40, 40, 40 }, -1);
#pragma warning restore CA1422
            }
        }

        private void vibrate(int durationMs, int amplitude)
        {
            if (canUseVibrationEffect)
            {
                vibrator!.Vibrate(VibrationEffect.CreateOneShot(durationMs, amplitude));
            }
            else
            {
#pragma warning disable CA1422
                vibrator!.Vibrate(durationMs);
#pragma warning restore CA1422
            }
        }
    }
}
