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
                    Url = new Uri("http://mouse.latercera.com/wp-content/uploads/2018/11/mortal-kombat-11.jpg"),
                    Size = AdaptiveImageSize.Stretch,
                    Style = AdaptiveImageStyle.Default
                });

            container.Items.Add(
                new AdaptiveTextBlock() {
                    Text = "Bienvenido. Choose your destiny.",
                    Size = AdaptiveTextSize.Large,
                    Color = AdaptiveTextColor.Good
                });

            adaptiveCard.Body.Add(container);

            return adaptiveCard;
        }
    }
}
