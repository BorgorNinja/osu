// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.UI
{
    /// <summary>
    /// A component that triggers <see cref="IHapticFeedbackProvider"/> on note judgements.
    /// Intended for use on mobile platforms (Android, iOS). Injected into gameplay by the
    /// platform-specific game class when an <see cref="IHapticFeedbackProvider"/> is available.
    /// </summary>
    public partial class MobileHapticFeedbackComponent : Component
    {
        private readonly IHapticFeedbackProvider haptics;

        [Resolved]
        private ScoreProcessor scoreProcessor { get; set; } = null!;

        public MobileHapticFeedbackComponent(IHapticFeedbackProvider haptics)
        {
            this.haptics = haptics;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            scoreProcessor.NewJudgement += onNewJudgement;
        }

        private void onNewJudgement(JudgementResult result)
        {
            if (result.Type.IsMiss())
                haptics.TriggerMissFeedback();
            else if (result.Type.IsHit())
                haptics.TriggerHitFeedback(result);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            scoreProcessor.NewJudgement -= onNewJudgement;
        }
    }
}
