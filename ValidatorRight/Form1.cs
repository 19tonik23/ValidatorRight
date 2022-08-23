using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace ValidatorRight
{
    public partial class Form1 : Form
    {

        private Validator val;
        private AllCards allCards;
        public Form1()
        {
            InitializeComponent();
            val = new Validator(panCheck, labTextCheck, labDate,labTime ,panBusMonitor,selectStop);
            allCards = new AllCards(tablePanCard,val.valCheckPan.validCheckPan,selectStop);
        }

        //событие после выбора остановки
        private void butSelectStop_Click(object sender, EventArgs e)
        {
            val.CheckPanel(allCards.panCards[allCards.cardIndex], allCards.allCardsBtn[allCards.cardIndex], allCards.cardIndex);
            val.GetCardData( allCards);
            selectStop.Clear();
            selectStop.Focus();
            
        }

       
    }
}
