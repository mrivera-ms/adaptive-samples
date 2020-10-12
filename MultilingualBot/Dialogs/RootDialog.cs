// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Builder.LanguageGeneration;

namespace WelcomeBot.Dialogs
{
    public class RootDialog : AdaptiveDialog
    {
        private Templates _templates;
        private readonly UserState _userState;
        private IStatePropertyAccessor<string> _languagePreference;

        public RootDialog() : base(nameof(RootDialog))
        {
            //_userState = userState ?? throw new NullReferenceException(nameof(userState));
            //_languagePreference = userState.CreateProperty<string>("LanguagePreference");

            string[] paths = { ".", "Dialogs", $"RootDialog.lg" };
            var fullPath = Path.Combine(paths);
            _templates = Templates.ParseFile(fullPath);

            Triggers = new List<OnCondition>
            {
                new OnConversationUpdateActivity()
                {
                    Actions = WelcomeUserSteps()
                },
                new OnUnknownIntent()
                {
                    Actions = new List<Dialog>()
                    {
                        new TextInput()
                        {
                            Property = "user.LanguagePreference",
                            Prompt = new ActivityTemplate("${LanguageChoicePrompt()}"),
                            Value = "${user.LanguagePreference}",
                            AlwaysPrompt = true
                        },
                        new SendActivity("Excellent: ${user.LanguagePreference} is a great choice."),
                        //new CodeAction(SetLanguagePreferenceToUserState),
                        new SendActivity("${SampleText()}"),
                    }
                }
            };

            Generator = new TemplateEngineLanguageGenerator(_templates);
        }

        //private async Task<DialogTurnResult> SetLanguagePreferenceToUserState(DialogContext dc, System.Object options)
        //{
        //    await _languagePreference.SetAsync(dc.Context, "${user.languagePreference}");
        //    await _userState.SaveChangesAsync(dc.Context, false);
        //    return await dc.EndDialogAsync(options);
        //}

        private static List<Dialog> WelcomeUserSteps() =>
            new List<Dialog>()
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
                                { new SendActivity("${AdaptiveCard()}") }
                            }
                        }
                    }
                }
            };

    }
}
