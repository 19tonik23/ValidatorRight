using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValidatorRight
{
    public class Card
    {
        public Button btnCard = new Button();
        public Card()
        {
            CreatCard();
        }

        //создание карты
        private void CreatCard(){
            btnCard.BackgroundImage = Properties.Resources.bus_card;
            btnCard.BackgroundImageLayout = ImageLayout.Stretch;
            btnCard.Dock = DockStyle.Fill;
            btnCard.FlatAppearance.BorderSize = 0;
            btnCard.FlatStyle = FlatStyle.Flat;
            btnCard.Cursor = Cursors.Hand;
        } 
        //создание рамки для выбранной карты при входе
        public void Check_Card_Entr(Button btn)
        {
            btnCard.FlatAppearance.BorderSize = 1;
            btnCard.FlatStyle = FlatStyle.Standard;
        }

        //убирается индикация рамки карты при выходе
        public void Check_Card_Exit(Button btn)
        {
            btnCard.FlatAppearance.BorderSize = 0;
            btnCard.FlatStyle = FlatStyle.Flat;
        }
    }
}
