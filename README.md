# Rocket Engine Simulator

A basic desktop rocket engine simulator written in C# with Avalonia for Hack Club Terra.

## Run

Install the .NET 11 SDK, then run this command from the repository root:

```sh
dotnet run --project "Rocket Engine Simulator.csproj"
```

## Use

1. Enter the desired thrust (kN), chamber pressure (bar), oxidizer-to-fuel mixture ratio, chamber temperature, convergent angle (degrees), and L* (m) in the left column. You can leave a field blank to use its built-in default.
2. Click **Compile** to calculate the results.
3. Read the performance, propellant flow, nozzle and chamber dimensions, and gas conditions in the right column. Change an input and click **Compile** again to recalculate.

**Temperature note:** The UI labels chamber temperature as °C, but the current calculation treats the entered value as kelvin. Enter kelvin values (the default is 3600 K) until the label and calculation are brought into agreement.

This is a simplified simulator; its results are estimates, not validated engine design data.
