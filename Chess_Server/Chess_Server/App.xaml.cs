using Chess_Server.CONTROLLER;
using Chess_Server.MODEL;
using Chess_Server.VIEW;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Chess_Server
{
    public partial class App : Application
    {
        public App()
        {
            Network network = new Network();
            View view = new View();
            Controller controller = new Controller(network, view);        
        }
    }
}