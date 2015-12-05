using CashTerminal.Models;

namespace CashTerminal.Commons
{
    internal interface IOverlayable
    {
        void CloseOverlay();

        void UpdateUI();

        SessionTimer Timer { get; }

        SettingsManager Settings { get; }

        MainModel Model { get; }
    }

    internal interface CopyOfIOverlayable
    {
        void CloseOverlay();

        void UpdateUI();

        SessionTimer Timer { get; }

        SettingsManager Settings { get; }

        MainModel Model { get; }
    }
}