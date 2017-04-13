using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    /// <summary>
    /// Represents the main class of the poker game.
    /// The player/round validation resides here
    /// </summary>
    public class GameEngine
    {
        //list of players who participate in the current game;
        List<IPlayer> Players { get; set; } = new List<IPlayer>();    //protected for testing
        IDeck Deck { get; set; }
        public int RoundsPlayed { get; private set; } = 0;
        public int RoundsToPlay { get; private set; }   //is set only in constructor
        public int PlayersCount { get; private set; }   //is set only in constructor

       

        /// <summary>
        /// Creates game engine.
        /// </summary>
        /// <param name="roundsToPlay">The total amount of rounds to play (2-5)</param>
        /// <param name="playersCount">The total amount of players (2-6)</param>
        /// <param name="kernel">The Ninject initialized kernel</param>
        public GameEngine(int roundsToPlay, int playersCount)
        {
            if (roundsToPlay < 2 || roundsToPlay > 5)
                throw new ArgumentOutOfRangeException("Only 2-5 rounds allowed!");

            if (playersCount < 2 || playersCount > 6)
                throw new ArgumentOutOfRangeException("Only 2-6 rounds allowed!");

            this.RoundsToPlay = roundsToPlay;
            this.PlayersCount = playersCount;
            InitPlayers();
        }

        /// <summary>
        /// Creates a player using the Ninject kernel
        /// </summary>
        /// <param name="id">Unique ID which is assigned to player</param>
        /// <returns>New player</returns>
        private IPlayer CreatePlayer(int id)
        {
            IPlayer player = new Player(id);
            return player;
        }

        /// <summary>
        /// Initializes all players when game is created or reset
        /// </summary>
        private void InitPlayers()
        {
            this.Players = new List<IPlayer>();
            for(int i=0; i<this.PlayersCount; i++)
            {
                this.Players.Add(CreatePlayer(id:i));
            }
        }

        /// <summary>
        /// Simulates one round
        /// </summary>
        /// <param name="deckShuffleTimes">The number of times to shuffle the deck before the round</param>
        public void PlayNewRound(int deckShuffleTimes=1)
        {
            if (RoundsPlayed < RoundsToPlay)
            {
                //The dealer shuffles the deck at the start of each round.

                if (deckShuffleTimes < 1)
                    throw new ArgumentOutOfRangeException("Deck shuffle times should be greater than 0!");

                this.Deck = new Deck(); //is guaranteed to be 52 cards when created
                this.Deck.Shuffle(deckShuffleTimes);
                
 
                //The dealer deals 2 cards to each player.
                foreach(IPlayer p in this.Players)
                {
                    p.ReceiveTwoCards(this.Deck.PopCardsFromTop(2));
                }

                //The dealer ranks each player’s hand according the poker hand ranking rules - is done implicitly when assigning score
                //At the end of each round, each player is assigned a score (0 – weakest to strongest x-1 (where x = number of players)).
                this.Players.Sort((p1, p2) => p1.GetCurrentHandRank().CompareTo(p2.GetCurrentHandRank())); //ASCENDING !!!
                for(int i=0; i<this.Players.Count; i++)
                {
                    this.Players[i].OverallScore += i;  //player with the lowest hand rank is assigned weaker score
                    this.Players[i].LastRoundScore = i;
                }

                RoundsPlayed++;
            }
            else throw new InvalidOperationException("No more rounds left to play! Please, reset the game engine.");
        }

        /// <summary>
        /// Gets the readonly list of players
        /// </summary>
        /// <returns>Readonly list of players</returns>
        public IList<IPlayer> GetPlayersReadOnly()
        {
            return this.Players.AsReadOnly();
        }

        /// <summary>
        /// Gets the winner of the game
        /// </summary>
        /// <returns>Player who currently wins the game</returns>
        public IPlayer GetTheWinner()
        {
            this.Players.Sort((p1, p2) => p2.OverallScore.CompareTo(p1.OverallScore)); //descending
            return this.Players[0];
        }


        /// <summary>
        /// Prints stats for each player for the round was recently played
        /// </summary>
        public void PrintStatsForLastRound()
        {
            this.Players.Sort((p1, p2) => p1.ID.CompareTo(p2.ID));  //ascending

            for(int i=0; i<this.Players.Count; i++)
            {
                Console.WriteLine($"Round #{RoundsPlayed} > {this.Players[i].ToString()}");
            }
        }

        /// <summary>
        /// Resets the players' scores, cards and counters
        /// </summary>
        public void ResetGame()
        {
            this.RoundsPlayed = 0;
            InitPlayers();
        }
    }
}
