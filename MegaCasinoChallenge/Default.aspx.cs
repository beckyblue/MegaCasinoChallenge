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
        //Keeps the images at random
        Random random = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Show Reel values
            if (!Page.IsPostBack)
            {
                string[] reels = new string[] { spinReel(), spinReel(), spinReel(), };
                displayImages(reels);
                ViewState.Add("PlayersMoney", 100);//initialize player $$
                displayPlayersMoney();
            }
        }


        protected void pullButton_Click(object sender, EventArgs e)
        {
            //Bet $$, must have a bet to run task
            int bet = 0;//conditional value for bet set at 0
            if (!int.TryParse(betTextBox.Text, out bet)) return;

            int winnings = pullLever(bet);
            displayResult(bet, winnings);
            adjustPlayersMoney(bet, winnings);
            displayPlayersMoney();
        }

        private void adjustPlayersMoney(int bet, int winnings)
        {
            int playersMoney = int.Parse(ViewState["PlayersMoney"].ToString());
            playersMoney -= bet;
            playersMoney += winnings;
            ViewState["PlayersMoney"] = playersMoney;
        }


        //Action of pulling lever with known bet amount (bet) int will return winning value 
        private int pullLever(int bet)
        {
            string[] reels = new string[] { spinReel(), spinReel(), spinReel() };
            displayImages(reels);

            //amount won/lost
            int multiplier = evaluateSpin(reels);
            return bet * multiplier;
        }

        //Reel variable amount return
        private int evaluateSpin(string[] reels)
        {
            if (isBar(reels)) return 0;//Just ONE "BAR" + 0
            if (isJackpot(reels)) return 100;//THREE "7" + 100
            // =<1 "Cherries"
            int multiplier = 0;
            if (isWinner(reels, out multiplier)) return multiplier;

            return 0;
        }

       
        //BAR Check Spin if true, and if not shows false return
        private bool isBar(string[] reels)
        {
            if (reels[0] == "Bar" || reels[1] == "Bar" || reels[2] == "Bar")
                return true;
            else
                return false;
        }

        //JACKPOT Check Spin to see if all reels = "7" 
        private bool isJackpot(string[] reels)
        {
            if (reels[0] == "Seven" && reels[1] == "Seven" && reels[2] == "Seven")
                return true;
            else
                return false;
        }

        //CHERRIES Check Spin (int due to # of reels)
        private int determineCherryCount(string[] reels)
        {
            int cherryCount = 0;
            if (reels[0] == "Cherry") cherryCount++;
            if (reels[1] == "Cherry") cherryCount++;
            if (reels[2] == "Cherry") cherryCount++;
            return cherryCount;
        }

        //CHERRIES figuring $$ multiplier
        private int determineMultiplier(string[] reels)
        {
            int cherryCount = determineCherryCount(reels);
            if (cherryCount == 1) return 2;
            if (cherryCount == 2) return 3;
            if (cherryCount == 3) return 4;
            return 0;
        }

        //WIN/LOSE
        private bool isWinner(string[] reels, out int multiplier)
        {
            multiplier = determineMultiplier(reels);
            if (multiplier > 0) return true;//winner
            return false;//looser
        }


        // Three images tied to reels and choosing images at random
        private void displayImages(string[] reels)
        {
            Image1.ImageUrl = "/Images/" + reels[0] + ".png";
            Image2.ImageUrl = "/Images/" + reels[1] + ".png";
            Image3.ImageUrl = "/Images/" + reels[2] + ".png";
        }

        // images to pull from set at random "spin"
        private string spinReel()
        {
            string[] images = new string[] { "Bar", "Bell", "Cherry", "Clover", "Diamond", "HorseShoe", "Lemon", "Orange", "Plum", "Seven", "Strawberry", "Watermelon" };
            return images[random.Next(11)];
        }

        //Display player actual winnings
        private void displayResult(int bet, int winnings)
        {
            if (winnings > 0)
                resultLabel.Text = String.Format("You bet {0:C} and won {1:C}!", bet, winnings);
            else
                resultLabel.Text = String.Format("Sorry, you lost {0:C}.  Better luck next time.", bet);
        }

        //Display players total money
        private void displayPlayersMoney()
        {
            moneyLabel.Text = String.Format("Player's Money: {0:C}", ViewState["PlayersMoney"]);
        }
    }

}