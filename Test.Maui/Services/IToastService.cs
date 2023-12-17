namespace Test.Maui.Services
{
    internal interface IToastService
    {
        void ShowMessage(string format, params object[] args);
    }
}
