using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CashTerminal.ViewModels;

namespace CashTerminal.Commons
{
    public class OverlayTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UnlockOverlayTemplate { get; set; }
        public DataTemplate LoginOverlayTemplate { get; set; }
        public DataTemplate SettingsOverlayTemplate { get; set; }
        public DataTemplate LoggerOverlayTemplate { get; set; }
        public DataTemplate SearchOverlayTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var overlay = (ViewModelBase) item;
            switch (overlay?.ToString())
            {
                case "UnlockControl":
                    return UnlockOverlayTemplate;
                case "LoginControl":
                    return LoginOverlayTemplate;
                case "SettingsControl":
                    return SettingsOverlayTemplate;
                case "LoggerControl":
                    return LoggerOverlayTemplate;
                case "SearchControl":
                    return SearchOverlayTemplate;
            }
            return null;
        }
    }
}