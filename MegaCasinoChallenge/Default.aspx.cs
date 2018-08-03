using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MegaCasinoChallenge
{
    public partial class Default : System.Web.UI.Page
    {
        Random random = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                string[] reels = new string[] { spinReel(), spinReel(), spinReel() };
                randomImage(reels);
                ViewState.Add("PlayersMoney", 100);
                playerMoney();
            }
        }

        protected void leverButton_Click(object sender, EventArgs e)
        {
            double betAmount = 0;
            if (!betExist()) return;
            if (!double.TryParse(betTextBox.Text.Trim(), out betAmount)) return;
            double winnings = activateLever(betAmount);

            results(betAmount, winnings);
            remainingMoney(betAmount, winnings);
            playerMoney();

        }

        private double activateLever(double betAmount)
        {
            string[] reels = new string[] { spinReel(), spinReel(), spinReel() };
            randomImage(reels);
            double multiplier = math(reels);
            return betAmount * multiplier;
        }

        private void remainingMoney(double betAmount, double winnings)
        {
            double playersMoney = int.Parse(ViewState["PlayersMoney"].ToString());
            playersMoney -= betAmount;
            playersMoney += winnings;
            ViewState["PlayersMoney"] = playersMoney;
        }

        private void playerMoney()
        {
            moneyLabel.Text = String.Format("Player's Money: {0:C}", ViewState["PlayersMoney"]);
        }

        private void results(double betAmount, double winnings)
        {
            if (winnings > 0)
            {
                resultLabel.Text = String.Format("You bet {0:C} and won {1:C}!", betAmount, winnings);
            }
            else
            {
                resultLabel.Text = String.Format("Sorry, you lost {0:C}. Better luck next time.", betAmount);
            }
        }

        private double math(string[] reels)
        {
            if (Bars(reels)) return 0;
            double multiplier = 0;
            if (Winner(reels, out multiplier)) return multiplier;
            if (Jackpot(reels)) return 100;
            return 0;
        }

        private bool Winner(string[] reels, out double multiplier)
        {
            multiplier = cherryMultiplier(reels);
            if (multiplier > 0)
                return true;
            else return false;
        }

        private double cherryMultiplier(string[] reels)
        {
            double cherryCount = findCherryCount(reels);
            if (cherryCount == 1) return 2;
            if (cherryCount == 2) return 3;
            if (cherryCount == 3) return 4;
            return 0;
           
        }
        private double findCherryCount(string[] reels)
        {
            int cherryCount = 0;
            if (reels[0] == "Cherry") cherryCount++;
            if (reels[1] == "Cherry") cherryCount++;
            if (reels[2] == "Cherry") cherryCount++;
            return cherryCount;
        }

        private bool Jackpot(string[] reels)
        {
            if (reels[0] == "Seven" && reels[1] == "Seven" && reels[2] == "Seven")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Bars(string[] reels)
        {
            if (reels[0] == "Bar" || reels[1] == "Bar" || reels[2] == "Bar") 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool betExist()
        {
            if (betTextBox.Text.Trim().Length == 0)
                return false;
            return true;
        }

        private void randomImage(string[] reels)
        {
            Image1.ImageUrl = "/Images/" + reels[0] + ".png";
            Image2.ImageUrl = "/Images/" + reels[1] + ".png";
            Image3.ImageUrl = "/Images/" + reels[2] + ".png";
        }

        private string spinReel()
        {
            string[] images = new string[] { "Strawberry", "Bar", "Lemon", "Bell", "Clover", "Cherry", "Diamond", "Orange", "Seven", "HorseShoe", "Plum", "Watermelon" };
            return images[random.Next(11)];
        }
    }
}