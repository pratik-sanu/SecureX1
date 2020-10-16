// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.3

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.Bot.Builder.AI.QnA;
using System.Net.Http;
using System.IO;
using System;

namespace EchoBot3.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //await turnContext.SendActivityAsync(MessageFactory.Text($"{turnContext.Activity.Text}"), cancellationToken);
            await AccessQnAMaker(turnContext, cancellationToken);
        }
        public QnAMaker EchoBotQnA { get; private set; }
        public EchoBot(QnAMakerEndpoint endpoint)
        {
            // connects to QnA Maker endpoint for each turn
            EchoBotQnA = new QnAMaker(endpoint);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("text", "This is a block of text"),});

            // Get the response.
            HttpResponseMessage response = await client.GetAsync(
                "http://otpserver-868380640.us-east-1.elb.amazonaws.com");

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.


            var welcomeText = "Hello I am SecureX";
            //string d = responseContent.ReadAsStringAsync().Result;
            //Console.WriteLine("Enter OTK");
            //string OTK = Console.ReadLine();
            //Console.WriteLine("OTK: " + OTK);
            //if (d != OTK)
            //{
            //    Console.WriteLine("Wrong OTK");
            //}
            //else
            //{
            //    Console.WriteLine("Done");
            //}

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
        private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            if (results.Any())
            {
                Console.WriteLine("abc "+ MessageFactory.Text(results.First().Answer));
                await turnContext.SendActivityAsync(MessageFactory.Text(results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
            }
        }
    }

}