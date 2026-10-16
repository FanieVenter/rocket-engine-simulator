using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using static System.Math;

namespace RocketEngineSimulator.Views;

public partial class MainWindow : Window
{


    private readonly Engine _engine = new();
    private void Compile(object? sender, RoutedEventArgs e)
    {
        _engine.Solve();
        SetTextValues();
        
    }

    private void ThrustInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (TryReadInput(sender, out var value))
        {
            _engine.Thrust_KN = value;
        }
    }
    private void ChamberPressureInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (TryReadInput(sender, out var value))
        {
            _engine.ChamberPressureBar = value;
        }
    }
    private void MixtureRatioInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (TryReadInput(sender, out var value))
        {
            _engine.MixtureRatio = value;
        }
    }
    private void ChamberTempInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (TryReadInput(sender, out var value))
        {
            _engine.ChamberTemperature = value;
        }
    }
    private void ConvergentAngleInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (TryReadInput(sender, out var value))
        {
            _engine.ConvergentHalfAngleDeg = value;
        }
    }
    private void LStarInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (TryReadInput(sender, out var value))
        {
            _engine.LStar = value;
        }
    }

    private static bool TryReadInput(object? sender, out double value)
    {
        value = default;
        return sender is TextBox textBox &&
               double.TryParse(textBox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out value) &&
               double.IsFinite(value);
    }

    private void SetTextValues()
    {
        SetTelemetry("Isp", $"Isp: {Round(_engine.Isp, 3)} s");
        SetTelemetry("Thrust_kN", $"Thrust: {Round(_engine.Thrust_KN, 3)} kN");
        SetTelemetry("ExhaustVelocity", $"Exhaust velocity: {Round(_engine.ExhaustVelocity, 3)} m/s");
        SetTelemetry("ExitMach", $"Exit Mach: {Round(_engine.ExitMach, 3)}");
        SetTelemetry("MassFlow", $"Total mass flow: {Round(_engine.MassFlow, 3)} kg/s");
        SetTelemetry("FuelRate", $"Fuel flow: {Round(_engine.FuelRate, 3)} kg/s");
        SetTelemetry("OxidizerRate", $"Oxidizer flow: {Round(_engine.OxidizerRate, 3)} kg/s");
        SetTelemetry("ThroatDiameter", $"Throat diameter: {Round(_engine.ThroatDiameter * 1000, 3)} mm");
        SetTelemetry("ExitDiameter", $"Exit diameter: {Round(_engine.ExitDiameter * 1000, 3)} mm");
        SetTelemetry("ChamberDiameter", $"Chamber diameter: {Round(_engine.ChamberDiameter * 1000, 3)} mm");
        SetTelemetry("ConvergentLength", $"Convergent length: {Round(_engine.ConvergentLength * 1000, 3)} mm");
        SetTelemetry("ChamberLength", $"Chamber length: {Round(_engine.ChamberLength * 1000, 3)} mm");
        SetTelemetry("ThroatArea", $"Throat area: {Round(_engine.ThroatArea, 6)} m²");
        SetTelemetry("ExitArea", $"Exit area: {Round(_engine.ExitArea, 6)} m²");
        SetTelemetry("ChamberVolume", $"Chamber volume: {Round(_engine.ChamberVolume * 1000, 3)} L");
        SetTelemetry("ThroatPressure", $"Throat pressure: {Round(_engine.ThroatPressure / 100000, 3)} bar");
        SetTelemetry("ThroatTemperature", $"Throat temperature: {Round(_engine.ThroatTemperature, 3)} K");
        SetTelemetry("ExitTemperature", $"Exit temperature: {Round(_engine.ExitTemperature, 3)} K");
    }

    private void SetTelemetry(string name, string text) => this.FindControl<TextBlock>(name)!.Text = text;
    
    public MainWindow()
    {
        InitializeComponent();
    }
}
