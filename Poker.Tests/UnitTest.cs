using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using Moq;

namespace Poker.Tests
{
    [TestClass]
    public class UnitTest
    {
        
        public UnitTest()
        {
            
        }

        [TestMethod]
        public void TestCard_GetCardValue()
        {
            //Arrange
            List<ICard> cards = new List<ICard>();

            //populate with cards
            int[] ranks = Array.ConvertAll((CardRank[])Enum.GetValues(typeof(CardRank)), value => (int)value);
            int[] suits = Array.ConvertAll((CardSuit[])Enum.GetValues(typeof(CardSuit)), value => (int)value);

            for (int r = 0; r < ranks.Length; r++)
            {
                CardRank rank = (CardRank)Enum.ToObject(typeof(CardRank), ranks[r]);

                for (int s = 0; s < suits.Length; s++)
                {
                    CardSuit suit = (CardSuit)Enum.ToObject(typeof(CardSuit), suits[s]);
                    cards.Add(new Card(rank, suit));
                }
            }

            //Act & Assert
            //Cards are generated in ascending order with respect to ranks and suits,
            //so, each next card in deck is higher than the previous one
            for(int i=1; i<cards.Count; i++)
            {
                Assert.IsTrue(cards[i].GetCardValue() > cards[i - 1].GetCardValue());
            }
        }

        [TestMethod]
        public void TestCard_GetHandRankValueStraightFlush()
        {
            //Arrange

            //Straight Flush (2 cards of sequential rank, same suit)
            
            ICard c1 = new Card(CardRank.Four, CardSuit.Clubs);
            ICard c2 = new Card(CardRank.Five, CardSuit.Clubs);

            //Act
            int hrank = Card.GetHandRankValue(c1, c2);

            //Assert
            Assert.AreEqual(HandRank.StraightFlush, Enum.ToObject(typeof(HandRank), hrank));
        }

        [TestMethod]
        public void TestCard_GetHandRankValueFlush()
        {
            //Arrange

            //Flush (2 cards, same suit)
            ICard c1 = new Card(CardRank.A, CardSuit.Clubs);
            ICard c2 = new Card(CardRank.Five, CardSuit.Clubs);

            //Act
            int hrank = Card.GetHandRankValue(c1, c2);

            //Assert
            Assert.AreEqual(HandRank.Flush, Enum.ToObject(typeof(HandRank), hrank));
        }

        [TestMethod]
        public void TestCard_GetHandRankValueStraight()
        {
            //Arrange

            //Straight(2 cards of sequential rank, different suit)
            ICard c1 = new Card(CardRank.Four, CardSuit.Spades);
            ICard c2 = new Card(CardRank.Five, CardSuit.Clubs);
            ICard c3 = new Card(CardRank.Four, CardSuit.Clubs);

            //Act
            int hrank1 = Card.GetHandRankValue(c1, c2);
            int hrank2 = Card.GetHandRankValue(c2, c3);

            //Assert
            Assert.AreEqual(HandRank.Straight, Enum.ToObject(typeof(HandRank), hrank1));
            Assert.AreNotEqual(HandRank.Pair, Enum.ToObject(typeof(HandRank), hrank2)); //StraightFlush
        }

        [TestMethod]
        public void TestCard_GetHandRankValuePair()
        {
            //Arrange

            //1 pair (2 cards of same rank)
            ICard c1 = new Card(CardRank.K, CardSuit.Spades);
            ICard c2 = new Card(CardRank.K, CardSuit.Clubs);
            ICard c3 = new Card(CardRank.K, CardSuit.Spades);

            //Act
            int hrank1 = Card.GetHandRankValue(c1, c2);
            int hrank2 = Card.GetHandRankValue(c1, c3);

            //Assert
            Assert.AreEqual(HandRank.Pair, Enum.ToObject(typeof(HandRank), hrank1));
            Assert.AreNotEqual(HandRank.Pair, Enum.ToObject(typeof(HandRank), hrank2)); //Flush
        }

        [TestMethod]
        public void TestCard_GetHandRankValueHighCard()
        {
            //Arrange

            //1 pair (2 cards of same rank)
            ICard c1 = new Card(CardRank.K, CardSuit.Spades);
            ICard c2 = new Card(CardRank.Four, CardSuit.Clubs);
            ICard c3 = new Card(CardRank.Two, CardSuit.Spades);

            //Act
            int hrank1 = Card.GetHandRankValue(c1, c2);
            int hrank2 = Card.GetHandRankValue(c1, c3);

            //Assert
            Assert.AreEqual(c1.GetCardValue(), hrank1); //c1 is the highest between c1 and c2
            Assert.AreNotEqual(HandRank.Pair, Enum.ToObject(typeof(HandRank), hrank2)); //Flush
        }

        [TestMethod]
        public void TestDeck_Create()
        {
            //Arrange & Act
            IDeck deck = new Deck();

            //Assert
            Assert.AreEqual(52, deck.DeckSize);
        }
        

