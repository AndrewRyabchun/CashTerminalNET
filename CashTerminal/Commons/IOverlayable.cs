using CashTerminal.Models;

namespace CashTerminal.Commons
{
    internal interface IOverlayable
    {
        void CloseOverlay();

        SessionTimer Timer { get; }

        SettingsManager Settings { get; }

        MainModel Model { get; }
    }
}