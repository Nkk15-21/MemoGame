using MemoGame.Services;

namespace MemoGame.Views;

public partial class SettingsPage : ContentPage // страница настроек
{
    public SettingsPage() => InitializeComponent();

    private void Apply(AppThemeName theme) // применить тему
        => App.ThemeManager.ApplyTheme(Application.Current!, theme);

    private void LightClicked(object sender, EventArgs e) => Apply(AppThemeName.Light);
    private void DarkClicked(object sender, EventArgs e) => Apply(AppThemeName.Dark);
    private void ColorfulClicked(object sender, EventArgs e) => Apply(AppThemeName.Colorful);
}
