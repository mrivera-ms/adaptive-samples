// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class DialogBot<T> : ActivityHandler
        where T : Dialog
    {
        private readonly DialogManager DialogManager;
        protected readonly ILogger Logger;

        public DialogBot(ConversationState conversationState, UserState userState, T rootDialog, ILogger<DialogBot<T>> logger)
        {
            Logger = logger;
            DialogManager = new DialogManager(rootDialog);
            DialogManager.ConversationState = conversationState;
            DialogManager.UserState = userState;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Running dialog with Activity.");
            await DialogManager.OnTurnAsync(turnContext, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}

