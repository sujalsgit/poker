using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Program
    {
        //Ninject
        

        static void Main(string[] args)
        {
            Console.WriteLine("--------------Poker Game by Sujal Shah--------------");
            Console.WriteLine("--------------Game Started--------------");
            GameEngine gameEngine = new GameEngine(roundsToPlay: 3, playersCount: 5);
            Random r = new Random();
            for(int round=0; round<gameEngine.RoundsToPlay; round++)
            {
                int deckShuffleTimes = r.Next(1, 10);
                gameEngine.PlayNewRound(deckShuffleTimes);
                gameEngine.PrintStatsForLastRound();
                Console.WriteLine();
            }

            Console.WriteLine("--------------Game Over--------------");
            IPlayer winner = gameEngine.GetTheWinner();
            Console.WriteLine($"The winner is: {winner.ID}. With overall score: {winner.OverallScore}");
            Console.ReadLine();
        }
    }
}
