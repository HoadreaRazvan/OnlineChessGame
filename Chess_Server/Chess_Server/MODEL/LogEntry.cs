using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Server.MODEL
{
    public class LogEntry
    {
        private int index;
        private string name;
        private string message;
        private string date;

        public LogEntry(int index,string name, string message)
        {
            this.index= index;
            this.name = name;
            this.message = message;
            this.date = DateTime.Now.ToString("dd-MM-yyyy  HH:mm:ss");
        }


        public int Index
        {
            get => this.index; set => this.index = value;
        }
        public string Name
        {
            get => this.name; set => this.name = value;
        }
        public string Message
        {
            get => this.message; set => this.message = value;
        }
        public string Date
        {
            get => this.date; set => this.date = value;
        }
    }
}
