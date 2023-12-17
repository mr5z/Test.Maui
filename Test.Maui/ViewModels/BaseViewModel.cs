
namespace Test.Maui.ViewModels;

internal class BaseViewModel : BindableBase, IInitialize, IPageLifecycleAware
{
    public virtual void Initialize(INavigationParameters parameters)
    {

    }

    public virtual void OnAppearing()
    {

    }

    public virtual void OnDisappearing()
    {

    }
}
