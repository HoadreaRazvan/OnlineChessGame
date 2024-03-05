using Chess_Client.CONTROLLER.LOGIN_CONTROLLER;
using Chess_Client.MODEL;
using Chess_Client.MODEL.LOGIN_MODEL;
using Chess_Client.VIEW.LOGIN_VIEW;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Chess_Client
{
    public partial class App : Application
    {
        public App()
        {
            LoginView loginView = new LoginView();
            LoginController loginController = new LoginController(loginView);
        }
    }
}
