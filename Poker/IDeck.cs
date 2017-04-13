using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public interface IDeck
    {
        int DeckSize { get; }
        List<ICard> PopCardsFromTop(int numberOfCardsToPop);
        void Shuffle(int times = 1);
    }
}
