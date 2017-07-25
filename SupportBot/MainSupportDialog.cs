using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using SupportBot.Helper;
using SupportBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SupportBot
{
    [LuisModel("b1a25b28-60c0-48a2-bd0b-206e6c143b12", "3a1a3ce7dd5b48549c1f64f7e599854a")]
    [Serializable]
    public class MainSupportDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            if(!isReply(context, result))
            {
                await context.PostAsync("Sorry, did not understand.");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("Approve")]
        public async Task Approve(IDialogContext context, LuisResult result)
        {
            isReply(context, result);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Deny")]
        public async Task Deny(IDialogContext context, LuisResult result)
        {
            if (!isReply(context, result))
            {
                await context.PostAsync("Okay.");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greet")]
        public async Task Greet(IDialogContext context, LuisResult result)
        {
            if (!isReply(context, result))
            {
                Random random = new Random();
                var greetings = new List<string> { "Hello. How can I help you?", "Hi. How can I help you?" };
                string msg = greetings[random.Next(greetings.Count)];
                await context.PostAsync(msg);
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("Request")]
        public async Task Request(IDialogContext context, LuisResult result)
        {
            if (!isReply(context, result))
            {
                EntityRecommendation RequestType;
                if (result.TryFindEntity("RequestType", out RequestType))
                {
                    if (RequestType.Entity == "password" || RequestType.Entity == "access")
                    {
                        UserData.EmployeeId = "flag";
                        await context.PostAsync("I can help with that. What is your Employee ID?");
                    }
                }
                else
                {
                    await context.PostAsync("No worries. Please tell me the error that is being displayed.");
                }
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("Create")]
        public async Task Create(IDialogContext context, LuisResult result)
        {
            if (!isReply(context, result))
            {
                EntityRecommendation IssueType;
                if (result.TryFindEntity("IssueType", out IssueType))
                {
                    if (IssueType.Entity == "user" || IssueType.Entity == "account")
                    {
                        UserData.FirstName = "flag";
                        await context.PostAsync("Sure! What is the first name of the user?");
                    }
                    else
                    {
                        UserData.IssueDesc = "flag";
                        await context.PostAsync("Of course! What is the description of the " + IssueType.Entity + "?");
                    }
                }
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("GetInfo")]
        public async Task GetInfo(IDialogContext context, LuisResult result)
        {
            if (!isReply(context, result))
            {
                EntityRecommendation InfoType;
                if (result.TryFindEntity("InfoType", out InfoType))
                {
                    KnowledgeBase kb = new KnowledgeBase();
                    if (InfoType.Entity.Contains("ox"))
                    {
                        await context.PostAsync(kb.GetArticle("KB0010016"));
                    }
                    else if(InfoType.Entity.Contains("rive"))
                    {
                        await context.PostAsync(kb.GetArticle("KB0010017"));
                    }
                    else if (InfoType.Entity.Contains("pproval"))
                    {
                        Approvals approvals = new Approvals();
                        await approvals.GetPending(context);
                    }
                    else
                    {
                        await context.PostAsync("Sorry. I couldn't find any article related to your question.");
                    }
                }
                else
                {
                    await context.PostAsync("Sorry. I couldn't find any article related to your question.");
                }
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("Compare")]
        public async Task Compare(IDialogContext context, LuisResult result)
        {
            if (!isReply(context, result))
            {
                KnowledgeBase kb = new KnowledgeBase();
                await context.PostAsync(kb.GetArticle("KB0010003"));
            }
            context.Wait(MessageReceived);
        }


        [LuisIntent("GetNews")]
        public async Task GetNews(IDialogContext context, LuisResult result)
        {
            if (!isReply(context, result))
            {
                await context.PostAsync("The latest updates are as follows :");
                KnowledgeBase kb = new KnowledgeBase();
                await kb.GetNews(context);
            }
            context.Wait(MessageReceived);
        }

        private bool isReply(IDialogContext context, LuisResult result)
        {
            if (UserData.EmployeeId == "flag")
            {
                if (result.Query == "101910")
                {
                    UserData.EmployeeId = result.Query;
                    UserData.SSN = "flag";
                    context.PostAsync("Okay. What are the last 4 digits of you SSN?");
                }
                else
                {
                    context.PostAsync("Oops! " + result.Query + " is not your correct Employee ID.");
                }
                return true;
            }
            else if (UserData.SSN == "flag")
            {
                if(result.Query == "2424")
                {
                    UserData.EmployeeId = null;
                    UserData.SSN = null;
                    context.PostAsync("Here is your temporary password: 6DF5S4");
                    context.PostAsync("Once you're logged in, you can go into the settings and update.");
                }
                else
                {
                    context.PostAsync("These digits are incorrect!");
                }
                return true;
            }
            else if (UserData.IssueDesc == "flag")
            {
                UserData.IssueDesc = null;
                Incident incident = new Incident();
                context.PostAsync(incident.Create(result.Query));
                return true;
            }
            else if (UserData.FirstName == "flag")
            {
                UserData.FirstName = result.Query;
                UserData.LastName = "flag";
                context.PostAsync("Okay. What is the last name?");
                return true;
            }
            else if (UserData.LastName == "flag")
            {
                UserData.LastName = result.Query;
                User user = new User();
                context.PostAsync(user.Create());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}