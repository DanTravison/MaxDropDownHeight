using MaxDropDownHeight.Resources;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace MaxDropDownHeight
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("fluentsystemicons-resizable.ttf", nameof(FluentUI));
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
