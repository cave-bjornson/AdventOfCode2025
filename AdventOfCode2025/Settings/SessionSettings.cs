using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdventOfCode2025.Settings;

/// <summary>
/// Represents the game settings, including display, language, and particle effects.
/// This class implements <see cref="INotifyPropertyChanged"/> to notify subscribers
/// when a property value changes, enabling data binding and UI updates.
/// </summary>
public class SessionSettings : INotifyPropertyChanged
{
    public string sessionKey { get; set; }

    // Add more settings as needed

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event to notify subscribers that a property value has changed.
    /// </summary>
    /// <param name="propertyName">
    /// The name of the property that changed. If not provided, the name of the calling member is used.
    /// </param>
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}