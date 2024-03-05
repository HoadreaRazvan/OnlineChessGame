using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess_Client.MODEL.GAME_MODEL
{
    public class Message
    {
        private string sender;
        private string text;
        private HorizontalAlignment alignment;

        public Message(string sender, string text, HorizontalAlignment alignment)
        {
            this.sender = sender;
            this.text = text;
            this.alignment = alignment;
        }

        public string Sender
        {
            get => this.sender; set => this.sender = value;
        }
        public string Text
        {
            get => this.text; set => this.text = value;
        }
        public HorizontalAlignment Alignment
        {
            get => this.alignment; set => this.alignment = value;
        }
    }
}
