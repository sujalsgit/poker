using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public interface IPlayer
    {
        int ID { get; }
        int LastRoundScore { get; set; }
        int OverallScore { get; set; }
        void ReceiveTwoCards(List<ICard> cards);
        int GetCurrentHandRank();
    }
}
