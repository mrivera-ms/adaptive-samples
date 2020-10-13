using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Translation;

namespace MultilingualBot.Translation
{
    public class MessageActivityTranslator
    {
        private readonly MicrosoftTranslator _translator;

        public MessageActivityTranslator(MicrosoftTranslator translator)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }

        /// <summary>
        /// Translates supported elements of an activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="targetLocale"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task TranslateActivityAsync(IMessageActivity activity, string targetLocale,
            CancellationToken cancellationToken = default)
        {
            if (activity.Type != ActivityTypes.Message)
            {
                return;
            }

            activity.Text = await TranslateTextAsync(activity.Text, targetLocale, cancellationToken);

            if (activity.SuggestedActions!=null && activity.SuggestedActions.Actions.Any())
            {
                await TranslateCardActionsAsync(activity.SuggestedActions.Actions, targetLocale, cancellationToken);
            }

            if (activity.Attachments.Any())
            {
                foreach (var attachment in activity.Attachments)
                {
                    switch (attachment.ContentType)
                    {
                        case HeroCard.ContentType:
                            await TranslateHeroCardAsync((HeroCard) attachment.Content, targetLocale, cancellationToken);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        internal async Task TranslateMessageActivityAsync(IMessageActivity activity, string targetLocale, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (activity.Type == ActivityTypes.Message)
            {
               activity.Text = await TranslateTextAsync(activity.Text, targetLocale, cancellationToken);
            }
        }

        internal async Task<string> TranslateTextAsync(string text, string targetLocale, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _translator.TranslateAsync(text, targetLocale, cancellationToken);
        }

        private async Task TranslateHeroCardAsync(HeroCard card, string targetLocale, CancellationToken cancellationToken = default)
        {
            card.Title = await TranslateTextAsync(card.Title, targetLocale, cancellationToken);
            card.Subtitle = await TranslateTextAsync(card.Subtitle, targetLocale, cancellationToken);
            card.Text = await TranslateTextAsync(card.Text, targetLocale, cancellationToken);
            await TranslateCardActionsAsync(card.Buttons, targetLocale, cancellationToken);
        }

        private async Task TranslateCardActionsAsync(IEnumerable<CardAction> cardActions, string targetLocale, CancellationToken cancellationToken)
        {
            foreach (var cardAction in cardActions)
            {
                cardAction.Text = await TranslateTextAsync(cardAction.Text, targetLocale, cancellationToken);
            }
        }
    }
}