        [TestMethod]
        public void TestDeck_PopCardsFromTop_Exception()
        {
            //Arrange
            IDeck deck = new Deck();

            //Arrange & Act
            bool throws1 = false;
            try
            {
                //Act
                deck.PopCardsFromTop(numberOfCardsToPop: 53);
            }
            catch (ArgumentOutOfRangeException)
            {
                throws1 = true;
            }


            bool throws2 = false;
            try
            {
                deck.PopCardsFromTop(numberOfCardsToPop: 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                throws2 = true;
            }

            Assert.IsTrue(throws1);
            Assert.IsTrue(throws2);
        }

       
        
        [TestMethod]
        public void TestPlayer_GetCurrentHandRank()
        {
            //Arrange
            IPlayer player = new Player(0);
            Mock<ICard> mock = new Mock<ICard>();
            mock.Setup(c => c.Rank).Returns(CardRank.Ten);
            mock.Setup(c => c.Suit).Returns(CardSuit.Diamonds);
            mock.Setup(c => c.GetCardValue()).Returns((int)CardRank.Ten * 10 + (int)CardSuit.Diamonds);

            //Act
            player.ReceiveTwoCards(new List<ICard>(new ICard[] { mock.Object, mock.Object })); //Flush -> same suit
            int handRank = player.GetCurrentHandRank();

            //Assert
            Assert.AreEqual((int)HandRank.Flush, handRank);
        }

        [TestMethod]
        public void TestPlayer_GetCurrentHandRank_Exception()
        {
            //Arrange
            IPlayer player = new Player(0);

            //Act
            try {
                player.GetCurrentHandRank();
            }
            catch(InvalidOperationException)
            {
                //OK - cannot get the rank, when no cards were given to player
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Test_GameEngine_Initialize()
        {
            //Arrange & Act
            GameEngine gameEngine = new GameEngine(roundsToPlay: 3, playersCount: 5);

            //Assert
            Assert.AreEqual(5, gameEngine.PlayersCount);
            Assert.AreEqual(3, gameEngine.RoundsToPlay);
            Assert.AreEqual(0, gameEngine.RoundsPlayed);
        }

        [TestMethod]
        public void Test_GameEngine_Initialize_Exception1()
        {
            //Arrange & Act - OUT OF RANGE ROUNDS
            bool throws1 = false;
            try {
                GameEngine gameEngine = new GameEngine(roundsToPlay: 1, playersCount: 5);
            }
            catch(ArgumentOutOfRangeException)
            {
                throws1 = true;
            }

            bool throws2 = false;
            try
            {
                GameEngine gameEngine = new GameEngine(roundsToPlay: 6, playersCount: 5);
            }
            catch (ArgumentOutOfRangeException)
            {
                throws2 = true;
            }

            Assert.IsTrue(throws1);
            Assert.IsTrue(throws2);
        }

        [TestMethod]
        public void Test_GameEngine_Initialize_Exception2()
        {
            //Arrange & Act - OUT OF RANGE PLAYERS
            bool throws1 = false;
            try
            {
                GameEngine gameEngine = new GameEngine(roundsToPlay: 2, playersCount: 7);
            }
            catch (ArgumentOutOfRangeException)
            {
                throws1 = true;
            }


            bool throws2 = false;
            try
            {
                GameEngine gameEngine = new GameEngine(roundsToPlay: 2, playersCount: 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                throws2 = true;
            }

            Assert.IsTrue(throws1);
            Assert.IsTrue(throws2);
        }

        [TestMethod]
        public void Test_GameEngine_PlayNewRound_Shuffle_Incorrect()
        {
            bool throws = false;
            try
            {
                GameEngine gameEngine = new GameEngine(roundsToPlay: 2, playersCount: 5);
                gameEngine.PlayNewRound(deckShuffleTimes: 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                throws = true;
            }
            Assert.IsTrue(throws);
        }

        [TestMethod]
        public void Test_GameEngine_PlayNewRound()
        {
            GameEngine gameEngine = new GameEngine(roundsToPlay: 2, playersCount: 5);
            gameEngine.PlayNewRound(deckShuffleTimes:2);
            Assert.AreEqual(1, gameEngine.RoundsPlayed);

            gameEngine.PlayNewRound(deckShuffleTimes: 5);
            Assert.AreEqual(2, gameEngine.RoundsPlayed);

            try
            {
                gameEngine.PlayNewRound();
            }
            catch(InvalidOperationException)
            {
                //can't play more rounds than were initialized
                return;
            }

            Assert.Fail(); //if exception was not catched
        }

        [TestMethod]
        public void Test_GameEngine_GetTheWinner()
        {
            //Arrange
            GameEngine gameEngine = new GameEngine(roundsToPlay: 2, playersCount: 5);
            gameEngine.PlayNewRound(deckShuffleTimes: 2);

            //Act
            //get the current winner
            IList<IPlayer> players = gameEngine.GetPlayersReadOnly();
            int maxScore = players[0].OverallScore;

            for(int i=1; i<players.Count; i++)
            {
                if (maxScore < players[i].OverallScore)
                    maxScore = players[i].OverallScore;
            }

            //Assert
            Assert.AreEqual(maxScore, gameEngine.GetTheWinner().OverallScore);
        }

        [TestMethod]
        public void Test_GameEngine_ResetGame()
        {
            //Arrange
            GameEngine gameEngine = new GameEngine(roundsToPlay: 2, playersCount: 5);
            gameEngine.PlayNewRound(deckShuffleTimes: 2);

            //Act
            gameEngine.ResetGame(); //all counters should reset to default ones

            //Assert
            Assert.AreEqual(5, gameEngine.PlayersCount);
            Assert.AreEqual(2, gameEngine.RoundsToPlay);
            Assert.AreEqual(0, gameEngine.RoundsPlayed);
            foreach(IPlayer p in gameEngine.GetPlayersReadOnly())
            {
                Assert.AreEqual(0, p.LastRoundScore);
                Assert.AreEqual(0, p.OverallScore);
            }
        }
    }
}
