// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        protected readonly IConfiguration Configuration;
        protected readonly ILogger Logger;
        private LUISService serviceLuis;
        public MainDialog(IConfiguration configuration, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            Configuration = configuration;
            Logger = logger;
               
            serviceLuis = new LUISService(configuration);

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        //TODO: LEE PALABRA, ANALIZA Y DEVUELVE LO QUE HAY EN LA DB
        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //TODO: UTILIZA EL SERVICIO PARA CONSUMIR SPELL CHECK Y AZURE SEARCH

            var mortalService = new MortalBotService(Configuration);

            var resultado = mortalService.ValidateSpellCheck(stepContext.Context.Activity.Text); //AZURE SPELLCHECK

            var resultado2 = mortalService.SearchByText(resultado); //AZURE SEARCH


            var response = await serviceLuis.GetIntentionsByText(resultado); //LUIS
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);

            JObject intents = JObject.Parse(jsonResponse);

            string intent = intents.First.Next.Next.First.First.First.ToString();
            var score = Convert.ToDouble(intents.First.Next.Next.First.First.Next.First.ToString());

            if (intent == "saludo" && score > 0.9)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("Hola!!!"), cancellationToken);
            }else if(intent == "nombre" && score > 0.9)
            {
                await stepContext.Context.SendActivityAsync(
                 MessageFactory.Text("Mi nombre es Mortal Bot!"), cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text(resultado2[0].Descripcion), cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);








            //if (string.IsNullOrEmpty(Configuration["LuisAppId"]) || string.IsNullOrEmpty(Configuration["LuisAPIKey"]) || string.IsNullOrEmpty(Configuration["LuisAPIHostName"]))
            //{
            //    var mortalService = new MortalBotService(Configuration);

            //    var resultado = mortalService.ValidateSpellCheck(stepContext.Context.Activity.Text);

            //    var resultado2 = mortalService.SearchByText(resultado);




            //    await stepContext.Context.SendActivityAsync(
            //        MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file."), cancellationToken);

            //    return await stepContext.NextAsync(null, cancellationToken);
            //}
            //else
            //{
            //    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("What can I help you with today?\nSay something like \"Book a flight from Paris to Berlin on March 22, 2020\"") }, cancellationToken);
            //}
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Call LUIS and gather any potential booking details. (Note the TurnContext has the response to the prompt.)
            var bookingDetails = stepContext.Result != null
                    ?
                await LuisHelper.ExecuteLuisQuery(Configuration, Logger, stepContext.Context, cancellationToken)
                    :
                new BookingDetails();

            // In this sample we only have a single Intent we are concerned with. However, typically a scenario
            // will have multiple different Intents each corresponding to starting a different child Dialog.

            // Run the BookingDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
            return await stepContext.BeginDialogAsync(nameof(BookingDialog), bookingDetails, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled or the user failed to confirm, the Result here will be null.
            if (stepContext.Result != null)
            {
                var result = (BookingDetails)stepContext.Result;

                // Now we have all the booking details call the booking service.

                // If the call to the booking service was successful tell the user.

                var timeProperty = new TimexProperty(result.TravelDate);
                var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                var msg = $"I have you booked to {result.Destination} from {result.Origin} on {travelDateMsg}";
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Thank you."), cancellationToken);
            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
