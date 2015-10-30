using CashTerminal.Models;

namespace CashTerminal.Commons
{
    public interface IOverlayable
    {
        void CloseOverlay();

        SessionTimer Timer { get; }
    }
}