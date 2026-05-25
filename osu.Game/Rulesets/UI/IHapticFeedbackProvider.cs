// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.UI
{
    /// <summary>
    /// Provides haptic/vibration feedback on supported platforms (e.g. Android, iOS).
    /// </summary>
    public interface IHapticFeedbackProvider
    {
        /// <summary>
        /// Trigger a short tap feedback scaled to the given hit judgement quality.
        /// </summary>
        void TriggerHitFeedback(JudgementResult result);

        /// <summary>
        /// Trigger a distinctive buzz pattern to signal a miss.
        /// </summary>
        void TriggerMissFeedback();
    }
}
