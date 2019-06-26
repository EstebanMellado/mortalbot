using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot.Cards
{
    public class WelcomeCard
    {
        public WelcomeCard() { }

        public AdaptiveCard GetWelcomeAdaptiveCard()
        {
            AdaptiveCard adaptiveCard = new AdaptiveCard();

            AdaptiveContainer container = new AdaptiveContainer();

            container.Items.Add(
                new AdaptiveImage() {
                    Url = new Uri("https://pbs.twimg.com/profile_images/1455756245/150264_458152750702_603795702_6151893_2736358_n_400x400.jpg"),
                    Size = AdaptiveImageSize.Medium,
                    Style = AdaptiveImageStyle.Default
                });

            container.Items.Add(
                new AdaptiveTextBlock() {
                    Text = "Este es el pie de foto",
                    Size = AdaptiveTextSize.Small,
                    Color = AdaptiveTextColor.Good
                });

            adaptiveCard.Body.Add(container);

            return adaptiveCard;
        }
    }
}
