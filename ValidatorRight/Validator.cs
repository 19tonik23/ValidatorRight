using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Media;
using System.Drawing;

namespace ValidatorRight
{
    public class Validator
    {
        public ValidCheckPanel valCheckPan;
        private ValidatorTextCheck valTextCheck;
        private ValidatorDate valTimer;
        private BusMonitorIcon busIcon;
        private XmlDocument document = new XmlDocument();
        private TextBox selectStop;
        private double fixSum = 0.5;
        private string[] filePath;
        private Dictionary<int, string> filesContainer = new Dictionary<int, string>();
        private bool[] ent_exit = new bool[] {false,false,false };
        private int cardIndex;
        public Validator(Panel validCheck, Label textCheck, Label labDate,
            
            Label labTime, Panel BusMonitorIcon,TextBox selectStop)
        {
            filePath = new string[] { "file_card0.xml", "file_card1.xml", "file_card2.xml" };
            this.selectStop = selectStop;
            valCheckPan = new ValidCheckPanel(validCheck);
            valTextCheck = new ValidatorTextCheck(textCheck);
            valTimer = new ValidatorDate(labDate, labTime, listId);
            busIcon = new BusMonitorIcon(BusMonitorIcon);
           
            CheckSound();
        }

        //прикладывание карты к валидатору
        public void CheckPanel(Panel panForCard,Button card,int cardIndex)
        {
            this.cardIndex = cardIndex;
            CheckSound();
            panForCard.Controls.Remove(card);
            busIcon.panBusMonitor.Visible = true;
            valCheckPan.validCheckPan.Controls.Add(card);
            NullBalance1(filePath[cardIndex]);
            valCheckPan.validCheckPan.Refresh();
            Thread.Sleep(700);
            valCheckPan.validCheckPan.Controls.Remove(card);
            panForCard.Controls.Add(card);
            busIcon.panBusMonitor.Visible = false;
            NullBalance2();

        }

        //звук считывания карты
        private void CheckSound()
        {
            SoundPlayer player = new SoundPlayer();
            player.Stream = Properties.Resources.check;
            player.Play();
        }

        //Обращение в файл
        private string fileId = "";
        private string fileBalance = "";
        public void GetCardData(AllCards allCards)
        {
            document.Load(filePath[allCards.cardIndex]);
            string fileStop = "";
            string fileDate = "";
            string fileTime = "";
            
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.LocalName == "id")
                {
                    fileId = node.InnerText;
                }
                if (node.LocalName == "balance")
                {
                    fileBalance = node.InnerText;
                }
                if (node.LocalName == "stop")
                {
                    fileStop = node.InnerText;
                }
               
                if (node.LocalName == "date")
                {
                    fileDate = node.InnerText;
                }
                if (node.LocalName == "time")
                {
                    fileTime = node.InnerText;
                }
            }
            if (Math.Abs(FileTime(fileTime) - CurrentValidatTime(valTimer.labTime.Text)) <= 100 &
                fileDate == valTimer.labDate.Text)
            {
                fixSum = 0;
            }
            else
            {
                fixSum = 0.5;
            }
            CalcBalance(fileBalance, fileStop,allCards);
            ent_exit[allCards.cardIndex] = !ent_exit[allCards.cardIndex];
        }

        //вход-выход
        private void CalcBalance(string fileBalance,string fileStop,AllCards allCards)
        {
              if (fileStop == "0")
            {
                WriteEntrStop(filePath[allCards.cardIndex]);
                FileContainerEnter(allCards);
            }
              else
            {  
                ReadStopBalance(fileBalance, fileStop,allCards);
                allCards.Check_Card_Exit(allCards.cardIndex);
            }
        }

        //Запись в файл остановку вход
        private void WriteEntrStop(string filePath)
        {
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.LocalName == "stop")
                {
                    node.InnerText = selectStop.Text;
                }
            }
            document.Save(filePath);
        }

        //выход и подсчёт баланса
        private void ReadStopBalance(string fileBalance, string fileStop,AllCards allCards)
        {
            double noMoney = double.Parse(fileBalance);
            double balance = 0;
            if (noMoney > 1)
            {
                balance = double.Parse(fileBalance) - fixSum - (Math.Abs((int.Parse(selectStop.Text)
                    - int.Parse(fileStop))));

                AccountBalance(balance);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.LocalName == "balance")
                    {
                        node.InnerText = balance.ToString();
                    }
                    if (node.LocalName == "stop")
                    {
                        node.InnerText = "0";
                    }
                    if (node.LocalName == "date")
                    {
                        node.InnerText = valTimer.labDate.Text;
                    }
                    if (node.LocalName == "time")
                    {
                        node.InnerText = valTimer.labTime.Text;
                    }
                }
                document.Save(filePath[allCards.cardIndex]);
                FileContainerExit(allCards);
            }
        }

        //конвертация времени из файла в int
        private int FileTime(string fileTime)
        {
            string[] time = fileTime.Split(':');
            string shortTime = time[0] + time[1];
            int fileTm = int.Parse(shortTime);
            return fileTm;
        }

        //конвертация времени на экране валидатора в int
        private int CurrentValidatTime(string validTime)
        {
            string[] time = validTime.Split(':');
            string shortTime = time[0] + time[1];
            int validTm = int.Parse(shortTime);
            return validTm;
        }

       //Cooбщение о недостаточной сумме на карточке
        private void NullBalance1(string filePath)
        {
            document.Load(filePath);
            string fileBalance = "";

            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {

                if (node.LocalName == "balance")
                {
                    fileBalance = node.InnerText;
                }
            }
            double balance = double.Parse(fileBalance);
            if (balance<=1)
            {
                valTextCheck.textCheck.Text = "No money";
                valTextCheck.textCheck.ForeColor = Color.Red;
                valTextCheck.textCheck.Refresh();
            }

        }

       //Возврат первоначального сообщения
        private void NullBalance2()
        {
            valTextCheck.textCheck.Text = "Check card";
                valTextCheck.textCheck.ForeColor = Color.White;
        }

        //остаток на счёте на мониторе валидатора после выхода 
        private void AccountBalance(double balance)
        {
            valTextCheck.textCheck.Text = "account balance-" + balance;
            valTextCheck.textCheck.ForeColor=Color.Yellow;
            valTextCheck.textCheck.Refresh();
            Thread.Sleep(700);
            valTextCheck.textCheck.Text = "Check card";
            valTextCheck.textCheck.ForeColor = Color.White;
        }

        //добавление карточек в коллекцию при входе
        private List<string> listId = new List<string>();
        private void FileContainerEnter(AllCards allCards)
        {
            document.Load(filePath[allCards.cardIndex]);
            string balance = "";
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {

                if (node.LocalName == "balance")
                {
                    balance = node.InnerText;
                }
               
            }
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.LocalName == "id"& double.Parse(balance)>1)
                {
                    listId.Add(node.InnerText);
                }

            }
        }

        //удаление карточек из коллекции при выходе
        private void FileContainerExit(AllCards allCards)
        {
            document.Load(filePath[allCards.cardIndex]);
           
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.LocalName == "id")
                {
                    listId.Remove(node.InnerText);
                }

            }
            
            
        }
    }
}
