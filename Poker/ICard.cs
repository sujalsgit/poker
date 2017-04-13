using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public interface ICard
    {
        CardRank Rank { get; set; }
        CardSuit Suit { get; set; }
        int GetCardValue();
    }
}
