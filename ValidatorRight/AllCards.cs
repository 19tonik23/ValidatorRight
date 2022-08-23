using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ValidatorRight
{
    public class AllCards
    {
        private int cardsCount = 3;
        public Card[] allCards;
        public Button[] allCardsBtn;
        public Panel[] panCards;
        private Panel validCheckPanel;
        private TextBox selectStop;
        public int cardIndex;
        public AllCards(TableLayoutPanel tablePanCard, Panel validCheckPanel, TextBox selectStop)
        {
           Cards(tablePanCard);
           this.validCheckPanel = validCheckPanel;
           this.selectStop = selectStop;
        }

        //создание карт
        private void Cards(TableLayoutPanel tablePanCard)
        {
            panCards = new Panel[cardsCount];
            allCards = new Card[cardsCount];
            allCardsBtn = new Button[cardsCount];
            for (int i = 0; i < allCards.Length;i++ )
            {
                panCards[i] = new Panel();
                panCards[i].Dock = DockStyle.Fill;
                allCards[i] = new Card();
                allCardsBtn[i] = allCards[i].btnCard;
                allCardsBtn[i].TabIndex = i;
                allCardsBtn[i].Click += Check_Card;
                tablePanCard.Controls.Add(panCards[i], i, 0);
                panCards[i].Controls.Add(allCardsBtn[i]);
                

            }
        }

        //событие выбора карты
        private void Check_Card(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            cardIndex = btn.TabIndex;
            allCards[cardIndex].Check_Card_Entr(allCardsBtn[cardIndex]);
            selectStop.Focus();
           
        }

        //убирается индикация рамки карты при выходе
        public void Check_Card_Exit(int cardIndex)
        {
            allCards[cardIndex].Check_Card_Exit(allCardsBtn[cardIndex]);
        }
        
    }
}
