
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Deck : IDeck
    {
        protected List<ICard> cards = new List<ICard>(52);
        public int DeckSize { get { return cards.Count; } }
        Random rng = new Random();

       

        public Deck()
        {

            //populate deck with cards and shuffle
            int[] ranks = Array.ConvertAll((CardRank[]) Enum.GetValues(typeof(CardRank)), value => (int)value);
            int[] suits = Array.ConvertAll((CardSuit[])Enum.GetValues(typeof(CardSuit)), value => (int)value);

            for (int r=0; r < ranks.Length; r++)
            {
                CardRank rank = (CardRank)Enum.ToObject(typeof(CardRank), ranks[r]);

                for(int s = 0; s < suits.Length; s++)
                {
                    CardSuit suit = (CardSuit)Enum.ToObject(typeof(CardSuit), suits[s]);
                    cards.Add(CreateCard(rank, suit));
                    //cards.Add(new Card(rank, suit));
                }
            }
            Shuffle();
        }

        private ICard CreateCard(CardRank rank, CardSuit suit)
        {
            Card card = new Card(rank, suit);
            card.Rank = rank;
            card.Suit = suit;
            return card;
        }

        public List<ICard> PopCardsFromTop(int numberOfCardsToPop)
        {
            if (numberOfCardsToPop <= cards.Count && numberOfCardsToPop > 0)
            {
                List<ICard> popped = new List<ICard>();
                for(int i=0; i<numberOfCardsToPop; i++)
                {
                    popped.Add(cards[0]);
                    this.cards.RemoveAt(0);
                }
                return popped;
            }
            else throw new ArgumentOutOfRangeException("Number of cards to pop should be less then the size of the deck!");
        }

        public void Shuffle(int times=1)
        {
            for(int i=0; i< times; i++)
            {
                Shuffle();
            }
        }

        //Fisher–Yates shuffle
        //https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
        private void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                ICard value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }
    }
}
