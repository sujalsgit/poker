using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Player : IPlayer
    {
        public int ID { get; private set; } //each player has ID for clarity
        protected List<ICard> Cards { get; set; } = new List<ICard>(2); //protected for testing
        public int OverallScore { get; set; } = 0;  //the score player has gained during the whole game
        public int LastRoundScore { get; set; } = 0;    //the score player has gained during last round
        
        /// <summary>
        /// Constructor for player with unique ID
        /// </summary>
        /// <param name="id">Unique ID to identify the player in the future. Uniqueness is guaranteed by the user.</param>
        public Player(int id)
        {
            this.ID = id;
        }

        /// <summary>
        /// Assigns two cards to player
        /// </summary>
        /// <param name="cards">List which contains two cards from the deck</param>
        public void ReceiveTwoCards(List<ICard> cards)
        {
            if (ID < 0)
                throw new InvalidOperationException("Player is not initialized!");

            this.Cards.Clear();
            this.Cards.AddRange(cards);
        }

        /// <summary>
        /// Returns rank of the current card combination
        /// </summary>
        /// <returns>The hand rank of the current card combination</returns>
        public int GetCurrentHandRank()
        {
            if (Cards.Count < 2)
                throw new InvalidOperationException("Hand ranking requires 2 cards!");

            return Card.GetHandRankValue(this.Cards[0], this.Cards[1]);
        }

        public override string ToString()
        {
            return $"Player {ID} -> Cards: {string.Join(" ", Cards)}\tRound Score: {LastRoundScore}\tHand rank: {GetCurrentHandRank()}";
        }
    }
}
