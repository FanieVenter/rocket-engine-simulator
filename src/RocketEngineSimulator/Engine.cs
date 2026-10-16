namespace RocketEngineSimulator;
using static System.Math;
using System;
using System.Collections.Generic;
public class Engine
{
    // General Variables
    public double GasConstant = 8.31446;


    public double ConvergentLength;
    // Geometric Variables
    public double ChamberLength;
    public double ChamberVolume;
    public double ThroatArea;
    // Output Variables
    public double MassFlow;
    public double Isp;
    public double ChamberDiameter;

    
    // Performance Variables
    public double ThroatPressure;
    public double ExitMach;
    public double ExitMachSqaure;
    public double ThroatTemperature;
    public double ExitTemperature;
    public double ExhaustVelocity;
    
    public double ExitArea;
    public double ThroatDiameter;
    public double ExitDiameter;
    public double ChamberArea;
    
    public double ConvergentVolume;


    // Propellant Vars
    public enum Propellant { LOX, GOX, LH2, CH4 }

    public record PropellantData(
        string Formula,
        double MolarMass,           // kg/mol
        double Density, // kg/m³ (liquid, at storage temp); 0 for gas
        double BoilingPoint,        // K
        double SpecificHeat,        // J/(kg·K), liquid
        double Viscosity            // Pa·s, liquid
    );
    public static readonly Dictionary<Propellant, PropellantData> Propellants = new()
    {
        [Propellant.LOX] = new("O2",  0.031999,1141,  90.19, 1700, 0.000195),
        [Propellant.CH4] = new("CH4", 0.016043,  423,  111.66, 3500, 0.000118),
        [Propellant.LH2] = new("H2",  0.002016,   71,   20.28, 9800, 0.000013),
        [Propellant.GOX] = new("O2",  0.031999,    0,    90.19,  918, 0.000020),
    };
    public double FuelRate;
    public double OxidizerRate;

    
    public double SpecificGasConstant;

    
// Input Vars
    public double Thrust_KN = 300;
    public double AtmosphericPressure_Kpa = 101.325;
    public double ExitPressure_Kpa = 0;            // 0 = match ambient
    public double ChamberPressureBar = 200;
    public double MixtureRatio = 3.3;              // O/F
    public double ChamberTemperature = 3600;       // K, placeholder until checked against CEA
    public double PropellantGamma = 1.2;
    public double ExhaustMolarMass = 0.0215;       // kg/mol
    public double ContractionRatio = 5;
    public double LStar = 1.2;                     // m
    public double ConvergentHalfAngleDeg = 30;
    public double GravityAcceleration = 9.80665;

// Converted to SI in UnitConversion()
    public double Thrust;                          // N
    public double ChamberPressure;                 // Pa
    public double AtmosphericPressure;             // Pa
    public double ExitPressure;                    // Pa
    
    
    public void Solve()
    {
        InputCalculations();
        UnitConversion();
        CombustionCalculations();
        FuelCalculations();
        GeometryCalculations();
        PerformanceCalculations();
    }
    
    public void InputCalculations()
    {

    }
    
    public void UnitConversion()
    {
        ChamberPressure = ChamberPressureBar * 100000;
        AtmosphericPressure = AtmosphericPressure_Kpa * 1000;
        Thrust = Thrust_KN * 1000;
        ExitPressure = ExitPressure_Kpa > 0 ? ExitPressure_Kpa * 1000 : AtmosphericPressure;

    }
    
    public void PerformanceCalculations()
    {
        Isp =  ExhaustVelocity / GravityAcceleration ;
        
    }

    public void GeometryCalculations()
    {
        ThroatTemperature = ChamberTemperature * (1 / (1 + (PropellantGamma - 1) / 2));
        ThroatPressure = ChamberPressure * Pow(1 + (PropellantGamma - 1) / 2, -PropellantGamma / (PropellantGamma - 1));
        ThroatArea = ((MassFlow * Sqrt(ChamberTemperature)) / ChamberPressure) * Sqrt((SpecificGasConstant / PropellantGamma) * Pow(((PropellantGamma + 1) / 2), ((PropellantGamma + 1) / (PropellantGamma - 1))));
        ThroatDiameter = Sqrt((4 * ThroatArea) / PI);
        
        ExitMachSqaure = (2 / (PropellantGamma - 1) ) * (Pow((ChamberPressure / ExitPressure), ((PropellantGamma - 1) / PropellantGamma)) - 1);
        ExitTemperature = ChamberTemperature / (1 + (PropellantGamma - 1) / 2 * ExitMachSqaure);
        ExitMach = Sqrt( ExitMachSqaure );
        ExitArea = (ThroatArea/ExitMach) * Pow(((1 + ((PropellantGamma - 1) / 2)*ExitMachSqaure) / ((PropellantGamma + 1) / 2)), (PropellantGamma + 1) / (2 * (PropellantGamma - 1)));
        ExitDiameter = Sqrt((4 * ExitArea) / PI);

        ChamberArea = ContractionRatio * ThroatArea;
        ChamberDiameter = Sqrt(ContractionRatio) * ThroatDiameter;
        ConvergentLength = (ChamberDiameter - ThroatDiameter) / 2 / Tan(ConvergentHalfAngleDeg * PI / 180);
        ConvergentVolume = PI * ConvergentLength / 12 * (Pow((ChamberDiameter), 2) + ChamberDiameter * ThroatDiameter + Pow((ThroatDiameter), 2)); 
        ChamberVolume = LStar * ThroatArea;
        ChamberLength = (ChamberVolume - ConvergentVolume) / ChamberArea;
    }

    public void CombustionCalculations()
    {
        SpecificGasConstant = GasConstant / ExhaustMolarMass;
        ExhaustVelocity = Sqrt(((2 * PropellantGamma) / (PropellantGamma - 1) * ( SpecificGasConstant * ChamberTemperature)) * (1 - Pow((ExitPressure / ChamberPressure), ((PropellantGamma - 1) / PropellantGamma))));
    }
    
    public void FuelCalculations()
    {
        MassFlow = Thrust / ExhaustVelocity;
        OxidizerRate = MassFlow * MixtureRatio / (1 + MixtureRatio);
        FuelRate = MassFlow / (1 + MixtureRatio);
        
    }


}
