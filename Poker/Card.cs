using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    /// <summary>
    /// Represents suit of a card. Constants assigned represent the suit rank.
    /// </summary>
    public enum CardSuit
    {
        Spades =    4,
        Clubs =     3,
        Hearts =    2,
        Diamonds =  1
    }

    /// <summary>
    /// Represents the card rank. Constants assigned represent the rank.
    /// </summary>
    public enum CardRank
    {
        Two =   2,
        Three = 3,
        Four =  4,
        Five =  5,
        Six =   6,
        Seven = 7,
        Eight = 8,
        Nine =  9,
        Ten =   10,
        J =     11,
        Q =     12,
        K =     13,
        A =     14
    }

    //arbitrary high constants, i.e. the lowest constant should be greater then the highest card value
    //according to Card.GetCardValue() method; here the highest card is A (Spades) = 14*10+4 = 144
    public enum HandRank 
    {
        StraightFlush = 500,
        Flush =         400,
        Straight =      300,
        Pair =          200
    }

    /// <summary>
    /// Represents the card with rank and suite
    /// </summary>
    public class Card : ICard
    {
        public CardRank Rank { get; set; }
        public CardSuit Suit { get; set; }

        public Card(CardRank rank, CardSuit suit)
        {
            this.Rank = rank;
            this.Suit = suit;
        }

        /// <summary>
        /// Calculates the card value which is unique for each card. The value
        /// allows to compare the cards between each other according to ranking rules.
        /// </summary>
        /// <returns>The card value</returns>
        public int GetCardValue()
        {
            return (int)Rank * 10 + (int)Suit;
        }

        /// <summary>
        /// Gets the value of the card combination. The value can be one of the predefined
        /// in HandRank enumeration or other value, which corresponds to 'High Card' hand rank.
        /// Any predefined HandRank constant from enumeration is greater then the value
        /// returned for 'High Card' hand rank.
        /// </summary>
        /// <param name="c1">First card</param>
        /// <param name="c2">Second card</param>
        /// <returns>Rank of the two-card combination</returns>
        public static int GetHandRankValue(ICard c1, ICard c2)
        {
            bool sequentialRank = (Math.Abs(c1.Rank - c2.Rank) == 1);
            bool sameSuit = (c1.Suit == c2.Suit);


            if (sequentialRank && sameSuit) //Straight Flush (2 cards of sequential rank, same suit)
            {
                return (int)HandRank.StraightFlush;
            }
            else if (sameSuit) //Flush (2 cards, same suit)
            {
                return (int)HandRank.Flush;
            }
            else if (!sameSuit && sequentialRank) //Straight (2 cards of sequential rank, different suit)
            {
                return (int)HandRank.Straight;
            }
            else if(c1.Rank==c2.Rank) //1 pair (2 cards of same rank)
            {
                return (int)HandRank.Pair;
            }
            else //High Card (2 cards, different rank, suit and not in sequence. Highest card wins)
            {
                int c1Value = c1.GetCardValue();
                int c2Value = c2.GetCardValue();

                if (c1Value > c2Value)
                    return c1Value;
                else return c2Value;
            }
        }

        public override string ToString()
        {
            string rank = string.Empty;
            if ((int)Rank <= 10)
            {
                rank = ((int)Rank).ToString();
            }
            else rank = Rank.ToString();

            string suit = string.Empty;

            switch (Suit)
            {
                case CardSuit.Clubs:
                    suit = "♣";
                    break;
                case CardSuit.Diamonds:
                    suit = "♦";
                    break;
                case CardSuit.Hearts:
                    suit = "♥";
                    break;
                case CardSuit.Spades:
                    suit = "♠";
                    break;
            }

            return $"{rank}{suit}";
        }
    }
}
