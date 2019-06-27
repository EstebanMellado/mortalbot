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
                    Url = new Uri("https://cdn-prod.mortalkombat.com/static/home-roster-fighter.png"),
                    Size = AdaptiveImageSize.Medium,
                    Style = AdaptiveImageStyle.Default
                });

            container.Items.Add(
                new AdaptiveTextBlock() {
                    Text = "Bienvenido. Choose your destiny.",
                    Size = AdaptiveTextSize.Small,
                    Color = AdaptiveTextColor.Good
                });

            adaptiveCard.Body.Add(container);

            return adaptiveCard;
        }
    }
}
