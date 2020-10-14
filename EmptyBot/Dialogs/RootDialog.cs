// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class RootDialog : AdaptiveDialog
    {
        public RootDialog() : base(nameof(RootDialog))
        {
            Triggers = new List<OnCondition>
            {
                new OnUnknownIntent
                {
                    Actions =
                    {
                        new SendActivity("Hi, we are up and running!!"),
                    }
                },
            };
        }
    }
}
