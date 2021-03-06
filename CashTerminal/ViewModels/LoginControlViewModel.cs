﻿using CashTerminal.Commons;
using System.Windows;
using System.Windows.Input;
using CashTerminal.Models;

namespace CashTerminal.ViewModels
{
    internal class LoginControlViewModel : ViewModelBase
    {
        private string _username;
        private string _password;

        private readonly IOverlayable _parent;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand InnerLoginCommand { get; set; }

        public LoginControlViewModel(IOverlayable parent)
        {
            InnerLoginCommand = new RelayCommand(Login, IsValidFields);
            _parent = parent;
            parent.Timer.Suspend();
        }

        private void Login(object obj)
        {
            _parent.Model.Validator = new Authorization(Username, Password);
            if (!_parent.Model.Validator.IsValid)
            {
                MessageBox.Show("Неверные данные", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _parent.Timer.Reset();
            _parent.CloseOverlay();
        }

        private bool IsValidFields(object obj)
        {
            if (string.IsNullOrEmpty(Username))
                return false;
            if (string.IsNullOrWhiteSpace(Password))
                return false;
            return true;
        }

        public override string ToString()
        {
            return "LoginControl";
        }
    }
}