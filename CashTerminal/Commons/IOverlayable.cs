using CashTerminal.Models;

namespace CashTerminal.Commons
{
    internal interface IOverlayable
    {
        void CloseOverlay();

        SessionTimer Timer { get; }

        SettingsManager Settings { get; }

        string LogText { get; }

        MainModel Model { get; }
    }
}