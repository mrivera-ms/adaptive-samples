using System.Collections.Generic;
using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class RootDialog : AdaptiveDialog
    {
        public RootDialog() : base(nameof(RootDialog))
        {
            Triggers = new List<OnCondition>
            {
                // Add a rule to welcome user
                new OnConversationUpdateActivity()
                {
                    Actions = WelcomeUserSteps()
                },

                new OnUnknownIntent
                {
                    Actions = GetUserCardChoice()
                },
            };
        }

        private List<Dialog> GetUserCardChoice()
        {
            return new List<Dialog>()
            {
                new ChoiceInput()
                {
                    // Output from the user is automatically set to this property
                    Property = "user.cardChoice",

                    // List of possible styles supported by choice prompt.
                    Style = Bot.Builder.Dialogs.Choices.ListStyle.Auto,
                    Prompt = new ActivityTemplate("What card would you like to see? You can click or type the card name"),
                    Choices = new ChoiceSet()
                    {
                        new Choice("Adaptive Card"),
                        new Choice("Animation Card"),
                        new Choice("Audio Card"),
                        new Choice("Hero Card"),
                        new Choice("OAuth Card"),
                        new Choice("Receipt Card"),
                        new Choice("Signin Card"),
                        new Choice("Thumbnail Card"),
                        new Choice("Video Card"),
                        new Choice("All cards")
                    }
                },
                new SendActivity("You chose ${user.cardChoice}")
                {
                    
                }
            };
        }

        private Activity CardAttachmentActivity()
        {
            return  new Activity()
            {
                AttachmentLayout = "carousel",
                //Attachments = GetCardAttachment("{user.cardChoice}")
                Attachments = GetAdaptiveCardAttachment(),
                Text = "hello ${user.cardChoice}"
            };
        }

        private IList<Attachment> GetAdaptiveCardAttachment()
        {
            return new List<Attachment>()
            {
                Cards.CreateAdaptiveCardAttachment()
            };
        }

        private IList<Attachment> GetCardAttachment(string choice)
        {
            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();

            // Reply to the activity we received with an activity.
            //var reply = MessageFactory.Attachment(attachments);
            // Decide which type of card(s) we are going to show the user
            switch (choice)
            {
                case "Adaptive Card":
                    // Display an Adaptive Card
                    attachments.Add(Cards.CreateAdaptiveCardAttachment());
                    break;
                case "Animation Card":
                    // Display an AnimationCard.
                    attachments.Add(Cards.GetAnimationCard().ToAttachment());
                    break;
                case "Audio Card":
                    // Display an AudioCard
                    attachments.Add(Cards.GetAudioCard().ToAttachment());
                    break;
                case "Hero Card":
                    // Display a HeroCard.
                    attachments.Add(Cards.GetHeroCard().ToAttachment());
                    break;
                case "OAuth Card":
                    // Display an OAuthCard
                    attachments.Add(Cards.GetOAuthCard().ToAttachment());
                    break;
                case "Receipt Card":
                    // Display a ReceiptCard.
                    attachments.Add(Cards.GetReceiptCard().ToAttachment());
                    break;
                case "Signin Card":
                    // Display a SignInCard.
                    attachments.Add(Cards.GetSigninCard().ToAttachment());
                    break;
                case "Thumbnail Card":
                    // Display a ThumbnailCard.
                    attachments.Add(Cards.GetThumbnailCard().ToAttachment());
                    break;
                case "Video Card":
                    // Display a VideoCard
                    attachments.Add(Cards.GetVideoCard().ToAttachment());
                    break;
                default:
                    // Display a carousel of all the rich card types.
                    //attachmentLayout = AttachmentLayoutTypes.Carousel;
                    attachments.Add(Cards.CreateAdaptiveCardAttachment());
                    attachments.Add(Cards.GetAnimationCard().ToAttachment());
                    attachments.Add(Cards.GetAudioCard().ToAttachment());
                    attachments.Add(Cards.GetHeroCard().ToAttachment());
                    attachments.Add(Cards.GetOAuthCard().ToAttachment());
                    attachments.Add(Cards.GetReceiptCard().ToAttachment());
                    attachments.Add(Cards.GetSigninCard().ToAttachment());
                    attachments.Add(Cards.GetThumbnailCard().ToAttachment());
                    attachments.Add(Cards.GetVideoCard().ToAttachment());
                    break;
            }

            return attachments;
        }

        private static List<Dialog> WelcomeUserSteps()
        {
            return new List<Dialog>()
            {
                // Iterate through membersAdded list and greet user added to the conversation.
                new Foreach()
                {
                    ItemsProperty = "turn.activity.membersAdded",
                    Actions = new List<Dialog>()
                    {
                        new IfCondition()
                        {
                            Condition = "$foreach.value.name != turn.activity.recipient.name",
                            Actions = new List<Dialog>()
                            {
                                new SendActivity("Welcome to CardBot."
                                                 + " This bot will show you different types of Rich Cards."
                                                 + " Please type anything to get started.")
                            }
                        }
                    }
                }
            };
        }
        private ChoiceSet GetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Adaptive Card", Synonyms = new List<string>() { "adaptive" } },
                new Choice() { Value = "Animation Card", Synonyms = new List<string>() { "animation" } },
                new Choice() { Value = "Audio Card", Synonyms = new List<string>() { "audio" } },
                new Choice() { Value = "Hero Card", Synonyms = new List<string>() { "hero" } },
                new Choice() { Value = "OAuth Card", Synonyms = new List<string>() { "oauth" } },
                new Choice() { Value = "Receipt Card", Synonyms = new List<string>() { "receipt" } },
                new Choice() { Value = "Signin Card", Synonyms = new List<string>() { "signin" } },
                new Choice() { Value = "Thumbnail Card", Synonyms = new List<string>() { "thumbnail", "thumb" } },
                new Choice() { Value = "Video Card", Synonyms = new List<string>() { "video" } },
                new Choice() { Value = "All cards", Synonyms = new List<string>() { "all" } },
            };

            return new ChoiceSet(cardOptions);
        }
    }
}
