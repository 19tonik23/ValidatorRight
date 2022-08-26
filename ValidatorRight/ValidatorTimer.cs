using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ValidatorRight
{
    public class ValidatorDate
    {
        public Label labDate = new Label();
        public Label labTime = new Label();
        private Timer timer = new Timer();
        private List<string> listId = new List<string>();
        public ValidatorDate(Label labDate, Label labTime, List<string> listId)
        {
            this.listId = listId;
            timer.Interval=1000;
            this.labDate = labDate;
            this.labTime = labTime;
            timer.Tick+=timer1_Tick;
            timer.Start();
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            SetDatETime();
            CardNotAttached();   
        }

        //установка даты времени на мониторе валидатора
        private void SetDatETime()
        {
            labDate.Text = DateTime.Now.Day.ToString("00") + "." + DateTime.Now.Month.ToString("00") + "." + DateTime.Now.Year.ToString("00");
            labTime.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
        }

        //сообщение о том что после определённого времени обнаружена карта с id.... не считанная при выходе
        private void CardNotAttached()
        {
            if (labTime.Text == "23:00:00")
            {
                foreach (string id in listId)
                    MessageBox.Show("The card with the number " + id + " is not attached to the validator at the output");
            }
        }
    }
}
