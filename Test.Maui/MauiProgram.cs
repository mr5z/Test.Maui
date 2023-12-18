using Microsoft.Extensions.Logging;
using PropertyValidator.Services;
using PropertyValidator.ValidationPack;
using Test.Maui.Resources.Texts;
using Test.Maui.Services;
using Test.Maui.ViewModels;
using CrossUtility.Extensions;

namespace Test.Maui;

public class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UsePrism(prism =>
            {
                prism.OnInitialized(container =>
                {
                    var eventAggregator = container.Resolve<IEventAggregator>();
                    eventAggregator.GetEvent<NavigationRequestEvent>().Subscribe(context => {
                        // Handle the event
                    });

                    ErrorMessageHelper.UpdateResource<ErrorMessages>();

                    var validationService = container.Resolve<IValidationService>();
                    validationService.SetErrorFormatter(errorMessages => 
                        string.Join(
                            Environment.NewLine, 
                            errorMessages.WhereNot(string.IsNullOrEmpty).Select(e => e + ".")
                        )
                    );
                });

                prism.RegisterTypes(registry =>
                {
                    registry
                        .RegisterForNavigation<MainPage, MainPageViewModel>()
                        .RegisterSingleton<IValidationService, ValidationService>()
                        .RegisterSingleton<IToastService, DummyToastService>();
                });

                prism.OnAppStart(async (container, navigation) =>
                {
                    var result = await navigation.CreateBuilder()
                        .AddSegment<MainPageViewModel>()
                        .NavigateAsync();

                    if (!result.Success)
                    {
                        var logger = container.Resolve<ILogger<MauiProgram>>();
                        var id = new EventId(1, "OnAppStart");
                        logger.LogError(id, result.Exception, "WHY DO I NEED TO PASS A MESSAGE WHEN THERE IS ALREADY AN EXCEPTION!?");
                    }
                });
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging
            .AddDebug();
#endif

        return builder.Build();
    }

    private static void HandleNavigationError(Exception ex)
    {
        Console.WriteLine(ex);
        System.Diagnostics.Debugger.Break();
    }
}
