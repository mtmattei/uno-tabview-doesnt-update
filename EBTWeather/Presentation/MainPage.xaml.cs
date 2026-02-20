using System;
using System.Threading;
using System.Threading.Tasks;
using EBTWeather.WeatherData;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uno.Extensions.Reactive;

namespace EBTWeather.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }
}
